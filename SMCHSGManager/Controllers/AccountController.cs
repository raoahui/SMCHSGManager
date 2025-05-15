using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using SMCHSGManager.Models;
using System.Data.EntityClient;
using System.Globalization;

namespace SMCHSGManager.Controllers
{

    [HandleError]
    public class AccountController : Controller
    {
        private SMCHDBEntities _entities = new SMCHDBEntities();

        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }

		public void InitializePublic(RequestContext requestContext)
		{
			Initialize(requestContext);
		}

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

        // **************************************
        // URL: /Account/LogOn
        // **************************************

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ValidateUser(model.UserName, model.Password))
                {
					Guid iMemberID = (Guid)Membership.GetUser(model.UserName).ProviderUserKey;
					if (_entities.BlackListMembers.Any(a => a.MemberD == iMemberID && a.IDCardTypeID == 3 && a.DateTo > DateTime.Today)
						|| _entities.BlackListMembers.Any(a => a.MemberD == iMemberID && a.IDCardTypeID == 3 && !a.DateTo.HasValue))
					{
						string remark = _entities.BlackListMembers.Where(a => a.MemberD == iMemberID && a.IDCardTypeID == 3).OrderByDescending(a => a.DateFrom).FirstOrDefault().Remark;
						ModelState.AddModelError("", model.UserName + ' ' + remark + ", please contact CP to login in.");
					}
					else
					{
						FormsService.SignIn(model.UserName, model.RememberMe);
						if (!String.IsNullOrEmpty(returnUrl))
						{
							return Redirect(returnUrl);
						}
						else
						{
                            return RedirectToAction("Index", "Home");
						}
					}
                }
                else
                {
                    MembershipUser memCheckUser = Membership.GetUser(model.UserName);
                    if (memCheckUser == null)
                    {
                        ModelState.AddModelError("", "The user name could not be found.");
                    }
                    else
                    {
                        // Is this user locked out?
                        if (memCheckUser.IsLockedOut)
                        {
							ModelState.AddModelError("", "Your account has been locked out as a result of too many invalid login attempts. Please reset your password with the \"Forgot Your Password?\" link and your account will be automatically unlocked.");
                        }
                        else if (!memCheckUser.IsApproved)
                        {
                            ModelState.AddModelError("", "Your account has not yet been approved. You cannot login until an administrator has approved your account.");
                        }
                        else
                        {
                            ModelState.AddModelError("", "The password provided is incorrect.");
                        }
                    }        
 
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************

        public ActionResult LogOff()
        {
            FormsService.SignOut();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult RecoverPassword()
        {
            RegisterModel register = new RegisterModel();
            return View(register);
        }

        [HttpPost]
        public ActionResult RecoverPassword(RegisterModel register, FormCollection collection)
        {
            MembershipUser user = Membership.GetUser(register.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "The User name could not be found, please check.");
                return View(register);
            }
            else
            {
                Guid newUserId = (Guid)user.ProviderUserKey;
                if (user.IsLockedOut)
                {
                    user.UnlockUser();
                }
                string newPassword = user.ResetPassword();

                EmailMessage em = new EmailMessage();
                em.Subject = "Your password has been reset.";
                em.To = user.Email;
                em.From = "admin@smchsg.com";

                //em.Message = "Your password has been reset, <%UserName%>! " + Environment.NewLine + Environment.NewLine + "According to our records, you have requested that your password be reset. Your new password is: <%Password%>" + Environment.NewLine + Environment.NewLine + "If you have any questions or trouble logging on please contact a site administrator." + Environment.NewLine + Environment.NewLine + "Thank you!";
                em.Message = "Dear <%UserName%> " + "\r\n" + "\r\n" + "According to our records, you have requested a password request for your account at www.smchsg.com. Your new password is: <%Password%>" + "\r\n" + "\r\n" + "Thank you!";

                em.Message = em.Message.Replace("<%Password%>", newPassword);
                em.Message = em.Message.Replace("<%UserName%>", register.UserName);

                EmailService es = new EmailService();
                es.SendMessage(em);

                return RedirectToAction("RecoverPasswordConfirmation", new {email = user.Email });
            }
 
        }

        public ActionResult RecoverPasswordConfirmation(string email)
        {
            ViewData["Email"] = email;
            return View();
        }

        protected void ValidateRegister(RegisterModel register)
        {
            if (string.IsNullOrEmpty(register.Name))
            {
                register.Name = register.UserName;
            }
            string trimmedUserName = register.UserName.Trim();
            if (register.UserName.Length != trimmedUserName.Length)
            {
                // Show the error message
                ModelState.AddModelError("", "The username cannot contain leading or trailing spaces.");
            }
            else
            {
                // Username is valid, make sure that the password does not contain the username
                if (register.Password.IndexOf(register.UserName, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    // Show the error message
                    ModelState.AddModelError("", "The username may not appear anywhere in the password.");
                }
            }
        }



        // **************************************
        // URL: /Account/Register
        // **************************************

        public void SetRoles(string role)
        {
            string[] sValues = new string[20];
            string[] roles = Roles.GetAllRoles();
            int j = 0;
            for (int i = 0; i < roles.Count(); i++)
            {
                if (roles[i] == role) //"Initiate")
                {
                    sValues[j++] = ((i + 1).ToString());
                }
            }
            ModelState.SetModelValue("RoleChecks", new ValueProviderResult(sValues, "", CultureInfo.InvariantCulture));
        }

        public ActionResult Register(bool initiateOnly)
        {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            ViewData["InitiateTypes"] = _entities.InitiateTypes.ToList();

            string[] roles = Roles.GetAllRoles();
            RegisterModel register = new RegisterModel();
            if (initiateOnly)
            {
                register.InitiateTypeID = 6;
                SetRoles("Initiate");
            }
            else{
                register.InitiateTypeID = 5;
                SetRoles("Guest");
            }
            //register.Email = "a@a.com";
            //register.UserName = "[please input your User Name]";

            TempData["initiateOnly"] = initiateOnly;

             return View(register);
        }

        [HttpPost]
        public ActionResult Register(RegisterModel register, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                if (string.IsNullOrEmpty(register.Name))
                {
                    register.Name = register.UserName;
                }
                register.UserName = register.UserName.Trim();
                register.Email = register.Email.Trim();

                MembershipCreateStatus createStatus = MembershipService.CreateUser(register.UserName, register.Password, register.Email, false);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    string[] roles = { "Guest" };
                    if (Roles.IsUserInRole("Administrator"))
                    {
                        roles = (collection.GetValues("RoleChecks")).ToArray();
                        string[] allRoles = Roles.GetAllRoles();
                        for (int i = 0; i < roles.Count(); i++)
                        {
                            int j = int.Parse(roles[i]) - 1;
                            roles[i] = allRoles[j];
                        }
  
                        // Approve the user
                        MembershipUser newUser = Membership.GetUser(register.UserName);
                        newUser.IsApproved = true;
                        Membership.UpdateUser(newUser);
                    }
                    else
                    {
                        string urlBase = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath;
                        RegisterSendingMail(register.UserName, urlBase);
                    }
                    register.Role = roles;

                    Roles.AddUserToRoles(register.UserName, register.Role);

					UpdateMemberInfo(register);

					//if (register.InitiateTypeID == 1 || register.InitiateTypeID == 2)
					//{
					//    //int temp = register.InitiateTypeID;
					//    //register.InitiateTypeID = 5;
					//    //UpdateMemberInfo(register);
					//    //register.InitiateTypeID = temp;
					//    TempData["register"] = register;
					//    return RedirectToAction("Create", "MemberInfo");
					//}
                     return RedirectToAction("RegisterConfirmation", new { initiate = false });
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            ViewData["InitiateTypes"] = _entities.InitiateTypes.ToList();

            return View(register);
        }

		public string RegisterOrdinaryMember(string userName, string email, ref Guid memberID)
		{
			//Guid memberID = Guid.Empty;
			string errorString = null;

			if (ModelState.IsValid)
			{
				// Attempt to register the user
				if (userName.Contains(','))
				{
					int index = userName.IndexOf('.');
					userName = userName.Substring(0, index);
				}
				if (string.IsNullOrEmpty(email))
				{
					email = "password@smchsg.com";
				}
				userName = userName.Trim();
				email = email.Trim();
				string password = "chinghai";

				SetRoles("Initiate");

				MembershipCreateStatus createStatus = MembershipService.CreateUser(userName, password, email, false);

				if (createStatus == MembershipCreateStatus.Success)
				{
					// Approve the user
					MembershipUser newUser = Membership.GetUser(userName);
					newUser.IsApproved = true;
					Membership.UpdateUser(newUser);

					string[] roles = { "initiate" };
					Roles.AddUserToRoles(userName, roles);

					memberID = (Guid)newUser.ProviderUserKey;
				}
				else
				{
					ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
					errorString = AccountValidation.ErrorCodeToString(createStatus);
				}
				
			}
			return errorString;
		}

        public void RegisterSendingMail(string userName, string urlBase)
        {
            // Get the UserId of the just-added user
            MembershipUser newUser = Membership.GetUser(userName);
            Guid newUserId = (Guid)newUser.ProviderUserKey;

            // Determine the full verification URL (i.e., http://smchsg.com/Verification.aspx?ID=...)
            // string urlBase = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath;
            string verifyUrl = "Account/Verification?userId=" + newUserId.ToString();
            string fullUrl = urlBase + verifyUrl;

            EmailMessage em = new EmailMessage();
            em.Subject = "Welcome to SMCHSG.COM! Please activate your account.";
            em.To = newUser.Email;
            em.From = "admin@smchsg.com";

            em.Message = "Hello <%UserName%>! Welcome aboard. " + Environment.NewLine + Environment.NewLine + " Your new account is almost ready, but before you can login you must first visit: " + Environment.NewLine + " <%VerificationUrl%> " + Environment.NewLine + Environment.NewLine + " Once you have visited the verification URL, you will be redirected to the login page. " + Environment.NewLine + Environment.NewLine + " If you have any problems or questions, please reply to this email. " + Environment.NewLine + Environment.NewLine + "Thanks!";

            em.Message = em.Message.Replace("<%VerificationUrl%>", fullUrl);
            em.Message = em.Message.Replace("<%UserName%>", userName);
            //em.Message = em.Message.Replace(Environment.NewLine, "<br />");

            EmailService es = new EmailService();
            es.SendMessage(em);

        }

        public ActionResult RegisterConfirmation(bool initiate)
        {
            ViewData["initiate"] = initiate;
            return View();
        }


        //public bool MemberRegister(RegisterModel model, string[] roles)
        //{
        //    bool result = false;
        //    if (ModelState.IsValid)
        //    {
        //        if (MembershipService == null)
        //        {
        //            MembershipService = new AccountMembershipService();
        //        }
        //        // Attempt to register the user
        //        MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserName, model.Password, model.Email, true);

        //        if (createStatus == MembershipCreateStatus.Success)
        //        {
        //            UpdateMemberInfo(model);
        //            Roles.AddUserToRoles(model.UserName, roles);
        //             result = true;
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
        //        }
               
        //    }
        //    return result;
        //}

        public void UpdateMemberInfo(RegisterModel register)
        {

            MemberInfo memberInfo = new MemberInfo();
            memberInfo.InitiateTypeID = register.InitiateTypeID;
            memberInfo.Name = register.Name;
 
           MembershipUser mUser = Membership.GetUser(register.UserName);
           memberInfo.MemberID = (Guid)(mUser.ProviderUserKey);
           _entities.AddToMemberInfos(memberInfo);
  
            _entities.SaveChanges();
        }

        public void UpdateEmail(String newEmail, Guid memberID)
        {
            MembershipUser user = Membership.GetUser(memberID);
            if (newEmail != user.Email)
            {
                user.Email = newEmail;
                Membership.UpdateUser(user);
            }

        }


        public ActionResult Verification(Guid userId)
        {
             MembershipUser usr = Membership.GetUser(userId);
             if (usr == null)
                 ViewData["Status"] = false;
             else
             {
                 // Approve the user
                 usr.IsApproved = true;
                 Membership.UpdateUser(usr);

                 ViewData["Status"] = true;
             }
            return View();
        }


        //#region MyRegion
        //  private bool CheckPassword(string enteredPassword, string storedPassword)
        //{

        //    bool success = false;

        //    switch (PasswordFormat)
        //    {
        //        case MembershipPasswordFormat.Clear:
        //            success = (enteredPassword == storedPassword);
        //            break;

        //        case MembershipPasswordFormat.Encrypted:
        //            success = (enteredPassword == UnEncodePassword(storedPassword));
        //            break;

        //        case MembershipPasswordFormat.Hashed:
        //            success = (EncodePassword(enteredPassword) == storedPassword);
        //            break;

        //        default:
        //            break;
        //    }

        //    return success;

        //}

        //private string EncodePassword(string password)
        //{
        //    string encodedPassword = password;

        //    switch (passwordFormat)
        //    {
        //        case MembershipPasswordFormat.Clear:
        //            break;

        //        case MembershipPasswordFormat.Encrypted:
        //            encodedPassword = Convert.ToBase64String(EncryptPassword(Encoding.Unicode.GetBytes(password)));
        //            break;

        //        case MembershipPasswordFormat.Hashed:
        //            HMACSHA1 hash = new HMACSHA1();
        //            hash.Key = HexToByte(MachineKey.ValidationKey);
        //            encodedPassword = Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));
        //            break;

        //        default:
        //            throw new ProviderException("Unsupported password format.");
        //    }

        //    return encodedPassword;
        //}

        //private string UnEncodePassword(string encodedPassword)
        //{
        //    string password = encodedPassword;

        //    switch (passwordFormat)
        //    {
        //        case MembershipPasswordFormat.Clear:
        //            break;

        //        case MembershipPasswordFormat.Encrypted:
        //            password = Encoding.Unicode.GetString(DecryptPassword(Convert.FromBase64String(password)));
        //            break;

        //        case MembershipPasswordFormat.Hashed:
        //            throw new ProviderException("Cannot unencode a hashed password.");

        //        default:
        //            throw new ProviderException("Unsupported password format.");
        //    }

        //    return password;
        //}

        //private byte[] HexToByte(string hexString)
        //{
        //    byte[] returnBytes = new byte[hexString.Length / 2];
        //    for (int i = 0; i < returnBytes.Length; i++)
        //        returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
        //    return returnBytes;
        //}
        //#endregion




        // **************************************
        // URL: /Account/ChangePassword
        // **************************************

        [Authorize]
        public ActionResult ChangePassword()
        {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {

                    //SMCHDBEntities _entities = new SMCHDBEntities();
                     _entities.SaveChanges();

                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePasswordSuccess
        // **************************************

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        //public ActionResult UpdateEmailAndGendar()
        //{
 
        //    List<initiateEmail> initiateEmails = _entities.initiateEmails.ToList();
        //    List<initiateEmail> NoInList = new List<initiateEmail>();
        //    List<RegisterModel> RemoveSMCHEmail = new List<RegisterModel>();
        //    //List<RegisterModel> AddSMCHEmail = new List<RegisterModel>();

        //    foreach (initiateEmail iEmail in initiateEmails)
        //    {
        //        string temp = iEmail.Name.Replace(',', ' ');
        //        temp = temp.Replace('-', ' ');
        //        temp = temp.Replace("  ", " ");

        //        char[] seperateChar = new char[] { '(', };
        //        string[] uName = temp.Split(seperateChar);
        //        string UserName = uName[0].Trim();

        //        int index = IndexOfMembershipUser(UserName);
        //        if (index >= 0)
        //        {
        //            Guid memberID = (Guid)Membership.GetUser(UserName).ProviderUserKey;
        //            InitiateMemberInfo initiateMemberInfo = _entities.InitiateMemberInfos.Single(a => a.IMemberID == memberID);
        //            if (initiateMemberInfo.GenderID != iEmail.GendarID.Value)
        //            {
        //                initiateMemberInfo.GenderID = iEmail.GendarID.Value;
        //                UpdateModel(initiateMemberInfo, "InitiateMemberInfo");
        //            }
        //            string oldEmail = Membership.GetUser(memberID).Email;
        //            MembershipUser user = Membership.GetUser(memberID);
        //            if (user.Email != iEmail.Email)
        //            {
        //                if (user.Email != "password@smchsg.com")
        //                {
        //                    RegisterModel register = GetRergisterModel(memberID, user);
        //                    RemoveSMCHEmail.Add(register);
        //                }
        //                user.Email = iEmail.Email;
        //                Membership.UpdateUser(user);
        //            }

        //            //iEmail.IsValid = true;
        //            //UpdateModel(iEmail, "initiateEmails");

        //            _entities.SaveChanges();

        //        }
        //        else
        //        {
        //            NoInList.Add(iEmail);
        //        }

        //    }

        //    List<InitiateMemberInfo> initiateMemberInfos = _entities.InitiateMemberInfos.ToList();
        //    int i = 1;
        //    foreach (InitiateMemberInfo im in initiateMemberInfos)
        //    {
        //        MembershipUser user = Membership.GetUser(im.IMemberID);
        //        RegisterModel register = GetRergisterModel(im.IMemberID, user);
        //        if (im.MemberFeeExpiredDate != null && im.MemberFeeExpiredDate <= new DateTime(2004, 12, 31))
        //        {
        //            if (user.Email != "password@smchsg.com" && user.Email.EndsWith("@smchsg.com"))
        //            {
        //                RemoveSMCHEmail.Add(register);

        //                user.Email = "password@smchsg.com";
        //                Membership.UpdateUser(user);
        //            }
        //            else
        //            {
        //                continue;
        //            }
        //        }
        //        else
        //        {
        //            if (user.Email == "password@smchsg.com")
        //            {
        //                string name = System.Text.RegularExpressions.Regex.Replace(user.UserName.ToLower(), @"\s", "");
        //                user.Email = name + "@smchsg.com";
        //                Membership.UpdateUser(user);
        //                register.Name = "Need to add";
        //                RemoveSMCHEmail.Add(register);
        //            }
        //        }
              
        //    }

        //    var SMCHEmail = RemoveSMCHEmail.OrderBy(a => a.Name).ThenBy(a=>a.UserName);

        //    //return View(NoInList);
        //    return View("UpdateEmailAndGendar1", SMCHEmail);
        //}

        private static RegisterModel GetRergisterModel(Guid memberID, MembershipUser user)
        {
            RegisterModel register = new RegisterModel();
            register.UserName = Membership.GetUser(memberID).UserName;
            register.Email = user.Email;
            return register;
        }

        public ActionResult CreateUsers4RealMemberInfo()
        {

            //// RandomGeneratePassword4Users
            //List<Guid> iMemberGuids = (from r in _entities.InitiateMemberInfos select r.IMemberID).ToList();
            //foreach (Guid gid in iMemberGuids)
            //{
            //    MembershipUser user = Membership.GetUser(gid);
            //    string[] roles = Roles.GetRolesForUser(user.UserName);
            //    if(!roles.Contains("Administrator"))
            //    {
            //        string oldp = user.GetPassword();
            //        user.ResetPassword();
            //        string newp = user.GetPassword();
            //    }
            //}


            //// Create email
            //MembershipUserCollection muc = Membership.GetAllUsers();
            //int i = 1;
            //foreach (MembershipUser mu in muc)
            //{
            //    string name = System.Text.RegularExpressions.Regex.Replace(mu.UserName.ToLower(), @"\s", "");

            //    string Email = name + "@smchsg.com";
            //    if (i++ > 250 && (!(Roles.IsUserInRole(mu.UserName, "Administrator") || Roles.IsUserInRole(mu.UserName, "Contact Person") || Roles.IsUserInRole(mu.UserName, "Loving Hut Singapore") || Roles.IsUserInRole(mu.UserName, "MemberAdmin") || Roles.IsUserInRole(mu.UserName, "SOS Team"))) || name.Contains('/'))
            //        Email = "password@smchsg.com";
            //    UpdateEmail(Email, (Guid)mu.ProviderUserKey);
            //}


            //foreach (var item in _entities.RealMemberInfoes.ToList())
            //{
            //    RegisterModel register = GetRealMemberRegisterInfo(item);

            //    int index = IndexOfMembershipUser(register.UserName);
            //    if (index >= 0)
            //    {
            //        Guid memberID = (Guid)Membership.GetUser(register.UserName).ProviderUserKey;
            //        InitiateMemberInfo initiateMemberInfo = _entities.InitiateMemberInfos.Single(a => a.IMemberID == memberID);
            //        GetRealInitiateMemberInfo(item, initiateMemberInfo);

            //        UpdateModel(initiateMemberInfo, "InitiateMemberInfo");
            //        _entities.SaveChanges();

            //        // get latest info:
            //        //MemberInfo memberInfo = _entities.MemberInfos.Single(a => a.MemberID == memberID);
            //        //if (!string.IsNullOrEmpty(memberInfo.ContactNo))
            //        //{
            //        //    register.ContactNo = memberInfo.ContactNo;
            //        //}
            //        UpdateMeberInfo(register, memberID);
            //    }
            //    else
            //    {
            //        InitiateMemberInfo initiateMemberInfo = new InitiateMemberInfo();
            //        GetRealInitiateMemberInfo(item, initiateMemberInfo);
            //        // get latest info:
            //        Guid oldMemberID = ResetLatestValues(item, initiateMemberInfo, ref index);
  
            //        if (!MemberRegister(register, register.Role))
            //        {
            //            throw new Exception();
            //        }

            //        MembershipUser mu = Membership.GetUser(register.UserName);
            //        initiateMemberInfo.IMemberID = (Guid)(mu.ProviderUserKey);
            //        _entities.AddToInitiateMemberInfos(initiateMemberInfo);
            //        _entities.SaveChanges();


            //        if (index >= 0)
            //        {
            //            RemoveAndModifyDatabases(initiateMemberInfo.IMemberID, oldMemberID);
            //        }

            //    }
            //}

            return View();
        }

		//public ActionResult RandomGeneratePassword4Users()
		//{
		//    List<Guid> iMemberGuids = (from r in _entities.InitiateMemberInfos select r.IMemberID).ToList();
		//    foreach (Guid gid in iMemberGuids)
		//    {
		//        if (!Roles.IsUserInRole("Administrator"))
		//        {
		//            MembershipUser user = Membership.GetUser(gid);
		//            string newpassword = user.ResetPassword();
		//        }
		//    }
		//    return View();
		//}
        
        
		//private void RemoveAndModifyDatabases(Guid newMemberID, Guid oldMemberID)
		//{
		//    List<EventRegistration> eventRegistrations = (from r in _entities.EventRegistrations where r.MemberID == oldMemberID select r).ToList();
		//    foreach (EventRegistration eventRegistration in eventRegistrations)
		//    {
		//        eventRegistration.MemberID = newMemberID;
		//        UpdateModel(eventRegistration, "EventRegistration");
		//        _entities.SaveChanges();
		//    }

		//    InitiateMemberInfo initiateMemberInfo = _entities.InitiateMemberInfos.Single(a => a.IMemberID == oldMemberID);
		//    MemberInfo memberInfo = _entities.MemberInfos.Single(a => a.MemberID == oldMemberID);

		//    _entities.DeleteObject(memberInfo);
		//    _entities.DeleteObject(initiateMemberInfo);
		//    _entities.SaveChanges();

		//    string userName = Membership.GetUser(oldMemberID).UserName;
		//    Membership.DeleteUser(userName, true);
		//}

		//private Guid ResetLatestValues(RealMemberInfo item, InitiateMemberInfo iMemberInfo, ref int index)
		//{
		//    Guid oldMemberID = new Guid();

		//    string uName = GetHeadName(item.Name.Trim());
		//    index = IndexOfMembershipUser(uName);
		//    if (index >= 0)
		//    {
		//        oldMemberID = (Guid)Membership.GetUser(uName).ProviderUserKey;
		//        InitiateMemberInfo initiateMemberInfo = _entities.InitiateMemberInfos.Single(a => a.IMemberID == oldMemberID);
		//        string IDCardNo = initiateMemberInfo.IDCardNo;
		//        string contactNo = initiateMemberInfo.ContactNo;

		//        if (!string.IsNullOrEmpty(IDCardNo) && IDCardNo != iMemberInfo.IDCardNo)
		//        {
		//            iMemberInfo.IDCardNo = IDCardNo;
		//        }
		//        if (!string.IsNullOrEmpty(contactNo) && contactNo != iMemberInfo.ContactNo)
		//        {
		//            iMemberInfo.ContactNo = contactNo;
		//        }
		//    }
		//    return oldMemberID;
		//}

        private static int IndexOfMembershipUser(string userName)
        {
            MembershipUserCollection muc = Membership.GetAllUsers();
            List<string> UserNames = new List<string>();
            foreach (MembershipUser user in muc)
            {
                UserNames.Add(user.UserName.ToLower());
            }
            int index = UserNames.IndexOf(userName.ToLower());
            return index;
        }
        
		//private static RegisterModel GetRealMemberRegisterInfo(RealMemberInfo item)
		//{
		//     RegisterModel register = new RegisterModel();
            
		//    register.Name = item.Name.Trim();
 
		//    register.UserName = GetUserName(item);

		//    if (string.IsNullOrEmpty(item.Email))
		//    {
		//        //register.Email = "a@a.com";
		//        // use the regular expression pattern "\s", which matches any whitespace character including the space, tab, linefeed and newline.
		//        string name = System.Text.RegularExpressions.Regex.Replace( register.UserName.ToLower(), @"\s", "" );
		//        register.Email = name + "@smchsg.com";
		//    }
		//    else
		//    {
		//        register.Email = item.Email.Trim();
		//    }
            
		//    register.InitiateTypeID = 1;
		//    register.Password = "chinghai";
		//    register.ConfirmPassword = "chinghai";

		//    string[] roles = { "Initiate" };
		//    register.Role = roles;
  
		//    return register;
		//}

		//private static string GetUserName(RealMemberInfo item)
		//{
		//    string username = null;// item.ICOrPassportNo;

		//    if (string.IsNullOrEmpty(username))
		//    {
		//        username = GetHeadName(item.Name.Trim());
		//    }

		//    return username;
		//}

        private static string GetHeadName(string name)
        {
            char[] seperateChar = new char[] { ',', '@', '-' };
            string[] uName = name.Split(seperateChar);
            return uName[0].Trim();
        }

		//private static void GetRealInitiateMemberInfo(RealMemberInfo item, InitiateMemberInfo iMemberInfo)
		//{
		//    //InitiateMemberInfo iMemberInfo = new InitiateMemberInfo();
		//    ////if (item.No.First() == 'A')
		//    ////{
		//    ////    iMemberInfo.MemberTypeID = 2;
		//    ////    iMemberInfo.MemberNo = int.Parse(item.No.Substring(1));
		//    ////}
		//    ////else
		//    ////{
		//    ////    iMemberInfo.MemberNo = int.Parse(item.No);
		//    ////}

		//    if (string.IsNullOrEmpty(iMemberInfo.IDCardNo) && !string.IsNullOrEmpty(item.IDCardNo))
		//    {
		//        iMemberInfo.IDCardNo = item.IDCardNo;
		//    }
		//    if (string.IsNullOrEmpty(iMemberInfo.ContactNo) && !string.IsNullOrEmpty(item.ContactNo))
		//    {
		//        iMemberInfo.ContactNo = item.ContactNo;
		//    }
		//    else if (string.IsNullOrEmpty(iMemberInfo.ContactNo) && string.IsNullOrEmpty(item.ContactNo))
		//    {
		//        iMemberInfo.ContactNo = "11111111";
		//    }

		//    iMemberInfo.MemberTypeID = item.MemberTypeID;
		//    iMemberInfo.MemberNo = item.MemberNo;

		//    DateTime expiredDate = GetCurrentMonthLastDay(DateTime.Today);
		//    iMemberInfo.MemberFeePayByID = 1;
		//    if (item.ExpiredDate.ToLower().Trim() != "giro")
		//    {
		//        string eDate = item.ExpiredDate.Trim();
		//        if (eDate.EndsWith("05-A"))
		//        {
		//            eDate = eDate.Substring(0, eDate.Length - 4) + "-05";
		//        }
		//        char[] seperateChar = new char[] { '\'', '-' };
		//        string[] yearMonth = eDate.Split(seperateChar);

		//        if (yearMonth[1].Trim().Length == 2)
		//        {
		//            int year = int.Parse(yearMonth[1]);
		//            if (year > 50)
		//            {
		//                yearMonth[1] = "19" + yearMonth[1].Trim();
		//            }
		//            else
		//            {
		//                yearMonth[1] = "20" + yearMonth[1].Trim();
		//            }
		//        }

		//        string temp = "1 " + yearMonth[0].Trim() + " " + yearMonth[1];

		//        expiredDate = DateTime.Parse(temp);
		//        iMemberInfo.MemberFeePayByID = 5;
		//    }
		//    iMemberInfo.MemberFeeExpiredDate = GetCurrentMonthLastDay(expiredDate);
 
		//    //iMemberInfo.ICOrPassportNo = GetUserName(item);
 
		//    iMemberInfo.NameInNative = item.NameInNative;
		//    //iMemberInfo.IDCardNo = item.IDCardNo;
		//    iMemberInfo.PlaceOfBirth = item.PlaceOfBirth;
		//    iMemberInfo.Remark = item.Remark;
		//    iMemberInfo.Occupation = item.Occupation;
		//    iMemberInfo.EducationLevel = item.EducationLevel;
		//    iMemberInfo.SpecialSkill = item.SpecialSkill;
		//    iMemberInfo.MemberApplyDate = item.MemberApplyDate;
		//    iMemberInfo.MemberEffectiveStartDate = item.MemberEffectiveStartDate;

		//    if (item.ICOrPassportNo == null)
		//    {
		//        iMemberInfo.ICOrPassportNo = "S1234567A";
		//    }
		//    else
		//    {
		//        iMemberInfo.ICOrPassportNo = item.ICOrPassportNo;
		//    }
            
		//    if (item.DateOfBirth == null)
		//    {
		//        iMemberInfo.DateOfBirth = DateTime.Today.AddYears(-40);
		//    }
		//    else
		//    {
		//        iMemberInfo.DateOfBirth = item.DateOfBirth;
		//    }

		//    if (item.DateOfInitiation == null || item.DateOfInitiation == DateTime.MinValue)
		//    {
		//        iMemberInfo.DateOfInitiation = DateTime.Today.AddYears(-15).Date;
		//    }
		//    else
		//    {
		//        iMemberInfo.DateOfInitiation = (DateTime)item.DateOfInitiation;
		//    }

		//    if (item.NationalityID == null)
		//    {
		//        iMemberInfo.NationalityID = 159; // Singaporean
		//    }
		//    else
		//    {
		//        iMemberInfo.NationalityID = (int)item.NationalityID;
		//    }

		//    if (item.GenderID == null)
		//    {
		//        iMemberInfo.GenderID = 2; // female
		//    }
		//    else
		//    {
		//        iMemberInfo.GenderID = (int)item.GenderID;
		//    }

		//    if (item.RaceID == null)
		//    {
		//        iMemberInfo.RaceID = 1; // chinese
		//    }
		//    else
		//    {
		//        iMemberInfo.RaceID = (int)item.RaceID;
		//    }

		//    if (item.EmploymentStatusID == null)
		//    {
		//        iMemberInfo.EmploymentStatusID = 1; // employed
		//    }
		//    else
		//    {
		//        iMemberInfo.EmploymentStatusID = (int)item.EmploymentStatusID;
		//    }

		//    if (item.PlaceOfInitiation == null)
		//    {
		//        iMemberInfo.PlaceOfInitiation = "Singapore";
		//    }
		//    else
		//    {
		//        iMemberInfo.PlaceOfInitiation = item.PlaceOfInitiation;
		//    }

		//    if (item.Address == null)
		//    {
		//        iMemberInfo.Address = "...";
		//    }
		//    else
		//    {
		//        iMemberInfo.Address = item.Address;
		//    }

		//    iMemberInfo.IsActive = true;

		// }

        private static DateTime GetCurrentMonthLastDay(DateTime expiredDate)
        {
            DateTime nextMonthFirstDay = new DateTime();
            if (expiredDate.Month < 12)
            {
                nextMonthFirstDay = new DateTime(expiredDate.Year, expiredDate.Month + 1, 1);
            }
            else
            {
                nextMonthFirstDay = new DateTime(expiredDate.Year + 1, 1, 1);
            }
            expiredDate = nextMonthFirstDay.AddDays(-1).Date;
            return expiredDate;
        }


        //public ActionResult CreateUsers4MemberInfo()
        //{
        //    SMCHSGManager.Models.SMCHDBEntities _entities = new SMCHSGManager.Models.SMCHDBEntities();
  
        //    MembershipUserCollection muc = Membership.GetAllUsers();
        //    foreach (MembershipUser user in muc)
        //    {
        //        SMCHSGManager.Models.MemberInfo mi = new SMCHSGManager.Models.MemberInfo();
        //        string userName = user.UserName;

        //        mi.Name = _entities.MemberInfo1s.SingleOrDefault(a=>a.ICorPassportNo == userName).Name;
        //        mi.MemberID = ((Guid)(user.ProviderUserKey));
        //        mi.Initiation = (bool)true;
        //        //mi.Address = 
        //        mi.ContactNo = _entities.MemberInfo1s.SingleOrDefault(a => a.ICorPassportNo == userName).ContactNo;
        //        _entities.AddToMemberInfos(mi);

        //        InitiateMemberInfo imi = new InitiateMemberInfo();
        //        imi.IMemberID = ((Guid)(user.ProviderUserKey));
        //        imi.DateOfInitiation = new DateTime(1995, 1, 1);
        //        imi.DateOfBirth = new DateTime(1950, 1, 1);
        //        imi.IDCardNo = _entities.MemberInfo1s.SingleOrDefault(a => a.ICorPassportNo == userName).IDCardNo;
        //        imi.MemberTypeID = _entities.MemberInfo1s.SingleOrDefault(a => a.ICorPassportNo == userName).MemberTypeID;
        //        imi.MemberNo = _entities.MemberInfo1s.SingleOrDefault(a => a.ICorPassportNo == userName).MemberNo;
        //        imi.Remark = _entities.MemberInfo1s.SingleOrDefault(a => a.ICorPassportNo == userName).Remark;
        //        imi.NameInNative = _entities.MemberInfo1s.SingleOrDefault(a => a.ICorPassportNo == userName).NameInNative;
        //        imi.PlaceOfBirth = _entities.MemberInfo1s.SingleOrDefault(a => a.ICorPassportNo == userName).PlaceOfBirth;
        //        imi.IsActive = _entities.MemberInfo1s.SingleOrDefault(a => a.ICorPassportNo == userName).IsActive;

        //        _entities.AddToInitiationMemberInfos(imi);
        //    }
        //    _entities.SaveChanges();
        //    return View();
        //}









        //public ActionResult CreateUsers4MemberInfo()
        //{
        //    if (ModelState.IsValid)
        //    {
        //        foreach (MembershipUser mu in Membership.GetAllUsers())
        //        {
        //            Membership.DeleteUser(mu.UserName, true);
        //        }

        //        SMCHDBEntities _entities = new SMCHDBEntities();
 
        //        foreach (MemberInfo mi in _entities.MemberInfos.ToList())
        //        {
                   
        //           MembershipCreateStatus createStatus = CreateAccountModel(mi);
        //           if (!ModelState.IsValid)
        //           {
        //               //if (ViewData["errorMessage"] == null)
        //               //{
        //               //    ViewData["errorMessage"] = mi.ICorPassportNo + " didn't created!";
        //               //}
        //               //else
        //               //{
        //               //    ViewData["errorMessage"] += mi.ICorPassportNo + " didn't created!";
        //               //}
        //           }
        //        }
        //    }
        //    return View();
        //}

        //public MembershipCreateStatus UpdateAccountModel(MemberInfo mi, string previousName, string previousEmail)
        //{
        //    MembershipCreateStatus createStatus = MembershipCreateStatus.Success ;

        //    if (ModelState.IsValid)
        //    {
        //        if (mi.ICorPassportNo == previousName)
        //        {
        //            Membership.DeleteUser(previousName, true);
        //         }

        //        createStatus = CreateAccountModel(mi);
                
        //        if (mi.ICorPassportNo != previousName && ModelState.IsValid)
        //        {
        //            Membership.DeleteUser(previousName, true);
        //        }
        //        else if (mi.ICorPassportNo == previousName && !ModelState.IsValid)
        //        {

        //        }

        //    }
        //    return createStatus;
        //}

        //public MembershipCreateStatus CreateAccountModel(MemberInfo mi)
        //{

            //if (MembershipService == null)
            //{
            //    MembershipService = new AccountMembershipService();
            //}

            //MembershipCreateStatus createStatus = MembershipService.CreateUser(mi.ICorPassportNo, mi.Password, mi.Email);

            //if (createStatus != MembershipCreateStatus.Success)
            //{
            //    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
            //}
            //else
            //{
            //    Roles.AddUserToRole(mi.ICorPassportNo, mi.RoleType.Name);
            //}

        //   return createStatus;
        //}


    }
}
