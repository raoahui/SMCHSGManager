using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web;

namespace SMCHSGManager.Models
{
    [MetadataType(typeof(InternationalGMApplicationInfoMetaDate))]
	public partial class InternationalGMApplicationInfo
    {
        [Bind(Exclude = "ID")]
		public class InternationalGMApplicationInfoMetaDate
        {
            [ScaffoldColumn(false)]
            public int ID { get; set; }

 
            [Required(ErrorMessage = "Arrival Date is required")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy}")]
            public DateTime ArrivalDate { get; set; }

			[Required(ErrorMessage = "Departure Date is required")]
			[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy}")]
			public DateTime DepartureDate { get; set; }
  
        }
    }


}