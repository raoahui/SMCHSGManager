using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMCHSGManager.Models;
using System.Web.Mvc.Html;
using System.Text;

namespace SMCHSGManager.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class VolunteerJobTypeController : Controller
    {
        //
        // GET: /VolunteerJobType/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /VolunteerJobType/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /VolunteerJobType/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /VolunteerJobType/Create

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
        // GET: /VolunteerJobType/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /VolunteerJobType/Edit/5

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
        // GET: /VolunteerJobType/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /VolunteerJobType/Delete/5

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
