#pragma checksum "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2deceb443190d3ecb437ca08c7bef97778ac0f6f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Member_Followup), @"mvc.1.0.view", @"/Views/Member/Followup.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2deceb443190d3ecb437ca08c7bef97778ac0f6f", @"/Views/Member/Followup.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"98be823b74c24c3530062c89b3b821261833111c", @"/Views/_ViewImports.cshtml")]
    public class Views_Member_Followup : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<YSLProject.Models.RecertificationFollowUp>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("text-danger"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Followup", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.ValidationSummaryTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationSummaryTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
  
    ViewData["Title"] = "Followup";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1>Member follow up</h1>\r\n\r\n<link rel=\"stylesheet\" href=\"https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css\">\r\n\r\n\r\n\r\n\r\n<div class=\"row\">\r\n    <div class=\"col-md-12\">\r\n        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "2deceb443190d3ecb437ca08c7bef97778ac0f6f4484", async() => {
                WriteLiteral("\r\n            ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("div", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "2deceb443190d3ecb437ca08c7bef97778ac0f6f4754", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationSummaryTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.ValidationSummaryTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_ValidationSummaryTagHelper);
#nullable restore
#line 16 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
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
                WriteLiteral("            ");
#nullable restore
#line 22 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
       Write(Html.HiddenFor(model => model.MemberId));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n            <div class=\"form-group row\">\r\n                <div class=\"col-md-4\">\r\n                    ");
#nullable restore
#line 25 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
               Write(Html.LabelFor(model => model.CurrentStatus, htmlAttributes: new { @class = "control-label " }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                    ");
#nullable restore
#line 26 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
               Write(Html.EditorFor(model => model.CurrentStatus, new { htmlAttributes = new { @class = "form-control" } }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                    ");
#nullable restore
#line 27 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
               Write(Html.ValidationMessageFor(model => model.CurrentStatus, "", new { @class = "text-danger" }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                </div>\r\n                <div class=\"col-md-4\">\r\n                    ");
#nullable restore
#line 30 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
               Write(Html.LabelFor(model => model.Outcome, htmlAttributes: new { @class = "control-label " }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                    ");
#nullable restore
#line 31 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
               Write(Html.EditorFor(model => model.Outcome, new { htmlAttributes = new { @class = "form-control" } }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                    ");
#nullable restore
#line 32 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
               Write(Html.ValidationMessageFor(model => model.Outcome, "", new { @class = "text-danger" }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                </div>\r\n                <div class=\"col-md-4\">\r\n                    ");
#nullable restore
#line 35 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
               Write(Html.LabelFor(model => model.Notes, htmlAttributes: new { @class = "control-label " }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                    ");
#nullable restore
#line 36 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
               Write(Html.EditorFor(model => model.Notes, new { htmlAttributes = new { @class = "form-control" } }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                    ");
#nullable restore
#line 37 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
               Write(Html.ValidationMessageFor(model => model.Notes, "", new { @class = "text-danger" }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                </div>\r\n            </div>\r\n            <div class=\"form-group row\">\r\n                <div class=\"col-md-3\">\r\n                    ");
#nullable restore
#line 42 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
               Write(Html.LabelFor(model => model.NextStepTask, htmlAttributes: new { @class = "control-label " }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                    ");
#nullable restore
#line 43 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
               Write(Html.EditorFor(model => model.NextStepTask, new { htmlAttributes = new { @class = "form-control" } }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                    ");
#nullable restore
#line 44 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
               Write(Html.ValidationMessageFor(model => model.NextStepTask, "", new { @class = "text-danger" }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                </div>\r\n                <div class=\"col-md-3\">\r\n                    ");
#nullable restore
#line 47 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
               Write(Html.LabelFor(model => model.NextStepDueNotes, htmlAttributes: new { @class = "control-label " }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                    ");
#nullable restore
#line 48 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
               Write(Html.EditorFor(model => model.NextStepDueNotes, new { htmlAttributes = new { @class = "form-control" } }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                    ");
#nullable restore
#line 49 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
               Write(Html.ValidationMessageFor(model => model.NextStepDueNotes, "", new { @class = "text-danger" }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                </div>\r\n                <div class=\"col-md-3\">\r\n                    ");
#nullable restore
#line 52 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
               Write(Html.LabelFor(model => model.Nextduedate, htmlAttributes: new { @class = "control-label " }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                    ");
#nullable restore
#line 53 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
               Write(Html.EditorFor(model => model.Nextduedate, new { htmlAttributes = new { @class = "form-control", @type = "date" } }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                    ");
#nullable restore
#line 54 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
               Write(Html.ValidationMessageFor(model => model.Nextduedate, "", new { @class = "text-danger" }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                </div>\r\n                <div class=\"col-md-3\">\r\n                    ");
#nullable restore
#line 57 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
               Write(Html.LabelFor(model => model.NewStatus, htmlAttributes: new { @class = "control-label " }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                    ");
#nullable restore
#line 58 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
               Write(Html.EditorFor(model => model.NewStatus, new { htmlAttributes = new { @class = "form-control" } }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                    ");
#nullable restore
#line 59 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
               Write(Html.ValidationMessageFor(model => model.NewStatus, "", new { @class = "text-danger" }));

#line default
#line hidden
#nullable disable
                WriteLiteral(@"
                </div>
            </div>
            <div class=""form-group "">
                <input type=""submit"" value=""Save"" class=""btn btn-primary"" />
            </div>
            <div class=""form-group row"" style=""display:none;"">
                <div class=""container"">
                    <div class=""table-responsive datatable"">
                        <table id=""example1"" class=""table table-bordered table-striped testing"">
                            <thead>
                                <tr>

                                    <th>
                                        ");
#nullable restore
#line 73 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
                                   Write(Html.DisplayName("Current Status"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                                    </th>\r\n                                    <th>\r\n                                        ");
#nullable restore
#line 76 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
                                   Write(Html.DisplayName("Out Come"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                                    </th>\r\n                                    <th>\r\n                                        ");
#nullable restore
#line 79 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
                                   Write(Html.DisplayName("Notes Required"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                                    </th>\r\n                                    <th>\r\n                                        ");
#nullable restore
#line 82 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
                                   Write(Html.DisplayName("Next Steps/Task"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                                    </th>\r\n                                    <th>\r\n                                        ");
#nullable restore
#line 85 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
                                   Write(Html.DisplayName("Next Steps Due Date"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                                    </th>\r\n                                    <th>\r\n                                        ");
#nullable restore
#line 88 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
                                   Write(Html.DisplayName("Next Status"));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                                    </th>\r\n                                </tr>\r\n                            </thead>\r\n                            <tbody id=\"dvBody\">\r\n");
#nullable restore
#line 93 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
                                 foreach (var item in Model.recertificLists)
                                {

#line default
#line hidden
#nullable disable
                WriteLiteral("                                    <tr>\r\n\r\n                                        <td>\r\n                                            ");
#nullable restore
#line 98 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
                                       Write(Html.DisplayFor(modelItem => item.CurrentStatus));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                                        </td>\r\n                                        <td>\r\n                                            ");
#nullable restore
#line 101 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
                                       Write(Html.DisplayFor(modelItem => item.Outcome));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                                        </td>\r\n                                        <td>\r\n                                            ");
#nullable restore
#line 104 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
                                       Write(Html.DisplayFor(modelItem => item.NextStepDueNotes));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                                        </td>\r\n                                        <td>\r\n                                            ");
#nullable restore
#line 107 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
                                       Write(Html.DisplayFor(modelItem => item.NextStepTask));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                                        </td>\r\n                                        <td>\r\n                                            ");
#nullable restore
#line 110 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
                                       Write(Html.DisplayFor(modelItem => item.NextStepDueNotes));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                                        </td>\r\n                                        <td>\r\n                                            ");
#nullable restore
#line 113 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
                                       Write(Html.DisplayFor(modelItem => item.NewStatus));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                                        </td>\r\n                                    </tr>\r\n");
#nullable restore
#line 116 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
                                }

#line default
#line hidden
#nullable disable
                WriteLiteral("                            </tbody>\r\n                        </table>\r\n                    </div>\r\n                    </div>\r\n                </div>\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n    </div>\r\n</div>\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral("\r\n");
#nullable restore
#line 126 "D:\Destiny Project\YSL\cpollak\FHS\YSLProject\Views\Member\Followup.cshtml"
      await Html.RenderPartialAsync("_ValidationScriptsPartial");

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n    \r\n\r\n\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<YSLProject.Models.RecertificationFollowUp> Html { get; private set; }
    }
}
#pragma warning restore 1591
