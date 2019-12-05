<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SMCHSGManager.Models.BlackListMember>" %>

 <div class="fullwidth">         
  
   <% Html.EnableClientValidation(); %>
		<% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>

                    <table style="border:0" >
                        <tr><td colspan=2 class="formlabel">
                             <%if (ViewData["Mode"] == "Create")
                               { %>
                                   <h1>New
                               <%}
                               else
                               { %>
                                   <h1>Edit 
                               <%} %>
							   Red ID Card Record</h1>
                        </td></tr>

                        <tr><td colspan=2 class="formlabel">
                            <div class="dashedline"></div>
                        </td></tr>

						<tr>
							<td class="formlabel" >Name</td>
							<td class="formvalue">
								<%: Html.DropDownList("MemberD", (List<SelectListItem>)ViewData["MemberInfos"]) %> 
								<%--<%: Html.DropDownList("MemberD", new SelectList(ViewData["MemberInfos"] as IEnumerable, "MemberID", "Name", Model.MemberD))%> --%>
							</td>
						</tr>

						<tr>
							<td class="formlabel" >ID Card Type</td>
							<td class="formvalue">
								<%: Html.DropDownList("IDCardTypeID", new SelectList(ViewData["IDCardTypes"] as IEnumerable, "ID", "Name", Model.ID))%> 
							</td>
						</tr>

   					<tr>
						<td class="formlabel" nowrap="nowrap">Date From</td>
						<td class="formvalue"><%: Html.EditorFor(model => model.DateFrom, "Date")%></td>
 					</tr>

  					<tr>
						<td class="formlabel" nowrap="nowrap">Date To</td>
						<td class="formvalue"><%: Html.EditorFor(model => model.DateTo, "Date")%></td>
  					</tr>

  					<tr>
						<td class="formlabel" nowrap="nowrap">Remark</td>
						<td class="formvalue">	<%: Html.TextBoxFor(model => model.Remark)%> </td>
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
                                 <%: Html.ActionLink("Back to List", "Index", null, new { @style = "color:white; ", @class = "buttonsmall" })%>
                               </div>
                       </td></tr>

                    </table>

     <% } %>

</div>

