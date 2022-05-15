using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMCHSGManager.Models;
using SMCHSGManager.ViewModel;
using System.Linq.Dynamic;
using System.Web.Security;
using System.Globalization;


namespace SMCHSGManager.Controllers
{

    public class MemberInfoController : Controller
    {
        private SMCHDBEntities _entities = new SMCHDBEntities();

        private int _pageSize = 100;


		[Authorize(Roles = "Administrator")]
		public ActionResult List(string sort, int? page, string searchContent, int? IsActive, int? initiateTypeID)
		{
			var currentPage = page ?? 1;
			ViewData["SortItem"] = sort;
			sort = sort ?? "UserName";
			ViewData["PageSize"] = _pageSize;

			ViewData["searchContent"] = searchContent;
			ViewData["initiateOnly"] = false;

            if (IsActive == null)
            {
                IsActive = 3;
            }
			ViewData["IsActive"] = IsActive;

			int initTypeCount = _entities.InitiateTypes.Count() + 1;
			if (initiateTypeID == null)
			{
				initiateTypeID = initTypeCount;
			}
			ViewData["initiateTypeID"] = initiateTypeID;

			List<MemberOnlineInfo> memberInfos = (from r in _entities.MemberInfos
												  where (r.aspnet_Users.UserName.Contains(searchContent) || searchContent == null) &&
															 (r.IsActive && IsActive == 1 || !r.IsActive && IsActive == 2 || IsActive == 3)
															   && (r.InitiateTypeID == initiateTypeID || initiateTypeID == initTypeCount)
												  select new MemberOnlineInfo()
												  {
													  ID = r.MemberID,
													  UserName = r.aspnet_Users.UserName,
													  //Name = r.Name, 
													  InitiateStatus = r.InitiateType.Name,
													  IsActive = r.IsActive,
												  }).ToList();

			ViewData["TotalPages"] = (int)Math.Ceiling((float)memberInfos.Count() / _pageSize);

			if ((int)ViewData["TotalPages"] < currentPage)
			{
				currentPage = 1;
			}
			ViewData["CurrentPage"] = currentPage;

			List<MemberOnlineInfo> memberInfos1 = new List<MemberOnlineInfo>();
			foreach (MemberOnlineInfo milvm in memberInfos)
			{
				MembershipUser mu = Membership.GetUser(milvm.ID);
				milvm.Email = mu.Email;
				milvm.IsOnline = mu.IsOnline;
				//milvm.IsApproved = mu.IsApproved;
				//milvm.IsLockedOut = mu.IsLockedOut;
				milvm.LastActivityDate = mu.LastActivityDate.ToUniversalTime().AddHours(8);
				memberInfos1.Add(milvm);
			}

			var sortedMemberInfos = (memberInfos1.AsQueryable().OrderBy(sort).Skip((currentPage - 1) * _pageSize).Take(_pageSize)).ToList();

			return View(sortedMemberInfos);
		}

        [Authorize(Roles = "Administrator, Dharma Protector")]
        public ActionResult iList(string sort, int? page, string searchContent, int? IsActive, int? initiateTypeID)
        {
            var currentPage = page ?? 1;
            ViewData["SortItem"] = sort;
            sort = sort ?? "MemberNo";
            ViewData["PageSize"] = _pageSize;

            ViewData["searchContent"] = searchContent;
            ViewData["initiateOnly"] = false;

            if (IsActive == null)
            {
                IsActive = 1;
            }
            ViewData["IsActive"] = IsActive;

            int initTypeCount = _entities.InitiateTypes.Count() + 1;
            if (initiateTypeID == null)
            {
                initiateTypeID = initTypeCount;
            }
            ViewData["initiateTypeID"] = initiateTypeID;

            List<PublicMemberShortInfo> memberInfos = (from r in _entities.MemberInfos
                                                  where (r.aspnet_Users.UserName.Contains(searchContent) || searchContent == null) &&
                                                             (r.IsActive && IsActive == 1 || !r.IsActive && IsActive == 2 || IsActive == 3)
                                                               && (r.InitiateTypeID == initiateTypeID || initiateTypeID == initTypeCount)
                                                       select new PublicMemberShortInfo()
                                                  {
                                                      ID = r.MemberID,
                                                      Name = r.Name, 
                                                      InitiateStatus = r.InitiateType.Name,
                                                      MemberNo = r.MemberNo,
                                                      DateOfInitiation = r.DateOfInitiation,
                                                      DateOfBirth = r.DateOfBirth,
                                                      Gender = r.Gender.Name,
                                                      IsActive = r.IsActive,
                                                  }).OrderBy(sort).ToList();

            MemberFeePaymentController mfpc = new MemberFeePaymentController();
            List<MemberFeeExpiredDateInfo> latestMemberFeePayments = mfpc.updateMemberFeeExipredDate();
            foreach (PublicMemberShortInfo pmi in memberInfos)
            {
                if (latestMemberFeePayments.Any(a => a.MemberID == pmi.ID))
                {
                    pmi.MemberFeeExpiredDate = latestMemberFeePayments.SingleOrDefault(a => a.MemberID == pmi.ID).MemberFeeExpiredDate;
                }
            }
 
            ViewData["TotalPages"] = 1;

            if ((int)ViewData["TotalPages"] < currentPage)
            {
                currentPage = 1;
            }
            ViewData["CurrentPage"] = currentPage;

            return View(memberInfos);
        }


