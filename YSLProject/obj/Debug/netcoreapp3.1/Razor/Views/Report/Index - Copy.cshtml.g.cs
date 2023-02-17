#pragma checksum "D:\Maulik\FHS\15-08-2022\YSL\FHS\YSLProject\Views\Report\Index - Copy.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a27fe88751eca76bed9baa19e16e195bd778e7a0"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Report_Index___Copy), @"mvc.1.0.view", @"/Views/Report/Index - Copy.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a27fe88751eca76bed9baa19e16e195bd778e7a0", @"/Views/Report/Index - Copy.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"98be823b74c24c3530062c89b3b821261833111c", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Report_Index___Copy : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<YSLProject.Table.MemberMaster>>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n\r\n\r\n\r\n");
#nullable restore
#line 6 "D:\Maulik\FHS\15-08-2022\YSL\FHS\YSLProject\Views\Report\Index - Copy.cshtml"
  
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
        <h1>Non Covered Report</h1>
    </div>
    <div class=""col-md-6"" style=""text-align:end;"">
");
            WriteLiteral("    </div>\r\n</div>\r\n");
            WriteLiteral(@"
    <div class=""col-md-12"" style=""text-align:right"">
        <label style=""font-weight:bold"">&nbsp;</label><br />
        <button style=""width:200px;"" onclick=""ExporttoExcel(1)"" title=""Export to Excel"" class=""btn btn-primary"">Export to Excel</button>
    </div>
<div>
    &nbsp;
</div>
    <div class=""card"">
        <div class=""card-content collapse show"">
            <div class=""card-body card-dashboard dataTables_wrapper dt-bootstrap"">
                <div class=""table-responsive"">
                    <table class=""table table-striped table-bordered"" id=""example"" style=""width:100%"">
                        <thead>
                            <tr>

                                <th>
                                    Medicaid ID
                                </th>
                                <th>
                                    Member
                                </th>
                                <th>
                                    Lost Eligibility Date
      ");
            WriteLiteral(@"                          </th>
                                <th>
                                    CPHL Comments
                                </th>
                            </tr>
                        </thead>
                        <tbody></tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>





    <script src=""https://code.jquery.com/jquery-3.5.1.js""></script>
    <script src=""https://cdn.datatables.net/1.10.24/js/jquery.dataTables.min.js""></script>


    <script>

                    $(document).ready(function () {
                        getMemeberList();
                    })

                    function getMemeberList() {
                             $('#example').DataTable({
                            processing: true,
                            serverSide: true,
                            searchable: true,
                                 destroy: true,
                                 ""order"": [");
            WriteLiteral("[0, \"desc\"]],\r\n                            ajax: {\r\n                                url: \'");
#nullable restore
#line 86 "D:\Maulik\FHS\15-08-2022\YSL\FHS\YSLProject\Views\Report\Index - Copy.cshtml"
                                 Write(Url.Action("NoncoveredReport", "Report"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"',
                                type: 'POST',
                                ""datatype"": ""json""
                                },
                                columns: [
                                    { data: ""medicaidID"", ""name"": ""MedicaidID""},
                                    { data: ""firstName"", ""name"": ""FirstName"" },
                                    { data: ""lostEligibilityDate"", ""name"": ""LostEligibilityDate"" },
                                    { data: ""cphlComments"", ""name"": ""CPHLComments"" }
                                ]
                            });
                    }
    function ExporttoExcel(iii) {

            $.ajax({
            url: '");
#nullable restore
#line 101 "D:\Maulik\FHS\15-08-2022\YSL\FHS\YSLProject\Views\Report\Index - Copy.cshtml"
             Write(Url.Action("ExportExcelData", "Report"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"',
                type: ""GET"",
                data: {
                    ""Report"": iii,
                },
                success: function (data) {
                
                var bytes = Base64ToBytes(data.fileContents);
                //var bytes = new Uint8Array(data);
                var blob = new Blob([bytes], { type: ""application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"" });
                var link = document.createElement('a');
                link.href = window.URL.createObjectURL(blob);
                link.download = ""NonCoveredReport.xlsx"";
                link.click();
            },
            error: function (Result) {
            }
        });
    }
    function Base64ToBytes(base64) {
        var s = window.atob(base64);
        var bytes = new Uint8Array(s.length);
        for (var i = 0; i < s.length; i++) {
            bytes[i] = s.charCodeAt(i);
        }
        return bytes;
    };

                    //$('#example').DataTable({
  ");
            WriteLiteral(@"                  //    'paging': true,
                    //    'lengthChange': true,
                    //    'searching': true,
                    //    ""order"": [],
                    //    'info': true,
                    //    'autoWidth': true,
                    //    'columnDefs': [{
                    //        'targets': [4],
                    //        'orderable': false,
                    //    }]
                    //})
    </script>

");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<YSLProject.Table.MemberMaster>> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591