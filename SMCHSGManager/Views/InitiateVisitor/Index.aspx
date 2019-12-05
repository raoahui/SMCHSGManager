<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<SMCHSGManager.Models.InitiateVisitor>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<div id="body">
  <h5>Initiate Visitor List</h5>

    <% if (Model.Count() == 0)
      {%>
          <div align="center" style=" margin-bottom:20px; margin-top:15px; color:Green; font-size:14px">
         There is no Initiate Visitor record in database, please use "Create New" button to create new one.
        </div>      
     <% }
      else
      {%>  
		   	<% if (User.IsInRole("SuperAdmin")){%>
                    <%: Html.ActionLink("ShowAll", "index", new { showAll = true })%>
			<%} %>
		  
		  <table>
        <tr>
 	<% if (User.IsInRole("SuperAdmin"))// || User.IsInRole("DP Admin"))
	 {%>
           <th></th>
    <%} %>
            <th>
                No
            </th>
            <th>
                Name
            </th>
            <th>
                Date Of Initiation
            </th>
            <th>
                ID Card No
            </th>
            <th>
                From Where
            </th>
            <th>
                Date From
            </th>
            <th>
                Date To
            </th>
            <th>
                Remark
            </th>
        </tr>

    <% int i = 0; 
    	foreach (var item in Model) { %>
    
        <tr>
	
	<% if (User.IsInRole("SuperAdmin"))// || User.IsInRole("DP Admin"))
	 {%>
            <td>
                <%: Html.ActionLink("Edit", "Edit", new { id = item.ID })%> |
                <%: Html.ActionLink("Delete", "Delete", new { id = item.ID })%>
            </td>
	<%} %>

            <td>
                <%: (++i).ToString() %>
            </td>
            <td><% string name = item.Name;
                    if (item.GenderID == 2)
                    {       
                        name += " (Sister)";
                    }
                    else if (item.GenderID == 1)
                    {
                        name += " (Brother)";
                    } %>
               <%: name %>
            </td>
              <td>
				<%: String.Format("{0:d MMM yyyy}", item.DateOfInitiation)%>
             </td>
            <td>
                <%: item.IDCardNo %>
            </td>
            <td>
                <%: item.FromWhere %>
            </td>
            <td>
                <%: String.Format("{0:d MMM yyyy}", item.DateFrom)%>
            </td>
            <td>
                <%: String.Format("{0:d MMM yyyy}", item.DateTo)%>
            </td>
            <td>
                <%: item.Remark %>
            </td>
        </tr>
    
    <% } %>

    </table>
    <% } %>
	</br>
  	<% if (User.IsInRole("SuperAdmin") || User.IsInRole("DP Admin"))
	  {%>
           <div class="editbuttons" align="center">
                    <%: Html.ActionLink("Create New", "Create", null, new { @style = "color:white;", @class = "buttonsmall" })%>
             </div>
	<%} %>
	</div>


</asp:Content>

