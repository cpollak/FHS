#pragma checksum "\\192.168.1.110\kirtan\Projects\15-08-2022\YSL\FHS\YSLProject\Views\Member\Reports.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "85c1e138a3402c07660516af01152f5468cd158e"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Member_Reports), @"mvc.1.0.view", @"/Views/Member/Reports.cshtml")]
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
#line 1 "\\192.168.1.110\kirtan\Projects\15-08-2022\YSL\FHS\YSLProject\Views\_ViewImports.cshtml"
using YSLProject;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "\\192.168.1.110\kirtan\Projects\15-08-2022\YSL\FHS\YSLProject\Views\_ViewImports.cshtml"
using YSLProject.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"85c1e138a3402c07660516af01152f5468cd158e", @"/Views/Member/Reports.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"98be823b74c24c3530062c89b3b821261833111c", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Member_Reports : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<YSLProject.Models.MemberMasterModel>
    #nullable disable
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Reports", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("enctype", new global::Microsoft.AspNetCore.Html.HtmlString("multipart/form-data"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "\\192.168.1.110\kirtan\Projects\15-08-2022\YSL\FHS\YSLProject\Views\Member\Reports.cshtml"
  
    ViewData["Title"] = "Reports";

#line default
#line hidden
#nullable disable
            WriteLiteral("<style>\r\n    .btn-warning, .btn-danger {\r\n        padding: 3px 1rem;\r\n    }\r\n</style>\r\n\r\n\r\n\r\n    <div class=\"col-md-12\">\r\n        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "85c1e138a3402c07660516af01152f5468cd158e4383", async() => {
                WriteLiteral(@"
            <div class=""form-group row"">
                <div class=""col-md-2"">
                    <label>
                        From Date
                    </label>
                    <input type=""date"" data-date-format=""MM/dd/yyyy"" id=""followupS"" name=""StartDate"" class=""form-control"" />
                </div>

                <div class=""col-md-2"">
                    <label> To Date :</label>
                    <input type=""date"" data-date-format=""MM/dd/yyyy"" id=""followupE"" name=""EndDate"" class=""form-control""/>
                </div>
                <div class=""col-md-3"">
                    <label>&nbsp;</label>
                    ");
#nullable restore
#line 30 "\\192.168.1.110\kirtan\Projects\15-08-2022\YSL\FHS\YSLProject\Views\Member\Reports.cshtml"
               Write(Html.DropDownListFor(model => model.ReportsType, (IEnumerable<SelectListItem>)ViewBag.ReportTypes, htmlAttributes: new { @class = "form-control" }));

#line default
#line hidden
#nullable disable
                WriteLiteral(@"

                </div>
                <div class=""col-md-3"">
                    <label>&nbsp;</label>
                    <input type=""submit"" value=""Generate Report"" class=""btn btn-primary"" style=""display: block;"" />

                </div>
            </div>
        ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n    </div>\r\n</div>\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral("\r\n\r\n");
#nullable restore
#line 44 "\\192.168.1.110\kirtan\Projects\15-08-2022\YSL\FHS\YSLProject\Views\Member\Reports.cshtml"
      await Html.RenderPartialAsync("_ValidationScriptsPartial");
    

#line default
#line hidden
#nullable disable
                WriteLiteral("    <script>\r\n        function GenerateReport() {\r\n\r\n             $.ajax({\r\n                        url: \'");
#nullable restore
#line 50 "\\192.168.1.110\kirtan\Projects\15-08-2022\YSL\FHS\YSLProject\Views\Member\Reports.cshtml"
                         Write(Url.Action("GenerateReport", "Member"));

#line default
#line hidden
#nullable disable
                WriteLiteral(@"',
                 type: ""POST"",
                 data: { ""RType"": $(""#ReportType"").val() },
                 success: function (data) {
                     //DownloadFile(""NoncoverdActivity.pdf"")
                        },
                        error: function (Result) {
                        }
                    });
        }


    </script>
");
            }
            );
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<YSLProject.Models.MemberMasterModel> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
