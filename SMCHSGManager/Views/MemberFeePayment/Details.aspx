<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SMCHSGManager.Models.MemberFeePayment>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="fullwidth">         
        
                    <table style="border:0" >
                         <tr><td colspan="4" class="formlabel">
							<h1>Member Fee Payment Details</h1>
                       </td></tr>

                       <tr><td colspan=2 class="formlabel">
                            <div class="dashedline"></div>
                       </td></tr>

                         <tr>
                            <td class="formlabel">Member Name</td>
                            <td class="formvalue" >
                                <%: Model.MemberInfo.Name %>
                            </td>
                          </tr>
   
                        <tr>
                            <td class="formlabel">Member No</td>
                            <td class="formvalue" >
                                <%: Model.MemberInfo.MemberNo %>
                            </td>
                          </tr>
                             
                          <tr>
                             <td class="formlabel"> From</td>
                             <td class="formvalue">
                                    <%: Html.DisplayFor(model => model.FromDate, "Date")%>    
                             </td>
                         </tr>
                             
                         <tr>
                            <td class="formlabel">To</td>
                            <td class="formvalue">
                                    <%: Html.DisplayFor(model => model.ToDate, "Date")%>  
                            </td>
                        </tr>

                         <tr>
                            <td class="formlabel">PayAmount</td>
                            <td  class="formvalue">
                                    <%: Model.PayAmount.ToString() %>    
                            </td>
                         </tr>
                             
                        <tr>
                            <td class="formlabel">Pay Method</td>
                            <td class="formvalue"  colspan=3>
								 <%: Model.PayMethod.Name %>
                            </td>
                        </tr>
                         
                        <tr>
                            <td class="formlabel1">Remark</td>
                            <td class="formvalue">
                                   <%: Model.Remark %>
                            </td>
                        </tr>

                        <tr><td colspan=2 class="formlabel">
                            <div class="dashedline"></div>
                        </td></tr>
                        
 
                       <tr>
                            <td align="Center" colspan=2 class="formlabel">
                               <%: Html.ActionLink("Edit", "Edit", new { IMemberID = Model.IMemberID, FromDate = Model.FromDate, ToDate = Model.ToDate }, new { @style = "color:white;", @class = "buttonsmall" })%> 
  					           <%: Html.ActionLink("Back to List", "Index", null, new { @style = "color:white; ", @class = "buttonsmall" })%>
 							</td>
						</tr>

                    </table>

</div>     


</asp:Content>

