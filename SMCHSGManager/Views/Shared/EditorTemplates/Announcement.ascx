<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SMCHSGManager.Models.Announcement>" %>

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
    <% using (Html.BeginForm((string)ViewData["Mode"], "Announcement", FormMethod.Post, new { enctype = "multipart/form-data" })){%>
        <%: Html.ValidationSummary(true) %>

                    <table style="border:0" >
                         <tr><td colspan=2 class="formlabel">
                             <%if (ViewData["Mode"] == "Create")
                               { %>
                                 <h1>New Announcement</h1>
                                 <%}
                               else
                               {
                                   %>
                                   <h1>Edit Announcement</h1>
                                <%} %>
                       </td></tr>

                       <tr><td colspan=2 class="formlabel">
                            <div class="dashedline"></div>
                       </td></tr>

                       <tr>
                            <td class="formlabel">
                                <font color="red" size="2">*</font>Announcement Title:
                            </td>
                            <td align="left" class="formvalue3" >
                                <%: Html.TextBoxFor(model => model.Name, new { style = "width:100%;", @class="title" })%>  
                                <%: Html.ValidationMessageFor(model => model.Name) %>
                             </td>
                        </tr>

                        <tr>
                            <td class="formlabel">
                                <font color="red" size="2">*</font>Is Public?
                            </td>
                            <td align="left" class="formvalue3">
                                <%: Html.CheckBoxFor(model => model.IsPublic)%>  
                                <%: Html.ValidationMessageFor(model => model.IsPublic) %>
                             </td>
                        </tr>

                        <tr>
	                        <td class="formlabel">
                                Link:
                            </td>
                            <td align="left" class="formvalue3">
                             <%-- <%: Html.CheckBox("URLChecked") %>  <font face="Arial" color="#2A4013" size="1">Use a link instead of inline content for this announcement. </font></br>
                                 URL:--%>
                                     <%: Html.TextBoxFor(model => model.StaticURL, new { style = "width:100%;" })%>
                                     <%: Html.ValidationMessageFor(model => model.StaticURL) %>
                            </td>
                        </tr>

                        <tr>
                            <td class="formlabel">
                                Description </td>
                            <td align="left" class="formvalue3">
                               <%--  <%: Html.TextAreaFor(model => model.Description, new {@class = "text-box multi-line", style = "width:650px; " })%>   
                                <%: Html.ValidationMessageFor(model => model.Description) %>--%>
								<%=Html.TextArea("Description", Model.Description, new { @name = "Editor1", style = "width:100%; height:320px" })%>
                            </td>
                        </tr>

                        <tr>
                            <td class="formlabel"> Release Date</td>
                            <td align="left" class="formvalue3">
                                    <%: Html.EditorFor(model => model.AnnounceDate)%>    
                                    <%= Html.ValidationMessageFor(model => model.AnnounceDate)%>
                                    <font face="Arial" color="#2A4013" size="1">The new announcement will not be visible to users until after this date.</font>
                            </td>
                        </tr>

                        <tr><td colspan=2 class="formlabel">
                            <div class="dashedline"></div>
                        </td></tr>
                        <tr>
                              <td  class="formlabel">
                                  <a id="ImageUploadTrigger" href="#" > Photo or File Upload</a>
                              </td>
                              <td class="formvalue">
                                  <div id ="ImageUpload">
                                    <input type="file" name="fileUpload" size="20" style="width: 262px"/>  
                                    <input type="submit" value="Upload" name="Upload" class ="buttonsmall" />
                                  </div>
                                   <%: Html.Editor("&", "ImageSelect")%>
                           </td>
                        </tr>
   
                       <tr><td colspan=2 class="formlabel">
                            <div class="dashedline"></div>
                       </td></tr>
                       <tr>
                            <td  colspan="2" class="formvalue">
                                  <a id="ImageSelectTrigger" href="#" > Photo or File Select</a>&nbsp;
								  <div id ="ImageSelect"><%: Html.Editor("&", "ImageSelectAll")%></div>
                            </td>
                     </tr>

                       <tr><td colspan=2 class="formlabel">
                            <div class="dashedline"></div>
                       </td></tr>
 
                       <tr><td  colspan=2 class="formlabel">
                             <div class="actionbuttons">
                                <% if (ViewData["Mode"] == "Create")
                                {
                                    TempData["AnnounceGroupID"] = ViewData["AnnounceGroupID"];  %>
                                 <input type="submit"  value="Create" name="Create" class ="buttonsmall" /> &nbsp;
                                <%: Html.ActionLink("Back to List", "Index", new { announceGroupID = (int)ViewData["AnnounceGroupID"] }, new { @style = "color:white; ", @class = "buttonsmall" })%>
                                  <%}else{ %>
                                  <input type="submit"  value="Save"  name="Save" class ="buttonsmall" /> &nbsp;
                                 <%: Html.ActionLink("Back to List", "Index", new { announceGroupID = Model.AnnounceGroupID }, new { @style = "color:white; ", @class = "buttonsmall" })%>
                                  <%} %>
                                <%-- <%: Html.ActionLink("Back to List", "Index", new { announceGroupID = (int)ViewData["AnnounceGroupID"] }, new { @style = "color:white; ", @class = "buttonsmall" })%>--%>
                               </div>
                       </td></tr>

                    </table>

     <% } %>
</div>

