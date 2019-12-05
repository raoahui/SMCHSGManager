<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SMCHSGManager.Models.Product>" %>

<script type="text/javascript">
    $(document).ready(function () {
        $('#ImageSelect').hide();

        $('a#ImageSelectTrigger').click(function () {
            $('#ImageSelect').toggle();
        });
    });

    $(document).ready(function () {
        $('#ImageUpload').hide();

        $('a#ImageUploadTrigger').click(function () {
            $('#ImageUpload').toggle();
        });
    });


    $(document).ready(function () {

        $('#fileUploadSection').jqm({ modal: true,
            ajax: '<%: Url.Action("FileUpload", "Image") %>',
            onHide: myAddClose
        });

        function myAddClose(hash) {
            hash.w.fadeOut('1000', function () { hash.o.remove(); });
        }
    }); 

</script>
    
	<%--    <script type="text/javascript">
	    	tinyMCE.init({
	    		mode: "textareas",
	    		theme: "advanced",

	    		//
	    		//  Theme options #1
	    		//
	    		theme_advanced_buttons1: "bold,italic,underline,formatselect,fontselect,fontsizeselect,forecolor,backcolor,|,justifyleft,justifycenter,justifyright,justifyfull,|,bullist,numlist",
	    		theme_advanced_buttons2: "",
	    		theme_advanced_buttons3: "",
	    		theme_advanced_buttons4: "",
	    		theme_advanced_toolbar_location: "top",
	    		theme_advanced_toolbar_align: "left",
	    		theme_advanced_resizing: true,
	    		readonly: false
	    	});

	    	$(function () {
	    		$("#texteditorform").dialog({
	    			height: 300,
	    			width: 520,
	    			autoOpen: false
	    		});
	    	});

	    	function ShowModalEditor() {
	    		$("#texteditorform").dialog('open');
	    	}

    </script>--%>

 <div class="fullwidth">         
  
   <% Html.EnableClientValidation(); %>
    <% using (Html.BeginForm((string)ViewData["Mode"], "Product", FormMethod.Post, new { enctype = "multipart/form-data" }))
       {%>
   
        <%: Html.ValidationSummary(true) %>

                    <table style="border:0" >
                         <tr><td colspan=2 class="formlabel">
                             <%if (ViewData["Mode"] == "Create")
                               { %>
                                 <h1>New Product</h1>
                                 <%}
                               else
                               { %>
                                   <h1>Edit Product</h1>
                                   
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
                                <%: Html.TextBoxFor(model => model.Name, new { style = "width:100%;", @class = "title" })%>  
                                <%: Html.ValidationMessageFor(model => model.Name) %>
                             </td>
                        </tr>
 
                        <tr>
                            <td class="formlabel">
                               Product Name (Chinese):
                            </td>
                            <td align="left" class="formvalue">
                                <%: Html.TextBoxFor(model => model.NameChi, new { style = "width:100%;", @class = "title" })%>  
                                <%: Html.ValidationMessageFor(model => model.NameChi)%>
                             </td>
                        </tr>
 
                        <tr>
                            <td class="formlabel">Category:  </td>
                            <td align="left" class="formvalue">
                                <%: Html.DropDownList("CategoryID", new SelectList(ViewData["Categories"] as IEnumerable, "ID", "Name", Model.CategoryID))%>  
                             </td>
                        </tr>

                        <tr>
                            <td class="formlabel">Item Code: </td>
                            <td align="left" class="formvalue">
                                <%: Html.TextBoxFor(model => model.ItemCode, new { style = "width:50%;", @class = "title" })%>  
                                <%: Html.ValidationMessageFor(model => model.ItemCode)%>
                             </td>
                        </tr>

                        <tr>
                            <td class="formlabel">
                                Quantity Per Unit:
                            </td>
                            <td align="left" class="formvalue">
                                <%: Html.TextBoxFor(model => model.QuantityPerUnit, new { style="width:50%;" })%>  
                                <%: Html.ValidationMessageFor(model => model.QuantityPerUnit)%>
                             </td>
                        </tr>
 
                          <tr>
                            <td class="formlabel">
                               Currency:
                            </td>
                            <td align="left" class="formvalue">
                                <%: Html.DropDownList("CurrencyCode", new SelectList(ViewData["Currencies"] as IEnumerable, "CODE", "Name", Model.CurrencyCode))%>  
                             </td>
                        </tr>

                        <tr>
                            <td class="formlabel">
                                <font color="red" size="2">*</font>Unit Price:
                            </td>
                            <td align="left" class="formvalue">
                               <%-- <%: Html.TextBoxFor(model => model.UnitPrice) %>--%>
                                <%: Html.TextBoxFor(model => model.UnitPrice, String.Format(Model.CurrencyCode + "{0:n}", Model.UnitPrice)) %>
                                <%: Html.ValidationMessageFor(model => model.UnitPrice)%>
                             </td>
                        </tr>
 
                  <%--      <tr>
                            <td class="formlabel">
                                Discount
                            </td>
                            <td align="left" class="formvalue">
                                <%: Html.TextBoxFor(model => model.Discount) %>
                                <%: Html.ValidationMessageFor(model => model.Discount)%>
                             </td>
                        </tr>--%>
                 
                        <tr>
                            <td class="formlabel">
                                Description</td>
                            <td align="left" class="formvalue">
                                <%: Html.TextAreaFor(model => model.Description, new { style = "height: 7em; width:650px; " })%>   
                                <%: Html.ValidationMessageFor(model => model.Description) %>
								<%--<%=Html.TextArea("Description", Model.Description, new { @name = "Editor1", style = "width:100%; height:100%" })%>--%>
                            </td>
                        </tr>

                        <tr><td colspan=2 class="formlabel">
                            <div class="dashedline"></div>
                        </td></tr>

					    <tr>
                             <td  class="formlabel">
                                  <a id="ImageUploadTrigger" href="#" > Photo Upload (need new photo)</a>
                              </td>
                              <td class="formvalue">
                                  <div id ="ImageUpload">
                                    <input type="file" name="fileUpload" size="20" style="width: 262px"/>  
                                    <input type="submit" value="Upload" name="Upload" class ="buttonsmall" />
                                  </div>
                                    <%: Html.Editor("&", "ImageSelect") %>
                            </td>
                        </tr>
						
		
                       <tr><td colspan=2 class="formlabel">
                            <div class="dashedline"></div>
                       </td></tr>
 
  				<%  if(Model.ProductUploadFiles.Count > 0){ %>
                          <tr>
							<td  class="formlabel"> Photo Uploaded</td>
							<td class="formvalue">
								<% SMCHSGManager.Models.UploadFile uploadFile = Model.ProductUploadFiles.FirstOrDefault().UploadFile;
									if (uploadFile.ContentType.StartsWith("image")){
										string srcImage = uploadFile.FilePath + uploadFile.Name; %>
									 <img src="<%: srcImage %>" height="72" alt="" />
								 <%} %>
								 <%: uploadFile.Name%>
								 <%= Html.CheckBox(uploadFile.ID.ToString(), true)%>
    						</td>
					    </tr>
						<tr><td colspan=2 class="formlabel">
                            <div class="dashedline"></div>
						</td></tr>
               <% } %>

                     <tr>
                            <td  colspan=2 class="formlabel">
                             <div class="actionbuttons">
                                <% if (ViewData["Mode"] == "Create")
                                { %>
                                 <input type="submit"  value="Create & next" name="Create" class ="buttonsmall" /> &nbsp;
                                 <%}else{ %>
                                  <input type="submit"  value="Save & next"  name="Save" class ="buttonsmall" /> &nbsp;
                                  <%} %>
                                 <%: Html.ActionLink("Back to List", "Index", null, new { @style = "color:white; ", @class = "buttonsmall" })%>
                               </div>
                       </td></tr>

                    </table>

     <% } %>
</div>


