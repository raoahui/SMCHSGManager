using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SMCHSGManager.Models
{
	[MetadataType(typeof(MemberFeePaymentMetaDate))]
	public partial class MemberFeePayment
	{
		  // Validation rules for the MemberInfo class
        public static DateTime ToDateGiro = new DateTime(3000, 1, 1);

		[Bind(Exclude = "IMemberID")]
		public class MemberFeePaymentMetaDate
		{
			[ScaffoldColumn(false)]
			public object IMemberID { get; set; }

			//[Required(ErrorMessage = "An Name is required")]
			//[StringLength(50)]
			//public String Name { get; set; }

			//[DateFrom] 
			[Required(ErrorMessage = "Date From is required")]
			[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy}")]
			public DateTime FromDate { get; set; }

			[Required(ErrorMessage = "Date To is required")]
			[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy}")]
			public DateTime ToDate { get; set; }
		}

	}

}