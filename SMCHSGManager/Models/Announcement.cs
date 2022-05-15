using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SMCHSGManager.Models
{
    [MetadataType(typeof(AnnouncementMetaDate))]
    public partial class Announcement
    {
          [Bind(Exclude = "ID")]
        public class AnnouncementMetaDate
        {
            [ScaffoldColumn(false)]
            public int ID { get; set; }

            //[DisplayName("Announcer Name")]
            //[Required(ErrorMessage = "Announcer Name is required")]
            //public Guid AnnouncerID { get; set; }

            [Required(ErrorMessage = "Announce Date Time is required")]
            [DisplayName("Announcer Date")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy HH:mm}")]
            public DateTime AnnounceDate { get; set; }

            [Required(ErrorMessage = "Announce Title is required")]
            [DisplayName("Title")]
            [StringLength(200)]
            public String Name { get; set; }

            [StringLength(20000)]
            public String Description { get; set; }

            [StringLength(800)]
            public String StaticURL { get; set; }

          }
    }
}