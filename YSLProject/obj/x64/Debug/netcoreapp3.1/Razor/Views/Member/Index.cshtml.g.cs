#pragma checksum "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f0cda35c132a161797c999b12fb6d3b9895cf0b7"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Member_Index), @"mvc.1.0.view", @"/Views/Member/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\_ViewImports.cshtml"
using YSLProject;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\_ViewImports.cshtml"
using YSLProject.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f0cda35c132a161797c999b12fb6d3b9895cf0b7", @"/Views/Member/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"98be823b74c24c3530062c89b3b821261833111c", @"/Views/_ViewImports.cshtml")]
    public class Views_Member_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<YSLProject.Table.MemberMaster>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("btn btn-primary"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-area", "", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Member", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "AddMember", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n\r\n\r\n\r\n");
#nullable restore
#line 6 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\Index.cshtml"
  
    ViewData["Title"] = "Index";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"<style>
    .btn-warning, .btn-danger {
        padding: 3px 1rem;
    }
</style>
<link rel=""stylesheet"" type=""text/css"" href=""https://cdn.datatables.net/v/dt/dt-1.10.24/datatables.min.css"" />

<div class=""form-group row"">
    <div class=""col-md-6"">
        <h1>Member List</h1>
    </div>
    <div class=""col-md-6"" style=""text-align:end;"">
        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "f0cda35c132a161797c999b12fb6d3b9895cf0b75260", async() => {
                WriteLiteral("Add New");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Area = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n    </div>\r\n</div>\r\n");
            WriteLiteral(@"
<div class=""card"">
    <div class=""card-content collapse show"">
        <div class=""card-body card-dashboard dataTables_wrapper dt-bootstrap"">
            <div class=""table-responsive"">
                <table class=""table table-striped table-bordered"" id=""example"" style=""width:100%"">
                    <thead>
                        <tr>

                            <th>
                                ");
#nullable restore
#line 37 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\Index.cshtml"
                           Write(Html.DisplayName("First Name"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            </th>\r\n                            <th>\r\n                                ");
#nullable restore
#line 40 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\Index.cshtml"
                           Write(Html.DisplayName("Last Name"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            </th>\r\n                            <th>\r\n                                ");
#nullable restore
#line 43 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\Index.cshtml"
                           Write(Html.DisplayName("Email"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            </th>\r\n                            <th>\r\n                                ");
#nullable restore
#line 46 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\Index.cshtml"
                           Write(Html.DisplayName("Primary Phone"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            </th>\r\n                            <th>Action</th>\r\n                        </tr>\r\n                    </thead>\r\n                    <tbody>\r\n");
#nullable restore
#line 52 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\Index.cshtml"
                         foreach (var item in Model)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                        <tr>\r\n\r\n                            <td>\r\n                                ");
#nullable restore
#line 57 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\Index.cshtml"
                           Write(Html.DisplayFor(modelItem => item.FirstName));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            </td>\r\n                            <td>\r\n                                ");
#nullable restore
#line 60 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\Index.cshtml"
                           Write(Html.DisplayFor(modelItem => item.LastName));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            </td>\r\n                            <td>\r\n                                ");
#nullable restore
#line 63 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\Index.cshtml"
                           Write(Html.DisplayFor(modelItem => item.Email));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            </td>\r\n                            <td>\r\n                                ");
#nullable restore
#line 66 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\Index.cshtml"
                           Write(Html.DisplayFor(modelItem => item.PrimaryPhone));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            </td>\r\n                            <td>\r\n");
            WriteLiteral("                                ");
#nullable restore
#line 70 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\Index.cshtml"
                           Write(Html.ActionLink("Edit", "EditMember", "Member", new { Id = item.MemberID }, new { @class = "btn btn-warning" }));

#line default
#line hidden
#nullable disable
            WriteLiteral(" |\r\n");
            WriteLiteral("                                ");
#nullable restore
#line 72 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\Index.cshtml"
                           Write(Html.ActionLink("Delete", "DeleteMember", "Member", new { Id = item.MemberID }, new { @class = "btn btn-danger" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            </td>\r\n                        </tr>\r\n");
#nullable restore
#line 75 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\Index.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"                    </tbody>
                </table>
            </div>
        </div>
    </div>
</div>

                



                <script src=""https://code.jquery.com/jquery-3.5.1.js""></script>
                <script src=""https://cdn.datatables.net/1.10.24/js/jquery.dataTables.min.js""></script>


                <script>
                    $('#example').DataTable({
                        'paging': true,
                        'lengthChange': true,
                        'searching': true,
                        ""order"": [],
                        'info': true,
                        'autoWidth': true,
                        'columnDefs': [{
                            'targets': [4],
                            'orderable': false,
                        }]
                    })
                </script>
");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<YSLProject.Table.MemberMaster>> Html { get; private set; }
    }
}
#pragma warning restore 1591