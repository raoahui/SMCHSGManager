using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMCHSGManager.Models;
using SMCHSGManager.ViewModel;
using System.Web.Security;
using System.IO;
using System.Text;

namespace SMCHSGManager.Controllers
{
    public class ImageController : Controller
    {

        private SMCHDBEntities _entities = new SMCHDBEntities();
        private int _pageSize = 50;

        //
        // GET: /Image/

		[Authorize(Roles = "Administrator")]
		public ActionResult Index(int? page)
        {
			//ChangeUploadFileValues();
			
			var currentPage = page ?? 1;
            ViewData["CurrentPage"] = currentPage;
            ViewData["PageSize"] = _pageSize;
            ViewData["TotalPages"] = (int)Math.Ceiling((float)_entities.UploadFiles.Count() / _pageSize);

			var viewModel = _entities.UploadFiles.OrderBy(a => a.ID); 
            var pageViewModel = viewModel.Skip((currentPage - 1) * _pageSize).Take(_pageSize);

			List<int> imageusedAll = new List<int> ();
			List<int> imageUsed = _entities.AnnouncementUploadFiles.Select(a => a.UploadFileID).Distinct().ToList();
			imageusedAll.AddRange(imageUsed);
			imageUsed = _entities.EventUploadFiles.Select(a => a.UploadFileID).Distinct().ToList();
			AddToAll(imageusedAll, imageUsed);
			imageUsed = _entities.LocationUploadFiles.Select(a => a.UploadFileID).Distinct().ToList();
			AddToAll(imageusedAll, imageUsed);
			imageUsed = _entities.ProductUploadFiles.Select(a => a.UploadFileID).Distinct().ToList();
			AddToAll(imageusedAll, imageUsed);

			//List<int> imageUsed = (from r in _entities.AnnouncementUploadFiles
			//                      group r by new
			//                          {
			//                              ID = r.UploadFileID
			//                          } into result
			//                      select result.Key.ID).ToList();

			ViewData["imageUsed"] = imageusedAll;
            ViewData["imageCount"] = viewModel.Count();

            return View(pageViewModel);
        }

		private static void AddToAll(List<int> imageusedAll, List<int> imageUsed)
		{
			foreach (int i in imageUsed)
			{
				if (!imageusedAll.Contains(i))
				{
					imageusedAll.Add(i);
				}
			}
		}

		public void ChangeUploadFileValues()
		{
			List<UploadFile> uploadFiles = _entities.UploadFiles.ToList();
			foreach (UploadFile up in uploadFiles)
			{
				if (up.Name.StartsWith("image") || up.Name.StartsWith("Jewelry"))
				{
					up.FilePath = "../../images/SMProducts/";
				}
			}
			_entities.SaveChanges();
		}

        //     
        // GET: /Image/Create

		[Authorize(Roles = "Administrator")]
		public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Image/Create

		//[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
		public ActionResult Create(FormCollection collection) 
        {
            try
            {
                HttpPostedFileBase file = Request.Files["fileUpload"];
                if (string.IsNullOrEmpty(file.FileName))
                {
                    throw new Exception("Please select a file!");
                }
				string savePath = "../images/SMProducts";
				string filePath = Path.Combine(HttpContext.Server.MapPath(savePath), Path.GetFileName(file.FileName));
				UploadFile uploadFile = ImageUpload(file, filePath, savePath);
				ViewData["CurrentUploadFile"] = uploadFile;

                //return RedirectToAction("index");
                return View();
            }
            catch
            {
                return View();
            }
        }