		//public ActionResult List(string sort, int? page, string searchContent, int? IsActive, int? initiateTypeID)
		//{
		//    var currentPage = page ?? 1;
		//    ViewData["SortItem"] = sort;       
		//    sort = sort ?? "UserName";
		//    ViewData["PageSize"] = _pageSize;

		//    ViewData["searchContent"] = searchContent;
		//    ViewData["initiateOnly"] = false;


		//    if (IsActive == null)
		//    {
		//        IsActive = 1;
		//    }
		//    ViewData["IsActive"] = IsActive;

		//    int initTypeCount = _entities.InitiateTypes.Count() + 1;
		//    if (initiateTypeID == null)
		//    {
		//        initiateTypeID = initTypeCount;
		//    }
		//    ViewData["initiateTypeID"] = initiateTypeID;

		//    List<PublicMemberInfo> memberInfos = (from r in _entities.MemberInfos 
		//                     where (r.aspnet_Users.UserName.Contains(searchContent) || searchContent == null) &&
		//                                (r.IsActive && IsActive == 1 || !r.IsActive && IsActive == 2 || IsActive == 3)
		//                                && (r.InitiateTypeID == initiateTypeID || initiateTypeID == initTypeCount)
		//                                select new PublicMemberInfo()
		//                         {
		//                             MemberID = r.MemberID,
		//                             Name = r.Name,
		//                             InitiateStatus = r.InitiateType.Name,
		//                             IDCardNo = r.IDCardNo,
		//                             GenderID = r.GenderID,
		//                             IsActive = r.IsActive,
		//                             MemberNo = r.MemberNo,
		//                             MemberFeeExpiredDate = r.MemberFeeExpiredDate,
		//                         }).ToList();
                
		//    ViewData["TotalPages"] = (int)Math.Ceiling((float)memberInfos.Count() / _pageSize);

		//    if ((int)ViewData["TotalPages"] < currentPage)
		//    {
		//        currentPage = 1;
		//    }
		//    ViewData["CurrentPage"] = currentPage;

		//    List<PublicMemberInfo> memberInfos1 = new List<PublicMemberInfo>();
		//    foreach (PublicMemberInfo milvm in memberInfos)
		//    {
		//        MembershipUser mu = Membership.GetUser(milvm.MemberID);
		//        milvm.Email = mu.Email;
		//        memberInfos1.Add(milvm);
		//    }

		//    var sortedMemberInfos = (memberInfos1.AsQueryable().OrderBy(sort).Skip((currentPage - 1) * _pageSize).Take(_pageSize)).ToList();
  
		//    return View(sortedMemberInfos); 
		//}


        // Tools 
		//public ActionResult GetAllInitiateUserNameNameEmals(string sort, int? page, string searchContent, int? memberTypeID, bool initiateOnly)
		//{
		//    _pageSize = 500;
		//    var currentPage = page ?? 1;
		//    ViewData["SortItem"] = sort;
		//    sort = sort ?? "UserName";
		//    ViewData["PageSize"] = _pageSize;

		//    ViewData["searchContent"] = searchContent;
		//    ViewData["initiateOnly"] = initiateOnly;

