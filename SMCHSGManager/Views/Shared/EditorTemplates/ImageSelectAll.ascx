<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="SMCHSGManager.Models" %>
&nbsp;
<div class="ImageContainer">
        <%  
            SMCHDBEntities _entities = new SMCHSGManager.Models.SMCHDBEntities();
			List<UploadFile> UploadFiles = (from r in _entities.UploadFiles where !r.FilePath.EndsWith("SMProducts/") orderby r.UploadTime descending select r).ToList();

        	// Just uploading one
			 UploadFile currentUploadFile = new UploadFile();
            if (ViewData["CurrentUploadFile"] != null)
            {
				currentUploadFile = (UploadFile)ViewData["CurrentUploadFile"];
				UploadFiles = _entities.UploadFiles.Where(a => a.ID != currentUploadFile.ID).OrderByDescending(a => a.UploadTime).ToList();
			}
				
			foreach (UploadFile uploadFile in UploadFiles)
            {
 				if (uploadFile.ContentType.StartsWith("image"))
                {
					//string srcImage = "/Image/ShowPhoto/" + uploadFile.ID.ToString();
					string srcImage = uploadFile.FilePath + uploadFile.Name; %>
                <img src="<%: srcImage %>" height="72" alt="" />
             <%}else{%>
                 <%: Html.Truncate(uploadFile.Name, 10)%>
            <% }%>

                <%= Html.CheckBox(uploadFile.ID.ToString())%>
        <% } %>      
 
</div>