        public String GetFileType(String filename)
        {
            String ext = Path.GetExtension(filename);
            String contenttype = "others"; //String.Empty;

            //Set the contenttype based on File Extension
            switch (ext.ToLower())
            {
                case ".doc":
                    contenttype = "application/vnd.ms-word";
                    break;
                case ".docx":
                    contenttype = "application/vnd.ms-word";
                    break;
                case ".xls":
                    contenttype = "application/vnd.ms-excel";
                    break;
                case ".xlsx":
                    contenttype = "application/vnd.ms-excel";
                    break;
                case ".ppt":
                    contenttype = "application/vnd.ms-ppt";
                    break;
                case ".jpg":
                    contenttype = "image/jpg";
                    break;
                case ".png":
                    contenttype = "image/png";
                    break;
                case ".gif":
                    contenttype = "image/gif";
                    break;
                case ".pdf":
                    contenttype = "application/pdf";
                    break;

            }
            return contenttype;
        }


		//public UploadFile ImageUpload(HttpPostedFileBase file)
		//{

		//    UploadFile image = new UploadFile();
		//    try
		//    {
		//        //Image is just a Entityframework class that has
		//        // String Name, Byte[] ActualImage, size and String ContentType it makes it infinitely easier to get it back later when trying to show the image.
		//        image.Name = file.FileName;
		//        image.Size = file.ContentLength;
		//        image.ContentType = GetFileType(file.FileName);
		//        //image.ContentType = file.ContentType;

		//        MembershipUser mu = Membership.GetUser();
		//        image.UploadTime = DateTime.Now.ToUniversalTime().AddHours(8);
		//        image.UploadBy = (Guid)(mu.ProviderUserKey);

		//        Int32 length = file.ContentLength;
		//        //This may seem odd, but the fun part is that if I didn't have a temp image to read into, it would  get memory issues. 
		//        byte[] tempImage = new byte[length];
		//        file.InputStream.Read(tempImage, 0, length);
		//        image.Data = tempImage;

		//        _entities.AddToUploadFiles(image);
		//        _entities.SaveChanges();

		//        image = (from r in _entities.UploadFiles orderby r.UploadTime descending select r).First();
            
		//    }
		//    catch{
                
		//    }

		//    return image;
		//}

		public UploadFile ImageUpload(HttpPostedFileBase file, string filePath, string sSavePath)
		{
			UploadFile image = new UploadFile();
			
			if (file.ContentLength > 0)
			{
				//string filePath = Path.Combine(HttpContext.Server.MapPath(sSavePath), Path.GetFileName(file.FileName));
				file.SaveAs(filePath);

				image.Name = file.FileName;
				image.ContentType = GetFileType(file.FileName);
				image.FilePath = sSavePath + '/'; // "../images/UploadFiles/";

				MembershipUser mu = Membership.GetUser();
				image.UploadTime = DateTime.Now.ToUniversalTime().AddHours(8);
				image.UploadBy = mu.UserName;
				_entities.AddToUploadFiles(image);
				_entities.SaveChanges();

				image = (from r in _entities.UploadFiles orderby r.UploadTime descending select r).First();
			}
			return image;
		}

	    public List<UploadFile> GenerateAttachFileCollectionID(FormCollection collection, string ControllerName)
        {
			List<UploadFile> selectUploadFiles = new List<UploadFile>();
			List<UploadFile> allUploadFiles = (from r in _entities.UploadFiles where !r.Name.StartsWith("image") select r).ToList();

			foreach (UploadFile uploadFile in allUploadFiles)
            {
				string temp = collection.Get(ControllerName + ".&." + uploadFile.ID.ToString());
                if (ControllerName == "")
                {
					temp = collection.Get("&." + uploadFile.ID.ToString());
                }
                
                if (string.IsNullOrEmpty(temp) || !temp.ToLower().StartsWith("true"))
                {
                    continue;
                }

				//AttachFileMap attachFileMap = new AttachFileMap();
				//attachFileMap.UploadFileID = i;
				//afcID.AttachFileMaps.Add(attachFileMap);
				selectUploadFiles.Add(uploadFile);
            }

			return selectUploadFiles;
        }

        public class ImageResult : ActionResult
        {
            public String ContentType { get; set; }
            public byte[] ImageBytes { get; set; }
            public String SourceFilename { get; set; }

