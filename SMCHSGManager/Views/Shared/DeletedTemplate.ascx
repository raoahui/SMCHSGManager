<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<string>" %>

 <div id="body">
    <h5>The <%: Model %> Deleted</h5>
        <div class="centerblock">
    <h6><%: Model %> was successfully deleted.</h6>
 
       <h6>
		    <%: Html.ActionLink("Click here", "Index")%>
            to return to the <%: Model %> List.
        </h6>
        </div>
  </div>
