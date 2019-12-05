﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SMCHSGManager.Models.MemberFeePayment>" %>

 <div class="fullwidth">         

           <table style="border:0" >
                         <tr><td colspan="4" class="formlabel">
                             <%if (ViewData["Mode"] == "Create")
                               { %>
									<h1>New Member Fee Payment</h1>
                                 <%}else {
                                     %>
                                   <h1>Edit Member Fee Payment</h1>
                                <%} %>
                       </td></tr>

                       <tr><td colspan=2 class="formlabel">
                            <div class="dashedline"></div>
                       </td></tr>

                        <tr>
                          <%if (ViewData["Mode"] == "Create")
                          { %>
							<td class="formlabel"><font color="red" size="2">*</font>Member Name</td>
                            <td class="formvalue" > 
 								<%: Html.DropDownList("IMemberID", (List<SelectListItem>)ViewData["MemberInfos"])%> 
                            </td>
 							<%}else{ %>
							<td class="formlabel">Member Name</td>
                            <td class="formvalue">  <%: Model.MemberInfo.Name %> </td>
  						   <%} %>
                         </tr>
   
                            <%if (ViewData["Mode"] == "Edit")
                               { %>
                           <tr>
                            <td class="formlabel">Member No</td>
                            <td class="formvalue" >
                                <%: Model.MemberInfo.MemberNo%>
                            </td>
                          </tr>
                             <%} %>

                          <tr>
                             <td class="formlabel" valign="middle"> <font color="red" size="2">*</font>FromDate  
							 </td>
                             <td valign="top" class="formvalue">
                                    <%: Html.EditorFor(model => model.FromDate, "Date")%>    
                                    <%= Html.ValidationMessageFor(model => model.FromDate)%>
                             </td>
                         </tr>
                             
                         <tr>
                            <td class="formlabel1"> <font color="red" size="2">*</font>ToDate</td>
                            <td class="formvalue">
                                    <%: Html.EditorFor(model => model.ToDate, "Date")%>    
                                    <%= Html.ValidationMessageFor(model => model.ToDate)%>
                            </td>
                        </tr>

                         <tr>
                            <td class="formlabel"><font color="red" size="2">*</font>PayAmount</td>
                            <td  class="formvalue">
                                    <%: Html.EditorFor(model => model.PayAmount)%>    
                                    <%= Html.ValidationMessageFor(model => model.PayAmount)%>
                            </td>
                         </tr>
                             
                        <tr>
                            <td class="formlabel">
                                <font color="red" size="2">*</font>Pay Method</td>
                            <td class="formvalue">
                                <%: Html.DropDownList("PayMethodID", new SelectList(ViewData["PayMethods"] as IEnumerable, "ID", "Name", Model.PayMethodID))%> 
                            </td>
                        </tr>
                         
                         <tr>
                            <td class="formlabel">Remark</td>
							<td class="formvalue" >
                                    <%: Html.TextBoxFor(model => model.Remark, new { style = "width:100%;", })%>    
                                    <%: Html.ValidationMessageFor(model => model.Remark) %>
                            </td>
                         </tr>
 
	                   <tr><td colspan=2 class="formlabel">
                            <div class="dashedline"></div>
                       </td></tr>
 
                       <%--<tr>
                            <td align="Center" colspan=2 class="formlabel">
                            
                                <% if (ViewData["Mode"] == "Create")
                                { %>
                                 <input type="submit"  value="Create" name="Create" class ="buttonsmall" /> &nbsp;
                                 <%}else{ %>
                                  <input type="submit"  value="Save"  name="Save" class ="buttonsmall" /> &nbsp;
                                  <%} %>
                                 <%: Html.ActionLink("Back to List", "Index", null, new { @style = "color:white; ", @class = "buttonsmall" })%>
                             
                       </td></tr>--%>

                    </table>

</div>     

