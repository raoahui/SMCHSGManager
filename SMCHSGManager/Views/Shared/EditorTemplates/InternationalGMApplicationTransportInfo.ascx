<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<SMCHSGManager.Models.InternationalGMApplicationTransportInfo>>" %>

<div class="fullwidth">         

<% 
	List<SelectListItem> assi = (List<SelectListItem>)ViewData["InternationalTransports"]; %>

<table style="border:0">

	<tr>
		<td colspan=2 style="border:0">
Please leave the cells empty, if you do not require any pick-up (arrival) and send-off (departure) transportation services provided by Penang Ashram.
		</td>
	</tr>

	<tr>
		<td  style="border:0">
			<table style="border:0">
                      <tr>
                            <td colspan = 2 class="formlabel"> Arrival: 	</td>
					   </tr>
					   <tr>
                            <td class="formlabel" nowrap = "nowrap"> Select Station: </td>
                            <td align="left" class="formvalue3"> 
								<% if (ViewData["InBoudInternationalTransports"] != null) { %>
									<%: Html.DropDownList("InternationalTransportIDInBound", (List<SelectListItem>)ViewData["InBoudInternationalTransports"])%>           
								<%}else{ %>
									<%: Html.DropDownList("InternationalTransportIDInBound", assi)%> 
								<%} %>            
                            </td>
  					   </tr>
						<%--<tr>
							<td colspan=2 style="border:0"> <font color="green" size="1">* .</font> </td>
						</tr>--%>					   
						<tr>
                           <td class="formlabel"> DateTime</td>
                            <td align="left" class="formvalue3">  
	<%:  string.Format("{0:d MMM yyyy} ", Model.SingleOrDefault(a => a.InBound).DateTime) %> 
	<%= Html.TextBox("TimeInBound", string.Format("{0:HH:mm}", Model.SingleOrDefault(a => a.InBound).DateTime), new { style = "width:30%;" })%> (24 hour format)
    <%= Html.ValidationMessageFor(model => model.SingleOrDefault(a => a.InBound).DateTime)%>   
									<%--<%: Html.EditorFor(model=>model.SingleOrDefault(a=>a.InBound).DateTime )%> --%>
							</td>
  					   </tr>
					   <tr>
                            <td class="formlabel"> FlightNo</td>
 						    <td align="left" class="formvalue3"><%: Html.TextBox("FlightNoInBound", Model.SingleOrDefault(a=>a.InBound).FlightNo )%> </td>
                    </tr>
		</table>
		</td>
		<td  style="border:0">
			Departure<table style="border:0">
					   <tr>
							<td class="formlabel" nowrap = "nowrap"> Select Station: </td>
                            <td align="left" class="formvalue3">
								<% if (ViewData["OutBoudInternationalTransports"] != null) { %>
									<%: Html.DropDownList("InternationalTransportIDOutBound", (List<SelectListItem>)ViewData["OutBoudInternationalTransports"])%>           
								<%}else{ %>
									<%: Html.DropDownList("InternationalTransportIDOutBound", assi)%> 
								<%} %>            
                            </td>
					   </tr>
						<%--<tr>
							<td colspan=2 style="border:0"> <font color="green" size="1">* You MUST select the time of day BEFORE you select the date.</font> </td>
						</tr>--%>					   
					   <tr>
                            <td class="formlabel"> DateTime</td>
                            <td align="left" class="formvalue3">
                           <%-- <%: Html.EditorFor(model => model.SingleOrDefault(a => !a.InBound).DateTime)%>--%> 
	<%:  string.Format("{0:d MMM yyyy} ", Model.SingleOrDefault(a => !a.InBound).DateTime) %>
	<%= Html.TextBox("TimeOutBound", string.Format("{0:HH:mm}", Model.SingleOrDefault(a => !a.InBound).DateTime), new { style = "width:30%;" })%> (24 hour format)
    <%= Html.ValidationMessageFor(model => model.SingleOrDefault(a => !a.InBound).DateTime)%>                            
                            </td>
 					   </tr>
					   <tr>
                            <td class="formlabel"> FlightNo</td>
 							<td align="left" class="formvalue3"><%: Html.TextBox("FlightNoOutBound", Model.SingleOrDefault(a => !a.InBound).FlightNo)%> </td>
                     </tr>
		</table>
		</td>
	</tr>
</table>

</div>
