<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<p>
<% SMCHSGManager.Models.UploadFile uploadFile = (SMCHSGManager.Models.UploadFile)ViewData["CurrentUploadFile"]; %>
<%: Html.ActionLink(uploadFile.Name, "FileDownload", "Image", new { imageID =uploadFile.ID }, new { style = "color:Olive " })%>
</p>