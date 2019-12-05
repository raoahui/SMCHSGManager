using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMCHSGManager.Models;
using SMCHSGManager.ViewModel;
using System.Globalization;
using System.Web.Security;

namespace SMCHSGManager.Controllers
{
    public class ProductOrderController : Controller
    {
        private SMCHDBEntities _entities = new SMCHDBEntities();
        private int _pageSize = 6;      //
        // GET: /ProductOrder/

		[Authorize]
		public ActionResult Index(int? page, bool recentOrder) 
        {
            var currentPage = page ?? 1;
            ViewData["CurrentPage"] = currentPage;
            ViewData["PageSize"] = _pageSize;
  
			DateTime now = DateTime.Now.ToUniversalTime().AddHours(8);
			var viewModel = from r in _entities.ProductOrders.Where(a => a.OrderCloseDate >= now && !recentOrder || recentOrder && a.OrderCloseDate < now) orderby r.OrderCloseDate, r.ID select r;
			if (recentOrder)
			{
				viewModel = viewModel.OrderByDescending(a => a.OrderCloseDate).ThenBy(a => a.ID);
			}
            var pageViewModel = viewModel.Skip((currentPage - 1) * _pageSize).Take(_pageSize);

            List<int> OrderStatus = new List<int> ();
			List<UploadFile> imageFiles = new List<UploadFile>();
            MembershipUser muc = Membership.GetUser();
            foreach(ProductOrder po in pageViewModel)
            {
                int memberOrderID = 0;
                var memberOrders = from r in _entities.MemberOrders 
								   where	r.ProductOrderID == po.ID && 
												r.MemberInfo.MemberID == (Guid)muc.ProviderUserKey &&
												r.OrderStatusID == 1
								   select r;
                if (memberOrders.Count() > 0)
                {
                    memberOrderID = memberOrders.FirstOrDefault().ID;
                }
                OrderStatus.Add(memberOrderID);
				imageFiles.Add(GetImageFile(po.ID));
            }

			ViewData["TotalPages"] = (int)Math.Ceiling((float)viewModel.Count() / _pageSize);

			ViewData["OrderStatus"] = OrderStatus;
			ViewData["RecentOrder"] = recentOrder;
			ViewData["ImageFiles"] = imageFiles;

            return View(pageViewModel);
        }

		public UploadFile GetImageFile(int productOrderID)
		{
			UploadFile imageFile = new UploadFile ();
			List<Product> products = _entities.ProductOrderDetails.Where(a => a.ProductOrderID == productOrderID).OrderBy(a=>a.ProductID).Select(a => a.Product).ToList();
			foreach(Product p in products)
			{
				if(p.ProductUploadFiles.Count() > 0)
				{
					imageFile = p.ProductUploadFiles.FirstOrDefault().UploadFile;
					break;
				}
			}
			
			return imageFile;
		}

        //
        // GET: /ProductOrder/Details/5

		[Authorize]
		public ActionResult Details(int id,  bool recentOrder )
        {
            List<int> productIDs = (from r in _entities.ProductOrderDetails where r.ProductOrderID == id select r.ProductID).ToList();
            List<Product> products = new List<Product> ();
            foreach(int iID in productIDs)
            {
                products.Add(_entities.Products.Single(a=>a.ID == iID));
            }

			MembershipUser muc = Membership.GetUser();
			int memberOrderID = (from r in _entities.MemberOrders where r.ProductOrderID == id && r.MemberInfo.MemberID == (Guid)muc.ProviderUserKey select r.ID).FirstOrDefault();

			ProductOrder productOrder = _entities.ProductOrders.Single(a => a.ID == id);
			DateTime today = DateTime.Today;
			bool uPO = true;
			if(productOrder.OrderCloseDate < today)
			{
				uPO = false;
			}

			ProductOrderViewModel viewModel = new ProductOrderViewModel
            {
				ProductOrder = productOrder,
				ProductOrderIDs = (from r in _entities.ProductOrders.Where(a => a.OrderCloseDate >= today && uPO || !uPO && a.OrderCloseDate < today) orderby r.OrderCloseDate, r.ID select r.ID).ToList(),
                Products = products, 
				MemberOrderID = memberOrderID,
            };

			ViewData["RecentOrder"] = recentOrder;

			return View(viewModel);
        }

        //
        // GET: /ProductOrder/Create

		[Authorize(Roles = "Administrator")]
		public ActionResult Create()
        {
            ProductOrderViewModel viewModel = new ProductOrderViewModel
            {
				ProductOrder = new ProductOrder(),
				//Products = _entities.Products.Take(200).ToList(),
            };

			viewModel.ProductOrder.Title = "[Please input order Title]";
			viewModel.ProductOrder.OrderOpenDate = DateTime.Today.ToUniversalTime().AddHours(8);
			viewModel.ProductOrder.OrderCloseDate = DateTime.Today.ToUniversalTime().AddHours(8).AddDays(10);

            return View(viewModel);
        }

        //
        // POST: /ProductOrder/Create

		[AcceptVerbs(HttpVerbs.Post),  ValidateInput(false), Authorize(Roles = "Administrator")]
		public ActionResult Create(FormCollection collection, ProductOrder productOrder)
        {
            try
            {
				_entities.AddToProductOrders(productOrder);
                _entities.SaveChanges();

				productOrder = (from r in _entities.ProductOrders select r).OrderByDescending(x => x.ID).First();
				return RedirectToAction("Index", "Product", new { productOrderID = productOrder.ID, recentOrder = false });
            }
            catch
            {
                ProductOrderViewModel viewModel = new ProductOrderViewModel
                {
					ProductOrder = productOrder,
                };

                return View(viewModel);
            }
        }
        
        //
        // GET: /ProductOrder/Edit/5

		[Authorize(Roles = "Administrator")]
		public ActionResult Edit(int id)
        {
			var productOrder = _entities.ProductOrders.Single(a => a.ID == id);

            List<Product> products = (from r in _entities.ProductOrderDetails where r.ProductOrderID == id select r.Product).ToList();

            ProductOrderViewModel viewModel = new ProductOrderViewModel
            {
				ProductOrder = productOrder,
            };

            return View(viewModel);
        }

        //
        // POST: /ProductOrder/Edit/5

		[AcceptVerbs(HttpVerbs.Post),  ValidateInput(false), Authorize(Roles = "Administrator")]
		public ActionResult Edit(int id, FormCollection collection)
        {
			var productOrder = _entities.ProductOrders.Single(a => a.ID == id);
            try
            {
				UpdateModel(productOrder, "ProductOrder");
                _entities.SaveChanges();

				return RedirectToAction("Index", "Product", new { productOrderID = id, recentOrder = false });
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /ProductOrder/Delete/5

		[Authorize(Roles = "Administrator")]
		public ActionResult Delete(int id)
        {
			var productOrder = _entities.ProductOrders.Single(a => a.ID == id);
			return View(productOrder);
        }

        //
        // POST: /ProductOrder/Delete/5

		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
		public ActionResult Delete(int id, FormCollection collection)
        {
            if (id != 0)
            {
				ProductOrder productOrder = _entities.ProductOrders.Single(a => a.ID == id);
				_entities.DeleteObject(productOrder);
                _entities.SaveChanges();
            }
            return View("Deleted");
        }
    }
}
