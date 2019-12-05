<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SMCHSGManager.Models.EventActivity>" %>

            
            <div class="editor-label">
                <%: Html.DisplayFor(model => model.Name) %>
               <%-- <%: Html.ValidationMessageFor(model => model.Name) %>--%>
            </div>
            </br>

             <div class="editor-label">
                <%: Html.LabelFor(model => model.Remark) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Remark) %>
                <%: Html.ValidationMessageFor(model => model.Remark) %>
            </div>
            
   

