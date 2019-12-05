using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMCHSGManager.Controllers
{
    public class LocalRetreatScheduleTemplateController : Controller
    {
        //
        // GET: /LocalRetreatScheduleTemplate/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /LocalRetreatScheduleTemplate/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /LocalRetreatScheduleTemplate/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /LocalRetreatScheduleTemplate/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /LocalRetreatScheduleTemplate/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /LocalRetreatScheduleTemplate/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /LocalRetreatScheduleTemplate/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /LocalRetreatScheduleTemplate/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
