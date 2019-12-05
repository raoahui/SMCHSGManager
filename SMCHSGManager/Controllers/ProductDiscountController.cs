using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMCHSGManager.Models;
using SMCHSGManager.ViewModel;

namespace SMCHSGManager.Controllers
{
    public class ProductDiscountController : Controller
    {

		private SMCHDBEntities _entities = new SMCHDBEntities();
        //
        // GET: /ProductDiscount/

		[Authorize(Roles = "Administrator")]
		public ActionResult Index()
        {
			List<ProductDiscount> viewModel = _entities.ProductDiscounts.ToList();
			return View(viewModel);
        }

        //
        // GET: /ProductDiscount/Details/5

		//public ActionResult Details(int id)
		//{
		//    return View();
		//}

        //
        // GET: /ProductDiscount/Create

		[Authorize(Roles = "Administrator")]
		public ActionResult Create()
        {
			
			List<Product> products = _entities.Products.Where(a => a.ProductDiscounts.Count() == 0 && a.ItemCode != null).OrderBy(a=>a.ItemCode).ToList();
			var viewModel = new ProductDiscountViewModel
			{
				ProductDiscount = new ProductDiscount(),
				Products = products,
			};
			viewModel.ProductDiscount.DateFrom = DateTime.Today;
			viewModel.ProductDiscount.DateTo = viewModel.ProductDiscount.DateFrom.AddDays(40);
			viewModel.ProductDiscount.Discount = (float)0.2;
            return View(viewModel);
        } 

        //
        // POST: /ProductDiscount/Create

        //[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
		public ActionResult Create(FormCollection collection, ProductDiscount productDiscount)
        {
            try
            {
				_entities.AddToProductDiscounts(productDiscount);
				_entities.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /ProductDiscount/Edit/5

		[Authorize(Roles = "Administrator")]
		public ActionResult Edit(int id)
        {
			var viewModel = new ProductDiscountViewModel
			{
				ProductDiscount = _entities.ProductDiscounts.SingleOrDefault(a => a.ID == id),
			};
            return View(viewModel);
        }

        //
        // POST: /ProductDiscount/Edit/5

        //[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
		public ActionResult Edit(int id, FormCollection collection)
        {
			ProductDiscount productDiscount = _entities.ProductDiscounts.SingleOrDefault(a => a.ID == id);
            try
            {
                // TODO: Add update logic here
				UpdateModel(productDiscount, "ProductDiscount");
				_entities.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /ProductDiscount/Delete/5

		[Authorize(Roles = "Administrator")]
		public ActionResult Delete(int id)
        {
			ProductDiscount productDiscount = _entities.ProductDiscounts.SingleOrDefault(a => a.ID == id);
			return View(productDiscount);
        }

        //
        // POST: /ProductDiscount/Delete/5

        //[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
		public ActionResult Delete(int id, FormCollection collection)
        {
			if (id != 0)
			{
				ProductDiscount productDiscount = _entities.ProductDiscounts.SingleOrDefault(a => a.ID == id);

				_entities.DeleteObject(productDiscount);
				_entities.SaveChanges();

			}

			return View("Deleted");
        }
    }
}
