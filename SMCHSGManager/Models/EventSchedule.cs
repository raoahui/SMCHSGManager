using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SMCHSGManager.Models
{
    [MetadataType(typeof(EventScheduleMetaDate))]
    public partial class EventSchedule
    {
        // Validation rules for the EventSchedule class

        [Bind(Exclude = "ID")]
        public class EventScheduleMetaDate
        {
            [ScaffoldColumn(false)]
            public int ID { get; set; }

            [DisplayName("Event Activity Name")]
            public int EventActivityID { get; set; }

            [Required(ErrorMessage = "Start Date Time is required")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy HH:mm}")]
            public DateTime? DateTimeFrom { get; set; }

            //[Required(ErrorMessage = "End Date Time is required")]
            //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:g}")]
            //public DateTime? DateTimeTo { get; set; }

         }
    }
}