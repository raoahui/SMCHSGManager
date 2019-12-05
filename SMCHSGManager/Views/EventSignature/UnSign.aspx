<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.Event>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
SMCH Association Singapore - 	Event UnSign
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

   
<script src="/Scripts/MicrosoftAjax.js" type="text/javascript"></script>
<script src="/Scripts/MicrosoftMvcAjax.js" type="text/javascript"></script>    

<script type="text/javascript">

    function AnimateRSVPMessage() {
        $("#rsvpmsg").animate({ fontSize: "1.5em" }, 400);
    }

</script>
    
<div id="body" align="center">

   <h5> Name : <font color="red"><%: Membership.GetUser(Model.OrganizerNameID).UserName%></font></h5> 

   <h3>Are you going to unsign :
 <% if (Model.EventTypeID == 1)
   { %>
    <%: Model.Title + ' ' + Model.EventType.Name%>
   <%}
   else
   { %>
   <%:  Model.EventType.Name + ' ' + String.Format("{0:ddd, d MMM yyyy  }", Model.StartDateTime) + String.Format("{0:HH:mm}", Model.StartDateTime) + " to " + String.Format("{0:HH:mm}", Model.EndDateTime)%>
   <%} %>
   </h3>

      <br />
      <br />
  <br />

   <div id="rsvpmsg">
      <%: Ajax.ActionLink("Confirm", "UnSigned", new { ID = Model.OrganizerNameID, eventID = Model.ID }, new AjaxOptions { UpdateTargetId = "rsvpmsg", OnSuccess = "AnimateRSVPMessage" }, new { @style = "color:white;", @class = "buttonsearch" })%>         
   </div>  
  
   <br />
   <br />
  <br />
  <br />
  <br />

        <% if (Model.EventTypeID == 1 || Model.EventTypeID == 2)
           { %>
            <%: Html.ActionLink("Click here", "SignatureList4GMAndLocalRetreat", new { eventTypeID = Model.EventTypeID, eventID = Model.ID }, null)%>
            <%}
           else
           { %>
            <%: Html.ActionLink("Click here", "SignatureList", new { eventTypeID = Model.EventTypeID }, null)%>
            <%} %>
            to return to the Event Signature List.
  
        
      </div>


</asp:Content>
