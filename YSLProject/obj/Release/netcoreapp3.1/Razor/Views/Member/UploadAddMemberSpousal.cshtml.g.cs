#pragma checksum "\\192.168.1.110\kirtan\Projects\15-08-2022\YSL\FHS\YSLProject\Views\Member\UploadAddMemberSpousal.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "aac03fcc2493ff3a11f60fcd0bffbc841f77b10d"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Member_UploadAddMemberSpousal), @"mvc.1.0.view", @"/Views/Member/UploadAddMemberSpousal.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"aac03fcc2493ff3a11f60fcd0bffbc841f77b10d", @"/Views/Member/UploadAddMemberSpousal.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"98be823b74c24c3530062c89b3b821261833111c", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Member_UploadAddMemberSpousal : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<YSLProject.Models.MemberMasterModel>
    #nullable disable
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("text-danger"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "UploadAddMemberSpousal", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("enctype", new global::Microsoft.AspNetCore.Html.HtmlString("multipart/form-data"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
            WriteLiteral("\r\n");
#nullable restore
#line 3 "\\192.168.1.110\kirtan\Projects\15-08-2022\YSL\FHS\YSLProject\Views\Member\UploadAddMemberSpousal.cshtml"
  
    ViewData["Title"] = "AddMember";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<h3>Import Member spousal</h3>

<link rel=""stylesheet"" href=""https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css"">
<hr />
<style>
    #Professional {
        margin-left: 54%;
        height: 90%;
    }

    #ProfessionalDelete {
        margin-left: 54%;
        height: 90%;
    }
</style>
<div class=""row"">
    <div class=""col-md-12"">
        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "aac03fcc2493ff3a11f60fcd0bffbc841f77b10d5250", async() => {
                WriteLiteral("\r\n            ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("div", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "aac03fcc2493ff3a11f60fcd0bffbc841f77b10d5520", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationSummaryTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.ValidationSummaryTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_ValidationSummaryTagHelper);
#nullable restore
#line 25 "\\192.168.1.110\kirtan\Projects\15-08-2022\YSL\FHS\YSLProject\Views\Member\UploadAddMemberSpousal.cshtml"
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
                WriteLiteral("           \r\n        <div class=\"form-group row\">\r\n\r\n            <div class=\"col-md-2\" id=\"divFacility\">\r\n                ");
#nullable restore
#line 35 "\\192.168.1.110\kirtan\Projects\15-08-2022\YSL\FHS\YSLProject\Views\Member\UploadAddMemberSpousal.cshtml"
           Write(Html.LabelFor(model => model.Facility, htmlAttributes: new { @class = "control-label " }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                ");
#nullable restore
#line 36 "\\192.168.1.110\kirtan\Projects\15-08-2022\YSL\FHS\YSLProject\Views\Member\UploadAddMemberSpousal.cshtml"
           Write(Html.DropDownListFor(model => model.Facility, (IEnumerable<SelectListItem>)ViewBag.FacilityType, htmlAttributes: new { @class = "form-control" }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                ");
#nullable restore
#line 37 "\\192.168.1.110\kirtan\Projects\15-08-2022\YSL\FHS\YSLProject\Views\Member\UploadAddMemberSpousal.cshtml"
           Write(Html.ValidationMessageFor(model => model.MemberStatus, "", new { @class = "text-danger", @Id = "EFacility" }));

#line default
#line hidden
#nullable disable
                WriteLiteral(@"
            </div>
            <div class=""col-md-4"">
                <label class=""control-label "">Upload File</label>
                <input type=""file"" name=""postedFile"" style=""display: block;"" required />
            </div>
            <div class=""col-md-2"">
                <label class=""control-label "">&nbsp;</label>
                <input type=""submit"" value=""Save"" class=""btn btn-primary sve"" style=""display: block;"" />
            </div>
        </div>
            <div class=""form-group"">
                
            </div>
        ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n    </div>\r\n</div>\r\n\r\n<div>\r\n");
            WriteLiteral("</div>\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral("\r\n");
#nullable restore
#line 59 "\\192.168.1.110\kirtan\Projects\15-08-2022\YSL\FHS\YSLProject\Views\Member\UploadAddMemberSpousal.cshtml"
      await Html.RenderPartialAsync("_ValidationScriptsPartial");

#line default
#line hidden
#nullable disable
                WriteLiteral(@"
<script>
    $("".sve"").click(function () {
       
        var check = false;

        if ($(""#Facility"").val() == '') {
            $(""#EFacility"").text('Select Facility..');
            check = true;
        }
        if (check == true)
            return false;
        if ($(""#Facility"").val() != '') {
            $("".desutj"").css(""display"", ""block"");
        }
    });
    //$("".sve"").click(function () {
    //    debugger;
    //    if ($(""form"").valid() == false) {
    //        return false;
    //    }
    //});

    //$(function () {
    //    $("".Fromdatepicker"").datepicker({

    //        dateFormat: ""dd-MM-yy"",
    //        changeMonth: true,
    //        changeYear: true,
    //        yearRange: ""c-20:c+20"",
    //        //  minDate: new Date(1999, 0, 1)
    //        //maxDate: new Date(2018, 0, 1)
    //    });

    //});

    function ChangeMemberStatus() {
        var status = $(""#ddlMemberStatus"").val();
        if (status == ""1"") {
            $(""");
                WriteLiteral("#divFacility\").css(\"display\", \"inline\")\r\n            $(\"#divDate\").css(\"display\", \"inline\")\r\n        }\r\n        else {\r\n            $(\"#divFacility\").css(\"display\", \"none\")\r\n            $(\"#divDate\").css(\"display\", \"none\")\r\n        }\r\n    }\r\n</script>\r\n");
            }
            );
            WriteLiteral("\r\n\r\n");
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
