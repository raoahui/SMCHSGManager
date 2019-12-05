<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DateTime?>" %>

<%--<div class="display-label"><strong><%=Html.LabelFor(model => model)%></strong></div>
<div class="display-field"><%=Html.Encode(Model.HasValue ? Model.Value.ToLongDateString() : string.Empty)%></div>--%>
<%--<%= Html.Encode(String.Format("{0:g}", Model))%> --%>

<%if(Model != null) { %>

<%= Html.Encode(String.Format("{0:d MMM yyyy HH:mm }", Model))%>

<%} %>