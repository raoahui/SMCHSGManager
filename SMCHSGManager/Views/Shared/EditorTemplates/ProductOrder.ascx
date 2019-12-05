<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SMCHSGManager.Models.ProductOrder>" %>

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
    <%-- <% using (Html.BeginForm()) {%>--%>
    <% using (Html.BeginForm((string)ViewData["Mode"], "ProductOrder", FormMethod.Post, new { enctype = "multipart/form-data" })){%>
        <%: Html.ValidationSummary(true) %>

                    <table style="border:0" >
                         <tr><td colspan=4 class="formlabel">
                             <%if (ViewData["Mode"] == "Create")
                               { %>
                                   <h1>New Product Order</h1>
                                 <%}
                               else
                               { %>
                                   <h1>Edit Product Order</h1>
                                   
                               <%} %>
                       </td></tr>

                       <tr><td colspan=4 class="formlabel">
                            <div class="dashedline"></div>
                       </td></tr>

                        <tr>
                            <td class="formlabel">Title:</td>
                            <td align="left" colspan=4 class="formvalue">
                                <%: Html.TextBoxFor(model => model.Title, new { style="width:650px;" })%>  
                                <%: Html.ValidationMessageFor(model => model.Title) %>
                             </td>
                        </tr>
 
                       <tr>
                             <td class="formlabel"><font color="red" size="2">*</font>Order Open Date</td>
                            <td align="left" class="formvalue">
                                    <%: Html.EditorFor(model => model.OrderOpenDate, "Date")%>    
                                    <%= Html.ValidationMessageFor(model => model.OrderOpenDate)%>
                            </td>
                      
                            <td class="formlabel"><font color="red" size="2">*</font>Order Close Date</td>
                            <td align="left" class="formvalue">
                                    <%: Html.EditorFor(model => model.OrderCloseDate, "Date")%>    
                                    <%= Html.ValidationMessageFor(model => model.OrderCloseDate)%>
                            </td>
                        </tr>
                   
                        <tr>
                            <td class="formlabel">Description</td>
                            <td align="left" colspan=4 class="formvalue">
                                	<%=Html.TextArea("Description", Model.Description, new { @name = "Editor1", style = "width:100%; height:100%" })%>
	                        </td>
                        </tr>

                       <tr><td colspan=4 class="formlabel">
                            <div class="dashedline"></div>
                        </td></tr>

             </table>

             <table style="border:0">

                      <tr>
                            <td class="formvalue" style=" padding-top:20px" >
                             <div class="actionbuttons">
                                <% if (ViewData["Mode"] == "Create")
                                { %>
                                 <input type="submit"  value="Create & Next" name="Create" class ="buttonsmall" /> &nbsp;
                                 <%}else{ %>
                                  <input type="submit"  value="Save & Next"  name="Save" class ="buttonsmall" /> &nbsp;
                                  <%} %>
                                 <%: Html.ActionLink("Back to List", "Index", new {recentOrder = false}, new { @style = "color:white; ", @class = "buttonsmall" })%>
                               </div>
                       </td></tr>

                    </table>

     <% } %>
</div>

