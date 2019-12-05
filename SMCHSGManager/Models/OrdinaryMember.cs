using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SMCHSGManager.Models
{
    [MetadataType(typeof(OrdinaryMemberInfoMetaDate))]
 	public partial class OrdinaryMemberInfo
	{
	
	// Validation rules for the MemberInfo class

		[Bind(Exclude = "IMemberID")]
		public class OrdinaryMemberInfoMetaDate
		{
			[ScaffoldColumn(false)]
			public object IMemberID { get; set; }

			//[Required(ErrorMessage = "An Name is required")]
			//[StringLength(50)]
			//public String Name { get; set; }

			//[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy}")]
			//public DateTime DateOfBirth { get; set; }

			[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy}")]
			public DateTime MemberApplyDate { get; set; }

			[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy}")]
			public DateTime MemberEffectiveStartDate { get; set; }

		}
	}
}