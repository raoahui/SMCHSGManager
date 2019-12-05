using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMCHSGManager.Models;
using SMCHSGManager.ViewModel;
using System.Web.Security;
using System.Globalization;
using System.IO;
using System.Text;

namespace SMCHSGManager.Controllers
{
    public class LocationController : Controller
    {

        private SMCHDBEntities _entities = new SMCHDBEntities();
        private int _pageSize = 6;
        //
        // GET: /Location/

		[Authorize(Roles = "Administrator")]
		public ActionResult Index(int? page)
        {
            var currentPage = page ?? 1;
            ViewData["CurrentPage"] = currentPage;
            ViewData["PageSize"] = _pageSize;
            ViewData["TotalPages"] = (int)Math.Ceiling((float)_entities.Locations.Count() / _pageSize);

            var viewModel = from r in _entities.Locations orderby r.ID select r;
            var pageViewModel = viewModel.Skip((currentPage - 1) * _pageSize).Take(_pageSize);

            return View(pageViewModel);
        }

        //
        // GET: /Location/Details/5

		[Authorize(Roles = "Administrator")]
		public ActionResult Details(int id)
        {
            var viewModel = new LocationViewModel
            {
                Location = _entities.Locations.Single(a => a.ID == id),
                LocationIDs = (from r in _entities.Locations orderby r.ID select r.ID).ToList(),
            };
            return View(viewModel);
        }

        //
        // GET: /Location/Create

		[Authorize(Roles = "Administrator")]
		public ActionResult Create()
        {
            var viewModel = new LocationViewModel()
            {
                Location = new Location(),
            };

            viewModel.Location.Name = "[Please input location name]";
            return View(viewModel);
        } 

        //
        // POST: /Location/Create

		//[HttpPost, ValidateInput(false)]
		[AcceptVerbs(HttpVerbs.Post), ValidateInput(false), Authorize(Roles = "Administrator")]
		public ActionResult Create(FormCollection collection, Location location, string Upload)
        {
                try
                {
                    ImageController ac = new ImageController();
					List<UploadFile> uploadFiles = ac.GenerateAttachFileCollectionID(collection, "Location");
 
                     if (Upload != null)
                    {
                        ImageUploadToServer();
						if (uploadFiles != null)
                        {
							ModelStateSetting(uploadFiles, "Location");
                        }
                        return View();
                    }

					 foreach (UploadFile uploadFile in uploadFiles)
					 {
						 LocationUploadFile auf = new LocationUploadFile();
						 auf.UploadFileID = uploadFile.ID;
						 location.LocationUploadFiles.Add(auf);
					 }

                    _entities.AddToLocations(location);
                    _entities.SaveChanges();

                    return RedirectToAction("Index");

                }
                catch
                {
                    return View();
                }
 
        }


		public void ModelStateSetting(List<UploadFile> uploadFiles, string ControllerName)
		{
			foreach (UploadFile uploadFile in uploadFiles)
			{
				ModelState.SetModelValue(ControllerName + ".&." + uploadFile.ID.ToString(), new ValueProviderResult("True", "", CultureInfo.InvariantCulture));
			}
		}

        //
        // GET: /Location/Edit/5

		[Authorize(Roles = "Administrator")]
		public ActionResult Edit(int id)
        {
            var location = _entities.Locations.Single(a => a.ID == id);

            if (location.LocationUploadFiles != null)
            {
				List<UploadFile> uploadFiles = location.LocationUploadFiles.Select(a => a.UploadFile).ToList();
				ModelStateSetting(uploadFiles, "Location");
                //ViewData["AttachFileCollectionIDID"] = location.AttachFileID;
            }

            var viewModel = new LocationViewModel()
            {
                Location = location,
            };
            return View(viewModel);
        }

        //
        // POST: /Location/Edit/5

		//[HttpPost, ValidateInput(false)]
		[AcceptVerbs(HttpVerbs.Post), ValidateInput(false), Authorize(Roles = "Administrator")]
		public ActionResult Edit(int id, FormCollection collection, string Upload)
        {
            var location = _entities.Locations.Single(a => a.ID == id);
   
            try
            {
                ImageController ac = new ImageController();
 				List<UploadFile> uploadFiles = ac.GenerateAttachFileCollectionID(collection, "Location");

                if (Upload != null)
                {
                    ImageUploadToServer();

					if (uploadFiles != null)
                    {
						ModelStateSetting(uploadFiles, "Location");
                    }

                    UpdateModel(location, "Location");
                    var viewModel = new LocationViewModel()
                    {
                        Location = location,
                    };
                    return View(viewModel);
                }

				List<LocationUploadFile> aLocationUploadFiles = location.LocationUploadFiles.ToList();
				foreach (LocationUploadFile aLocationUploadFile in aLocationUploadFiles)
				{
					location.LocationUploadFiles.Remove(aLocationUploadFile);
				}

				foreach (UploadFile uploadFile in uploadFiles)
				{
					LocationUploadFile auf = new LocationUploadFile();
					auf.UploadFileID = uploadFile.ID;
					location.LocationUploadFiles.Add(auf);
				}

                UpdateModel(location, "Location");
                _entities.SaveChanges(); 

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private void ImageUploadToServer()
        {
            HttpPostedFileBase file = Request.Files["fileUpload"];
            if (string.IsNullOrEmpty(file.FileName))
            {
                throw new Exception("Please select a file!");
            }
            
            ImageController ic = new ImageController();
			string savePath = "../images/UploadFiles";
			string filePath = Path.Combine(HttpContext.Server.MapPath(savePath), Path.GetFileName(file.FileName));
			UploadFile uploadFile = ic.ImageUpload(file, filePath, savePath);
			ViewData["CurrentUploadFile"] = uploadFile;
        }
        
        //
        // GET: /Location/Delete/5

		[Authorize(Roles = "Administrator")]
		public ActionResult Delete(int id)
        {
            var location = _entities.Locations.Single(a => a.ID == id);
            return View(location);
        }

        //
        // POST: /Location/Delete/5

        //[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
		public ActionResult Delete(int id, FormCollection collection)
        {
            if (id != 0)
            {
                 var location = _entities.Locations.Single(a => a.ID == id);

                  _entities.DeleteObject(location);

                _entities.SaveChanges();
            }
            return View("Deleted");
        }
    }
}
