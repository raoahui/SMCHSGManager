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
    public class ProductController : Controller
    {
        private SMCHDBEntities _entities = new SMCHDBEntities();
        private int _pageSize = 50;

		public static DateTime updateTime = new DateTime(2012, 2, 1);

        //
        // GET: /Product/

		[Authorize(Roles = "Administrator")]
		public ActionResult Index(int? page, int? categoryID, int? productOrderID, FormCollection collection, string AddToProductOrder, string searchContent)
        {

            var currentPage = page ?? 1;
			var currentcategoryID = categoryID ?? 1;
            ViewData["PageSize"] = _pageSize;
			ViewData["searchContent"] = searchContent;

			var viewModel = from r in _entities.Products where (r.CategoryID == currentcategoryID && string.IsNullOrEmpty(searchContent) || r.Name.Contains(searchContent) || r.NameChi.Contains(searchContent) || r.ItemCode.Contains(searchContent)) && r.UpdateDate > updateTime orderby r.ItemCode select r;
		
			ViewData["TotalPages"] = (int)Math.Ceiling((float)viewModel.Count() / _pageSize);

			if ((int)ViewData["TotalPages"] < currentPage)
				currentPage = 1;

            var pageViewModel = viewModel.Skip((currentPage - 1) * _pageSize).Take(_pageSize).ToList();

			if (!string.IsNullOrEmpty(AddToProductOrder) && productOrderID.HasValue)
			{
				AddToProductOrders(collection, productOrderID.Value);
			}

			List<bool> productUseds = new List<bool>();
			List<bool> productSelecteds = new List<bool>();
			List<ProductDiscount> discounts = new List<ProductDiscount>();
			MemberOrderController moc = new MemberOrderController ();

			foreach (Product product in pageViewModel)
			{
				if (_entities.ProductOrderDetails.Any(a => a.ProductID == product.ID &&( a.ProductOrderID == productOrderID && productOrderID.HasValue || !productOrderID.HasValue) ))
				{
					productUseds.Add(true);
				}
				else
				{
					productUseds.Add(false);
				}
				ProductDiscount discount = new ProductDiscount ();
				if (product.ProductDiscounts.Any(a => a.DateTo >= DateTime.Today))
				{
					discount = product.ProductDiscounts.Where(a => a.DateTo >= DateTime.Today).FirstOrDefault();
				}
				discounts.Add(discount);
			}

			ViewData["Discounts"] = discounts;
			ViewData["productUseds"] = productUseds;

			ViewData["Categories"] = (from r in _entities.Products join h in _entities.ProductCategories on r.CategoryID equals h.ID select h).Distinct().OrderBy(a=>a.ID).ToList();
	
	
			
			ViewData["currentcategoryID"] = currentcategoryID;

			ViewData["CurrentPage"] = currentPage;
			ViewData["productOrderID"] = productOrderID;
	
            return View(pageViewModel);
        }

		public void AddToProductOrders(FormCollection  collection, int productOrderID)
		{
			ProductOrder productOrder = _entities.ProductOrders.SingleOrDefault(a => a.ID == productOrderID);
			List<ProductOrderDetail> productOrderDetails = _entities.ProductOrderDetails.Where(a => a.ProductOrderID == productOrderID).ToList();
			List<int> productIDs = productOrderDetails.Select(a=>a.ProductID).ToList();

			//if (selectedProductIDs == null)
			//{
			//    List<int> temp = new List<int>();
			//    foreach (Product product in viewModel)
			//    {
			//        temp.Add(product.ID);
			//    }
			//    selectedProductIDs = temp.AsEnumerable();
			//}

			string[] keys = collection.AllKeys;
			for (int i = 1; i < keys.Length-1; i++)  //Remove DropDownList [0] and buttom [Length]
			{
				string temp = collection.Get(keys[i].ToString());
				int productID = int.Parse(keys[i]);
				if (!string.IsNullOrEmpty(temp) && temp.ToLower().StartsWith("true") && !productIDs.Contains(productID))
				{
					ProductOrderDetail pod = new ProductOrderDetail();
					pod.ProductID = productID;
					productOrder.ProductOrderDetails.Add(pod);
				}
				else if (!string.IsNullOrEmpty(temp) && temp.ToLower().StartsWith("false") && productIDs.Contains(productID))
				{
					ProductOrderDetail pod = _entities.ProductOrderDetails.SingleOrDefault(a => a.ProductID == productID && a.ProductOrderID == productOrderID);
					_entities.DeleteObject(pod);
				}
			}
			UpdateModel(productOrder, "ProductOrder");
			_entities.SaveChanges();

		}

        //
        // GET: /Product/Create

		[Authorize(Roles = "Administrator")]
		public ActionResult Create()
        {
            ProductViewModel viewModel = new ProductViewModel
            {
                Product = new Product(),
                Currencies = _entities.Currencies.ToList(),
				Categories = _entities.ProductCategories.ToList(),
            };

            viewModel.Product.CurrencyCode = "USD";
            viewModel.Product.UnitPrice = (Decimal)0;
            viewModel.Product.Name = "[Please input product name]";

            return View(viewModel);
        } 

        //
        // POST: /Product/Create

		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
		public ActionResult Create(FormCollection collection, Product product, string Upload)
        {
            try
            {

                if (Upload != null)
                {
					ImageUploadToServer(product.ID);

                    ProductViewModel viewModel = new ProductViewModel
                    {
                        Product = product,
                        Currencies = _entities.Currencies.ToList(),
						Categories = _entities.ProductCategories.ToList(),
                    };
                    return View(viewModel);
                }

				AddUploadFile(collection, product);

                _entities.AddToProducts(product);
                _entities.SaveChanges();

				product = _entities.Products.OrderByDescending(a=>a.ID).First();
				ProductDetail pd = new ProductDetail();
				product.ProductDetails.Add(pd);
				_entities.SaveChanges();

				return RedirectToAction("index", "ProductDetail", new {productID = product.ID });

				//return RedirectToAction("Index");
            }
            catch
            {
                ProductViewModel viewModel = new ProductViewModel
                {
                    Product = product,
                    Currencies = _entities.Currencies.ToList(),
					Categories = _entities.ProductCategories.ToList(),
                };
                return View(viewModel);
            }

        }

		public void AddUploadFile(FormCollection collection, Product product)
		{
			int startNo = 8;
			string temp = collection.GetKey(startNo);
			int index = temp.LastIndexOf('.');
			int uploadFileID = 0;
			if (index > 0 && int.TryParse(temp.Substring(index + 1), out uploadFileID))
			{
				string temp1 = collection.Get(temp);
				if (temp.StartsWith("Product.&.") && temp1.StartsWith("true"))
				{
					RemoveUploadFile(product);
	
					ProductUploadFile puf = new ProductUploadFile();
					puf.UploadFileID = uploadFileID;
					product.ProductUploadFiles.Add(puf);
				}
				else if (temp.StartsWith("Product.") && temp1.StartsWith("false"))
				{
					RemoveUploadFile(product);
				}
			}
		}

		private void RemoveUploadFile(Product product)
		{
			if (product.ProductUploadFiles.Count() > 0)
			{
				ProductUploadFile puf1 = _entities.ProductUploadFiles.SingleOrDefault(a => a.ProductID == product.ID);
				int uploadFileID = puf1.UploadFileID;
				_entities.DeleteObject(puf1);
				if (_entities.ProductUploadFiles.Where(a => a.UploadFileID == uploadFileID).Count() == 1)
				{
					UploadFile uploadFile = _entities.UploadFiles.SingleOrDefault(a => a.ID == uploadFileID);
					_entities.DeleteObject(uploadFile);
					//HttpPostedFileBase file = Request.Files["fileUpload"];
					//file.
				}
			}
		}

		public void ModelStateSetting(List<UploadFile> uploadFiles, string ControllerName)
		{
			foreach (UploadFile uploadFile in uploadFiles)
			{
				ModelState.SetModelValue(ControllerName + ".&." + uploadFile.ID.ToString(), new ValueProviderResult("True", "", CultureInfo.InvariantCulture));
			}
		}

        //
        // GET: /Product/Edit/5

		[Authorize(Roles = "Administrator")]
		public ActionResult Edit(int id)
        {
            var product = _entities.Products.Single(a => a.ID == id);

			//if (product.ProductUploadFiles != null)
			//{
			//    List<UploadFile> uploadFiles = product.ProductUploadFiles.Where(a=>a.ProductID == id).Select(a => a.UploadFile).ToList();
			//    ModelStateSetting(uploadFiles, "Product");
			//    //ModelStateSetting(product.AttachFileCollectionID1, "Product");
			//    //ViewData["AttachFileCollectionIDID"] = product.AttachFileID;
			//}

            ProductViewModel viewModel = new ProductViewModel
            {
                Product = product,
                Currencies = _entities.Currencies.ToList(),
				Categories = _entities.ProductCategories.ToList(),
            };

            return View(viewModel);
        }

        //
        // POST: /Product/Edit/5

		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
		public ActionResult Edit(int id, FormCollection collection, string Upload)
        {
            var product = _entities.Products.Single(a => a.ID == id);
  
            try
            {
                if (Upload != null)
                {
                    ImageUploadToServer(product.ID);

					UpdateModel(product, "Product");

                    ProductViewModel viewModel = new ProductViewModel
                    {
                        Product = product,
                        Currencies = _entities.Currencies.ToList(),
						Categories = _entities.ProductCategories.ToList(),
                    };
                    return View(viewModel);
                }

				AddUploadFile(collection, product);
			
				UpdateModel(product, "Product");
                _entities.SaveChanges();

				return RedirectToAction("index", "ProductDetail", new { productID = product.ID });
				//return RedirectToAction("Index");
            }
            catch
            {
                ProductViewModel viewModel = new ProductViewModel
                {
                    Product = product,
                    Currencies = _entities.Currencies.ToList(),
					Categories = _entities.ProductCategories.ToList(),
                };
                return View(viewModel);
            }
        }

		private UploadFile ImageUploadToServer(int productID)
        {
            HttpPostedFileBase file = Request.Files["fileUpload"];
            if (string.IsNullOrEmpty(file.FileName))
            {
                throw new Exception("Please select a file!");
            }

			string savePath = "../images/SMProducts";
			if (productID > 0)
			{
				savePath = "../" + savePath;
			}
			string filePath = Path.Combine(HttpContext.Server.MapPath(savePath), Path.GetFileName(file.FileName));
			ImageController ic = new ImageController();
			UploadFile uploadFile = ic.ImageUpload(file, filePath, savePath);
			ViewData["CurrentUploadFile"] = uploadFile;
			return uploadFile;
         }

		//
        // GET: /Product/Delete/5

		[Authorize(Roles = "Administrator")]
		public ActionResult Delete(int id)
        {
            ProductOrderDetail productOrder = _entities.ProductOrderDetails.SingleOrDefault(a => a.ProductID == id);
            if (productOrder != null)
            {
                TempData["ProductStatus"] = "This product was used by another user, so it is impossible to delete.";
            }
            return View();
        }
        
        //
        // POST: /Product/Delete/5

        //[HttpPost]
		[AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Administrator")]
		public ActionResult Delete(int id, FormCollection collection)
        {
            if (id != 0)
            {
                var product = _entities.Products.Single(a => a.ID == id);

                _entities.DeleteObject(product);

                _entities.SaveChanges();
            }
            return View("Deleted");
        }


		public ActionResult UpdateProducts()
		{

			//for (int poNo = 56; poNo <= 58; poNo++)
			//{
			//    ProductOrder po = _entities.ProductOrders.Single(a => a.ID == poNo);
			//    int categoryID = 1;
			//    if (poNo == 57)
			//        categoryID = 3;
			//    else if (poNo == 58)
			//        categoryID = 4;
			//    List<Product> pAll = _entities.Products.Where(a => a.CategoryID == categoryID && a.UpdateDate > updateTime).ToList();
			//    foreach (Product p in pAll)
			//    {
			//        if (!_entities.ProductOrderDetails.Any(a => a.ProductOrderID == poNo && a.ProductID == p.ID && a.Product.UpdateDate > updateTime))
			//        {
			//            ProductOrderDetail pod = new ProductOrderDetail();
			//            pod.ProductID = p.ID;
			//            po.ProductOrderDetails.Add(pod);
			//            _entities.SaveChanges();
			//        }
			//    }
			//}


			//return View();
	
			List<Product> product1s = _entities.Products.Where(a => a.UpdateDate > new DateTime(2012,2,1) && a.NewProduct).ToList();
			foreach (Product p in product1s)
			{
				p.NewProduct = false;
				_entities.SaveChanges();
			}
			return View();

            //List<Product> products = _entities.Products.Where(a => a.ItemCode != null).OrderBy(a => a.ItemCode).ThenBy(a => a.ID).ToList();

            //Product product = new Product();
            //List<Product> deleteProducts = new List<Product>();
            //foreach (Product tempProduct in products)
            //{
            //    if (tempProduct.ItemCode != product.ItemCode)
            //    {
            //        product = tempProduct;
            //    }
            //    else
            //    {
            //        if (!string.IsNullOrEmpty(tempProduct.QuantityPerUnit))
            //        {

            //            product.UpdateDate = DateTime.Today;

            //            List<ProductDetail> tempProductDetails = tempProduct.ProductDetails.ToList();
            //            List<ProductDetail> productDetails = product.ProductDetails.ToList();
            //            foreach (ProductDetail tempProductDetail in tempProductDetails)
            //            {
            //                if (productDetails.Any(a => a.SizeDescription == tempProductDetail.SizeDescription))
            //                {
            //                    ProductDetail productDetail = productDetails.FirstOrDefault(a => a.SizeDescription == tempProductDetail.SizeDescription);
            //                    productDetail.UnitsInStock = tempProductDetail.UnitsInStock;
            //                    productDetail.UpdateDate = DateTime.Today;
            //                }
            //                else
            //                {
            //                    ProductDetail productDetail = new ProductDetail();
            //                    productDetail.SizeDescription = tempProductDetail.SizeDescription;
            //                    productDetail.UnitsInStock = tempProductDetail.UnitsInStock;
            //                    productDetail.UpdateDate = DateTime.Today;
            //                    product.ProductDetails.Add(productDetail);
            //                }
            //            }
            //            if (tempProduct.ProductDiscounts.Count() > 0)
            //            {
            //                foreach (ProductDiscount tProductDiscount in tempProduct.ProductDiscounts.ToList())
            //                {
            //                    ProductDiscount pd = new ProductDiscount();
            //                    pd.Discount = tProductDiscount.Discount;
            //                    pd.DateFrom = tProductDiscount.DateFrom;
            //                    pd.DateTo = tProductDiscount.DateTo;
            //                    product.ProductDiscounts.Add(pd);
            //                }
            //            }
            //            deleteProducts.Add(tempProduct);
            //        }
            //        _entities.SaveChanges();
            //    }

			//}
            //
			//foreach (Product p in deleteProducts)
			//{
			//	_entities.DeleteObject(p);
			//	_entities.SaveChanges();
			//}
            //
            //
			//return View();


			//foreach (TempProductDiscount tempProductDiscount in _entities.TempProductDiscounts.ToList())
			//{
			//    string itemCode = _entities.TempProducts.Single(a => a.ID == tempProductDiscount.ProductID).ItemCode;
			//    Product product = _entities.Products.Single(a => a.ItemCode == itemCode);
			//    ProductDiscount productDiscount = new ProductDiscount();
			//    productDiscount.DateFrom = tempProductDiscount.DateFrom;
			//    productDiscount.DateTo = tempProductDiscount.DateTo;
			//    productDiscount.Discount = tempProductDiscount.Discount;
			//    product.ProductDiscounts.Add(productDiscount);
			//    _entities.SaveChanges();
			//}
			//return View();

			////263	O6711G3 not unique,
			//List<TempProduct> tempProducts = _entities.TempProducts.Where(a=>a.ID >=263).ToList();
			//List<Product> products = _entities.Products.ToList();

			//foreach (TempProduct tempProduct in tempProducts)
			//{
			//    Product product = new Product();
			//    string itemCode = tempProduct.ItemCode.Trim();
			//    if (products.Any(a => a.ItemCode == itemCode))
			//    {
			//        product = products.FirstOrDefault(a => a.ItemCode == tempProduct.ItemCode);
			//        product.UpdateDate = DateTime.Today;

			//        List<ProductDetail> productDetails = product.ProductDetails.ToList();
			//        List<TempProductDetail> tempProductDetails = tempProduct.TempProductDetails.ToList();
			//        foreach (TempProductDetail tempProductDetail in tempProductDetails)
			//        {
			//            if (productDetails.Any(a => a.SizeDescription == tempProductDetail.SizeDescription))
			//            {
			//                ProductDetail productDetail = productDetails.FirstOrDefault(a => a.SizeDescription == tempProductDetail.SizeDescription);
			//                productDetail.UnitsInStock = tempProductDetail.UnitsInStock;
			//                productDetail.UpdateDate = DateTime.Today;
			//            }
			//            else
			//            {
			//                AddProductDetailToProduct(product, tempProductDetail);
			//            }
			//        }
			//    }
			//    else
			//    {
			//        product.CategoryID = tempProduct.CategoryID;
			//        product.CurrencyCode = tempProduct.CurrencyCode;
			//        product.Description = tempProduct.Description;
			//        product.ItemCode = tempProduct.ItemCode;
			//        product.Name = tempProduct.Name;
			//        product.NameChi = tempProduct.NameChi;
			//        product.NewProduct = tempProduct.NewProduct;
			//        product.QuantityPerUnit = tempProduct.QuantityPerUnit;
			//        product.UnitPrice = tempProduct.UnitPrice;
			//        product.UpdateDate = DateTime.Today;
			//        product.QuantityPerUnit = tempProduct.ID.ToString();

			//        List<TempProductDetail> tempProductDetails = tempProduct.TempProductDetails.ToList();
			//        foreach (TempProductDetail tempProductDetail in tempProductDetails)
			//        {
			//            AddProductDetailToProduct(product, tempProductDetail);
			//        }
			//    }

			//    AddDiscountTable(tempProduct, product);
			//    if (product.ID == 0)
			//    {
			//        _entities.AddToProducts(product);
			//    }
			//    _entities.SaveChanges();

			//}

			//return View();
		}

		private static void AddProductDetailToProduct(Product product, TempProductDetail tempProductDetail)
		{
			ProductDetail productDetail = new ProductDetail();
			productDetail.SizeDescription = tempProductDetail.SizeDescription;
			productDetail.UnitsInStock = tempProductDetail.UnitsInStock;
			productDetail.UpdateDate = DateTime.Today;
			product.ProductDetails.Add(productDetail);
		}


		private void AddDiscountTable(TempProduct tempProduct, Product product)
		{
			// add discount table
			if (_entities.TempProductDiscounts.Any(a => a.ProductID == tempProduct.ID))
			{
				TempProductDiscount tempProductDiscount = _entities.TempProductDiscounts.FirstOrDefault(a => a.ID == tempProduct.ID);
				ProductDiscount productDiscount = new ProductDiscount();
				productDiscount.DateFrom = tempProductDiscount.DateFrom;
				productDiscount.DateTo = tempProductDiscount.DateTo;
				productDiscount.Discount = tempProductDiscount.Discount;
				product.ProductDiscounts.Add(productDiscount);
			}
		}
    }
}
