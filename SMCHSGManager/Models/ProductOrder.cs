using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SMCHSGManager.Models
{
	[MetadataType(typeof(ProductOrderMetaDate))]
    public partial class ProductOrder
    {
        // Validation rules for the MemberInfo class

        [Bind(Exclude = "ID")]
		public class ProductOrderMetaDate
        {
            [ScaffoldColumn(false)]
            public object ID { get; set; }

            [Required(ErrorMessage = "Order Open Date is required")]
            [DisplayName("Order Open Date")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy}")]
            public DateTime OrderOpenDate { get; set; }

            [Required(ErrorMessage = "Order Close Date is required")]
            [DisplayName("Order Close Date")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy}")]
            public DateTime OrderCloseDate { get; set; }

        }
    }


 


}