#pragma checksum "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\GeneralNotes.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a6e9ddd5c3c12650d29b997234b5ae48f2769424"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Member_GeneralNotes), @"mvc.1.0.view", @"/Views/Member/GeneralNotes.cshtml")]
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
#line 1 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\_ViewImports.cshtml"
using YSLProject;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\_ViewImports.cshtml"
using YSLProject.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a6e9ddd5c3c12650d29b997234b5ae48f2769424", @"/Views/Member/GeneralNotes.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"98be823b74c24c3530062c89b3b821261833111c", @"/Views/_ViewImports.cshtml")]
    public class Views_Member_GeneralNotes : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<YSLProject.Models.GeneralNotesModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("text-danger"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.ValidationSummaryTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationSummaryTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\GeneralNotes.cshtml"
  
    ViewData["Title"] = "General Notes";
    Layout = "~/Views/Shared/_Layoutpopup.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<style>
    .modal-body .btn-row {
        margin-top: 0rem;
        margin-bottom: 0rem;
        padding-top: 15px;
        padding-bottom: 15px;
    }

    .btn-row {
        border-top: 1px solid #ccc;
        text-align: right;
    }
</style>
");
#nullable restore
#line 20 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\GeneralNotes.cshtml"
 using (Html.BeginForm("GeneralNotes", "Member", FormMethod.Post,
new
{
   enctype = "multipart/form-data"
}))
{
    

#line default
#line hidden
#nullable disable
#nullable restore
#line 26 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\GeneralNotes.cshtml"
Write(Html.HiddenFor(model => model.MemberId));

#line default
#line hidden
#nullable disable
#nullable restore
#line 27 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\GeneralNotes.cshtml"
Write(Html.HiddenFor(model => model.ID));

#line default
#line hidden
#nullable disable
            WriteLiteral("    <div class=\"form-horizontal\">\r\n        <div>\r\n            <div class=\"modal-padding\">\r\n\r\n                <div class=\"detail-section\">\r\n\r\n                    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("div", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "a6e9ddd5c3c12650d29b997234b5ae48f27694244906", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationSummaryTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.ValidationSummaryTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_ValidationSummaryTagHelper);
#nullable restore
#line 35 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\GeneralNotes.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_ValidationSummaryTagHelper.ValidationSummary = global::Microsoft.AspNetCore.Mvc.Rendering.ValidationSummary.ModelOnly;

#line default
#line hidden
#nullable disable
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-validation-summary", __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationSummaryTagHelper.ValidationSummary, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n                    <div class=\"form-group row\">\r\n                        <div class=\"col-md-3\">\r\n                            ");
#nullable restore
#line 39 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\GeneralNotes.cshtml"
                       Write(Html.LabelFor(model => model.Type, htmlAttributes: new { @class = "control-label " }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            ");
#nullable restore
#line 40 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\GeneralNotes.cshtml"
                       Write(Html.TextBoxFor(model => model.Type, htmlAttributes: new { @class = "form-control" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n                            ");
#nullable restore
#line 42 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\GeneralNotes.cshtml"
                       Write(Html.ValidationMessageFor(model => model.Type, "", new { @class = "text-danger" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        </div>\r\n\r\n\r\n                        <div class=\"col-md-3\">\r\n                            ");
#nullable restore
#line 47 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\GeneralNotes.cshtml"
                       Write(Html.LabelFor(model => model.Notes, htmlAttributes: new { @class = "control-label " }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            ");
#nullable restore
#line 48 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\GeneralNotes.cshtml"
                       Write(Html.TextAreaFor(model => model.Notes, new { htmlAttributes = new { @class = "form-control" }, rows = "2", @style = "width: 100%;" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                            ");
#nullable restore
#line 49 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\GeneralNotes.cshtml"
                       Write(Html.ValidationMessageFor(model => model.Notes, "", new { @class = "text-danger" }));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                        </div>
                    </div>


                </div>

            </div>
        </div>
        <div class=""form-group row btn-row"">
            <div class="" col-md-12 "">
                <button type=""submit"" class=""btn btn-primary sve""><i class=""fa fa-save""></i> Submit</button>
                <a class=""btn btn-primary""");
            BeginWriteAttribute("href", " href=\"", 2132, "\"", 2182, 2);
            WriteAttributeValue("", 2139, "/Member/Workfollow?memberId=", 2139, 28, true);
#nullable restore
#line 61 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\GeneralNotes.cshtml"
WriteAttributeValue("", 2167, Model.MemberId, 2167, 15, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">Cancel</a>\r\n            </div>\r\n        </div>\r\n    </div>\r\n");
#nullable restore
#line 65 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\GeneralNotes.cshtml"
}

#line default
#line hidden
#nullable disable
            DefineSection("Scripts", async() => {
                WriteLiteral("\r\n    <script>\r\n       \r\n    </script>\r\n");
            }
            );
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<YSLProject.Models.GeneralNotesModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
