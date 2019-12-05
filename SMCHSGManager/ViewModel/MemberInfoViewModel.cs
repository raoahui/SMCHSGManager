using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMCHSGManager.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SMCHSGManager.ViewModel
{
	public class MemberOnlineInfo
	{
		public Guid ID { get; set; }
		//public string Name { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public string InitiateStatus { get; set; }
		public bool IsOnline { get; set; }
		public DateTime LastActivityDate { get; set; }
		public bool IsActive { get; set; }

	}

	public class PublicMemberInfo
	{
		public Guid ID { get; set; }
		public string UserName { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }

        public int InitiateTypeID { get; set; }
        public string InitiateType { get; set; }

		public int? MemberNo { get; set; }

		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy}")]
		public DateTime? MemberFeeExpiredDate { get; set; }

		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy}")]
		public DateTime? DateOfInitiation { get; set; }

		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy}")]
		public DateTime? DateOfBirth { get; set; }

		public string CountryOfBirth { get; set; }
		public string PassportNo { get; set; }
		public string Remark { get; set; }

		public string IDCardNo { get; set; }
		public string ContactNo { get; set; }
		public int GenderID { get; set; }
		public string Gender { get; set; }
		public bool IsActive { get; set; }
	}

	public class MemberInfoShortListViewModel
	{
		public Guid ID { get; set; }
		public string Name { get; set; }
		public string MemberType { get; set; }
		public int? MemberNo { get; set; }
		public DateTime? MemberFeeExpiredDate { get; set; }
		public string IDCardNo { get; set; }
		public int? Count { get; set; }
		public decimal? Money { get; set; }
	}

	public class LocalRetreatRegisterList
	{
		public int No { get; set; }
		public string Name { get; set; }
		public string IDCardNo { get; set; }
		public string ContactNo { get; set; }
		public string VolunteerJobs { get; set; }
		public string MealBooking { get; set; }
		public decimal? Money { get; set; }
		public string Remark { get; set; }
	}

	//public class MemberCheckInfo
	//{
	//    public Guid ID { get; set; }
	//    public int MemberID { get; set; }
	//    public DateTime CheckInDateTime { get; set; }
	//}

	public class OrdinaryMemberViewModel
	{
		public OrdinaryMemberInfo OrdinaryMemberInfo { get; set; }
		public PublicMemberInfo PublicMemberInfo { get; set; }
		//public List<Nationality> Nationality { get; set; }
		public List<Gender> Gender { get; set; }
		//public List<Race> Race { get; set; }
		//public List<EmploymentStatus> EmploymentStatus { get; set; }
		public List<PayMethod> PayMethod { get; set; }
        public List<InitiateType> InitiateType { get; set; }
	}

	public class MemberFeePaymentListViewModel
	{
		public Guid IMemberID { get; set; }
		public string Name { get; set; }
		public int MemberNo { get; set; }

		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy}")]
		public DateTime FromDate { get; set; }

		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy}")]
		public DateTime ToDate { get; set; }

		public decimal PayAmount { get; set; }
        public string PaymentMethod { get; set; }

 		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy}")]
		public DateTime ReceievedDate { get; set; }
	}

    public class GMAttendanceMonthCount
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy}")]
        public DateTime FromDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy}")]
        public DateTime ToDate { get; set; }

        public int Count { get; set;}

        public bool MeetRequire { get; set;}
    }

    public class GMAttendanceCountViewModel
    {
		//public Guid IMemberID { get; set; }
		//public string Name { get; set; }
		//public int MemberNo { get; set; }
		public MemberInfo MemberInfo { get; set; }

        public List<GMAttendanceMonthCount> GMAttendanceMonthCounts { get; set; }
		public bool MeetRequire { get; set; }
        public int TotalCount { get; set;}
    }

	public class MemberFeePaymentViewModel
	{
		public MemberFeePayment MemberFeePayment { get; set; }
        public List<SelectListItem> MemberInfos { get; set; }
		//public List<MemberInfo> MemberInfo { get; set; }
		public List<PayMethod> PayMethod { get; set; }
	}

	public class DPRosterViewModel
	{
		public string[,] MonthDpList { get; set; }
		public List<List<MemberInfo>> WeekNoDPLists { get; set; }
		public int NextMonth { get; set; }
		public string NextMonthStr { get; set; }
		public bool Edit { get; set; }
		public bool HavePreviousMonth { get; set; }
		public bool HaveNextMonth { get; set; }

	}

	public class GMAttendanceViewModel
	{
		public List<GroupMeditation> nextMonthGMs { get; set; }
		public List<MemberInfo> Members { get; set; }
		public int NextMonth { get; set; }
		public string NextMonthStr { get; set; }
		public bool HavePreviousMonth { get; set; }
		public bool HaveNextMonth { get; set; }
		public bool[,] AttendenceChecks { get; set; }
	}

	public class InternationalGMApplicationViewModel
	{
		public InternationalGMApplicationInfo InternationalGMApplicationInfo { get; set; }
		public List<AshramAndCenterInfo> AshramAndCenterInfos { get; set; }
		public List<InternationalGMApplicationTransportInfo> TransportInfos { get; set; }
		//public InternationalGMApplicationTransportInfo InBoundTransportInfo { get; set; }
		//public InternationalGMApplicationTransportInfo OutBoundTransportInfo { get; set; }
		public List<InternationalTransport> TransportStations { get; set; }
		public HsihuAshramEventModel HsihuAshramEvent { get; set; }
	}

	public class BlackListMemberViewModel
	{
		public BlackListMember BlackListMember { get; set; }
		public List<SelectListItem> MemberInfos { get; set; }
		public List<IDCardType> IDCardTypes { get; set; }
	}

	public class InternationalGMApplicationInfo1
	{
		public InternationalGMApplicationInfo InternationalGMApplicationInfo { get; set; }
		//public int ID { get; set; }
		//public string Name { get; set; }
		//public string AshramName { get; set; }
		//public DateTime ArrivalDate { get; set; }
		//public DateTime DepartureDate { get; set; }
		//public DateTime ApplyDate { get; set; }
		//public string Remark {get; set;}
		//public int? ParentID { get; set; }
		public string ApplicationStatus {get; set;}
	}


	public class ApplicationPanangForm
	{
		public string Center {get; set;}
		public string Country {get; set;}
		public string Email {get; set;}
		public string Name { get; set; }
		public string IDCardNo {get; set;}
		public string Gender {get; set;}
		public string InitiateType { get; set; }

		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy}")]
		public DateTime? DateOfBirth { get; set; }

		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy}")]
		public DateTime? DateOfInitiation { get; set; }


		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy}")]
		public DateTime ArrivalDate { get; set; }

		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy}")]
		public DateTime DepartureDate { get; set; }

		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy HH:mm}")]
		public DateTime ApplicationDate { get; set; }

		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy HH:mm}")]
		public DateTime? PenangReplyDate { get; set; }

		public string Remark { get; set; }
		public bool? AccomodationNeeded { get; set; }

		public string CountryOfBirth { get; set; }

		public string InBoundStationName { get; set; }
		public string InBoundFlightNo { get; set; }
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy HH:mm}")]
		public DateTime InBoundDateTime { get; set; }

		public string OutBoundStationName { get; set; }
		public string OutBoundFlightNo { get; set; }
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d MMM yyyy HH:mm}")]
		public DateTime OutBoundDateTime { get; set; }
		
	}

	public class HsihuAshramEventModel
	{
		public List<DateTime> TwoDaysRetreats { get; set; }
		public List<DateTime> SundayGMs { get; set; }
		public List<bool> TwoDaysRetreatCheckeds { get; set; }
		public List<bool> SundayGMCheckeds { get; set; }
		public int AshramID { get; set; }
		public string Remark { get; set; }

	}

	public class SelectListModel
	{
		public int ID { get; set; }
		public Guid GID { get; set; }
		public string Name { get; set; }
	}

    ////for datagrid:
    //public interface IMemberInfosRepository
    //{
    //    IQueryable<InitiateMemberInfo> InitiateMemberInfos { get; set; }
    //}

    //public class MemberInfosRepositoryEF : IMemberInfosRepository
    //{
 
    //    private SMCHDBEntities _db = new SMCHDBEntities();

    //    public IQueryable<InitiateMemberInfo> InitiateMemberInfos
    //    {
    //        get
    //        {
    //            return _db.InitiateMemberInfos;
    //        }
    //        set { }
    //    }
    // }

 
}