<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<%if(Model != null) { %>

<%= Html.Encode(String.Format("{0:d MMM yyyy}", Model))%>

<%} %>