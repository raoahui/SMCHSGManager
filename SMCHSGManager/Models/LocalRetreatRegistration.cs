using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SMCHSGManager.Models
{
    [MetadataType(typeof(LocalRetreatRegistrationMetaDate))]
    public partial class LocalRetreatRegistration
    {
        // Validation rules for the MemberInfo class

        [Bind(Exclude = "ID")]
        public class LocalRetreatRegistrationMetaDate
        {
            [ScaffoldColumn(false)]
            public int ID { get; set; }

            [DisplayName("Local Retreat Name")]
            public int LocalRetreatID { get; set; }

            //[DisplayName("Name")]
            //public Guid MemberInfoID { get; set; }

            [Required(ErrorMessage = "Leaving Time is required")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy HH:mm}")]
            public DateTime? BackDateTime { get; set; }

        }
    }
}