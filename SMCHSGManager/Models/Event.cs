using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web;

namespace SMCHSGManager.Models
{
    [MetadataType(typeof(EventMetaDate))]
    public partial class Event
    {
        [Bind(Exclude = "ID")]
        public class EventMetaDate
        {
            [ScaffoldColumn(false)]
            public int ID { get; set; }

            [DisplayName("Organiser Name")]
            public Guid OrganizerNameID { get; set; }

            [Required(ErrorMessage = "An Title is required")]
            [StringLength(50)]
            public String Title { get; set; }

            //[DateStart] 
            [Required(ErrorMessage = "Start Date Time is required")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy HH:mm}")]
            public DateTime StartDateTime { get; set; }

            //[DateEnd(DateStartProperty = "StartDateTime")] 
            [Required(ErrorMessage = "End Date Time is required")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy HH:mm}")]
            public DateTime EndDateTime { get; set; }

            [Required(ErrorMessage = "Registration Open Date required")]
            [DisplayName("Registration Open Date")]
			//[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy}")]
			[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy HH:mm}")]
            public DateTime? RegistrationOpenDate { get; set; }

            [Required(ErrorMessage = "Registration Close Date required")]
            [DisplayName("Registration Close Date")]
			//[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy}")]
			[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy HH:mm}")]
            public DateTime? RegistrationCloseDate { get; set; }


            //[StringLength(1200)]
            [StringLength(20000)]
            public String Description { get; set; }

            [StringLength(800)]
            public String StaticURL { get; set; }

            //public sealed class DateStartAttribute : ValidationAttribute
            //{
            //    public override bool IsValid(object value)
            //    {
            //        DateTime dateStart = (DateTime)value;
            //        // Cannot shedule meeting back in the past. 
            //        return (dateStart > DateTime.Now);
            //    }
            //}

            //public sealed class DateEndAttribute : ValidationAttribute
            //{
            //    public string DateStartProperty { get; set; }
            //    public override bool IsValid(object value)
            //    {
            //        // Get Value of the DateStart property 
            //        //Request.ServerVariables("HTTP_REFERER")
            //        string dateStartString = HttpContext.Current.Request[DateStartProperty];
            //        DateTime dateEnd = (DateTime)value;
            //        DateTime dateStart = DateTime.Parse(dateStartString);

            //        // Meeting start time must be before the end time 
            //        return dateStart < dateEnd;
            //    }
            //} 

        }


        [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
        public class EndDateAttribute : ValidationAttribute
        {
            public EndDateAttribute(DateTime endDate)
            {
                EndDate = endDate;
            }

            public DateTime EndDate { get; set; }

            public override bool IsValid(object value)
            {
                if (value == null)
                    return false;

                DateTime val;
                try
                {
                    val = (DateTime)value;
                }
                catch (InvalidCastException)
                {
                    return false;
                }

                if (val >= EndDate)
                    return false;

                return true;
            }

        }


    }
}