		//        int memberTypeCount = _entities.MemberTypes.Count() + 1;

		//        if (memberTypeID == null)
		//        {
		//            memberTypeID = memberTypeCount;
		//        }

		//        ViewData["memberTypeID"] = memberTypeID;

		//       var memberInfos = from p in _entities.InitiateMemberInfos
		//                      where (p.MemberInfo.Name.Contains(searchContent) || searchContent == null) &&
		//                                (p.MemberInfo.InitiateMemberInfo.MemberTypeID == memberTypeID || memberTypeID == memberTypeCount)
		//                      select new OrdinaryMemberListViewModel()
		//                      {
		//                          ID = p.IMemberID,
		//                          UserName = p.MemberInfo.aspnet_Users.UserName,
		//                          Email = null,
		//                          Name = p.MemberInfo.Name,
		//                          MemberType = p.MemberType.Name,
		//                          MemberNo = p.MemberNo,
		//                          MemberFeeExpiredDate = p.MemberFeeExpiredDate,
		//                      };
    
		//    ViewData["TotalPages"] = (int)Math.Ceiling((float)memberInfos.Count() / _pageSize);

		//    if ((int)ViewData["TotalPages"] < currentPage)
		//    {
		//        currentPage = 1;
		//    }
		//    ViewData["CurrentPage"] = currentPage;

		//    List<OrdinaryMemberListViewModel> memberInfos1 = new List<OrdinaryMemberListViewModel>();
		//    foreach (OrdinaryMemberListViewModel milvm in memberInfos)
		//    {
		//        MembershipUser mu = Membership.GetUser(milvm.ID);
		//        //if (mu.Email.EndsWith("@smchsg.com"))
		//        //{
		//        //    milvm.Email = mu.Email;
		//        // }
		//        //else
		//        //{
		//        //    milvm.Email = "*********";
		//        //}

		//        //Get all ***@smchsg.com account Begin
		//        if (mu.Email.EndsWith("@smchsg.com") && mu.Email != "password@smchsg.com")
		//        {
		//            milvm.Email = mu.Email;
		//        }
		//        else
		//        {
		//            sort = "Email";
		//            continue;
		//         }
		//        //Get all ***@smchsg.com account end

		//         memberInfos1.Add(milvm);
		//    }

		//    var sortedMemberInfos = (memberInfos1.AsQueryable().OrderBy(sort).Skip((currentPage - 1) * _pageSize).Take(_pageSize)).ToList();

		//    return View("List", sortedMemberInfos);
		//}

    
             //
        // GET: /Home/

		//[Authorize(Roles = "Contact Person")]
		//public ActionResult Index()
		//{
		//    var iMemberInfos = from r in _entities.InitiateMemberInfos where (r.IsActive == false) orderby r.MemberInfo.Name select r;
		//    return View(iMemberInfos);
		//}

		//public ActionResult Accept(Guid id)
		//{
		//    InitiateMemberInfo iMemberInfo = _entities.InitiateMemberInfos.Single(a => a.IMemberID == id);
		//    iMemberInfo.IsActive = true;
		//    UpdateModel(iMemberInfo, "InitiateMemberInfo");
		//    _entities.SaveChanges();

		//    MembershipUser user = Membership.GetUser(id);
		//    Roles.AddUserToRole(user.UserName, "Initiate");

		//    EmailMessage em = GenerateEmailMessage(true, id);
		//    EmailService es = new EmailService();
		//    es.SendMessage(em);

		//    var iMemberInfos = from r in _entities.InitiateMemberInfos where (r.IsActive == false) orderby r.MemberInfo.Name select r;
		//    if (iMemberInfos.Count() > 0)
		//    {
		//        return RedirectToAction("Index");
		//    }
		//    else
		//    {
		//        return RedirectToAction("Index", "Home");
		//    }
		//}

		//public ActionResult Reject(Guid id)
		//{
		//    EmailMessage em = GenerateEmailMessage(false, id);
		//      EmailService es = new EmailService();
		//    es.SendMessage(em);
		//    return RedirectToAction("Index", "Home");
		//}

		//public EmailMessage GenerateEmailMessage(bool accept, Guid id)
		//{
		//    EmailMessage em = new EmailMessage();
		//    em.Subject = "Status of member application";
		//    if (accept)
		//    {
		//        string urlBase = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath;
		//        string verifyUrl = "Account/logon";
		//        string fullUrl = urlBase + verifyUrl;

