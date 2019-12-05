<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SMCHSGManager.Models.OrdinaryMemberInfo>" %>

         <style type="text/css">
			 .style1
			 {
				 width: 115px;
			 }
			 .style2
			 {
				 width: 124px;
			 }
		 </style>

         <table style="border:0" >     
 
					<tr>
 						<td class="formlabel5" nowrap="nowrap">Name in Native</td>
						<td class="style1" nowrap="nowrap"><%: Html.TextBoxFor(model => model.NameInNative)%></td>
 						<td class="formlabel5" nowrap="nowrap" >NRIC / FIN No</td>
						<td class="style2" nowrap="nowrap"><%: Html.TextBoxFor(model => model.NRICOrFINNo)%></td>
                   </tr>

   					<tr >
						<td  class="formlabel5" nowrap="nowrap">Address</td>
                        <td  class="formlabel6" nowrap="nowrap" colspan = 3><%: Html.TextBoxFor(model => model.Address, new { style = "width:100%;", })%> </td>
                    </tr>
  
                    <tr>
 						<td class="formlabel5" nowrap="nowrap">Nationality</td>
                        <td class="style1" nowrap="nowrap" > <%: Html.DropDownList("NationalityID", new SelectList(ViewData["Nationalities"] as IEnumerable, "ID", "Name", Model.NationalityID))%>  	</td>
  						<td class="formlabel5" nowrap="nowrap">Race </td>
						<td class="style2" nowrap="nowrap" > <%: Html.DropDownList("RaceID", new SelectList(ViewData["Races"] as IEnumerable, "ID", "Name", Model.RaceID))%>    </td>
					</tr>

					<tr>
						<%--<td class="formlabel5" nowrap="nowrap">Date of Birth</td>
						<td class="style1" nowrap="nowrap"><%: Html.EditorFor(model => model.DateOfBirth, "Date")%></td>--%>
 						<%--<td class="formlabel5" nowrap="nowrap">Place of Birth</td>
						<td class="style2" nowrap="nowrap"><%: Html.TextBoxFor(model => model.PlaceOfBirth)%> </td>--%>
						<td class="formlabel5" nowrap="nowrap" >Place of Initiation</td>
						<td class="style1" nowrap="nowrap" ><%: Html.TextBoxFor(model => model.PlaceOfInitiation)%></td>
						<td class="formlabel5" nowrap="nowrap" >Employment Status</td>
						<td class="style1" nowrap="nowrap" ><%: Html.DropDownList("EmploymentStatusID", new SelectList(ViewData["EmploymentStatuses"] as IEnumerable, "ID", "Name", Model.EmploymentStatusID))%>   </td>
				    </tr>

 					<%--<tr>
   						<td  class="formlabel5" nowrap="nowrap">Member Fee PayBy</td>
                        <td class="style2" nowrap="nowrap"><%: Html.DropDownList("MemberFeePayByID", new SelectList(ViewData["PayMethods"] as IEnumerable, "ID", "Name", Model.MemberFeePayByID))%> </td>
					</tr>--%>

					<tr>
						<td class="formlabel5" nowrap="nowrap">Occupation</td>
						<td class="style2" nowrap="nowrap" ><%: Html.TextBoxFor(model => model.Occupation)%> </td>
						<td class="formlabel5" nowrap="nowrap">Education Level</td>
						<td class="style1" nowrap="nowrap" ><%: Html.TextBoxFor(model => model.EducationLevel)%> </td>
					</tr>   
   
					<tr>
						<%--<td  class="formlabel5" nowrap="nowrap">Member Fee Expired Date</td>
                        <td class="formlabel6" nowrap="nowrap"> <%: Html.DisplayFor(model => model.MemberFeeExpiredDate, "Date")%>  </td>--%>
						<%--<td class="formlabel5" nowrap="nowrap" >Place of Initiation</td>
						<td class="style1" nowrap="nowrap" ><%: Html.TextBoxFor(model => model.PlaceOfInitiation)%></td>--%>
 						<td class="formlabel5" nowrap="nowrap" >Special Skill</td>
						<td class="formlabel6" nowrap="nowrap" colspan=3 ><%: Html.TextBoxFor(model => model.SpecialSkill, new { style = "width:100%;", })%> </td>
					</tr>

					<tr>
                        <td class="formlabel5" nowrap="nowrap">Member Apply Date</td>
                        <td class="style1" nowrap="nowrap" > <%: Html.EditorFor(model => model.MemberApplyDate, "Date")%> </td>
                        <td class="formlabel5" nowrap="nowrap">Member Effective Start Date</td>
						<td class="style2" nowrap="nowrap" > <%: Html.EditorFor(model => model.MemberEffectiveStartDate, "Date")%> </td>
 					</tr>

 					<%--<tr>
						<td class="formlabel5" nowrap="nowrap">Remark</td>
						<td class="formlabel6" nowrap="nowrap" colspan=3 ><%: Html.TextBoxFor(model => model.Remark, new { style = "width:100%;", })%> </td>
					</tr>--%>

 					                  
                </table>

