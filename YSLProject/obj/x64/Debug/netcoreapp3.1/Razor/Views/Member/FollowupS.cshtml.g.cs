#pragma checksum "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\FollowupS.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "861632cf97c26db249ade8a0c07e9c4c3fbf7c8d"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Member_FollowupS), @"mvc.1.0.view", @"/Views/Member/FollowupS.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"861632cf97c26db249ade8a0c07e9c4c3fbf7c8d", @"/Views/Member/FollowupS.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"98be823b74c24c3530062c89b3b821261833111c", @"/Views/_ViewImports.cshtml")]
    public class Views_Member_FollowupS : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<YSLProject.Models.RecertificationFollowUp>
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
#line 2 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\FollowupS.cshtml"
  
    ViewData["Title"] = "FollowupS";
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
#line 19 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\FollowupS.cshtml"
 using (Html.BeginForm("FollowupS", "Member", FormMethod.Post,
 new
 {
     enctype = "multipart/form-data"
 }))
{


#line default
#line hidden
#nullable disable
            WriteLiteral("<div class=\"form-horizontal\">\r\n    <div >\r\n        <div class=\"modal-padding\">\r\n\r\n            <div class=\"detail-section\">\r\n\r\n                ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("div", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "861632cf97c26db249ade8a0c07e9c4c3fbf7c8d4842", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationSummaryTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.ValidationSummaryTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_ValidationSummaryTagHelper);
#nullable restore
#line 32 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\FollowupS.cshtml"
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
            WriteLiteral("\r\n");
            WriteLiteral("                ");
#nullable restore
#line 38 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\FollowupS.cshtml"
           Write(Html.HiddenFor(model => model.MemberId));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                ");
#nullable restore
#line 39 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\FollowupS.cshtml"
           Write(Html.HiddenFor(model => model.FollowUpID));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n                <div class=\"form-group row\">\r\n                    <div class=\"col-md-4\">\r\n                        ");
#nullable restore
#line 43 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\FollowupS.cshtml"
                   Write(Html.LabelFor(model => model.CurrentStatus, htmlAttributes: new { @class = "control-label " }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        ");
#nullable restore
#line 44 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\FollowupS.cshtml"
                   Write(Html.DropDownListFor(model => model.CurrentStatus, (IEnumerable<SelectListItem>)ViewBag.CuurentStatusList, htmlAttributes: new { @class = "form-control" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        ");
#nullable restore
#line 45 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\FollowupS.cshtml"
                   Write(Html.ValidationMessageFor(model => model.CurrentStatus, "", new { @class = "text-danger" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </div>\r\n                    <div class=\"col-md-4\">\r\n                        ");
#nullable restore
#line 48 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\FollowupS.cshtml"
                   Write(Html.LabelFor(model => model.Outcome, htmlAttributes: new { @class = "control-label " }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        ");
#nullable restore
#line 49 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\FollowupS.cshtml"
                   Write(Html.EditorFor(model => model.Outcome, new { htmlAttributes = new { @class = "form-control" } }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        ");
#nullable restore
#line 50 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\FollowupS.cshtml"
                   Write(Html.ValidationMessageFor(model => model.Outcome, "", new { @class = "text-danger" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </div>\r\n                    <div class=\"col-md-4\">\r\n                        ");
#nullable restore
#line 53 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\FollowupS.cshtml"
                   Write(Html.LabelFor(model => model.Notes, htmlAttributes: new { @class = "control-label " }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        ");
#nullable restore
#line 54 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\FollowupS.cshtml"
                   Write(Html.EditorFor(model => model.Notes, new { htmlAttributes = new { @class = "form-control" } }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        ");
#nullable restore
#line 55 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\FollowupS.cshtml"
                   Write(Html.ValidationMessageFor(model => model.Notes, "", new { @class = "text-danger" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </div>\r\n                </div>\r\n                <div class=\"form-group row\">\r\n                    <div class=\"col-md-3\">\r\n                        ");
#nullable restore
#line 60 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\FollowupS.cshtml"
                   Write(Html.LabelFor(model => model.NextStepTask, htmlAttributes: new { @class = "control-label " }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        ");
#nullable restore
#line 61 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\FollowupS.cshtml"
                   Write(Html.DropDownListFor(model => model.NextStepTask, (IEnumerable<SelectListItem>)ViewBag.NextStepTaskList, htmlAttributes: new { @class = "form-control" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        ");
#nullable restore
#line 62 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\FollowupS.cshtml"
                   Write(Html.ValidationMessageFor(model => model.NextStepTask, "", new { @class = "text-danger" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </div>\r\n                    <div class=\"col-md-3\">\r\n                        ");
#nullable restore
#line 65 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\FollowupS.cshtml"
                   Write(Html.LabelFor(model => model.NextStepDueNotes, htmlAttributes: new { @class = "control-label " }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        ");
#nullable restore
#line 66 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\FollowupS.cshtml"
                   Write(Html.EditorFor(model => model.NextStepDueNotes, new { htmlAttributes = new { @class = "form-control" } }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        ");
#nullable restore
#line 67 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\FollowupS.cshtml"
                   Write(Html.ValidationMessageFor(model => model.NextStepDueNotes, "", new { @class = "text-danger" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </div>\r\n                    <div class=\"col-md-3\">\r\n                        ");
#nullable restore
#line 70 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\FollowupS.cshtml"
                   Write(Html.LabelFor(model => model.Nextduedate, htmlAttributes: new { @class = "control-label " }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        ");
#nullable restore
#line 71 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\FollowupS.cshtml"
                   Write(Html.EditorFor(model => model.Nextduedate, new { htmlAttributes = new { @class = "form-control", @type = "date" } }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        ");
#nullable restore
#line 72 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\FollowupS.cshtml"
                   Write(Html.ValidationMessageFor(model => model.Nextduedate, "", new { @class = "text-danger" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </div>\r\n                    <div class=\"col-md-3\">\r\n                        ");
#nullable restore
#line 75 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\FollowupS.cshtml"
                   Write(Html.LabelFor(model => model.NewStatus, htmlAttributes: new { @class = "control-label " }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        ");
#nullable restore
#line 76 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\FollowupS.cshtml"
                   Write(Html.DropDownListFor(model => model.NewStatus, (IEnumerable<SelectListItem>)ViewBag.NewStatusList, htmlAttributes: new { @class = "form-control" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        ");
#nullable restore
#line 77 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\FollowupS.cshtml"
                   Write(Html.ValidationMessageFor(model => model.NewStatus, "", new { @class = "text-danger" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </div>\r\n                </div>\r\n\r\n\r\n            </div>\r\n\r\n        </div>\r\n    </div>\r\n");
            WriteLiteral("    <div class=\"form-group row btn-row\">\r\n        <div class=\" col-md-12 \">\r\n            <button type=\"submit\" class=\"btn btn-primary sve\"><i class=\"fa fa-save\"></i> Submit</button>\r\n            <a class=\"btn btn-primary\"");
            BeginWriteAttribute("href", " href=\"", 4867, "\"", 4917, 2);
            WriteAttributeValue("", 4874, "/Member/Workfollow?memberId=", 4874, 28, true);
#nullable restore
#line 92 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\FollowupS.cshtml"
WriteAttributeValue("", 4902, Model.MemberId, 4902, 15, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">Cancel</a>\r\n            \r\n            \r\n        </div>\r\n\r\n    </div>\r\n</div>\r\n");
#nullable restore
#line 99 "D:\Destiny Project\modern-admin-clean-bootstrap-4-dashboard-html-temp()\Project\03_05_2021\03_05_2021\YSLProject\YSLProject\Views\Member\FollowupS.cshtml"
}

#line default
#line hidden
#nullable disable
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<YSLProject.Models.RecertificationFollowUp> Html { get; private set; }
    }
}
#pragma warning restore 1591
