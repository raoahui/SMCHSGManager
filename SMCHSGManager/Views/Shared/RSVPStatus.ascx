<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SMCHSGManager.Models.Event>" %>

<script src="/Scripts/MicrosoftAjax.js" type="text/javascript"></script>
<script src="/Scripts/MicrosoftMvcAjax.js" type="text/javascript"></script>    

<script type="text/javascript">

    function AnimateRSVPMessage() {
        $("#rsvpmsg").animate({ fontSize: "1.5em" }, 400);
    }

</script>
    
<div id="rsvpmsg">

<% if (Request.IsAuthenticated) { %>

    <% SMCHSGManager.Models.SMCHDBEntities _entities = new SMCHSGManager.Models.SMCHDBEntities ();
        MembershipUser muc = Membership.GetUser();
        DateTime today = DateTime.Today.ToUniversalTime().AddHours(8);
		DateTime now = DateTime.Now.ToUniversalTime().AddHours(8);
        int eventRegisterID = (from r in _entities.EventRegistrations
                              where r.EventID == Model.ID && r.MemberInfo.MemberID == (Guid)muc.ProviderUserKey
                              select r.ID).FirstOrDefault();
        if (eventRegisterID > 0)
        { %>      
            You are already registered for this event. 
            <% if ((Model.EventTypeID == 1 || Model.EventTypeID == 5) && Page.User.IsInRole("Initiate"))
               {%>
               <%-- <%: Html.ActionLink("Details", "Details", "LocalRetreatRegistration", new { id = eventRegisterID }, new { @style = "color:white;", @class = "buttonsmall" })%> --%>
                <%: Html.ActionLink("Details", "Details", "EventRegistration", new { id = eventRegisterID }, new { @style = "color:white;", @class = "buttonsmall" })%> 
           <%} %>
    <% }
        else if (Model.RegistrationOpenDate <= now && now <= Model.RegistrationCloseDate)
        {
                if ((Model.EventTypeID == 1 || Model.EventTypeID == 5) && Page.User.IsInRole("Initiate"))
                {%>
                        <%: Html.ActionLink("Register", "Create", "EventRegistration", new { eventID = Model.ID }, new { @style = "color:white;", @class = "buttonsmall" })%> 
                        Please Register By <%: string.Format("{0: ddd, MMM d yyyy HH:mm}", Model.RegistrationCloseDate)%>
                        <%if (Model.EventTypeID == 1)
                          { %>
                            </br></br>1.	Initiates need to sign up even if only attending the first session
                            </br>2.	Half-initiates or initiates who are sick are NOT allowed to join the retreat
                        <%} %>
                <%}
                else
                {%>   
                        <%: Ajax.ActionLink("RSVP for this event", "Register", "Event", new { id = Model.ID }, new AjaxOptions { UpdateTargetId = "rsvpmsg", OnSuccess = "AnimateRSVPMessage" })%>         
                <% }
        }%>
    <% } 
else 
{ %>
   Please  <%: Html.ActionLink("Login", "LogOn", "Account", null, new { @style = "font-weight:bolder;" })%> to register this event!
<% } %>
    
</div>    