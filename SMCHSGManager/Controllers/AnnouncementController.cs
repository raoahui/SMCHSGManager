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
    public class AnnouncementController : Controller
    {

		private SMCHDBEntities _entities = new SMCHDBEntities();
		private int _pageSize = 8;

        //
        // GET: /Announcements/

		public ActionResult Index(int? page, int? announceGroupID, string searchContent)
        {
            var currentPage = page ?? 1;
            //var curAnnounceGroupID = announceGroupID ?? _entities.AnnounceGroups.Count() ;

            ViewData["AnnounceGroupID"] = announceGroupID;
            ViewData["CurrentPage"] = currentPage;
            ViewData["PageSize"] = _pageSize;

			var allAnnouncements = GetAnnouncements(announceGroupID, User.IsInRole("Initiate"), searchContent);

			ViewData["TotalPages"] = (int)Math.Ceiling((float)allAnnouncements.Count() / _pageSize);

			var pageViewModel = allAnnouncements.Skip((currentPage - 1) * _pageSize).Take(_pageSize);

            //ViewData["AnnouncementImageIDs"] = GetAnnouncementImage(pageViewModel.ToList());

			var viewModel = new HomeViewModel
			{
				Announcements = pageViewModel.ToList(),
				AnnouncementImages = GetAnnouncementImage(pageViewModel.ToList()),
			};

            return View(viewModel);
        }

		public IOrderedQueryable<Announcement> GetAnnouncements(int? announceGroupID, bool initiateOnly, string searchContent)
        {
			var viewModel = from r in _entities.Announcements where (r.AnnounceGroupID == announceGroupID || announceGroupID == null) && (string.IsNullOrEmpty(searchContent) || r.Name.Contains(searchContent) || r.Description.Contains(searchContent)) 
							orderby r.AnnounceDate descending select r;
            if (!initiateOnly)
            {
                var viewModel1 = from r in viewModel where r.IsPublic orderby r.AnnounceDate descending select r;
                viewModel = viewModel1;
            }
            return viewModel;
        }

        public string[] GetAnnouncementImage(List<Announcement> announcements)
        {
			string[] AnnouncementImages = new string[_pageSize];
            int i = 0;
            foreach (Announcement an in announcements)
            {
				if (an != null && an.AnnouncementUploadFiles != null)
				{
					foreach (UploadFile uploadFile in an.AnnouncementUploadFiles.Select(a=>a.UploadFile).ToList())
					{
						if (uploadFile.ContentType.StartsWith("image"))
						{
							 AnnouncementImages[i] = uploadFile.FilePath + uploadFile.Name; 
							break;
						}
					}

					i++;
				}
            }
            return AnnouncementImages;
        }

        //
        // GET: /Announcements/Details/5

        public ActionResult Details(int id)
        {
            int announceGroupID = _entities.Announcements.Single(a => a.ID == id).AnnounceGroupID;
            var announcementIDs = (from r in _entities.Announcements where r.AnnounceGroupID == announceGroupID orderby r.AnnounceDate descending select r.ID).ToList();
            if (!User.IsInRole("Initiate"))
            {
                announcementIDs = (from r in _entities.Announcements where r.IsPublic orderby r.AnnounceDate descending select r.ID).ToList();
            }
 
            var viewModel = new AnnouncementViewModel
            {
                Announcement = _entities.Announcements.Single(a => a.ID == id),
                AnnouncementIDs = announcementIDs,
            };

            return View(viewModel);
        }

        //
        // GET: /Announcements/Create

		[Authorize(Roles = "Administrator")]
		public ActionResult Create(int announceGroupID)
        {
            var viewModel = new AnnouncementViewModel()
            {
                 Announcement = new Announcement(),
                 AnnouncementIDs = new List<int>(),
				 //UploadFiles = (from r in _entities.UploadFiles orderby r.UploadTime descending select r).ToList(),
            };
            viewModel.Announcement.Name = "[Please input announcement name]";
            viewModel.Announcement.AnnounceDate = DateTime.Now.ToUniversalTime().AddHours(8);
            //viewModel.Announcement.IsPublic = true;

            viewModel.AnnouncementIDs.Add(announceGroupID);

            return View(viewModel);
        }

        //
        // POST: /Announcements/Create

        //[HttpPost, ValidateInput(false)]
		[AcceptVerbs(HttpVerbs.Post), ValidateInput(false), Authorize(Roles = "Administrator")]
		public ActionResult Create(FormCollection collection, Announcement announcement, string Upload)
		{
			MembershipUser mu = Membership.GetUser();

            try
            {
                ImageController ac = new ImageController();
				List<UploadFile> uploadFiles = ac.GenerateAttachFileCollectionID(collection, "Announcement");

                if (Upload != null)
                {
                    ImageUploadToServer();
					if (uploadFiles != null)
                    {
						ModelStateSetting(uploadFiles, "Announcement");
                    }
                    var viewModel = new AnnouncementViewModel
                    {
                        Announcement = announcement,
                        AnnouncementIDs = new List<int>(),
                    };
                    viewModel.AnnouncementIDs.Add((int)TempData["AnnounceGroupID"]);
                    return View(viewModel);
                }

				foreach (UploadFile uploadFile in uploadFiles)
				{
					AnnouncementUploadFile auf = new AnnouncementUploadFile();
					auf.UploadFileID = uploadFile.ID;
					announcement.AnnouncementUploadFiles.Add(auf);
				}

				announcement.AnnouncerID = (Guid)(mu.ProviderUserKey);
                announcement.AnnounceGroupID =  (int)TempData["AnnounceGroupID"];

                _entities.AddToAnnouncements(announcement);
                _entities.SaveChanges();

                return RedirectToAction("Index", new { announceGroupID = (int)TempData["AnnounceGroupID"] });
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
        // GET: /Announcements/Edit/5

		[Authorize(Roles = "Administrator")]
		public ActionResult Edit(int id)
        {
            var announcement = _entities.Announcements.Single(a => a.ID == id);

            if (announcement.AnnouncementUploadFiles != null)
            {
				List<UploadFile> uploadFiles = announcement.AnnouncementUploadFiles.Select(a => a.UploadFile).ToList();
				ModelStateSetting(uploadFiles, "Announcement");
            }

            var viewModel = new AnnouncementViewModel()
            {
                Announcement = announcement,
           };
            viewModel.Announcement.AnnounceDate = DateTime.Now.ToUniversalTime().AddHours(8);
            return View(viewModel);
        }


        //
        // POST: /Announcements/Edit/5

        //[HttpPost, ValidateInput(false)]
		[AcceptVerbs(HttpVerbs.Post), ValidateInput(false), Authorize(Roles = "Administrator")]
		public ActionResult Edit(int id, FormCollection collection, string Upload)
        {
            var announcement = _entities.Announcements.Single(a => a.ID == id);
	
            try
            {
                ImageController ac = new ImageController();
				List<UploadFile> uploadFiles = ac.GenerateAttachFileCollectionID(collection, "Announcement");

				if (Upload != null)
				{
					ImageUploadToServer();

					if (uploadFiles != null)
					{
						ModelStateSetting(uploadFiles, "Announcement");
					}

					UpdateModel(announcement, "Announcement");
					var viewModel = new AnnouncementViewModel()
					{
						Announcement = announcement,
					};
					return View(viewModel);
				}

				List<AnnouncementUploadFile> aUploadFiles = announcement.AnnouncementUploadFiles.ToList();
				foreach (AnnouncementUploadFile aUploadFile in aUploadFiles)
				{
					announcement.AnnouncementUploadFiles.Remove(aUploadFile);
				}

				foreach (UploadFile uploadFile in uploadFiles)
				{
					AnnouncementUploadFile auf = new AnnouncementUploadFile();
					auf.UploadFileID = uploadFile.ID;
					announcement.AnnouncementUploadFiles.Add(auf);
				}

				announcement.AnnounceDate = DateTime.Parse(collection.GetValues("Announcement.AnnounceDate")[0]);
				announcement.Description = collection.GetValues("Announcement.Description")[0];
				announcement.IsPublic = bool.Parse(collection.GetValues("Announcement.IsPublic")[0]);
				announcement.Name = collection.GetValues("Announcement.Name")[0];
				announcement.StaticURL = collection.GetValues("Announcement.StaticURL")[0];
				//announcement. = collection.Get("Announcement.URLChecked");
				
				//UpdateModel(announcement, "Announcement");
               _entities.SaveChanges();

                return RedirectToAction("Index", new { announceGroupID = announcement.AnnounceGroupID });
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

		//private UploadFile ImageUploadToServer()
		//{
		//    HttpPostedFileBase file = Request.Files["fileUpload"];
		//    if (string.IsNullOrEmpty(file.FileName))
		//    {
		//        throw new Exception("Please select a file!");
		//    }

		//    ImageController ic = new ImageController();
		//    UploadFile uploadFile = ic.ImageUpload(file);
		//    return uploadFile;
		//}

        //
        // GET: /Announcements/Delete/5

		[Authorize(Roles = "Administrator")]
		public ActionResult Delete(int id)
        {
            var announcement = _entities.Announcements.Single(a => a.ID == id);
            return View(announcement);
        }

        //
        // POST: /Announcements/Delete/5

        //[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), ValidateInput(false), Authorize(Roles = "Administrator")]
		public ActionResult Delete(int id, FormCollection collection)
        {
            if (id != 0)
            {
                var announcement = _entities.Announcements.Single(a => a.ID == id);
                ViewData["AnnounceGroupID"] = announcement.AnnounceGroupID;

                _entities.DeleteObject(announcement);

                _entities.SaveChanges();
            }
            return View("Deleted");
        }

    
    }
}
