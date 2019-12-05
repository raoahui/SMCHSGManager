using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Web;

namespace SMCHSGManager.Models
{
	[MetadataType(typeof(InternationalGMApplicationTransportInfoMetaDate))]
	public partial class InternationalGMApplicationTransportInfo
    {
        [Bind(Exclude = "ID")]
		public class InternationalGMApplicationTransportInfoMetaDate
        {
            [ScaffoldColumn(false)]
            public int ID { get; set; }

			[Required(ErrorMessage = "DateTime is required")]
			[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy HH:mm}")]
			public DateTime DateTime { get; set; }
			//[Required(ErrorMessage = "Arrival DateTime is required")]
			//[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy HH:mm}")]
			//public DateTime ArrivalDateTime { get; set; }

			//[Required(ErrorMessage = "Departure DateTime is required")]
			//[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy HH:mm}")]
			//public DateTime DepartureDateTime { get; set; }
  
        }
    }


}