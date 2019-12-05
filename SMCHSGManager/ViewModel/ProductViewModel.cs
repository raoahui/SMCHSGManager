using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMCHSGManager.ViewModel;
using SMCHSGManager.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SMCHSGManager.ViewModel
{

    public class ProductViewModel
    {
        public Product Product { get; set; }
        public List<Currency> Currencies { get; set; }
		public List<ProductCategory> Categories { get; set; }
    }
 
    public class ProductOrderViewModel
    {
		public ProductOrder ProductOrder { get; set; }
        public List<Product> Products { get; set; }
		public List<int> ProductOrderIDs { get; set; }
		public int MemberOrderID { get; set; }

     }

 
	public class ProductDetailViewModel
	{
		public Product Product { get; set; }
		public ProductDiscount ProductDiscount { get; set; }
		public decimal ReadPrice { get; set; }
		public List<string> Sizes { get; set; }
		public UploadFile UploadFile { get; set; }

	}

	public class ProductDetailListViewModel
	{
		public ProductDetail ProductDetail { get; set; }
		public OrderStatus OrderStatus { get; set; }
		public DateTime? SubmitDateTime { get; set; }
		//public int ProductID { get; set; }
		//public string ItemCode { get; set; }
		//public string ProductName { get; set; }
		//public string SizeDes { get; set; }
		//public decimal? UnitPrice { get; set; }
		//public string CurrencyCode { get; set; }
		//public float Discount { get; set; }
		public int OrderTotalNumber { get; set; }
		//public UploadFile UploadFile { get; set; }
		
	}

	public class NamesByProductDetailViewModel
	{
		public string name {get; set;}
		public int qty {get; set;}
	}

	public class ProductDiscountViewModel
	{
		public ProductDiscount ProductDiscount { get; set; }
		public List<Product> Products { get; set; }
	}

	public class YouTubePlayerListViewModel
	{
		public int smtvVideoID { get; set; }
		public int partNo { get; set; }
		public int channelID { get; set; }

		public string title { get; set; }
		public string youTubeTitle { get; set; }
		public string duration { get; set; }
		public string thumbnailURL { get; set; }
		public IEnumerable<String> youtubeVideoIDs { get; set; }

	}

	public class YouTubePlayerViewModel
	{
		public string title { get; set; }
		public string youTubeTitle { get; set; }
		public string videoID { get; set; }
		public DateTime update { get; set; }

		public int viewCount { get; set; }
		public string author { get; set; }

		public string description { get; set; }
		public string tag { get; set; }
		public string prev { get; set; }
		public string next { get; set; }
		public string scriptDownloadLink { get; set; }
		public string videoDownloadLink { get; set; }

	}
}