<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<bool>" %>

<div class="display-label"><strong><%=Html.LabelFor(model => model)%></strong></div>
<div class="display-field"><%= Html.Encode(String.Format("{0:Yes;;No}", Model))%> </div>
