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
    public class MemberOrderController : Controller
    {
        private SMCHDBEntities _entities = new SMCHDBEntities();
		public static int allStatusID = 1000;
        //
        // GET: /MemberOrder/

		[Authorize(Roles = "Administrator")]
		public ActionResult IndexByMemberOrder(int productOrderID, int? orderStatusID, DateTime? submitDateTime)
        {
			orderStatusID = GetNowOrderStatus(orderStatusID);            

            List<MemberOrder> memberOrders = (from r in _entities.MemberOrders
											  where r.ProductOrderID == productOrderID && (orderStatusID.Value == allStatusID || r.OrderStatusID == orderStatusID.Value)
											  select r).ToList();

			submitDateTime = ProcessSubmitProductOrder(productOrderID, orderStatusID, submitDateTime);

			if (submitDateTime.HasValue)
			{
				memberOrders = memberOrders.Where(a => a.SubmitDateTime == submitDateTime.Value).ToList();
			}

            decimal totalPrice = 0;
            foreach(MemberOrder memberOrder in memberOrders)
            {
                   totalPrice += memberOrder.Price;
            }

            ViewData["TotalPrice"] = totalPrice;

			ViewData[""] = memberOrders;

			return View(memberOrders);
        }

		private int? GetNowOrderStatus(int? orderStatusID)
		{
			List<SelectListModel> selectList = (from r in _entities.OrderStatuses
												orderby r.ID
												select new SelectListModel
												{
													ID = r.ID,
													Name = r.Status,
												}).ToList();

			if (!orderStatusID.HasValue)
			{
			    orderStatusID = allStatusID;
			}

			ViewData["OrderStatuses"] = InternationalGMApplicationController.GetSelectListItem(allStatusID, allStatusID, null, selectList);

			return orderStatusID;
		}

		public ActionResult OrderSubmit(int productOrderID, int orderStatusID)
		{
			List<MemberOrder> memberOrders = (from r in _entities.MemberOrders
											  where r.ProductOrderID == productOrderID && r.OrderStatusID == orderStatusID
											  select r).ToList();
			if (orderStatusID == 1)
			{
				DateTime now = DateTime.Now.ToUniversalTime().AddHours(8);
				now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
				foreach (MemberOrder memberOrder in memberOrders)
				{
					memberOrder.OrderStatusID = 2;
					memberOrder.SubmitDateTime = now;
					_entities.SaveChanges();
				}
			}
			return RedirectToAction("IndexByMemberOrder", new { productOrderID = productOrderID, orderStatusID = 2 });
		}



		[Authorize(Roles = "Administrator")]
		public ActionResult IndexByProductOrder(int productOrderID, int? orderStatusID, DateTime? submitDateTime)
		{
			orderStatusID = GetNowOrderStatus(orderStatusID);            

			DateTime today = DateTime.Today.ToUniversalTime().AddHours(8);
           
            var productOrderList = (from r in _entities.MemberOrderDetails
									where r.MemberOrder.ProductOrderID == productOrderID && (orderStatusID.Value == allStatusID || r.MemberOrder.OrderStatusID == orderStatusID.Value && (!submitDateTime.HasValue || submitDateTime.HasValue && r.MemberOrder.SubmitDateTime.Value == submitDateTime.Value))
									orderby r.ProductDetail.Product.ItemCode
									group r by new 
						{
							productDetail = r.ProductDetail,
							orderStatus = r.MemberOrder.OrderStatus,
							submitDateTime = r.MemberOrder.SubmitDateTime,
						} into result

						select new ProductDetailListViewModel()
						{
							 ProductDetail = result.Key.productDetail,
					   		OrderStatus = result.Key.orderStatus,
							 SubmitDateTime = result.Key.submitDateTime,
							OrderTotalNumber = result.Sum(a => a.Quantity),
						}).ToList();

			submitDateTime = ProcessSubmitProductOrder(productOrderID, orderStatusID, submitDateTime);

			if (submitDateTime.HasValue)
			{
				productOrderList = productOrderList.Where(a => a.SubmitDateTime == submitDateTime.Value).ToList();
			}

			 ViewData["productOrderID"] = productOrderID;
	
			 ViewData["OrderTitle"] = _entities.ProductOrders.Single(a => a.ID == productOrderID).Title;
			 ViewData["OrderCloseDate"] = _entities.ProductOrders.Single(a => a.ID == productOrderID).OrderCloseDate;

			 List<decimal> realPrices = new List<decimal>();
			 foreach (ProductDetailListViewModel memberOrderDetail in productOrderList)
			 {
				 decimal realPrice = GetProductPriceAfterDiscount(memberOrderDetail.ProductDetail.Product, memberOrderDetail.OrderTotalNumber);
				 realPrices.Add(realPrice);
			 }
			 ViewData["RealPrices"] = realPrices;

			 return View(productOrderList);
		}

		private DateTime? ProcessSubmitProductOrder(int productOrderID, int? orderStatusID, DateTime? submitDateTime)
		{
			List<DateTime> submitDateTimes = new List<DateTime>();
			if (orderStatusID == 2 && _entities.MemberOrders.Any(a => a.ProductOrderID == productOrderID && a.OrderStatusID == orderStatusID))
			{
				submitDateTimes = (_entities.MemberOrders.Where(a => a.ProductOrderID == productOrderID && a.OrderStatusID == orderStatusID).OrderBy(a => a.SubmitDateTime.Value).Select(a => a.SubmitDateTime.Value)).Distinct().ToList();
				if (!submitDateTime.HasValue)
				{
					submitDateTime = submitDateTimes[0];
				}
			}
			else if (orderStatusID != 2 && submitDateTime.HasValue)
			{
				submitDateTime = null;
			}
			ViewData["SubmitDateTimes"] = submitDateTimes;
			return submitDateTime;
		}

		[Authorize(Roles = "Administrator")]
		public ActionResult NamesByProductDetail(int productID, string sizeDes, int productOrderID, int orderStatusID, DateTime? submitDateTime)
		{
			
			var viewModel = (from r in _entities.MemberOrderDetails
							 where r.MemberOrder.ProductOrderID == productOrderID &&
									 r.ProductDetail.ProductID == productID && 
									 r.MemberOrder.OrderStatusID == orderStatusID && 
									 (!submitDateTime.HasValue || submitDateTime.HasValue && r.MemberOrder.SubmitDateTime == submitDateTime) &&
									 (r.ProductDetail.SizeDescription == sizeDes && sizeDes != null || sizeDes == null)
							 select new NamesByProductDetailViewModel()
							 {
								 name = r.MemberOrder.MemberInfo.Name,
								 qty = r.Quantity,
							 }).ToList();


			Product product = _entities.Products.Single(a => a.ID == productID);
			ViewData["ProductName"] = product.Name;
			ViewData["ProductItemCode"] = product.ItemCode;
			ViewData["SizeDes"] = sizeDes;

			return View(viewModel);
		}

		[Authorize]
		public ActionResult MyOrder(Guid? memberID, DateTime? startDate, DateTime? endDate)
        {

            if (memberID == null)
            {
                MembershipUser user = Membership.GetUser();
                memberID = ((Guid)(user.ProviderUserKey));
            }

            var memberOrders = from r in _entities.MemberOrders
                                            where (r.MemberInfo.MemberID == memberID) &&
                                                        (startDate == null || r.LatestOrderDateTime >= startDate.Value) &&
                                                          (endDate == null || r.LatestOrderDateTime <= endDate.Value)
                                            orderby r.MemberInfo.Name
                                            select r;

             ViewData["MemberOrderCount"] = memberOrders.Count();

             return View(memberOrders);
        }
       
        //
        // GET: /MemberOrder/Details/5

		[Authorize]
		public ActionResult Details(int memberOrderID, int orderStatusID, DateTime? submitDateTime)
        {
            List<MemberOrderDetail> viewModel = (from r in _entities.MemberOrderDetails 
												 where  r.MemberOrder.ID == memberOrderID &&
												 r.MemberOrder.OrderStatusID == orderStatusID &&
												 (!submitDateTime.HasValue || submitDateTime.HasValue && r.MemberOrder.SubmitDateTime.Value == submitDateTime.Value)
												 select r).ToList();
			
			List<decimal> realPrices = new List<decimal> ();
			foreach(MemberOrderDetail memberOrderDetail in viewModel)
			{
				decimal realPrice = GetProductPriceAfterDiscount(memberOrderDetail.ProductDetail.Product, memberOrderDetail.Quantity);
				realPrices.Add(realPrice);
			}
			ViewData["RealPrices"] = realPrices;

            return View(viewModel);
        }

		//
		// GET: /Product/Details/5
		[Authorize]
		public ActionResult AddToOrderBag(int productID, int productOrderID, int? page, Guid? memberID)
		{
			if (!memberID.HasValue)
			{
				memberID = (Guid)Membership.GetUser().ProviderUserKey;
			}

			List<ProductDetail> productDetails = _entities.ProductDetails.Where(a => a.ProductID == productID && a.UpdateDate > SMCHSGManager.Controllers.ProductController.updateTime && (a.UnitsInStock == null || a.UnitsInStock != null && a.UnitsInStock > 0)).OrderBy(a => a.ID).ToList();

			List<string> productQuantities = GetProductDetailQuantities(productID, productOrderID, memberID.Value, productDetails);

			ViewData["productQuantities"] = productQuantities.ToArray();
			ViewData["productOrderID"] = productOrderID;
			
			var viewModel = new ProductDetailViewModel
			{
				Sizes = productDetails.Select(a=>a.SizeDescription).ToList(),
				Product = _entities.Products.Single(a => a.ID == productID),
			};

			viewModel.ProductDiscount = viewModel.Product.ProductDiscounts.Where(a => a.DateTo >= DateTime.Today).FirstOrDefault();
			viewModel.ReadPrice = GetProductPriceAfterDiscount(viewModel.Product, 1);

			if (viewModel.Product.ProductUploadFiles.Count == 0)
			{
				UploadFile upload = _entities.UploadFiles.Single(a => a.ID == 1276);
				ProductUploadFile productUploadFile = new ProductUploadFile();
				productUploadFile.UploadFile = upload;
				viewModel.Product.ProductUploadFiles.Add(productUploadFile);
			}

			return View(viewModel);
		}

		private List<string> GetProductDetailQuantities(int productID, int productOrderID, Guid memberID, List<ProductDetail> productDetails)
		{
			List<MemberOrderDetail> selestMemberOrderDetails = (from r in _entities.MemberOrderDetails where r.MemberOrder.ProductOrderID == productOrderID && r.ProductDetail.ProductID == productID && r.MemberOrder.MemberID == memberID orderby r.ProductDetail.ID select r).ToList();

			List<string> productQuantities = new List<string>();
			foreach (ProductDetail pd in productDetails)
			{
				bool selected = false;
				foreach (MemberOrderDetail mod in selestMemberOrderDetails)
				{
					if (mod.ProductDetail.ID == pd.ID)
					{
						productQuantities.Add(mod.Quantity.ToString());
						selestMemberOrderDetails.Remove(mod);
						selected = true;
						break;
					}
				}
				if (!selected)
				{
					productQuantities.Add("0");
				}
			}
			return productQuantities;
		}

		public void AddToMemberOrderDetailForProduct(FormCollection collection, MemberOrder memberOrder, int productID)
		{

			string[] productQuantitys = collection.Get("Quantity").Split(',');

			int j = 0;
			decimal totalPrice = 0;

			List<ProductDetail> productDetails = _entities.ProductDetails.Where(a => a.ProductID == productID && a.UpdateDate > SMCHSGManager.Controllers.ProductController.updateTime ).ToList();

			foreach (ProductDetail pd in productDetails)
			{
				if (!string.IsNullOrEmpty(productQuantitys[j]) && IsDigitString(productQuantitys[j]) && int.Parse(productQuantitys[j]) > 0)
				{
					MemberOrderDetail memberOrderDetail = new MemberOrderDetail();
					memberOrderDetail.ProductDetail = pd;
					memberOrderDetail.Quantity = short.Parse(productQuantitys[j]);
					memberOrder.MemberOrderDetails.Add(memberOrderDetail);
					float discount = 0;
					if (pd.Product.ProductDiscounts.Any(a => a.DateTo >= DateTime.Today))
					{
						discount = pd.Product.ProductDiscounts.Where(a=>a.DateTo >= DateTime.Today).FirstOrDefault().Discount;
					}
					if (discount != 0.3333f)
					{
						totalPrice += memberOrderDetail.ProductDetail.Product.UnitPrice * (1 - (decimal)discount) * memberOrderDetail.Quantity;
					}
					else
					{
						int backNos = memberOrderDetail.Quantity/3;
						totalPrice += memberOrderDetail.ProductDetail.Product.UnitPrice * (memberOrderDetail.Quantity - backNos);
					}
				}
				j++;
			}
			if (totalPrice == 0)
			{
				ModelState.AddModelError("", "please input the quantity of product.");
			}
			else
			{
				memberOrder.Price += totalPrice;
				memberOrder.CurrencyCode = memberOrder.MemberOrderDetails.FirstOrDefault().ProductDetail.Product.CurrencyCode;

			}
			
		}
	
		//
		// POST: /MemberOrder/Create

		//[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize]
		public ActionResult AddToOrderBag(FormCollection collection, int productID, int productOrderID, int? page, Guid? memberID)
		{
			if (!memberID.HasValue)
			{
				memberID = (Guid)Membership.GetUser().ProviderUserKey;
			}
			MemberOrder memberOrder = new MemberOrder();
			if (_entities.MemberOrders.Where(a => a.ProductOrderID == productOrderID && a.MemberID == memberID.Value && a.OrderStatusID == 1).Any())
			{
				memberOrder = _entities.MemberOrders.Single(a => a.ProductOrderID == productOrderID && a.MemberID == memberID.Value && a.OrderStatusID == 1);
			}
			else
			{
				memberOrder.MemberID = memberID.Value;
				memberOrder.ProductOrderID = productOrderID;
				memberOrder.OrderStatusID = 1;
			}
			memberOrder.LatestOrderDateTime = DateTime.Now.ToUniversalTime().AddHours(8);
			
			try
			{
				int memberOrderDetailCounts = RemoveMemberOrderDetailsFromMemberOrder(productID, memberOrder);
				AddToMemberOrderDetailForProduct(collection, memberOrder, productID);
				_entities.SaveChanges();

				if (!ModelState.IsValid)
				{
					if (memberOrderDetailCounts > 0 && memberOrder.MemberOrderDetails.Count == 0)
					{
						_entities.DeleteObject(memberOrder);
						_entities.SaveChanges();
						return RedirectToAction("ProductList", new { productOrderID = productOrderID, page = page });
					}
					else if (memberOrderDetailCounts > 0)
					{
						return RedirectToAction("Details", new { memberOrderID = memberOrder.ID, orderStatusID = memberOrder.OrderStatusID, page = page});
					}
					throw new Exception();
				}

				if (memberOrder.ID == 0)
				{
					_entities.AddToMemberOrders(memberOrder);
					memberOrder = (from r in _entities.MemberOrders where r.ProductOrderID == productOrderID select r).OrderByDescending(x => x.ID).First();
				}
				else
				{
					UpdateModel(memberOrder, "MemberOrder");
				}
				_entities.SaveChanges();

				return RedirectToAction("Details", new { memberOrderID = memberOrder.ID, orderStatusID = memberOrder.OrderStatusID, page = page });
			}
			catch
			{
			    List<ProductDetail> productDetails = _entities.ProductDetails.Where(a => a.ProductID == productID && a.UpdateDate > SMCHSGManager.Controllers.ProductController.updateTime && (a.UnitsInStock == null || a.UnitsInStock != null && a.UnitsInStock > 0)).OrderBy(a => a.ID).ToList();

				List<string> productQuantities = GetProductDetailQuantities(productID, productOrderID, memberID.Value, productDetails);
				ViewData["productQuantities"] = productQuantities.ToArray();

				var viewModel = new ProductDetailViewModel
				{
					Sizes = productDetails.Select(a => a.SizeDescription).ToList(),
					Product = _entities.Products.Single(a => a.ID == productID),
				};
				viewModel.ProductDiscount = viewModel.Product.ProductDiscounts.Where(a => a.DateTo >= DateTime.Today).FirstOrDefault();
				viewModel.ReadPrice = GetProductPriceAfterDiscount(viewModel.Product, 1);

				if (viewModel.Product.ProductUploadFiles.Count == 0)
				{
					UploadFile upload = _entities.UploadFiles.Single(a => a.ID == 1276);
					ProductUploadFile productUploadFile = new ProductUploadFile();
					productUploadFile.UploadFile = upload;
					viewModel.Product.ProductUploadFiles.Add(productUploadFile);
				}

				return View(viewModel);
			}
		}

		private int RemoveMemberOrderDetailsFromMemberOrder(int productID, MemberOrder memberOrder)
		{
			List<MemberOrderDetail> memberOrderDetails = memberOrder.MemberOrderDetails.Where(a => a.ProductDetail.ProductID == productID).ToList();
			int memberOrderDetailCounts = memberOrderDetails.Count;
			foreach (MemberOrderDetail memberOrderDetail in memberOrderDetails)
			{
				//MinusMemberOrderPrice(memberOrder, memberOrderDetail);
				decimal realPrice = GetProductPriceAfterDiscount(memberOrderDetail.ProductDetail.Product, memberOrderDetail.Quantity);
				memberOrder.Price -= realPrice;
				_entities.MemberOrderDetails.DeleteObject(memberOrderDetail);
			}
			return memberOrderDetailCounts;
		}

		//private static void MinusMemberOrderPrice(MemberOrder memberOrder, MemberOrderDetail memberOrderDetail)
		//{
		//    float discount = 0;
		//    if (memberOrderDetail.ProductDetail.Product.ProductDiscounts.Any(a => a.DateTo >= DateTime.Today))
		//    {
		//        discount = memberOrderDetail.ProductDetail.Product.ProductDiscounts.Where(a => a.DateTo >= DateTime.Today).FirstOrDefault().Discount;
		//    }
		//    if (discount == 0.3333f)
		//    {
		//        int backNos = memberOrderDetail.Quantity / 3;
		//        memberOrder.Price -= memberOrderDetail.ProductDetail.Product.UnitPrice * (memberOrderDetail.Quantity - backNos);
		//    }
		//    else
		//    {
		//        memberOrder.Price -= memberOrderDetail.ProductDetail.Product.UnitPrice * (1 - (decimal)discount) * memberOrderDetail.Quantity;
		//    }
		//}

		private static decimal GetProductPriceAfterDiscount(Product product, int quantity)
		{
			float discount = 0;
			decimal realPrice;
			if (product.ProductDiscounts.Any(a => a.DateTo >= DateTime.Today))
			{
				discount = product.ProductDiscounts.Where(a => a.DateTo >= DateTime.Today).FirstOrDefault().Discount;
			}
			if (discount == 0.3333f)
			{
				int backNos = quantity / 3;
				realPrice = product.UnitPrice * (quantity - backNos);
			}
			else
			{
				realPrice = product.UnitPrice * (1 - (decimal)discount) * quantity;
			}

			return realPrice;
		}
	
	
		private int _pageSize = 20;
		[Authorize]
		public ActionResult ProductList(int productOrderID, int? page, string searchContent)
        {
			var currentPage = page ?? 1;
			ViewData["PageSize"] = _pageSize;
			ViewData["searchContent"] = searchContent;

           List<Product> products = (from r in _entities.Products 
										join h in _entities.ProductOrderDetails on r.ID equals h.ProductID
                                        where h.ProductOrderID == productOrderID && (string.IsNullOrEmpty(searchContent) 
                                                 || r.Name.Contains(searchContent) 
                                                 || r.NameChi.Contains(searchContent) 
                                                 || r.ItemCode.Contains(searchContent)) 
                                        select r).ToList();

			ViewData["TotalPages"] = (int)Math.Ceiling((float)products.Count() / _pageSize);

			if ((int)ViewData["TotalPages"] < currentPage)
			{
				currentPage = 1;
			}
			ViewData["CurrentPage"] = currentPage;

            products = (products.AsQueryable().OrderBy(a => a.ItemCode).Skip((currentPage - 1) * _pageSize).Take(_pageSize)).ToList();

			var viewModel = new ProductOrderViewModel
			{
				Products = products,
				ProductOrder = _entities.ProductOrders.SingleOrDefault(a => a.ID == productOrderID),
			};

			return View(viewModel);
        }


        public bool IsDigitString(string s)
        {
            foreach (char c in s)
            {
                if (!(c >= '0' && c <= '9'))
                    return false;
            }
            return true;
        }


		[Authorize]
		public ActionResult DeleteMemberOrderDetail(int productDetailID, int memberOrderID)
		{
				MemberOrder memberOrder = _entities.MemberOrders.Single(a => a.ID == memberOrderID);

				MemberOrderDetail memberOrderDetail = _entities.MemberOrderDetails.Single(a => a.ProductDetailID == productDetailID && a.MemberOrderID == memberOrderID && a.MemberOrder.OrderStatusID == 1);

				decimal realPrice = GetProductPriceAfterDiscount(memberOrderDetail.ProductDetail.Product, memberOrderDetail.Quantity);
				memberOrder.Price -= realPrice;

				//float discount = 0;
				//if (memberOrderDetail.ProductDetail.Product.ProductDiscounts.Any(a => a.DateTo >= DateTime.Today))
				//{
				//    discount = memberOrderDetail.ProductDetail.Product.ProductDiscounts.Where(a => a.DateTo >= DateTime.Today).FirstOrDefault().Discount;
				//}
				//memberOrder.Price -= memberOrderDetail.Quantity * memberOrderDetail.ProductDetail.Product.UnitPrice * (1 - (decimal)discount);

				_entities.DeleteObject(memberOrderDetail);
				_entities.SaveChanges();

				bool exist = _entities.MemberOrderDetails.Any(a=>a.MemberOrderID == memberOrderID);
				int productOrderID = memberOrder.ProductOrderID;
			
				if (exist)
				{
					return RedirectToAction("Details", new { memberOrderID = memberOrderID, orderStatusID = memberOrder.OrderStatusID });
				}
				else
				{
					_entities.DeleteObject(memberOrder);
					_entities.SaveChanges();
					return RedirectToAction("ProductList", new { productOrderID = productOrderID });
				}
			
		}


		//
        // GET: /MemberOrder/Delete/5

		[Authorize]
		public ActionResult Delete(int id)
        {
            MemberOrder memberOrder = _entities.MemberOrders.Single(a => a.ID == id);
            return View(memberOrder);
        }

        //
        // POST: /MemberOrder/Delete/5

        //[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize]
		public ActionResult Delete(int id, FormCollection collection)
        {
            if (id != 0)
            {
                MemberOrder memberOrder = _entities.MemberOrders.Single(a => a.ID == id);

                _entities.DeleteObject(memberOrder);
                _entities.SaveChanges();
            }
            return View("Deleted");
        }

    }
}
