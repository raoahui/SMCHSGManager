<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SMCHSGManager.Models.Location>" %>

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
  
   <script type="text/javascript">
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

    </script>
  
    
 <div class="fullwidth">         
  
   <% Html.EnableClientValidation(); %>
    <% using (Html.BeginForm((string)ViewData["Mode"], "Location", FormMethod.Post, new { enctype = "multipart/form-data" })){%>
   <%-- <% using (Html.BeginForm()) {%>--%>
        <%: Html.ValidationSummary(true) %>

                    <table style="border:0" >
                         <tr><td colspan=2 class="formlabel">
                             <%if (ViewData["Mode"] == "Create")
                               { %>
                                 <h1>New Location</h1>
                                 <%}
                               else
                               { %>
                                   <h1>Edit Location</h1>
                                <%} %>
                       </td></tr>

                       <tr><td colspan=2 class="formlabel">
                            <div class="dashedline"></div>
                       </td></tr>

                        <tr>
                            <td class="formlabel">
                                <font color="red" size="2">*</font>Location Name:
                            </td>
                            <td align="left" class="formvalue">
                                <%: Html.TextBoxFor(model => model.Name, new { style="width:100%x;" })%>  
                                <%: Html.ValidationMessageFor(model => model.Name) %>
                             </td>
                        </tr>
                        <tr>
                            <td class="formlabel">
                                Link:
                            </td>
                            <td align="left" class="formvalue">
                                     <%: Html.TextBoxFor(model => model.LinkURL, new { style = "width:95%;" })%>
                                     <%: Html.ValidationMessageFor(model => model.LinkURL) %>
                            </td>
                        </tr>
                        <tr>
                            <td class="formlabel">
                                Description</td>
                            <td align="left" class="formvalue">
                               <%-- <%: Html.TextAreaFor(model => model.Description, new { style = "height: 7em; width:650px; " })%>   
                                <%: Html.ValidationMessageFor(model => model.Description) %>--%>
  								<%=Html.TextArea("Description", Model.Description, new { @name = "Editor1", style = "width:100%; height:100%" })%>
                          </td>
                        </tr>
                        <tr>
                            <td class="formlabel">
                                Directions</td>
                            <td align="left" class="formvalue">
                                 <%: Html.TextAreaFor(model => model.Directions, new {style = "height: 7em; width:650px; " })%>   
                                  <%= Html.ValidationMessageFor(model => model.Directions)%>
                            </td>
                        </tr>
                        <tr>
                            <td class="formlabel">
                                Address</td>
                            <td align="left" class="formvalue">
                                <%: Html.TextAreaFor(model => model.Address, new { style = "height: 4em; width:650px; " })%>   
                                    <%= Html.ValidationMessageFor(model => model.Address)%>
                            </td>
                        </tr>

                        <tr><td colspan=2 class="formlabel">
                            <div class="dashedline"></div>
                        </td></tr>

                      
                        <tr>
                             <td  class="formlabel">
                                  <a id="ImageUploadTrigger" href="#" > <font color="red" size="2">*</font>Photo or File Upload</a>
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
                       <tr>
                            <td  colspan="2" class="formvalue">
                                  <a id="ImageSelectTrigger" href="#" > <font color="red" size="2">*</font>Photo or File Select</a>&nbsp;
                                  <div id ="ImageSelect"><%: Html.Editor("&", "ImageSelectAll") %></div>
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


