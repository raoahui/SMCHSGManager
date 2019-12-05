using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMCHSGManager.Controllers
{
    public class HelpController : Controller
    {
        //
        // GET: /Help/

        public ActionResult Index()
        {
            return View();
        }
		public ActionResult RegisterEvent()
		{
			return View();
		}
		
		public ActionResult Login()
		{
			return View();
		}

		public ActionResult ModifyRegistration()
		{
			return View();
		}
		public ActionResult CancelRegistration()
		{
			return View();
		}

		public ActionResult UpdateAccount()
		{
			return View();
		}

		public ActionResult ChangeDate()
		{
			return View();
		}

		public ActionResult UpdateAccountChinese()
		{
			return View();
		}

		public ActionResult DPDuties()
		{
			return View();
		}

		public ActionResult MakeOrder()
		{
			return View();
		}

    }
}