		//        em.Message = "Your application already approved! your account has been fully activated, please " + fullUrl + " to the site";
		//    }
		//    else
		//    {
		//        em.Message = "Your application has been rejected, please check again the your information you input.";
		//    }
		//    em.From = Membership.GetUser().Email;
		//    em.To = Membership.GetUser(id).Email;
		//    return em;

		//}

		//public void SendEmailToContactPerson(RegisterModel register)
		//{
		//    EmailMessage em = new EmailMessage();
		//    em.Subject = "Please confirm my initiate status";
		//    em.Message = " Halo, my name is " + register.UserName + "I need you to confirm my initiate status, Thanks!.";
		//    em.From = register.Email;

		//    string CPName = Roles.GetUsersInRole("Contact Person").FirstOrDefault();
		//    em.To = Membership.GetUser(CPName).Email;
		//   // em.To = "ftmnwl64@singnet.com.sg";

		//    EmailService es = new EmailService();
		//    es.SendMessage(em);
		//}


        //
        // GET: /Home/Details/5

		//public ActionResult Details(Guid? id)
		//{

		//    MembershipUser user = Membership.GetUser();
		//    if (id == null)
		//    {
		//        id = ((Guid)(user.ProviderUserKey));
		//    }
		//    user = Membership.GetUser(id);
		//    MemberInfo mi = _entities.MemberInfos.Single(a => a.MemberID == id.Value);

		//    var viewModel = new PublicMemberInfo
		//    {
		//         Name = mi.Name,
		//         DateOfInitiation = mi.DateOfInitiation,
		//         GenderName = mi.Gender.Name,
		//         //GenderID = mi.GenderID,
		//         IDCardNo = mi.IDCardNo,
		//         InitiateStatus = mi.InitiateType.Name,
		//         IsActive = mi.IsActive,
		//         Email = user.Email,
		//         ContactNo = mi.ContactNo,
		//         MemberNo = mi.MemberNo, 
		//    };

		//    return View(viewModel);

		//}
		

		////  POST: /Home/Create
		////[Authorize(Roles = "Administrator")]
		//public ActionResult Create()
		//{
		//    //RegisterModel register = GenerateFromMemberInfo(memberID);
		//    RegisterModel register = (RegisterModel)TempData["register"];

		//    var viewModel = new PublicMemberInfo
		//    {
		//        RegisterModel = register,
		//        Gender = _entities.Genders.ToList(),
		//    };

		//    if (!Roles.IsUserInRole("Administrator"))
		//    {
		//        viewModel.InitiateMemberInfo.MemberTypeID = 4;
		//    }

		//    TempData["register"] = register;
		//    TempData["initiateOnly"] = initiateOnly;

		//    return View(viewModel);
		//}



		//// //  POST: /Home/Create
		//// [HttpPost]
		//public ActionResult Create(InitiateMemberInfo initiateMemberInfo, FormCollection collection, string Upload)
		//{
		//    RegisterModel register = (RegisterModel)TempData["register"];

		//    try
		//    {
		//        ValidateMemberInfo(initiateMemberInfo);
		//        if (!ModelState.IsValid)
		//        {
		//            throw new Exception();
		//        }

		//        if (Roles.IsUserInRole("Administrator"))
		//        {
		//            initiateMemberInfo.IsActive = true;
		//        }
		//        else
		//        {
		//            initiateMemberInfo.MemberTypeID = 4;
		//        }

		//        MembershipUser mu = Membership.GetUser(register.UserName);
		//        initiateMemberInfo.IMemberID = (Guid)(mu.ProviderUserKey);

		//        _entities.AddToInitiateMemberInfos(initiateMemberInfo);

		//        var memberInfo = _entities.MemberInfos.Single(a => a.MemberID == initiateMemberInfo.IMemberID);
		//        memberInfo.InitiateTypeID = register.InitiateTypeID;

		//        UpdateModel(memberInfo, "MemberInfo");

		//        _entities.SaveChanges();

