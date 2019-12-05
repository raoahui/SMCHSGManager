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
                                 <%}
                               else
                               {
                                   %>
                                   <h1>Edit  <%: ViewData["VolunteerJobTypeName"]%></h1>
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
		
		   <tr>
    	                    <td class="formlabel">
                                 <%: Html.LabelFor(model=>model.Monday) %>
                            </td>
                            <td align="left" class="formvalue3">
								 <%: Html.CheckBoxFor(model=>model.Monday)%>
							     <%: Html.ValidationMessageFor(model=>model.Monday) %>
                            </td>
                        </tr>

 	                   <tr>
    	                    <td class="formlabel">
                                  <%: Html.LabelFor(model=>model.Tuesday) %>
                            </td>
                            <td align="left" class="formvalue3">
								 <%: Html.CheckBoxFor(model => model.Tuesday)%>
							     <%: Html.ValidationMessageFor(model => model.Tuesday)%>
                            </td>
                        </tr>

	                   <tr>
    	                    <td class="formlabel">
                                 <%: Html.LabelFor(model=>model.WednesdayOvernight) %>
                            </td>
                            <td align="left" class="formvalue3">
								 <%: Html.CheckBoxFor(model => model.WednesdayOvernight)%>
							     <%: Html.ValidationMessageFor(model => model.WednesdayOvernight)%>
                            </td>
                        </tr>

	                   <tr>
    	                    <td class="formlabel">
                                 <%: Html.LabelFor(model=>model.Thursday) %>
                            </td>
                            <td align="left" class="formvalue3">
								 <%: Html.CheckBoxFor(model=>model.Thursday)%>
							     <%: Html.ValidationMessageFor(model=>model.Thursday)%>
                            </td>
                        </tr>

		                   <tr>
    	                    <td class="formlabel">
                                 <%: Html.LabelFor(model=>model.Friday) %> (CM)
                            </td>
                            <td align="left" class="formvalue3">
								 <%: Html.CheckBoxFor(model => model.Friday)%>
							     <%: Html.ValidationMessageFor(model=>model.Friday)%>
                            </td>
                        </tr>

						<tr>
    	                    <td class="formlabel">
                                 <%: Html.LabelFor(model=>model.SaturdayDay) %>
                            </td>
                            <td align="left" class="formvalue3">
								 <%: Html.CheckBoxFor(model => model.SaturdayDay)%>
							     <%: Html.ValidationMessageFor(model => model.SaturdayDay)%>
                            </td>
                        </tr>

						<tr>
    	                    <td class="formlabel">
                                 <%: Html.LabelFor(model=>model.SaturdayEvening) %>
                            </td>
                            <td align="left" class="formvalue3">
								 <%: Html.CheckBoxFor(model => model.SaturdayEvening)%>
							     <%: Html.ValidationMessageFor(model => model.SaturdayEvening)%>
                            </td>
                        </tr>

	                   <tr>
    	                    <td class="formlabel">
                                 <%: Html.LabelFor(model=>model.SaturdayOvernight) %>
                            </td>
                            <td align="left" class="formvalue3">
								 <%: Html.CheckBoxFor(model=>model.SaturdayOvernight)%>
							     <%: Html.ValidationMessageFor(model=>model.SaturdayOvernight)%>
                            </td>
                        </tr>

	                   <tr>
    	                    <td class="formlabel">
                                 <%: Html.LabelFor(model=>model.Sunday) %>
                            </td>
                            <td align="left" class="formvalue3">
								 <%: Html.CheckBoxFor(model=>model.Sunday)%>
							     <%: Html.ValidationMessageFor(model=>model.Sunday)%>
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

