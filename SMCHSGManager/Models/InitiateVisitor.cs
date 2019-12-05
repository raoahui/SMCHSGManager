using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web;

namespace SMCHSGManager.Models
{
    [MetadataType(typeof(InitiateVisitorMetaDate))]
	public partial class InitiateVisitor
    {
        [Bind(Exclude = "ID")]
		public class InitiateVisitorMetaDate
        {
            [ScaffoldColumn(false)]
            public int ID { get; set; }

            [Required(ErrorMessage = "An Name is required")]
            [StringLength(50)]
            public String Name { get; set; }

            //[DateFrom] 
            [Required(ErrorMessage = "Date From is required")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy}")]
            public DateTime DateFrom { get; set; }

            //[DateEnd(DateStartProperty = "StartDateTime")] 
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy}")]
			public DateTime DateTo { get; set; }

			[Required(ErrorMessage = "Date Of Initiation is required")]
			[DisplayName("Date Of Initiation")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy}")]
			public DateTime DateOfInitiation { get; set; }

            [StringLength(300)]
			[Required(ErrorMessage = "FromWhere is required")]
            public String FromWhere { get; set; }

      

      

        }


   

    }
}