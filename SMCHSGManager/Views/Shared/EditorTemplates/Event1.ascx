<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SMCHSGManager.Models.Event>" %>

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
   		theme_advanced_buttons1: "bold,italic,underline,formatselect,fontselect,fontsizeselect,forecolor,backcolor, |,justifyleft,justifycenter,justifyright,justifyfull,|,bullist,numlist",
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
    <% using (Html.BeginForm((string)ViewData["Mode"], "Event", FormMethod.Post, new { enctype = "multipart/form-data" })){%>
         <%: Html.ValidationSummary(true) %>
         
                    <table style="border:0" >
                         <tr><td colspan="4" class="formlabel">
                             <%if (ViewData["Mode"] == "Create")
                               { %>
                                 <h1>New Event</h1>
                                 <%}else {
                                     %>
                                   <h1>Edit Event</h1>
                                   
                               <%} %>
                       </td></tr>

                       <tr><td colspan=4 class="formlabel">
                            <div class="dashedline"></div>
                       </td></tr>
                         <tr>
                            <td class="formlabel" >
                                <font color="red" size="2">*</font>Event Title:
                            </td>
                            <td align="left" class="formvalue" colspan = 3>
                                <%: Html.TextBoxFor(model => model.Title, new { style = "width:100%;", @class = "title" })%>  
                                <%: Html.ValidationMessageFor(model => model.Title)%>
                             </td>
                            <%-- <td class="formvalue" >
                                <font color="red" size="2">*</font>Public?
                            </td>
                            <td align="left" class="formvalue" >
                                <%: Html.CheckBoxFor(model => model.IsPublic)%>  
                                <%: Html.ValidationMessageFor(model => model.IsPublic) %>
                             </td>     --%>                  
                        </tr>
                        <tr>
                            <td class="formlabel">
                                <font color="red" size="2">*</font>Event Type</td>
                            <td align="left" class="formvalue" >
                                <%: Html.DropDownList("EventTypeID", new SelectList(ViewData["EventTypes"] as IEnumerable, "ID", "Name", Model.EventTypeID))%> 
                            </td>
                             <td  align="right" class="formlabel" >
                                <font color="red" size="2">*</font>Public?
                            </td>
                            <td align="left" class="formvalue" >
                                <%: Html.CheckBoxFor(model => model.IsPublic)%>  
                                <%: Html.ValidationMessageFor(model => model.IsPublic) %>
                             </td> 
                            <%--  <td class="formlabel1" >
                               <% if (ViewData["Mode"] == "Create")
                                  { %>
                                <font color="red" size="2">*</font>Schedule Model
                                <%: Html.DropDownList("LocalRetreatScheduleModel", (IEnumerable<SelectListItem>)ViewData["LocalRetreatScheduleModelSelectLists"])%>
                                <%} %>
                            </td>--%>
                           <%-- <td class="formlabel1" >
                                <font color="red" size="2">*</font>Public?
                            </td>
                            <td align="left" class="formvalue" >
                                <%: Html.CheckBoxFor(model => model.IsPublic)%>  
                                <%: Html.ValidationMessageFor(model => model.IsPublic) %>
                             </td>--%>
                        </tr>
                        <tr>
                            <td class="formlabel">
                                Link:
                            </td>
                            <td align="left" class="formvalue" colspan = 3>
                                     <%: Html.TextBoxFor(model => model.StaticURL, new { style = "width:610px;" })%>
                                     <%: Html.ValidationMessageFor(model => model.StaticURL) %>
                            </td>
                        </tr>
                        
                         <tr>
                             <td class="formlabel"><font color="red" size="2">*</font>Register Open</td>
                            <td align="left" class="formvalue">
                                  <%--  <%: Html.EditorFor(model => model.RegistrationOpenDate, "Date")%>  --%>
								    <%: Html.EditorFor(model => model.RegistrationOpenDate)%>    
                                    <%= Html.ValidationMessageFor(model => model.RegistrationOpenDate)%>
                            </td>
                            <td align="right" class="formlabel1"><font color="red" size="2">*</font>Close</td>
                            <td align="left" class="formvalue">
                                   <%-- <%: Html.EditorFor(model => model.RegistrationCloseDate, "Date")%>    --%>
								    <%: Html.EditorFor(model => model.RegistrationCloseDate)%>    
                                    <%= Html.ValidationMessageFor(model => model.RegistrationCloseDate)%>
                            </td>
                        </tr>

                        &nbsp;
                        <tr>
                            <td class="formlabel">
                                <font color="red" size="2">*</font>Location</td>
                            <td align="left" class="formvalue"  colspan=3>
                                <%: Html.DropDownList("LocationID", new SelectList(ViewData["Locations"] as IEnumerable, "ID", "Name", Model.LocationID))%> 
                            </td>
                        </tr>
                       &nbsp;
                       <tr>
                            <td class="formlabel">
                               Description </td>
                            <td align="left" class="formvalue"  colspan=3>
                                <%--<%: Html.TextAreaFor(model => model.Description, new {@class = "text-box multi-line", style = "width:650px; " })%>   
                                <%: Html.ValidationMessageFor(model => model.Description) %>--%>
								<%=Html.TextArea("Description", Model.Description, new { @name = "Editor1", style = "width:100%; height:100%" })%>
                            </td>
                        </tr>
 
                          <tr>
                              <td class="formlabel" valign="middle"> <font color="red" size="2">*</font>Local Retreat Activity  </td>
                              <td >
					        		<%: Html.DropDownList("EventActivityID", new SelectList(ViewData["EventActivity"] as IEnumerable, "ID", "Name", Model.EventActivityID))%>     
                              </td>

                             <td class="formlabel" valign="middle"> <font color="red" size="2">*</font>Start From  </td>
                             <td align="left"  valign="top" class="formvalue">
                                    <%: Html.EditorFor(model => model.StartDateTime)%>    
                                    <%= Html.ValidationMessageFor(model => model.StartDateTime)%>
                            </td>
 
                            <td align="right" class="formlabel1"> <font color="red" size="2">*</font>To</td>
                            <td align="left" class="formvalue">
                                    <%: Html.EditorFor(model => model.EndDateTime)%>    
                                    <%= Html.ValidationMessageFor(model => model.EndDateTime)%>
                            </td>
                        </tr>

 
                       
<%--                       <tr><td colspan=2 class="formlabel">
                            <div class="dashedline"></div>
                        </td></tr>
                        <tr>
                              <td  class="formlabel">
                                  <a id="ImageUploadTrigger" href="#" > Photo or File Upload</a>
                              </td>
                              <td class="formvalue"  colspan=3>
                                  <div id ="ImageUpload">
                                    <input type="file" name="fileUpload" size="20" style="width: 262px"/>  
                                    <input type="submit" value="Upload" name="Upload" class ="buttonsmall" />
                                  </div>
                                    <%: Html.Editor("&", "ImageSelect") %>
                           </td>
                        </tr>
   
                       <tr><td colspan=4 class="formlabel">
                            <div class="dashedline"></div>
                       </td></tr>
                       <tr>
                            <td  colspan="4" class="formvalue">
                                  <a id="ImageSelectTrigger" href="#" > Photo or File Select</a>&nbsp;
                                  <div id ="ImageSelect"><%: Html.Editor("&", "ImageSelectAll") %></div>
                            </td>
                     </tr>
--%>
                       <tr><td colspan=4 class="formlabel">
                            <div class="dashedline"></div>
                       </td></tr>
 
                       <tr>
                            <td  colspan=4 class="formlabel">
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


