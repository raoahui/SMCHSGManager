<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SMCHSGManager.Models.EventPrice>" %>

            
            <div class="editor-label">
                <%: Html.LabelFor(model => model.UnitPrice) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.UnitPrice, String.Format("{0:F}", Model.UnitPrice)) %>
                <%: Html.ValidationMessageFor(model => model.UnitPrice) %>
            </div>
            
            
   