		//        if (Roles.IsUserInRole("Administrator"))
		//        {
		//            return RedirectToAction("List", new { initiateOnly = initiateOnly });
		//        }
		//        else
		//        {
		//            SendEmailToContactPerson(register);
		//            return RedirectToAction("RegisterConfirmation", "Account", new { initiate = true });
		//        }
		//    }
		//    catch
		//    {
		//        //Invalid - redisplay with errors
		//        var viewModel = new OrdinaryMemberViewModel
		//        {
		//            InitiateMemberInfo = initiateMemberInfo,
		//            MemberType = _entities.MemberTypes.ToList(),
		//            RegisterModel = register,
		//            InitiateType = _entities.InitiateTypes.ToList(),
		//            Nationality = _entities.Nationalities.ToList(),
		//            Gender = _entities.Genders.ToList(),
		//            Race = _entities.Races.ToList(),
		//            EmploymentStatus = _entities.EmploymentStatuses.ToList(),
		//            PayMethod = _entities.PayMethods.ToList(),
		//        };

		//        TempData["register"] = register;
		//        TempData["initiateOnly"] = initiateOnly;

		//        return View(viewModel);
		//    }
		//}
       
	   //private static RegisterModel GetRegisterModelInfo(FormCollection collection, out string[] roles)
	   //{
	   //    RegisterModel register = new RegisterModel();
	   //    register.Email = collection.Get("RegisterModel.Email");
	   //    register.Name = collection.Get("RegisterModel.Name");
	   //    register.InitiateTypeID = 1;
	   //    register.Password = collection.Get("RegisterModel.Password");
	   //    register.ConfirmPassword = collection.Get("RegisterModel.ConfirmPassword");
	   //    register.UserName = collection.Get("RegisterModel.UserName");

	   //    roles = (collection.GetValues("RoleChecks")).ToArray();
	   //    string[] allRoles = Roles.GetAllRoles();
	   //    for (int i = 0; i < roles.Count(); i++)
	   //    {
	   //        int j = int.Parse(roles[i]) - 1;
	   //        roles[i] = allRoles[j];
	   //    }

	   //    return register;
	   //}

	   //private void ImageUploadToServer()
	   //{
	   //    HttpPostedFileBase file = Request.Files["fileUpload"];
	   //    if (string.IsNullOrEmpty(file.FileName))
	   //    {
	   //        ModelState.AddModelError("", "Please select a file !.");
	   //        //throw new Exception("Please select a file!");
	   //    }
	   //    if (file.ContentLength > 60000)
	   //    {
	   //        ModelState.AddModelError("", "Your upload file should not exceed 60kb!");
	   //    }
	   //    if (file.ContentType.EndsWith("jpg"))
	   //    {
	   //        ModelState.AddModelError("", "The photo must be submitted in jpg format!");
	   //    }
	   //    if (!ModelState.IsValid)
	   //    {
	   //        throw new Exception();
	   //    }

	   //    ImageController ic = new ImageController();
	   //    UploadFile uploadFile = ic.ImageUpload(file);
	   //    ViewData["CurrentUploadFile"] = uploadFile;
	   //}

	   //public void RemoveUploadFileFromServer(int uploadFileID)
	   //{
	   //    UploadFile uploadFile = _entities.UploadFiles.Single(a => a.ID == uploadFileID);
	   //    _entities.DeleteObject(uploadFile);
	   //    _entities.SaveChanges();
	   //}


	 

	   //private RegisterModel GenerateRegisterModel(Guid id, MemberInfo memberInfo)
	   //{
	   //    RegisterModel register = new RegisterModel();
	   //    MembershipUser user = Membership.GetUser(id);
	   //    List<string> allRoles = Roles.GetAllRoles().ToList();
	   //    string[] roles = Roles.GetRolesForUser(user.UserName);

	   //    if (memberInfo == null)
	   //    {
	   //        memberInfo = _entities.MemberInfos.Single(a => a.MemberID == id);
	   //        register.Email = user.Email;
	   //     }
	   //    else
	   //    {
	   //        register.Email = (string)ViewData["Email"];
	   //        roles = (string[])ViewData["roles"];
	   //     }

	   //    register.Name = memberInfo.Name;
	   //    register.InitiateTypeID = memberInfo.InitiateTypeID;
	   //    register.UserName = user.UserName;

