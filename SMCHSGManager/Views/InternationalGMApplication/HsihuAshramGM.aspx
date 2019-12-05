<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.ViewModel.HsihuAshramEventModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	HsihuAshramEvent
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<div class="fullwidth">      
      <% using (Html.BeginForm()) { %>
	   <h6> Please confirm the following at Hsihu Ashram </h6>

	   <%: Html.EditorFor(model => model, new { Mode = "Create" })%>
	         <%--  <table style="border:0" >
  
                       <tr><td colspan=2 class="formlabel">
                            <div class="dashedline"></div>
                       </td></tr>

                        <tr>
                            <td class="formlabel" nowrap="nowrap" style="width: 279px" >
								<% if ((int)ViewData["AshramID"] == 2){ %>
									Apply Stay & Meditate
								<%}else{ %>
									Accommodation Needed
								<%} %>
							</td>
                            <td align="left" class="formvalue3" >
								<%: Html.CheckBox("AccommodationNeededCheckBox", true)%>
							</td>
                        </tr>

 <% List<DateTime> retreats = (List<DateTime>)ViewData["retreats"];
   List<DateTime> sundayGMs = (List<DateTime>)ViewData["sundayGMs"]; %>
			 
					<% if (retreats != null && retreats.Count > 0){%>                       
                       <tr><td colspan=2 class="formlabel">
                            <div class="dashedline"></div>
                       </td></tr>
						<tr>
                            <td class="formlabel" nowrap="nowrap" style="width: 279px" >Apply 2 Days Retreat </td>
                            <td align="left" class="formvalue3" >
								<% foreach(DateTime retreat in retreats){ %>
 									 <%: string.Format("{0: d MMM yyyy}", retreat)%> to <%: string.Format("{0: d MMM yyyy}", retreat.AddDays(1))%> 	<%: Html.CheckBox("RetreatCheckBox" + string.Format("{0:d-MMM-yyyy}", retreat))%> &nbsp;&nbsp;&nbsp;
								<%} %>							
							</td>
                        </tr>
					<%} %>

             	 <% if (sundayGMs != null && sundayGMs.Count > 0){%>         
                       <tr><td colspan=2 class="formlabel">
                            <div class="dashedline"></div>
                       </td></tr>
						<tr>
                            <td class="formlabel" nowrap="nowrap" style="width: 279px" >Apply Sunday Group Meditation </td>
                            <td align="left" class="formvalue3" >
								<% foreach (DateTime sundayGM in sundayGMs){ %>
 									<%: string.Format("{0: d MMM yyyy}", sundayGM)%> 	<%: Html.CheckBox("SundayGMCheckBox"+ string.Format("{0:d MMM yyyy}", sundayGM))%>  &nbsp;&nbsp;&nbsp;
								<%} %>
							</td>
                        </tr>
				<%} %>

					<tr><td colspan=2 class="formlabel">
                           <div class="dashedline"></div>
                   </td></tr>
	
				   <% if (ViewData["remark"] != null){ %>
                   <tr>
                       <td class="formlabel"> Note</td>
                       <td align="left" class="formvalue3">
                                	<%: MvcHtmlString.Create((string)ViewData["remark"])%> 
                       </td>
                   </tr>
				   <%} %>

					<tr><td colspan=2 class="formlabel">
                           <div class="dashedline"></div>
                   </td></tr>

	</table>--%>

			<p align="center">  <input type="submit" value="Submit"  class ="buttonsmall"/> </p>

	<%} %>

</div>

</asp:Content>
