using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SMCHSGManager.Models
{
	[MetadataType(typeof(ProductDiscountMetaDate))]
    public partial class ProductDiscount
    {
        // Validation rules for the MemberInfo class

        [Bind(Exclude = "ID")]
		public class ProductDiscountMetaDate
        {
            [ScaffoldColumn(false)]
            public object ID { get; set; }

            [Required(ErrorMessage = "Date From is required")]
            [DisplayName("Date From")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy}")]
            public DateTime DateFrom { get; set; }

            [Required(ErrorMessage = "Date To is required")]
            [DisplayName("Date To")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy}")]
            public DateTime DateTo { get; set; }

        }
    }


 


}