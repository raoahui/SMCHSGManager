using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SMCHSGManager.Models
{
	[MetadataType(typeof(MemberInfoMetaDate))]
	public partial class MemberInfo
	{
		// Validation rules for the MemberInfo class

		[Bind(Exclude = "MemberID")]
		public class MemberInfoMetaDate
		{
			[ScaffoldColumn(false)]
			public object MemberID { get; set; }

			[Required(ErrorMessage = "An Name is required")]
			[StringLength(50)]
			public String Name { get; set; }

			//[DateOfInitiation] 
			[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy}")]
			public DateTime DateOfInitiation { get; set; }

			[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy}")]
			public DateTime MemberFeeExpiredDate { get; set; }

			[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy}")]
			public DateTime DateOfBirth { get; set; }
		}

	}
	
}