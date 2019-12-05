<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<List<List<string>>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	SMCH Association Singapore - 	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 
<div id="body">

<% string title = (string)ViewData["EventTitle"];
   string cTitle = null;
   if ((int)ViewData["EventTypeID"] == 1)
   {
       title += "Local Retreat Registration List";
       cTitle = "本地禅";
   } %>
   <h5><%: title %> </h5>
   <h6>Start at <%= ViewData["StartDateTime"] %>, end at <%= ViewData["EndDateTime"]%> <%: cTitle %></h6>
   <h6>Registration close on 报名截至 <%= ViewData["RegistrationCloseDate"] %></h6>
   
  <table id="MyTable">
        
        <tr >
            <th rowspan="3">S/N</th>
            <th rowspan="3">
                Name in ID Card</br> 识别证姓名
            </th>
           <th rowspan="3">
                Member</br> No
            </th>
           <th rowspan="3">
                ID Card No </br>识别证编号
            </th>
            <th rowspan="3">
                Contact No</br> 联络号码
            </th>

            <% if (!string.IsNullOrEmpty(cTitle))
               { %>
             <th rowspan="3">
               Volunteer Job Selection 打禅义工</br>
               DP = 护法</br>
               Clean = 清洁</br>
               Video = 音响&影像
             </th>
             <%} %>

             <% 
                if ((int)ViewData["MealCounts"] > 0)
                {%>
             <th colspan="<%: ViewData["MealCounts"] %>" >
                Meals Selection
             </th>
         <%} %>

              <th rowspan="3">To Pay</th>

            <% if (!string.IsNullOrEmpty(cTitle))
               { %>
             <th rowspan="3">
                Remark</br>
                Early back time</br>
                提前离开时间
             </th>
            <%}else{ %>
           <th rowspan="3">
                Remark</br>
             </th>
            <%}%>

        </tr>
     
        <tr><% SMCHSGManager.Controllers.EventRegistrationController erc = new SMCHSGManager.Controllers.EventRegistrationController(); %>
          <%= erc.GetMealDateTitles((List<string>)ViewData["mLabels"])%>
        </tr>
   
        <tr>
         <%= erc.GetMealTitles((List<string>)ViewData["mLabels"])%>
        </tr>

        <% int i=0; 
              foreach (List<string> row in Model) {%>
            <tr>
                <td>
                    <% if (i < Model.Count-1){%>
                        <%: Html.ActionLink((++i).ToString(), "Details", new { id = int.Parse(row[0]) }, null)%>
                    <%} %>
                </td>
                <%  for (int col = 1; col < row.Count; col++) {
                        string item = row[col];
                        if ((col == 5) && string.IsNullOrEmpty(cTitle))
                        {
                            continue;
                        }
                        if (col == 6){%>   
                            <td align="center">
                        <%}else{ %>
                            <td nowrap="nowrap">
                        <%}%>
                        <%  if(!string.IsNullOrEmpty(item)){
                              string[] volunterJobs = item.Split('\n');
                              for(int vi = 0; vi < volunterJobs.Count(); vi++){
                        %>
                                   <%: volunterJobs[vi]%>
                                   <%  if(vi < volunterJobs.Count()){%> 
                                       </br>
                                   <%} %>
                              <%}%>
                        <%}%>
                    </td>
                <%} %>
            </tr>
        <%} %>
 
   </table>

   <div align="center" style="margin-bottom:20px; margin-top:20px">
        <%= Html.ActionLink("Get Excel", "GenerateExcel2", "EventRegistration", null, new { @style = "color:white;", @class = "buttonsmall" })%>  
   </div>

</div>

</asp:Content>


