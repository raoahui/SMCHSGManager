<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<String>" %>

<div class="editor-label">
    <%= Html.LabelFor(model => model) %>
</div>
<div class="editor-field">
    <%= Html.TextBoxFor(model => model) %>
    <%= Html.ValidationMessageFor(model => model) %>
</div>