            //This is used for times where you have a physical location
            public ImageResult(String sourceFilename, String contentType)
            {
                SourceFilename = sourceFilename;
                ContentType = contentType;
            }

            //This is used for when you have the actual image in byte form
            //  which is more important for this post.
            public ImageResult(byte[] sourceStream, String contentType)
            {
                ImageBytes = sourceStream;
                ContentType = contentType;
            }

            public override void ExecuteResult(ControllerContext context)
            {
                var response = context.HttpContext.Response;
                response.Clear();
                response.Cache.SetCacheability(HttpCacheability.NoCache);
                response.ContentType = ContentType;

                //Check to see if this is done from bytes or physical location
                //  If you're really paranoid you could set a true/false flag in
                //  the constructor.
                if (ImageBytes != null)
                {
                    var stream = new MemoryStream(ImageBytes);
                    stream.WriteTo(response.OutputStream);
                    stream.Dispose();
                }
                else
                {
                    response.TransmitFile(SourceFilename);
                }
            }
        }

		//[AcceptVerbs(HttpVerbs.Get)]
		//public ActionResult ShowPhoto(Int32 id)
		//{
		//    //This is my method for getting the image information
		//    // including the image byte array from the image column in
		//    // a database.
		//    UploadFile image = _entities.UploadFiles.Single(a=>a.ID==id);
		//    //As you can see the use is stupid simple.  Just get the image bytes and the
		//    //  saved content type.  See this is where the contentType comes in real handy.
		//    ImageResult result = new ImageResult(image.Data, image.ContentType);

		//    return result;
		//}

		//[AcceptVerbs(HttpVerbs.Get)]
		//public ActionResult ShowImage(Int32 id)
		//{
		//    //This is my method for getting the image information
		//    // including the image byte array from the image column in
		//    // a database.
		//    SMCHDBLocalEntities _localEntities = new SMCHDBLocalEntities();
		//    AttachedFile image = _localEntities.AttachedFiles.Single(a => a.ID == id);
		//    //As you can see the use is stupid simple.  Just get the image bytes and the
		//    //  saved content type.  See this is where the contentType comes in real handy.
		//    ImageResult result = new ImageResult(image.Data, image.ContentType);

		//    return result;
		//}

		//
        // GET: /Image/Delete/5


		[Authorize(Roles = "Administrator")]
		public ActionResult Delete(int id)
        {
			List<int> UploadFileIDs = _entities.AnnouncementUploadFiles.Where(a => a.UploadFileID == id).Select(a=>a.AnnouncementID).ToList();
			if (UploadFileIDs.Count() ==0)
			{
				UploadFileIDs = _entities.EventUploadFiles.Where(a => a.UploadFileID == id).Select(a => a.EventID).ToList();
			}
			else if (UploadFileIDs.Count() == 0)
			{
				UploadFileIDs = _entities.ProductUploadFiles.Where(a => a.UploadFileID == id).Select(a => a.ProductID).ToList();
			}
			else if (UploadFileIDs.Count() == 0)
			{
				UploadFileIDs = _entities.LocationUploadFiles.Where(a => a.UploadFileID == id).Select(a => a.LocationID).ToList();
			}

			if (UploadFileIDs.Count() > 0)
			{
				TempData["ImageStatus"] = "Image was used by another user, so it is impossible to delete.";
			}
			else
			{
				var uploadFile = _entities.UploadFiles.Single(a => a.ID == id);
				_entities.DeleteObject(uploadFile);
			}

			return View();
            
        }

        //
        // POST: /Image/Delete/5

		//[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
		public ActionResult Delete(int id, FormCollection collection)
        {
            var image = _entities.UploadFiles.Single(a => a.ID == id);
            _entities.DeleteObject(image);
            _entities.SaveChanges();

            return View("Deleted");
 
        }


  

    }
}
