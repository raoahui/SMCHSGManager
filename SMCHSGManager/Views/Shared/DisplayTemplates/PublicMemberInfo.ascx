<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SMCHSGManager.ViewModel.PublicMemberInfo>" %>

<table  style="border:0" >
            <tr>
              <%-- <%if (Model.AttachFileCollectionIDCollectionID != null)
               {                  
                    List<int> imageIDs = (from r in Model.AttachFileCollectionIDCollectionID.AttachFileCollectionIDMaps select r.UploadFileID).ToList();
                    foreach(int imageID in imageIDs)
                    {
                        string srcImage = "/Image/ShowPhoto/" + imageID.ToString(); %>

            <td style="padding-right: 3em;  border:0">
                      
                               <p> <img src="<%: srcImage %>"  width = "150" alt="" /></p>
                      <%} %>
             </td>
                <%} %>--%>

				<td style="border:0">
              <table style="border:0" >     
                       
					<%--<tr>
						<td class="formlabel5" nowrap="nowrap">User Name</td>
						<td class="formlabel6" nowrap="nowrap"><%: Model.UserName%> </td>
						<td class="formlabel5" nowrap="nowrap">Name </td>
						<td class="formlabel6" nowrap="nowrap"><%: Model.Name%>  </td>
					</tr>--%>

	                <tr>
						<td class="formlabel5" nowrap="nowrap">Name </td>
						<td class="formlabel6" nowrap="nowrap"><%: Model.Name%>  </td>
						<td class="formlabel5" nowrap="nowrap">Member Fee Expire Date</td>
						<% if (Model.MemberFeeExpiredDate.HasValue && Model.MemberFeeExpiredDate.Value == new DateTime(2050, 12, 31, 0, 0, 11))
						{ %>
						<td class="formlabel6" nowrap="nowrap"> Giro </td>
						<%}else { %>
						<td class="formlabel6" nowrap="nowrap"><%: Html.DisplayFor(model=>model.MemberFeeExpiredDate, "Date")%></td>
						<%} %>
                    </tr>   	

					<tr>
						<td class="formlabel5" nowrap="nowrap">Member No</td>
						<td class="formlabel6" nowrap="nowrap"><%: Model.MemberNo%> </td>
						<td class="formlabel5" nowrap="nowrap">Gender </td>
						<td class="formlabel6" nowrap="nowrap"><%: Model.Gender%>  </td>
					</tr>
 
 					<tr>
						<td class="formlabel5" nowrap="nowrap">ID Card No</td>
						<td class="formlabel6" nowrap="nowrap"><%: Model.IDCardNo%></td>
						<td class="formlabel5" nowrap="nowrap">Contact No</td>
						<td class="formlabel6" nowrap="nowrap"><%: Model.ContactNo%></td>  
					</tr>

  					<tr>
						<td class="formlabel5" nowrap="nowrap">Date of Initiation</td>
						<td class="formlabel6" nowrap="nowrap"><%: Html.DisplayFor(model => model.DateOfInitiation, "Date")%></td>
						<td class="formlabel5" nowrap="nowrap">Date of Birth</td>
						<td class="formlabel6" nowrap="nowrap"><%: Html.DisplayFor(model => model.DateOfBirth, "Date")%></td>
					</tr>

    				<tr>
						<td class="formlabel5" nowrap="nowrap">Email</td>
						<td class="formlabel6" nowrap="nowrap"><%: Model.Email%> </td>
						<td class="formlabel5" nowrap="nowrap">Country of Birth</td>
						<td class="formlabel6" nowrap="nowrap"><%: Model.CountryOfBirth %></td>
					</tr>

			<%   if (!Roles.IsUserInRole("Administrator")){ %>
	                <tr>
						<td class="formlabel6" colspan="4" nowrap="nowrap" > <font color="Green" size="1">Member is encouraged to pay on half or yearly basis for ease of administation.</font></td>
                    </tr>   	
			<%}%>

   					<tr>
						<td class="formlabel5" nowrap="nowrap">Initiate Type </td>
						<td class="formlabel6" nowrap="nowrap">							
						    <%: Model.InitiateType %>  
                        </td>
 						<td class="formlabel5" nowrap="nowrap" >Passport No</td>
						<td class="formlabel6" nowrap="nowrap"><%: Model.PassportNo%></td>
				</tr>

					<tr>
						<td class="formlabel5" nowrap="nowrap">IsActive </td>
						<td class="formlabel6" nowrap="nowrap"><%: Model.IsActive %> </td>
	                   </tr>

					<tr>
						<td class="formlabel5" nowrap="nowrap">Remark</td>
						<td class="formlabel6" nowrap="nowrap" colspan=3 ><%: Model.Remark %> </td>
					</tr>

                 </table>
                </td>
          </tr>

 </table>
 



