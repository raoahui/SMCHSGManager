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
    public class LocalRetreatScheduleTemplateController : Controller
    {
        private SMCHDBEntities _entities = new SMCHDBEntities();
        //
        // GET: /LocalRetreatScheduleTemplate/

        // GET: /EventSchedule/ScheduleModelSelect
		[Authorize(Roles = "Administrator")]
        public ActionResult ScheduleModelSelect()
        {
            List<SelectListItem> scheduleModelSelectLists = new List<SelectListItem>();
            List<int> scheduleModels = ((from r in _entities.LocalRetreatScheduleTemplates select r.Model).Distinct().OrderByDescending(a => a)).ToList();
            foreach (int i in scheduleModels)
            {
                SelectListItem item = new SelectListItem { Text = "Model " + i.ToString(), Value = i.ToString() };
                scheduleModelSelectLists.Add(item);
            }
            SelectListItem item1 = new SelectListItem { Text = "New Model", Value = (scheduleModels.Count() + 1).ToString() };
            scheduleModelSelectLists.Add(item1);
            ViewData["LocalRetreatScheduleModelSelectLists"] = scheduleModelSelectLists;

            return View();
        }

        // POST: /EventSchedule/ScheduleModelSelect
        //[HttpPost]
        [AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
        public ActionResult ScheduleModelSelect(FormCollection collection)
        {
            int ModelNo = int.Parse(collection.Get("LocalRetreatScheduleModel"));
            int count = ((from r in _entities.LocalRetreatScheduleTemplates select r.Model).Distinct()).Count();
            return RedirectToAction("Index", new { modelID = ModelNo });
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Index(int? modelID)
        {
            if (!modelID.HasValue)
            {
                modelID = 1;
            }
             List<LocalRetreatScheduleTemplate> scheduleModels = _entities.LocalRetreatScheduleTemplates.Where(a => a.Model == modelID).OrderBy(a => a.ID).ToList();
             ViewData["modelID"] = modelID;
             return View(scheduleModels);
        }

        //
        // GET: /LocalRetreatScheduleTemplate/Details/5

        [Authorize(Roles = "Administrator")]
        public ActionResult Details(int id)
        {
            var localRetreatScheduleTemplate = _entities.LocalRetreatScheduleTemplates.Single(a => a.ID == id);
            return View(localRetreatScheduleTemplate);
        }

        //
        // GET: /LocalRetreatScheduleTemplate/Create

        [Authorize(Roles = "Administrator")]
        public ActionResult Create(int modelID)
        {
            var viewModel = new LocalRetreatScheduleTemplateViewModel
            {
                LocalRetreatScheduleTemplate = new LocalRetreatScheduleTemplate(),
                EventActivity = _entities.EventActivities.ToList(),
                ScheduleOffset = _entities.ScheduleOffsets.ToList(),
            };
            viewModel.LocalRetreatScheduleTemplate.Model = modelID;
             return View(viewModel);
         } 

        //
        // POST: /LocalRetreatScheduleTemplate/Create

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
 		public ActionResult Create(FormCollection collection, LocalRetreatScheduleTemplate localRetreatScheduleTemplate)
        {
            //collection["LocalRetreatScheduleTemplate"]
            if (localRetreatScheduleTemplate == null)
            {
                localRetreatScheduleTemplate = new LocalRetreatScheduleTemplate();
                localRetreatScheduleTemplate.Model = int.Parse(collection.GetValues("LocalRetreatScheduleTemplate")[0]);
                localRetreatScheduleTemplate.Clean_PersonNeeded = int.Parse(collection.GetValues("LocalRetreatScheduleTemplate.Clean_PersonNeeded")[0]);
                localRetreatScheduleTemplate.DP_PersonNeeded = int.Parse(collection.GetValues("LocalRetreatScheduleTemplate.DP_PersonNeeded")[0]);
                localRetreatScheduleTemplate.Video_PersonNeeded = int.Parse(collection.GetValues("LocalRetreatScheduleTemplate.Video_PersonNeeded")[0]);
                localRetreatScheduleTemplate.EventActivityID = int.Parse(collection.GetValues("LocalRetreatScheduleTemplate.EventActivityID")[0]);
                localRetreatScheduleTemplate.ScheduleOffsetID = int.Parse(collection.GetValues("LocalRetreatScheduleTemplate.ScheduleOffsetID")[0]);
            }
            try
            {
                // TODO: Add insert logic here
                _entities.AddToLocalRetreatScheduleTemplates(localRetreatScheduleTemplate);
                _entities.SaveChanges();
                return RedirectToAction("Index", new { modelID = localRetreatScheduleTemplate.Model });
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /LocalRetreatScheduleTemplate/Edit/5

        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            var viewModel = new LocalRetreatScheduleTemplateViewModel
            {
                LocalRetreatScheduleTemplate = _entities.LocalRetreatScheduleTemplates.Single(a => a.ID == id),
                EventActivity = _entities.EventActivities.ToList(),
                ScheduleOffset = _entities.ScheduleOffsets.ToList(),
            };
            
            return View(viewModel);
        }

        //
        // POST: /LocalRetreatScheduleTemplate/Edit/5

        //[HttpPost]
        [AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id, FormCollection collection)//, LocalRetreatScheduleTemplate localRetreatScheduleTemplate)
        {
            var localRetreatScheduleTemplate = _entities.LocalRetreatScheduleTemplates.Single(a => a.ID == id); 
            try
            {
                // TODO: Add update logic here
                UpdateModel(localRetreatScheduleTemplate, "LocalRetreatScheduleTemplate");
                _entities.SaveChanges();

                return RedirectToAction("Index", new {modelID = localRetreatScheduleTemplate.Model });
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /LocalRetreatScheduleTemplate/Delete/5

        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            var localRetreatScheduleTemplate = _entities.LocalRetreatScheduleTemplates.Single(a => a.ID == id);
            return View(localRetreatScheduleTemplate);
        }

        //
        // POST: /LocalRetreatScheduleTemplate/Delete/5

        //[HttpPost]
        [AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id, FormCollection collection)
        {
            if (id != 0)
            {
                var localRetreatScheduleTemplate = _entities.LocalRetreatScheduleTemplates.Single(a => a.ID == id);

                _entities.DeleteObject(localRetreatScheduleTemplate);

                _entities.SaveChanges();
            }
            return View("Deleted");
        }
    }
}
