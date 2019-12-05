<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SMCHSGManager.Models.UploadFile>" %>

<div class="dashedline"></div>




<h2>Photo Images Uploaded to Server</h2>

                <div class="dashedline"> </div>
                 <table border=0>
                 <% using (Html.BeginForm("Create", "Image", FormMethod.Post, new { enctype = "multipart/form-data" }))
                { %>
                <%: Html.ValidationSummary(true) %>
                         <tr>
                            <td class="formlabel">Select a File:
                            </td>
                            <td align="left" class="formvalue">
                                 &nbsp;<input type="file" name="fileUpload" size="20" style="width: 262px"/></td>
                        </tr>
  
                        <tr>
                            <td colspan=2 class="formlabel">
                               <p align=center>
                               <% if (ViewData["CurrentUploadFile"] != null)
                                  {
									  SMCHSGManager.Models.UploadFile uploadFile = (SMCHSGManager.Models.UploadFile)ViewData["CurrentUploadFile"];
                                      if( uploadFile.ContentType.StartsWith("image"))
                                      {
										  string srcImage = uploadFile.FilePath + uploadFile.Name;  %>
										<img src="<%: srcImage %>" width="109" height="72" alt="" />
                                    <%}
                                      else
                                      {%>
                                          <%: uploadFile.Name %>
                                      <%}%>
                                <%} %>
                              </p>

                                 <div class="actionbuttons">
                                    <input type="submit" value="Upload" class ="buttonsmall" /> &nbsp;
                                    <%: Html.ActionLink("Back to List", "Index", null, new { @style = "color:white; ", @class = "buttonsmall" })%>  &nbsp;
                                </div>

                           </td>
                        </tr>
                    <% } %>

                    </table>

                    <div class="dashedline"></div>

