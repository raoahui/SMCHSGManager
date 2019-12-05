using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SMCHSGManager.Models
{
    [MetadataType(typeof(LocationMetaDate))]
    public partial class Location
    {
        [Bind(Exclude = "ID")]
        public class LocationMetaDate
        {
            [ScaffoldColumn(false)]
            public int ID { get; set; }

            [DisplayName("Announcer Name")]
            [Required(ErrorMessage = "Location Name is required")]
            public string Name { get; set; }

			//[Required(ErrorMessage = "Photo is required, minimum one photo")]
			//[DisplayName("Photo")]
			//public int AttachFileCollectionIDID { get; set; }

            [StringLength(800)]
            public String Description { get; set; }

            [StringLength(800)]
            public String LinkURL { get; set; }

            [StringLength(1000)]
            public String Directions { get; set; }

            [StringLength(300)]
            public String Address { get; set; }

         }
    }
}