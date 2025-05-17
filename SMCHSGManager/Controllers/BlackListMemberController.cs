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
    public class BlackListMemberController : Controller
    {
		private SMCHDBEntities _entities = new SMCHDBEntities();
		//
        // GET: /BlackListMember/

		[Authorize]
		public ActionResult Index()
        {
            return View(_entities.BlackListMembers.ToList());
        }

        //
        // GET: /BlackListMember/Create

		[Authorize(Roles = "Administrator")]
		public ActionResult Create()
        {
			BlackListMemberViewModel viewModel = new BlackListMemberViewModel
			{
				BlackListMember = new BlackListMember(),
				IDCardTypes = _entities.IDCardTypes.Where(a=>a.ID != 1).ToList(),
			};

			viewModel.MemberInfos = GetMemberNameSelectList(Guid.Empty);

            viewModel.BlackListMember.DateFrom = DateTime.Today.ToUniversalTime().AddHours(8);

			return View(viewModel);
        }

		public static List<SelectListItem> GetMemberNameSelectList(Guid selectID)
		{
            SMCHDBEntities _entities = new SMCHDBEntities();
			List<MemberInfo> memberInfos = (from r in _entities.MemberInfos
											where r.MemberNo.HasValue
											orderby r.MemberNo
											select r).ToList();
			List<SelectListItem> si = new List<SelectListItem>();
			foreach (MemberInfo selectItem in memberInfos)
			{
				SelectListItem item = new SelectListItem { Text = selectItem.MemberNo.Value.ToString() + ' ' + selectItem.Name, Value = selectItem.MemberID.ToString() };
				if (selectID == selectItem.MemberID)
				{
					item.Selected = true;
				}
				si.Add(item);
			}
			return si;
		} 

        //
        // POST: /BlackListMember/Create

		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
		public ActionResult Create(FormCollection collection, BlackListMember BlackListMember)
        {
            try
            {
				//BlackListMember.MemberD = Guid.Parse(collection.GetValues("BlackListMember.MemberID")[0]);
				_entities.AddToBlackListMembers(BlackListMember);
				_entities.SaveChanges();

				return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /BlackListMember/Edit/5

		[Authorize(Roles = "Administrator")]
		public ActionResult Edit(int id)
        {
			BlackListMemberViewModel viewModel = new BlackListMemberViewModel
			{
				BlackListMember = _entities.BlackListMembers.SingleOrDefault(a=>a.ID == id),
				IDCardTypes = _entities.IDCardTypes.Where(a => a.ID != 1).ToList(),
			};
			viewModel.MemberInfos = GetMemberNameSelectList(viewModel.BlackListMember.MemberD);

			return View(viewModel);
		}

        //
        // POST: /BlackListMember/Edit/5

		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
		public ActionResult Edit(int id, FormCollection collection)
        {
			var BlackListMember = _entities.BlackListMembers.Single(a => a.ID == id);
			try
			{
				UpdateModel(BlackListMember, "BlackListMember");
				_entities.SaveChanges();

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

        //
        // GET: /BlackListMember/Delete/5

		[Authorize(Roles = "Administrator")]
		public ActionResult Delete(int id)
        {
			var BlackListMembers = _entities.BlackListMembers.Single(a => a.ID == id);
			return View(BlackListMembers);
		}

        //
        // POST: /BlackListMember/Delete/5

		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
		public ActionResult Delete(int id, FormCollection collection)
        {
			if (id != 0)
            {
				BlackListMember BlackListMembers = _entities.BlackListMembers.Single(a => a.ID == id);
				_entities.DeleteObject(BlackListMembers);
                _entities.SaveChanges();
            }
            return View("Deleted");
        }


    }
}
