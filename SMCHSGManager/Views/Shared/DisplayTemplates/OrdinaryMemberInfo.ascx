<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SMCHSGManager.Models.OrdinaryMemberInfo>" %>

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
 
					<tr>
 						<td class="formlabel5" nowrap="nowrap">Name in Native</td>
						<td class="formlabel6" nowrap="nowrap" ><%: Model.NameInNative%>  
 						<td class="formlabel5" nowrap="nowrap" >NRIC / FIN No</td>
						<td class="formlabel6" nowrap="nowrap"><%: Model.NRICOrFINNo%></td>
                   </tr>

   					<tr >
						<td  class="formlabel5" nowrap="nowrap">Address</td>
                        <td  class="formlabel6" nowrap="nowrap" colspan = 3>  <%: Model.Address%></td>
                    </tr>
  
                    <tr>
 						<td class="formlabel5" nowrap="nowrap">Nationality</td>
                        <td class="formlabel6" nowrap="nowrap" >
							<% if (Model.Nationality != null)
							{ %>
								<%: Model.Nationality.Name%>  
							<%} %>
						</td>
  						<td class="formlabel5" nowrap="nowrap">Race </td>
						<td class="formlabel6" nowrap="nowrap" >
							<% if (Model.Race != null)
							{ %>
								<%: Model.Race.Name%>  
							<%} %>
						 </td>
					</tr>

					<tr>
 						<%--<td class="formlabel5" nowrap="nowrap">Place of Birth</td>
						<td class="formlabel6" nowrap="nowrap"><%: Model.PlaceOfBirth%></td>--%>
						<td class="formlabel5" nowrap="nowrap" >Place of Initiation</td>
						<td class="formlabel6" nowrap="nowrap" ><%: Model.PlaceOfInitiation%></td>
					</tr>

 					<tr>
						<td class="formlabel5" nowrap="nowrap" >Employment Status</td>
						<td class="formlabel6" nowrap="nowrap" >
							<% if (Model.EmploymentStatus != null)
							{ %>
								<%: Model.EmploymentStatus.Name%>  
							<%} %>
						</td>
  						<td  class="formlabel5" nowrap="nowrap">Member Fee PayBy</td>
                        <td class="formlabel6" nowrap="nowrap">
							<%: (string)ViewData["PayMethodName"]%> 
						</td>
					</tr>
					<tr>
 						<td class="formlabel5" nowrap="nowrap">Occupation</td>
						<td class="formlabel6" nowrap="nowrap" ><%: Model.Occupation%></td>
						<td class="formlabel5" nowrap="nowrap">Education Level</td>
						<td class="formlabel6" nowrap="nowrap" ><%: Model.EducationLevel%> </td>
 					</tr>   
   
					<tr>
						<td class="formlabel5" nowrap="nowrap" >Special Skill</td>
						<td class="formlabel6" nowrap="nowrap"  colspan=3 ><%: Model.SpecialSkill%>   </td>
						<%--<td  class="formlabel5" nowrap="nowrap">Member Fee Expired Date</td>
                        <td class="formlabel6" nowrap="nowrap"> <%: Html.DisplayFor(model => model.MemberFeeExpiredDate, "Date")%>  </td>--%>
					</tr>

					<tr>
                        <td class="formlabel5" nowrap="nowrap">Member Apply Date</td>
                        <td class="formlabel6" nowrap="nowrap" > <%: Html.DisplayFor(model => model.MemberApplyDate, "Date")%> </td>
                        <td class="formlabel5" nowrap="nowrap">Member Effective Start Date</td>
						<td class="formlabel6" nowrap="nowrap" > <%: Html.DisplayFor(model => model.MemberEffectiveStartDate, "Date")%> </td>
 					</tr>

 				<%--	<tr>
						<td class="formlabel5" nowrap="nowrap">Remark</td>
						<td class="formlabel6" nowrap="nowrap" colspan=3 ><%: Model.Remark %> </td>
					</tr>--%>

 					                  
                </table>
                </td>
          </tr>
 </table>
  
