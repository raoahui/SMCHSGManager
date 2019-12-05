<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SMCHSGManager.Models.UploadFile>" %>
<%@ Import Namespace="SMCHSGManager.Models" %>
    <% 
		if (ViewData["CurrentUploadFile"] != null)
        {
			UploadFile uploadFile = (UploadFile)ViewData["CurrentUploadFile"];

			//string srcImage = "/Image/ShowPhoto/" + uploadFile.ID;
			string srcImage = uploadFile.FilePath + uploadFile.Name; 
			
			if (uploadFile.ContentType != null && uploadFile.ContentType.ToString().StartsWith("image"))
           {%>
 				<img src="<%: srcImage %>" height="72" alt="" />
          <%} %>
           
			 <%: uploadFile.Name%>
          
            <%= Html.CheckBox(uploadFile.ID.ToString(), true)%>
  
    <%} %>


