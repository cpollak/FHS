#pragma checksum "D:\Maulik\FHS\15-08-2022\YSL\FHS\YSLProject\Views\User\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e86dad138760771fc5ab6a43c14ea9ede56ea1b2"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_User_Index), @"mvc.1.0.view", @"/Views/User/Index.cshtml")]
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
#line 1 "D:\Maulik\FHS\15-08-2022\YSL\FHS\YSLProject\Views\_ViewImports.cshtml"
using YSLProject;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Maulik\FHS\15-08-2022\YSL\FHS\YSLProject\Views\_ViewImports.cshtml"
using YSLProject.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e86dad138760771fc5ab6a43c14ea9ede56ea1b2", @"/Views/User/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"98be823b74c24c3530062c89b3b821261833111c", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_User_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<YSLProject.Models.UserMasterModel>>
    #nullable disable
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("btn btn-primary"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-area", "", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "User", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Create", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
            WriteLiteral("\r\n");
#nullable restore
#line 3 "D:\Maulik\FHS\15-08-2022\YSL\FHS\YSLProject\Views\User\Index.cshtml"
  
    ViewData["Title"] = "Index";

#line default
#line hidden
#nullable disable
            WriteLiteral("<style>\r\n    .btn-warning, .btn-danger {\r\n        padding: 3px 1rem;\r\n    }\r\n</style>\r\n<div class=\"form-group row\">\r\n    <div class=\"col-md-6\">\r\n        <h2>User List</h2>\r\n    </div>\r\n    <div class=\"col-md-6\" style=\"text-align:end;\">\r\n        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "e86dad138760771fc5ab6a43c14ea9ede56ea1b24858", async() => {
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
            WriteLiteral("\r\n    </div>\r\n</div>\r\n\r\n");
            WriteLiteral("\r\n<div class=\"card\">\r\n    <div class=\"card-content collapse show\">\r\n        <div class=\"card-body card-dashboard dataTables_wrapper dt-bootstrap\">\r\n            <div class=\"table-responsive\">\r\n");
            WriteLiteral("                <table id=\"tblUser\" class=\"table table-striped table-bordered\">\r\n                    <thead>\r\n                        <tr>\r\n\r\n                            <th>\r\n                                ");
#nullable restore
#line 34 "D:\Maulik\FHS\15-08-2022\YSL\FHS\YSLProject\Views\User\Index.cshtml"
                           Write(Html.DisplayName("User Name"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            </th>\r\n                            <th>\r\n                                ");
#nullable restore
#line 37 "D:\Maulik\FHS\15-08-2022\YSL\FHS\YSLProject\Views\User\Index.cshtml"
                           Write(Html.DisplayName("Email"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            </th>\r\n                            <th>\r\n                                ");
#nullable restore
#line 40 "D:\Maulik\FHS\15-08-2022\YSL\FHS\YSLProject\Views\User\Index.cshtml"
                           Write(Html.DisplayName("IsActive"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            </th>\r\n                            <th>\r\n                                ");
#nullable restore
#line 43 "D:\Maulik\FHS\15-08-2022\YSL\FHS\YSLProject\Views\User\Index.cshtml"
                           Write(Html.DisplayName("Last LoginDate"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            </th>\r\n                            <th>\r\n                                ");
#nullable restore
#line 46 "D:\Maulik\FHS\15-08-2022\YSL\FHS\YSLProject\Views\User\Index.cshtml"
                           Write(Html.DisplayName("User Type"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            </th>\r\n                            <th></th>\r\n                        </tr>\r\n                    </thead>\r\n                    <tbody>\r\n");
#nullable restore
#line 52 "D:\Maulik\FHS\15-08-2022\YSL\FHS\YSLProject\Views\User\Index.cshtml"
                         foreach (var item in Model)
                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <tr>\r\n                                <td>\r\n                                    ");
#nullable restore
#line 56 "D:\Maulik\FHS\15-08-2022\YSL\FHS\YSLProject\Views\User\Index.cshtml"
                               Write(Html.DisplayFor(modelItem => item.UserName));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                </td>\r\n                                <td>\r\n                                    ");
#nullable restore
#line 59 "D:\Maulik\FHS\15-08-2022\YSL\FHS\YSLProject\Views\User\Index.cshtml"
                               Write(Html.DisplayFor(modelItem => item.UserEmail));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                </td>\r\n                                <td>\r\n                                    ");
#nullable restore
#line 62 "D:\Maulik\FHS\15-08-2022\YSL\FHS\YSLProject\Views\User\Index.cshtml"
                               Write(Html.DisplayFor(modelItem => item.IsActives));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                </td>\r\n                                <td>\r\n                                    ");
#nullable restore
#line 65 "D:\Maulik\FHS\15-08-2022\YSL\FHS\YSLProject\Views\User\Index.cshtml"
                               Write(Html.DisplayFor(modelItem => item.LoginDate));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                </td>\r\n                                <td>\r\n                                    ");
#nullable restore
#line 68 "D:\Maulik\FHS\15-08-2022\YSL\FHS\YSLProject\Views\User\Index.cshtml"
                               Write(Html.DisplayFor(modelItem => item.UserTypes));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                </td>\r\n                                <td class=\"text-center\">\r\n                                    ");
#nullable restore
#line 71 "D:\Maulik\FHS\15-08-2022\YSL\FHS\YSLProject\Views\User\Index.cshtml"
                               Write(Html.ActionLink("Edit", "Edit", new { Id = item.UserID }, new { @class = "btn btn-warning" }));

#line default
#line hidden
#nullable disable
            WriteLiteral(" |\r\n");
            WriteLiteral("                                    <button class=\"btn btn-danger\"");
            BeginWriteAttribute("onclick", " onclick=\"", 3199, "\"", 3242, 4);
            WriteAttributeValue("", 3209, "return", 3209, 6, true);
            WriteAttributeValue(" ", 3215, "deleteUser(\'", 3216, 13, true);
#nullable restore
#line 74 "D:\Maulik\FHS\15-08-2022\YSL\FHS\YSLProject\Views\User\Index.cshtml"
WriteAttributeValue("", 3228, item.UserID, 3228, 12, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 3240, "\')", 3240, 2, true);
            EndWriteAttribute();
            WriteLiteral(" id=\"btnUDelete\">Delete</button>\r\n\r\n                                </td>\r\n                            </tr>\r\n");
#nullable restore
#line 78 "D:\Maulik\FHS\15-08-2022\YSL\FHS\YSLProject\Views\User\Index.cshtml"
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
<script
  src=""https://code.jquery.com/jquery-3.3.1.min.js""
  integrity=""sha256-FgpCb/KJQlLNfOu91ta32o/NMZxltwRo8QtmkMRdAu8=""
  crossorigin=""anonymous""></script>
<script>

    function deleteUser(id) {
        if (confirm('Are you sure you want to delete this user - have you removed all roles for this user?')) {
            var url = ""/User/Delete"";
            $.post(url, { Id: id }, function (data) {
                location.reload();
                alert(""Delete user successfully"");
            });
            return true;
        }
        else {
            return false;
        }
    }
    $(document).ready(function () {
        $('#tblUser').DataTable({
            'paging': true,
            'lengthChange': true,
            'searching': true,
            ""order"": [0,""asc""],
            'info': true,
            'autoWidth': true,
            //'columnDefs': [{
 ");
            WriteLiteral("           //    \'targets\': [4],\r\n            //    \'orderable\': false,\r\n            //}]\r\n        })\r\n    });\r\n</script> ");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<YSLProject.Models.UserMasterModel>> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
