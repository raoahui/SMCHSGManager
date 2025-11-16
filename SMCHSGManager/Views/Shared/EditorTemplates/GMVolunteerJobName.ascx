<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SMCHSGManager.Models.GMVolunteerJobName>" %>

  <div class="fullwidth">         
  
    <% Html.EnableClientValidation(); %>
    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>

                    <table style="border:0" >
                         <tr><td colspan=2 class="formlabel">
                             <%if (ViewData["Mode"] == "Create")
                               { %>
                                 <h1>New   <%: ViewData["VolunteerJobTypeName"]%> </h1>
                                <%} %>
                       </td></tr>

                       <tr><td colspan=2 class="formlabel">
                            <div class="dashedline"></div>
                       </td></tr>

                       <tr>
                            <td class="formlabel">
                            Name
                            </td>
                            <td align="left" class="formvalue3">
							<% if (ViewData["Mode"] == "Create")
							 { %>
							     <%: Html.DropDownList("MemberID", new SelectList(ViewData["MemberInfo"] as IEnumerable, "MemberID", "Name", Model.MemberID))%> 
                              <%}
							else
							 { %>
							 <%: Model.MemberInfo.Name %>
							  <%} %>
                             </td>
                        </tr>
		
                           <tr><td colspan=2 class="formlabel">
                            <div class="dashedline"></div>
                       </td></tr>
 
                       <tr>
                            <td  colspan=2 class="formlabel">
                             <div class="actionbuttons">
                                <% if (ViewData["Mode"] == "Create")
                                { %>
                                 <input type="submit"  value="Create" name="Create" class ="buttonsmall" /> &nbsp;
                                   <%}else{ %>
                                  <input type="submit"  value="Save"  name="Save" class ="buttonsmall" /> &nbsp;
                                   <%} %>
                                <%: Html.ActionLink("Back to List", "Index", new { volunteerJobTypeID = Model.VolunteerJobTypeID },  new { @style = "color:white; ", @class = "buttonsmall" })%>
                               </div>
                       </td></tr>

                    </table>

     <% } %>
</div>

