<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SMCHSGManager.Models.AttachedFile>" %>
<%@ Import Namespace="SMCHSGManager.Models" %>
    <% 
		if (ViewData["CurrentAttachedFile"] != null)
        {
			AttachedFile attachedFile = (AttachedFile)ViewData["CurrentAttachedFile"];

			string srcImage = "/Image/ShowImage/" + attachedFile.ID;
			if (attachedFile.ContentType != null && attachedFile.ContentType.ToString().StartsWith("image"))
           {%>
           <img src="<%: srcImage %>" height="72" alt="" />
           <%} %>
           
			 <%: attachedFile.Name%>

             <div >
            <%= Html.CheckBox(attachedFile.ID.ToString(), true)%>
            </div>

    <%} %>


