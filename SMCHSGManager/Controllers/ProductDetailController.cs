using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMCHSGManager.Models;
using SMCHSGManager.ViewModel;

namespace SMCHSGManager.Controllers
{
    public class ProductDetailController : Controller
    {
		private SMCHDBEntities _entities = new SMCHDBEntities();
		
		//
        // GET: /ProductDetail/

		[Authorize(Roles = "Administrator")]
		public ActionResult Index(int productID)
        {
			var viewModel = _entities.ProductDetails.Where(a => a.ProductID == productID && a.UpdateDate > SMCHSGManager.Controllers.ProductController.updateTime ).ToList();

			return View(viewModel);
        }

        //
        // GET: /ProductDetail/Details/5

		[Authorize(Roles = "Administrator")]
		public ActionResult Details(int id)
        {


            return View();
        }

        //
        // GET: /ProductDetail/Create

		[Authorize(Roles = "Administrator")]
		public ActionResult Create(int productID)
        {
			Product product = _entities.Products.SingleOrDefault(a => a.ID == productID);
			ViewData["Name"] = product.Name;
			ViewData["NameChi"] = product.NameChi;

			ProductDetail productDetail = new ProductDetail();

			return View(productDetail);
        } 

        //
        // POST: /ProductDetail/Create

        //[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
		public ActionResult Create(FormCollection collection, ProductDetail productDetail, int productID)
        {
			//Product product = _entities.Products.SingleOrDefault(a => a.ID == productID);
            try
            {
				//product.ProductDetails.Add(productDetail);
				_entities.AddToProductDetails(productDetail);
				_entities.SaveChanges();
				return RedirectToAction("Index", "ProductDetail", new { productID = productID });
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /ProductDetail/Edit/5

		[Authorize(Roles = "Administrator")]
		public ActionResult Edit(int id)
        {
			ProductDetail productDetail = _entities.ProductDetails.Single(a => a.ID == id);
			return View(productDetail);
        }

        //
        // POST: /ProductDetail/Edit/5

        //[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
		public ActionResult Edit(int id, FormCollection collection)
        {
			var productDetail = _entities.ProductDetails.Single(a => a.ID == id);
            try
            {
				productDetail.SizeDescription = collection.Get("SizeDescription");
				if (!string.IsNullOrEmpty(collection.Get("UnitsInStock")))
				{
					productDetail.UnitsInStock = short.Parse(collection.Get("UnitsInStock"));
				}
				else
				{
					productDetail.UnitsInStock = 0;
				}

				if (!string.IsNullOrEmpty(collection.Get("UnitsOnOrder")))
				{
					productDetail.UnitsOnOrder = short.Parse(collection.Get("UnitsOnOrder"));
				}
				else
				{
					productDetail.UnitsOnOrder = 0;
				}

				//UpdateModel(productDetail, "ProductDetail");   not working....
				_entities.SaveChanges();
				return RedirectToAction("Index", "ProductDetail", new { productID = productDetail.ProductID });
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /ProductDetail/Delete/5

		[Authorize(Roles = "Administrator")]
		public ActionResult Delete(int id)
        {
			ProductDetail productDetail = _entities.ProductDetails.SingleOrDefault(a => a.ID == id);
			return View(productDetail);
        }

        //
        // POST: /ProductDetail/Delete/5

        //[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
		public ActionResult Delete(int id, FormCollection collection)
        {
			
			if (id != 0)
			{
				ProductDetail productDetail = _entities.ProductDetails.SingleOrDefault(a => a.ID == id);
				ViewData["ProductID"] = productDetail.ProductID;

				_entities.DeleteObject(productDetail);
				_entities.SaveChanges();

			}
			
			return View("Deleted");
        }
    }
}
