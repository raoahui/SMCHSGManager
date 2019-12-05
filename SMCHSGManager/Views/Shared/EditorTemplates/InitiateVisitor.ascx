<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SMCHSGManager.Models.InitiateVisitor>" %>

<head id="Head1" runat="server">
  <script src="<%= Url.Content("~/Scripts/MicrosoftAjax.debug.js") %>" type="text/javascript"></script>
  <script src="<%= Url.Content("~/Scripts/MicrosoftMvcAjax.debug.js") %>" type="text/javascript"></script>
  <script src="<%= Url.Content("~/Scripts/MicrosoftMvcValidation.debug.js") %>" type="text/javascript"></script>
  <% Html.EnableClientValidation();%>
</head> 

<div class="fullwidth">         
 
 
    <% Html.EnableClientValidation(); %>
    <% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>

                    <table style="border:0" >
                         <tr><td colspan=2 class="formlabel">
                             <%if (ViewData["Mode"] == "Create")
                               { %>
                                 <h1>New Initiate Visitor</h1>
                                 <%}
                               else
                               {
                                   %>
                                   <h1>Edit Initiate Visitor</h1>
                                <%} %>
                       </td></tr>

                       <tr><td colspan=2 class="formlabel">
                            <div class="dashedline"></div>
                       </td></tr>

                       <tr>
                            <td class="formlabel">
                                <font color="red" size="2">*</font>Name
                            </td>
                            <td align="left" class="formvalue3" >
                                <%: Html.TextBoxFor(model => model.Name)%>  
                                <%: Html.ValidationMessageFor(model => model.Name) %>
                             </td>
                        </tr>

                        <tr>
                            <td class="formlabel">
                           Brother/Sister
                            </td>
                            <td align="left" class="formvalue3">
                                 <%: Html.DropDownList("GenderID", new SelectList(ViewData["Genders"] as IEnumerable, "ID", "Name", Model.GenderID))%> 
                                <%: Html.ValidationMessageFor(model => model.GenderID)%>
                             </td>
                        </tr>
    
	                   <tr>
    	                        <td class="formlabel">
                                 <font color="red" size="2">*</font> Date Of Initiation
                            </td>
                            <td align="left" class="formvalue3">
                                      <%: Html.EditorFor(model => model.DateOfInitiation, "Date")%>
                                     <%: Html.ValidationMessageFor(model => model.DateOfInitiation)%>
                            </td>
                        </tr>

                        <tr>
                            <td class="formlabel">
                                ID Card No
                            </td>
                            <td align="left" class="formvalue3" >
                                <%: Html.TextBoxFor(model => model.IDCardNo)%>  
                                <%: Html.ValidationMessageFor(model => model.IDCardNo)%>
                             </td>
                        </tr>

                       <tr>
                            <td class="formlabel">
                                <font color="red" size="2">*</font>DateFrom </td>
                            <td align="left" class="formvalue3">
                                 <%: Html.EditorFor(model => model.DateFrom, "Date")%>   
                                <%: Html.ValidationMessageFor(model => model.DateFrom)%>
                            </td>
                        </tr>

                        <tr>
                            <td class="formlabel">
                                <font color="red" size="2">*</font>From Where
                            </td>
                            <td align="left" class="formvalue3" >
                                <%: Html.TextBoxFor(model => model.FromWhere)%>  
                                <%: Html.ValidationMessageFor(model => model.FromWhere)%>
                             </td>
                        </tr>

                       <tr>
                            <td class="formlabel">
                                DateTo </td>
                            <td align="left" class="formvalue3">
                                 <%: Html.EditorFor(model => model.DateTo, "Date")%>   
                                <%: Html.ValidationMessageFor(model => model.DateTo)%>
                            </td>
                        </tr>

                         <tr>
                            <td class="formlabel">
                                Remark
                            </td>
                            <td align="left" class="formvalue3" >
                                <%: Html.TextBoxFor(model => model.Remark)%>  
                                <%: Html.ValidationMessageFor(model => model.Remark)%>
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
                                <%: Html.ActionLink("Back to List", "Index", null, new { @style = "color:white; ", @class = "buttonsmall" })%>
                               </div>
                       </td></tr>

                    </table>

     <% } %>
</div>