	   //    for (int i = 0; i < roles.Count(); i++)
	   //    {
	   //        int j = allRoles.IndexOf(roles[i]) +1;
	   //        roles[i] = j.ToString();
	   //    }
	   //    register.Role = roles;
	   //    ModelState.SetModelValue("RoleChecks", new ValueProviderResult(register.Role, "", CultureInfo.InvariantCulture));
           
	   //    return register;
	   //}

	   //protected void ValidateMemberInfo(InitiateMemberInfo iMemberInfoToValidate)
	   //{
	   //    DateTime minDOB = new DateTime(1900, 1, 1);
	   //    if (!(minDOB <= iMemberInfoToValidate.DateOfBirth && iMemberInfoToValidate.DateOfBirth < iMemberInfoToValidate.DateOfInitiation && iMemberInfoToValidate.DateOfInitiation < DateTime.Now))
	   //    {
	   //        ModelState.AddModelError("InitiateMemberInfo.DateOfBirth", "DateOfBirth or DateOfInitiate not correct!.");
	   //    }
          
	   //}


	   //[HttpPost]
	   //public ActionResult Edit(Guid id, FormCollection collection, string Upload)
	   //{
	   //     var memberInfo = _entities.MemberInfos.Single(a => a.MemberID == id);
	   //     var iMemberInfo = new InitiateMemberInfo();
	   //     if (memberInfo.InitiateTypeID == 1 || memberInfo.InitiateTypeID == 2)
	   //     {
	   //         iMemberInfo = _entities.InitiateMemberInfos.Single(a => a.IMemberID == id);
	   //     }

	   //     RegisterModel register = new RegisterModel();
	   //     register.Email = collection.Get("RegisterModel.Email");
	   //     register.Name = collection.Get("RegisterModel.Name");

	   //    string[] roles = Roles.GetRolesForUser();
	   //    if (Roles.IsUserInRole("Administrator"))
	   //    {
	   //        register.InitiateTypeID = (int.Parse)(collection.Get("RegisterModel.InitiateTypeID"));
	   //        roles = (collection.GetValues("RoleChecks")).ToArray();
	   //        string[] allRoles = Roles.GetAllRoles();
	   //        for (int i = 0; i < roles.Count(); i++)
	   //        {
	   //            int j = int.Parse(roles[i]) - 1;
	   //            roles[i] = allRoles[j];
	   //        }
	   //    }
	   //    else
	   //    {
	   //        register.InitiateTypeID = memberInfo.InitiateTypeID;
	   //        register.Role = Roles.GetRolesForUser();
	   //    }

	   //    try
	   //    {
	   //        if (memberInfo.InitiateTypeID == 1 || memberInfo.InitiateTypeID == 2)
	   //        {
	   //            UpdateModel(iMemberInfo, "InitiateMemberInfo");
	   //            ValidateMemberInfo(iMemberInfo);
	   //            if (!ModelState.IsValid)
	   //            {
	   //                throw new Exception();
	   //            }
	   //        }
	   //        UpdateModel(memberInfo, "MemberInfo");
	   //        _entities.SaveChanges();

	   //        AccountController ac = new AccountController();
	   //        ac.UpdateEmail(register.Email, id);
               
	   //        //MembershipUser user = Membership.GetUser(id);
	   //        //if (register.Email != user.Email)
	   //        //{
	   //        //    user.Email = register.Email;
	   //        //    Membership.UpdateUser(user);
	   //        //}

	   //        if (Roles.IsUserInRole("Administrator"))
	   //        {
	   //            MembershipUser user = Membership.GetUser(id);
	   //            string[] oldRoles = Roles.GetRolesForUser(user.UserName);
	   //            if (!IsEqual(roles, oldRoles))
	   //            {
	   //                Roles.RemoveUserFromRoles(user.UserName, oldRoles);
	   //                Roles.AddUserToRoles(user.UserName, roles);
	   //            }
	   //        }

	   //        return RedirectToAction("Details", new { id = id });

	   //    }
	   //    catch
	   //    {

	   //        var viewModel = new OrdinaryMemberViewModel
	   //        {
	   //            InitiateMemberInfo = iMemberInfo,
	   //            MemberType = _entities.MemberTypes.ToList(),
	   //            RegisterModel = GenerateRegisterModel(id, null),
	   //            InitiateType = _entities.InitiateTypes.ToList(),
	   //            Nationality = _entities.Nationalities.ToList(),
	   //            Gender = _entities.Genders.ToList(),
	   //            Race = _entities.Races.ToList(),
	   //            EmploymentStatus = _entities.EmploymentStatuses.ToList(),
	   //            PayMethod = _entities.PayMethods.ToList(),
	   //        };

