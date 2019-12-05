using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMCHSGManager.Models;
using SMCHSGManager.ViewModel;
using System.Web.Security;

namespace SMCHSGManager.Controllers
{
    public class InitiateVisitorController : Controller
    {

		private SMCHDBEntities _entities = new SMCHDBEntities();

        //
        // GET: /InitiateVisitor/

		[Authorize]
		public ActionResult Index(bool? showAll)
        {
			DateTime now = DateTime.Now.ToUniversalTime().AddHours(8);
            DateTime today = new DateTime(now.Year, now.Month, now.Day);

			var viewModel = (from r in _entities.InitiateVisitors
						 where !r.DateTo.HasValue ||
                         r.DateTo.HasValue && r.DateTo.Value >= today
						 select r).OrderByDescending(a => a.ID).ToList();

			if ((User.IsInRole("DP Admin") || User.IsInRole("SuperAdmin")) && showAll.HasValue && showAll.Value)
			{
				viewModel = _entities.InitiateVisitors.OrderByDescending(a => a.ID).ToList();
			}
            return View(viewModel);
        }

        //
        // GET: /InitiateVisitor/Details/5

		//public ActionResult Details(int id)
		//{
		//    return View();
		//}

        //
        // GET: /InitiateVisitor/Create

		[Authorize(Roles = "Administrator, DP Admin")]
		public ActionResult Create()
        {
			var viewModel = new InitiateVisitorViewModel
			{
				 InitiateVisitor = new InitiateVisitor (),
				 Genders = _entities.Genders.ToList(),
			};
			viewModel.InitiateVisitor.DateFrom = DateTime.Today.Date;
			viewModel.InitiateVisitor.DateOfInitiation = new DateTime(2010, 1, 1);
			viewModel.InitiateVisitor.DateTo = DateTime.Today.AddMonths(6).Date;
			//viewModel.InitiateVisitor.FromWhere = "...";
			viewModel.InitiateVisitor.GenderID = 1;

			return View(viewModel);
        } 

        //
        // POST: /InitiateVisitor/Create

		//[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator, DP Admin")]
		public ActionResult Create(FormCollection collection, InitiateVisitor initiateVisitor)
        {
            try
            {
				_entities.AddToInitiateVisitors(initiateVisitor);
				_entities.SaveChanges();

                GenerateEmailMessage(initiateVisitor);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

		public void GenerateEmailMessage(InitiateVisitor initiateVisitor)
		{
			EmailMessage em = new EmailMessage();

			em.Subject = "New initiate Visitor: " + initiateVisitor.Name;
			em.Message = "Name : " + initiateVisitor.Name;
 
            if (initiateVisitor.GenderID == 2)
            {
                em.Subject += " (Sister)";
				em.Message += " (Sister) ";
            }
            else if(initiateVisitor.GenderID == 1)
            {
                em.Subject += " (Brother)";
				em.Message += " (Brother)";
            }
 
			if (!string.IsNullOrEmpty(initiateVisitor.FromWhere))
			{
				em.Subject += ' ' + initiateVisitor.FromWhere;
				em.Message += "\r\nFromWhere " + initiateVisitor.FromWhere;
			}
 
			em.Subject += ' ' + string.Format("{0: d-MMM-yyyy}", initiateVisitor.DateFrom);
            em.Message = "\r\nFrom: " + string.Format("{0: d-MMM-yyyy}", initiateVisitor.DateFrom);

            if (initiateVisitor.DateTo.HasValue)
            {
                em.Message += " To: " + string.Format("{0: d-MMM-yyyy}", initiateVisitor.DateTo.Value);
            }

            em.Message += "\r\nDate of Initiation: " + string.Format("{0: d-MMM-yyyy}", initiateVisitor.DateOfInitiation);
            if(!string.IsNullOrEmpty(initiateVisitor.IDCardNo))
            {
                em.Message += "\r\nID Card No: " + initiateVisitor.IDCardNo;
            }
            if (!string.IsNullOrEmpty(initiateVisitor.Remark))
            {
                em.Message += "\r\nRemark: " + initiateVisitor.Remark;
            }

			//em.From = Membership.GetUser().Email;
            em.From = "admin@smchsg.com";

            string CPName = Roles.GetUsersInRole("Contact Person").FirstOrDefault();
            string CPEmail = Membership.GetUser(CPName).Email;

            string localCenterEmail = _entities.AshramAndCenterInfos.SingleOrDefault(a => a.ID == 1).Email; // "Singapore Center"

            string DPName = Roles.GetUsersInRole("DP Admin").FirstOrDefault();
            string DPEmail = Membership.GetUser(DPName).Email;

            em.To = DPEmail;
            em.cc = localCenterEmail + ", " + CPEmail;
            em.bcc = "admin@smchsg.com";

			EmailService es = new EmailService();
			es.SendMessage(em);
		}     

	
        //
        // GET: /InitiateVisitor/Edit/5

		[Authorize(Roles = "Administrator, DP Admin")]
		public ActionResult Edit(int id)
        {
			var viewModel = new InitiateVisitorViewModel
			{
				InitiateVisitor = _entities.InitiateVisitors.Single(a=>a.ID == id),
				Genders = _entities.Genders.ToList(),
			};

			return View(viewModel);
        }

        //
        // POST: /InitiateVisitor/Edit/5

        [HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator, DP Admin")]
		public ActionResult Edit(int id, FormCollection collection)
        {
			InitiateVisitor initiateVisitor = _entities.InitiateVisitors.Single(a => a.ID == id);
            try
            {
				UpdateModel(initiateVisitor, "InitiateVisitor");
				_entities.SaveChanges(); 

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /InitiateVisitor/Delete/5

		[Authorize(Roles = "Administrator, DP Admin")]
		public ActionResult Delete(int id)
        {
			var initateVisitor = _entities.InitiateVisitors.Single(a => a.ID == id);
			return View(initateVisitor);
        }

        //
        // POST: /InitiateVisitor/Delete/5

        [HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator, DP Admin")]
		public ActionResult Delete(int id, FormCollection collection)
        {
			if (id != 0)
			{
				var initateVisitor = _entities.InitiateVisitors.Single(a => a.ID == id);

				_entities.DeleteObject(initateVisitor);

				_entities.SaveChanges();
			}
			return View("Deleted");
        }
    }
}
