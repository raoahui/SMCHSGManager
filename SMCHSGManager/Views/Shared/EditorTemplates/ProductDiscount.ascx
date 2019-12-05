<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SMCHSGManager.Models.ProductDiscount>" %>

 <div class="fullwidth">         
  
   <% Html.EnableClientValidation(); %>
    <% using (Html.BeginForm()) {%>
   
        <%: Html.ValidationSummary(true) %>

                    <table style="border:0" >
                          <tr><td colspan=2 class="formlabel">
                             <%if (ViewData["Mode"] == "Create")
                               { %>
                                 <h1>New Product Discount</h1>
                                 <%}
                               else
                               { %>
                                   <h1>Edit Product Discount</h1>
                               <%} %>
                        </td></tr>

                        <tr><td colspan=2 class="formlabel">
                            <div class="dashedline"></div>
                        </td></tr>

                        <tr>
                            <td class="formlabel">
                                <font color="red" size="2">*</font>Product Name:
                            </td>
                            <td align="left" class="formvalue">
							   <% 
									if (ViewData["Mode"] == "Create"){ %>
										<%: Html.DropDownList("ProductID", new SelectList(ViewData["Products"] as IEnumerable, "ID", "ItemCode", Model.ProductID))%> 
									<%}else{ %>
										<%: Model.Product.ItemCode %>
									<%} %>
                             </td>
                        </tr>
    
                        <tr>
                            <td class="formlabel">
                                Discount
                            </td>
                            <td align="left" class="formvalue">
                                <%: Html.TextBoxFor(model => model.Discount) %>
                                <%: Html.ValidationMessageFor(model => model.Discount)%>
                             </td>
                        </tr>
                 
                        <tr>
                             <td class="formlabel"><font color="red" size="2">*</font>Date From</td>
                            <td align="left" class="formvalue">
                                    <%: Html.EditorFor(model => model.DateFrom, "Date")%>    
                                    <%= Html.ValidationMessageFor(model => model.DateFrom)%>
                            </td>
                        </tr>
                 
                        <tr>
                            <td class="formlabel"><font color="red" size="2">*</font>Date To</td>
                            <td align="left" class="formvalue">
                                    <%: Html.EditorFor(model => model.DateTo, "Date")%>    
                                    <%= Html.ValidationMessageFor(model => model.DateTo)%>
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

  