	   //        ViewData["memberID"] = id;

	   //        return View(viewModel);
	   //    }
	   //}

	   //public bool IsEqual(string[] first, string[] second)
	   //{
	   //    //bool value = true;

	   //    if (first.Count() != second.Count())
	   //    {
	   //        return false;
	   //    }

	   //    for (int i = 0; i < first.Count(); i++)
	   //    {
	   //        if (first[i] != second[i])
	   //        {
	   //                return false;
	   //        }
	   //    }
	   //    return true;
	   //}


	   //public ActionResult ApplyMember(Guid id, int memberTypeID)
	   //{

	   //    InitiateMemberInfo initiateMemberInfo = _entities.InitiateMemberInfos.Single(a => a.IMemberID == id);
	   //    //if (initiateMemberInfo.AttachFileCollectionIDCollectionID != null)
	   //    //{
	   //    //    ModelStateSetting(initiateMemberInfo.AttachFileCollectionIDCollectionID, "InitiateMemberInfo");
	   //    //    ViewData["AttachFileCollectionIDID"] = initiateMemberInfo.AttachFileCollectionIDID;
	   //    //}

	   //    var viewModel = new OrdinaryMemberViewModel
	   //    {
	   //        InitiateMemberInfo = initiateMemberInfo,
	   //        MemberType = _entities.MemberTypes.ToList(),
	   //        Nationality = _entities.Nationalities.ToList(),
	   //        Gender = _entities.Genders.ToList(),
	   //        Race = _entities.Races.ToList(),
	   //        EmploymentStatus = _entities.EmploymentStatuses.ToList(),
	   //        PayMethod = _entities.PayMethods.ToList(),
	   //    };

	   //    ViewData["memberTypeID"] = memberTypeID;
	   //    ViewData["memberID"] = id;
	   //    viewModel.MemberType.RemoveRange(2, 2);
	   //    viewModel.InitiateMemberInfo.MemberTypeID = memberTypeID;

	   //    ViewData["MemberFee"] = (decimal)2;
	   //    if (memberTypeID == 1)
	   //    {
	   //        ViewData["MemberFee"] = _entities.EmploymentStatuses.Single(a => a.ID == initiateMemberInfo.EmploymentStatusID).MemberFee.Value;
	   //    }

	   //    return View(viewModel);
	   //}

	   //[HttpPost]
	   //public ActionResult ApplyMember(Guid id,  int memberTypeID, FormCollection collection)
	   //{
	   //    var iMemberInfo = _entities.InitiateMemberInfos.Single(a => a.IMemberID == id);
	   //    int count = (from r in _entities.InitiateMemberInfos where r.MemberTypeID == memberTypeID orderby r.MemberNo descending select r.MemberNo.Value).First();

	   //    try
	   //    {
	   //        iMemberInfo.MemberTypeID = memberTypeID;
	   //        iMemberInfo.MemberNo = count + 1;
	   //        iMemberInfo.MemberEffectiveStartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1);
	   //        iMemberInfo.MemberApplyDate = DateTime.Today.ToUniversalTime().AddHours(8);
	   //        //iMemberInfo.MemberFeeExpiredDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1);
	   //        iMemberInfo.MemberFeePayByID = int.Parse(collection.Get("MemberFeePayByID"));

	   //        UpdateModel(iMemberInfo, "InitiateMemberInfo");
	   //        _entities.SaveChanges();

	   //        EmailMessage em = new EmailMessage();
	   //        em.Subject = _entities.MemberTypes.Single(a=>a.ID == memberTypeID).Name + " Member Application";
	   //        em.Message = "Your application has been approved, please login your account to check your menber deails information.";
	   //        em.From = "admin@smchsg.con";
	   //        em.To = Membership.GetUser(id).Email;

	   //        EmailService es = new EmailService();
	   //        es.SendMessage(em);


	   //        return RedirectToAction("Details", new { id = id });

	   //    }
	   //    catch
	   //    {
	   //         return View();
	   //    }
	   //}

       

    }
}
