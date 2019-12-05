<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SMCHSGManager.Models.ProductDetail>" %>

 <div class="fullwidth">         
  
   <% Html.EnableClientValidation(); %>
		<% using (Html.BeginForm()) {%>
        <%: Html.ValidationSummary(true) %>

                    <table style="border:0" >
                         <tr><td colspan=2 class="formlabel">
                             <%if (ViewData["Mode"] == "Create")
                               { %>
                                 <h1>New Product Detail</h1>
                                 <%}
                               else
                               { %>
                                   <h1>Edit Product Detail</h1>
                                   
                               <%} %>
                       </td></tr>

                       <tr><td colspan=2 class="formlabel">
                            <div class="dashedline"></div>
                       </td></tr>

						<tr>
							<td class="formlabel"> Product Name	</td>
							
							<td class="formlabel"> 
								<%  if(ViewData["Mode"] == "Create"){ %>
									<%: ViewData["Name"] %>
								<%}else{ %>
									<%: Model.Product.Name %>
								<%} %>
							</td>
						</tr>

						<tr>
							<td class="formlabel"> </td>
							<td class="formlabel"> 
								<%  if(ViewData["Mode"] == "Create"){ %>
									<%: ViewData["NameChi"] %>
								<%}else{ %>
									<%: Model.Product.NameChi %>
								<%} %>
							</td>
						</tr>


                        <tr>
                            <td class="formlabel">
                                <font color="red" size="2">*</font>Size Description:
                            </td>
                            <td align="left"  class="formvalue3">  
                                <%: Html.TextBoxFor(model => model.SizeDescription, new { style = "width:100%;"})%>  
                                <%: Html.ValidationMessageFor(model => model.SizeDescription) %>
                             </td>
                        </tr>
 
 
                        <tr>
                            <td class="formlabel">
                                Units In Stock:
                            </td>
                            <td align="left" class="formvalue">
                                <%: Html.TextBoxFor(model => model.UnitsInStock, new { style="width:50%;" })%>  
                                <%: Html.ValidationMessageFor(model => model.UnitsInStock)%>
                             </td>
                        </tr>
 
                          <tr>
                            <td class="formlabel">
                                Units On Order:
                            </td>
                            <td align="left" class="formvalue">
                                <%: Html.TextBoxFor(model => model.UnitsOnOrder, new { style="width:50%;" })%>  
                                <%: Html.ValidationMessageFor(model => model.UnitsOnOrder)%>
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

   

