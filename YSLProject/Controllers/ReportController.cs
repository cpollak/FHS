using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSLProject.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using YSLProject.Table;
using System.IO;
using System.Data;
using Newtonsoft.Json;
using ClosedXML.Excel;
using static YSLProject.Models.Enumdata;
using System.Reflection;
using System.Text;

namespace YSLProject.Controllers
{
    [Autho]
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        private IHostingEnvironment Environment;

        private IConfiguration Configuration;

        public ReportController(IHostingEnvironment _environment, ApplicationDbContext context, IConfiguration _configuration)
        {
            Environment = _environment;
            _context = context;
            Configuration = _configuration;
        }
        public IActionResult Index()
        {
            HttpContext.Session.SetString("followupS", "");
            HttpContext.Session.SetString("followupE", "");
            HttpContext.Session.SetString("Phone", "");
            HttpContext.Session.SetString("ResidenceID", "");
            HttpContext.Session.SetString("MedicaidID", "");
            HttpContext.Session.SetString("RecertMonth", "");
            HttpContext.Session.SetString("Facility", "");
            HttpContext.Session.SetString("Language", "");
            HttpContext.Session.SetString("MembershipStatus", "");
            HttpContext.Session.SetString("RecertMonth", "");
            return View();
        }

        [HttpPost]
        public IActionResult NoncoveredReport(int? length, int? start)
        {

            var FollowupStatusData = _context.FollowupStatusMaster.ToList();
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var sortColumn = HttpContext.Request.Form["columns[" + HttpContext.Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDir = HttpContext.Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = HttpContext.Request.Form["search[value]"].FirstOrDefault();
            int recordsTotal = 0;
            List<CenterReportModelNon> obj = null;
            try
            {
                SqlParameter TotalCount = new SqlParameter("@TotalCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
                obj = _context.CenterReportModelNonSPs.FromSqlRaw("SP_CenterReport @Spara,@SearchbyName,@SortColumn,@SortOrder,@Start,@Length,@UserId,@RecertMonth,@TotalCount OUT",
                                      new Object[]
                                      {
                                           new SqlParameter("@Spara", "4"),
                    new SqlParameter("@SearchbyName",(searchValue == "" ? ""  : searchValue)),
                    new SqlParameter("@SortColumn",sortColumn),
                    new SqlParameter("@SortOrder",sortColumnDir),
                    new SqlParameter("@Start",start),
                    new SqlParameter("@Length",length),
                    new SqlParameter("@UserId",DBNull.Value),
                    new SqlParameter("@RecertMonth",""),
                    TotalCount
                   }).ToList();

                obj = obj.Select(s => new CenterReportModelNon
                {
                    MemberID = s.MemberID,
                    Plan = s.Plan,
                    County = s.County,
                    MemberCIN = s.MemberCIN,
                    Member = s.Member,
                    CurrentStatus = Convert.ToInt32(s.CurrentStatus) > 0 ? FollowupStatusData.Where(k => k.Id == Convert.ToInt32(s.CurrentStatus)).FirstOrDefault().Name.Replace("_", " ") : "",
                    NextStepTask = s.NextStepTask != "" ? Convert.ToInt32(s.NextStepTask) > 0 ? FollowupStatusData.Where(k => k.Id == Convert.ToInt32(s.NextStepTask)).FirstOrDefault().Name.Replace("_", " ") : "":"",
                    MedicalExpDate = s.MedicalExpDate,
                    RecertDueDate = s.RecertDueDate,
                    DateLettSent = s.DateLettSent,
                    DateofFirstcallAttm = s.DateofFirstcallAttm,
                    DateofFirstSuces = s.DateofFirstSuces,
                    DateofFinalAttm = s.DateofFinalAttm,
                    CountofOut = s.CountofOut,
                    DatesubReceCon = s.DatesubReceCon,
                    SubmittedbyFhs = s.SubmittedbyFhs,
                    ReasonFhs = s.ReasonFhs,
                    SubmittedbyHra = s.SubmittedbyHra,
                    DidMemberLos = s.DidMemberLos,
                    DateofFiout = s.DateofFiout,
                    ReasonMediClose = s.ReasonMediClose,
                    Comment = (s.Comment != null ? s.Comment.Split('~').Length > 0 ? s.RecertDate.Split('~')[0].ToString() + "-" + s.Comment.Split('~')[0].ToString() : "" : ""),
                    Comment2 = (s.Comment != null ? s.Comment.Split('~').Length > 1 ? s.RecertDate.Split('~')[1].ToString() + "-" + s.Comment.Split('~')[1].ToString() : "" : ""),
                    Comment3 = (s.Comment != null ? s.Comment.Split('~').Length > 2 ? s.RecertDate.Split('~')[2].ToString() + "-" + s.Comment.Split('~')[2].ToString() : "" : ""),
                    Comment4 = (s.Comment != null ? s.Comment.Split('~').Length > 3 ? s.RecertDate.Split('~')[3].ToString() + "-" + s.Comment.Split('~')[3].ToString() : "" : ""),
                    Comment5 = (s.Comment != null ? s.Comment.Split('~').Length > 4 ? s.RecertDate.Split('~')[4].ToString() + "-" + s.Comment.Split('~')[4].ToString() : "" : ""),
                    Comment6 = (s.Comment != null ? s.Comment.Split('~').Length > 5 ? s.RecertDate.Split('~')[5].ToString() + "-" + s.Comment.Split('~')[5].ToString() : "" : ""),
                    Comment7 = (s.Comment != null ? s.Comment.Split('~').Length > 6 ? s.RecertDate.Split('~')[6].ToString() + "-" + s.Comment.Split('~')[6].ToString() : "" : ""),
                    Comment8 = (s.Comment != null ? s.Comment.Split('~').Length > 7 ? s.RecertDate.Split('~')[7].ToString() + "-" + s.Comment.Split('~')[7].ToString() : "" : ""),
                    Comment9 = (s.Comment != null ? s.Comment.Split('~').Length > 8 ? s.RecertDate.Split('~')[8].ToString() + "-" + s.Comment.Split('~')[8].ToString() : "" : ""),
                    Comment10 = (s.Comment != null ? s.Comment.Split('~').Length > 9 ? s.RecertDate.Split('~')[9].ToString() + "-" + s.Comment.Split('~')[9].ToString() : "" : "")
                }).ToList();

                recordsTotal = Convert.ToInt32(TotalCount.Value);

                //var Data = obj.Where(k => k.DatesubReceCon != null).Select(s => s.DatesubReceCon).ToList();                //if (HttpContext.Session.GetString("UserName") == null)
                //{
                //    return RedirectToAction("Login", "Login");
                //}
                //var sortColumn = HttpContext.Request.Form["columns[" + HttpContext.Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                //var sortColumnDir = HttpContext.Request.Form["order[0][dir]"].FirstOrDefault();
                //var searchValue = HttpContext.Request.Form["search[value]"].FirstOrDefault();

                //var results = _context.MemberMaster.Where(s=>s.MembershipStatus==4).ToList().Select(a => new MemberMasterModel
                //{
                //    MedicaidID=a.MedicaidID,
                //    FirstName = a.LastName +','+ a.FirstName,
                //    LostEligibilityDate = null,
                //    CphlComments = null,
                //});
                //int recordsTot = results.Count();
                //results = results.Where(c => (c.MedicaidID != null && c.MedicaidID.Contains(searchValue)) || c.FirstName.Contains(searchValue)).ToList();
                //int recordsFilter = results.Count();
                //if (sortColumnDir == "asc")
                //{
                //    if (sortColumn == "FirstName")
                //        results = results.OrderBy(x => x.FirstName).ToList();
                //    else if (sortColumn == "MedicaidID")
                //        results = results.OrderBy(x => x.MedicaidID).ToList();
                //    else if (sortColumn == "LostEligibilityDate")
                //        results = results.OrderBy(x => x.LostEligibilityDate).ToList();
                //    else
                //        results = results.OrderBy(x => x.CphlComments).ToList();
                //}
                //else
                //{
                //    if (sortColumn == "FirstName")
                //        results = results.OrderByDescending(x => x.FirstName).ToList();
                //    else if (sortColumn == "MedicaidID")
                //        results = results.OrderByDescending(x => x.MedicaidID).ToList();
                //    else if (sortColumn == "LostEligibilityDate")
                //        results = results.OrderByDescending(x => x.LostEligibilityDate).ToList();
                //    else
                //        results = results.OrderByDescending(x => x.CphlComments).ToList();
                //}
                //var data = results.Skip(start.GetValueOrDefault()).Take(length.GetValueOrDefault()).ToList();

                //var response = new { data = data, recordsFiltered = recordsFilter, recordsTotal = recordsTot };
                //return Json(response);
                //return Json(data);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            //var data = obj.Skip(start.GetValueOrDefault()).Take(length.GetValueOrDefault()).ToList();
            var data = obj;
            var response = new { data = data, recordsFiltered = recordsTotal, recordsTotal = recordsTotal };
            return Json(response);
        }
        [HttpGet]
        public IActionResult ExportExcelData(string Report,string RecertMonth ="")
        {
            if(RecertMonth !=null)
                    RecertMonth = RecertMonth.TrimEnd(',');
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var FollowupStatusData = _context.FollowupStatusMaster.ToList();
            DataSet ds = new DataSet();
            if (Report == "1")
            {

                //var results = _context.MemberMaster.Where(s => s.MembershipStatus == 4).ToList().Select(a => new NonCoveredExportExcel
                //{
                //    MedicaidID = a.MedicaidID,
                //    Name = a.LastName + ',' + a.FirstName,
                //    LostEligibilityDate = null,
                //    CphlComments = null,
                //});
                List<ReportNonCExcelExportModel> obj = new List<ReportNonCExcelExportModel>();
                obj = _context.ReportNonCExcelExportModelSps.FromSqlRaw("SP_CenterReport @Spara",
                                      new Object[]
                                      {
                                           new SqlParameter("@Spara", "5")}).ToList();

                obj = obj.Select(s => new ReportNonCExcelExportModel
                {
                    MemberID = s.MemberID,
                    Plan = s.Plan,
                    County = s.County,
                    MemberCIN = s.MemberCIN,
                    Member = s.Member,
                    CurrentStatus = Convert.ToInt32(s.CurrentStatus) > 0 ? FollowupStatusData.Where(k => k.Id == Convert.ToInt32(s.CurrentStatus)).FirstOrDefault().Name.Replace("_", " ") : "",
                    NextStepTask = Convert.ToInt32(s.NextStepTask) > 0 ? FollowupStatusData.Where(k => k.Id == Convert.ToInt32(s.NextStepTask)).FirstOrDefault().Name.Replace("_", " ") : "",
                    DatesubReceCon = s.DatesubReceCon,
                    Comment = (s.Comment != null ? s.Comment.Split('~').Length > 0 ? s.Comment.Split('~')[0].ToString() : "" : ""),
                    Comment2 = (s.Comment != null ? s.Comment.Split('~').Length > 1 ? s.Comment.Split('~')[1].ToString() : "" : ""),
                    Comment3 = (s.Comment != null ? s.Comment.Split('~').Length > 2 ? s.Comment.Split('~')[2].ToString() : "" : ""),
                    Comment4 = (s.Comment != null ? s.Comment.Split('~').Length > 3 ? s.Comment.Split('~')[3].ToString() : "" : ""),
                    Comment5 = (s.Comment != null ? s.Comment.Split('~').Length > 4 ? s.Comment.Split('~')[4].ToString() : "" : ""),
                    Comment6 = (s.Comment != null ? s.Comment.Split('~').Length > 5 ? s.Comment.Split('~')[5].ToString() : "" : ""),
                    Comment7 = (s.Comment != null ? s.Comment.Split('~').Length > 6 ? s.Comment.Split('~')[6].ToString() : "" : ""),
                    Comment8 = (s.Comment != null ? s.Comment.Split('~').Length > 7 ? s.Comment.Split('~')[7].ToString() : "" : ""),
                    Comment9 = (s.Comment != null ? s.Comment.Split('~').Length > 8 ? s.Comment.Split('~')[8].ToString() : "" : ""),
                    Comment10 = (s.Comment != null ? s.Comment.Split('~').Length > 9 ? s.Comment.Split('~')[9].ToString() : "" : "")
                }).ToList();

                DataTable table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(obj), (typeof(DataTable)));

                var Followuplist = JsonConvert.SerializeObject(table);
                DataTable dtFollowUp = (DataTable)JsonConvert.DeserializeObject(Followuplist, (typeof(DataTable)));
                dtFollowUp.TableName = "Non Covered Report";




                ds.Tables.Add(dtFollowUp);


            }
            else if (Report == "2")
            {
                List<ReportExcelExportModel> obj = null;
                //obj = _context.ReportExcelExportModelSps.FromSqlRaw("SP_CenterReport @Spara",
                //                      new Object[]
                //                      {
                //                           new SqlParameter("@Spara", "3")}).ToList();

                var output = new SqlParameter();
                output.ParameterName = "@TotalCount";
                output.SqlDbType = SqlDbType.Int;
                output.Direction = ParameterDirection.Output;


                obj = _context.ReportExcelExportModelSps.FromSqlRaw("SP_CenterReport @Spara,@SearchbyName,@SortColumn,@SortOrder,@Start,@Length,@UserId,@RecertMonth,@TotalCount OUT",
                                     new Object[]
                                     {
                    new SqlParameter("@Spara", "3"),
                    new SqlParameter("@SearchbyName",DBNull.Value),
                    new SqlParameter("@SortColumn",DBNull.Value),
                    new SqlParameter("@SortOrder",DBNull.Value),
                    new SqlParameter("@Start",DBNull.Value),
                    new SqlParameter("@Length",DBNull.Value),
                    new SqlParameter("@UserId",DBNull.Value),
                    new SqlParameter("@RecertMonth",RecertMonth == null ? "" : RecertMonth),
                    output
                  }).ToList();


                var list = obj.Where(o => o.CurrentStatus == null || o.CurrentStatus == "").Select(s => s.CurrentStatus).ToList();
                var list1 = obj.Where(o => o.NextStepTask == null || o.NextStepTask == "").Select(s => s.NextStepTask).ToList();
                obj = obj.Select(s => new ReportExcelExportModel
                {
                    MemberID = s.MemberID,
                    Plan = s.Plan,
                    County = s.County,
                    MemberCIN = s.MemberCIN,
                    Member = s.Member,
                    CurrentStatus = Convert.ToInt32(s.CurrentStatus) > 0 ? FollowupStatusData.Where(k => k.Id == Convert.ToInt32(s.CurrentStatus)).FirstOrDefault().Name.Replace("_", " ") : "",
                    NextStepTask = s.NextStepTask != "" ? Convert.ToInt32(s.NextStepTask) > 0 ? FollowupStatusData.Where(k => k.Id == Convert.ToInt32(s.NextStepTask)).FirstOrDefault().Name.Replace("_", " ") : "":"",
                    MedicalExpDate = s.MedicalExpDate,
                    RecertDueDate = s.RecertDueDate,
                    DateLettSent = s.DateLettSent,
                    DateofFirstcallAttm = s.DateofFirstcallAttm,
                    Comment = (s.Comment != null ? s.Comment.Split('~').Length > 0 ? s.Comment.Split('~')[0].ToString() : "" : ""),
                    Comment2 = (s.Comment != null ? s.Comment.Split('~').Length > 1 ? s.Comment.Split('~')[1].ToString() : "" : ""),
                    Comment3 = (s.Comment != null ? s.Comment.Split('~').Length > 2 ? s.Comment.Split('~')[2].ToString() : "" : ""),
                    Comment4 = (s.Comment != null ? s.Comment.Split('~').Length > 3 ? s.Comment.Split('~')[3].ToString() : "" : ""),
                    Comment5 = (s.Comment != null ? s.Comment.Split('~').Length > 4 ? s.Comment.Split('~')[4].ToString() : "" : ""),
                    Comment6 = (s.Comment != null ? s.Comment.Split('~').Length > 5 ? s.Comment.Split('~')[5].ToString() : "" : ""),
                    Comment7 = (s.Comment != null ? s.Comment.Split('~').Length > 6 ? s.Comment.Split('~')[6].ToString() : "" : ""),
                    Comment8 = (s.Comment != null ? s.Comment.Split('~').Length > 7 ? s.Comment.Split('~')[7].ToString() : "" : ""),
                    Comment9 = (s.Comment != null ? s.Comment.Split('~').Length > 8 ? s.Comment.Split('~')[8].ToString() : "" : ""),
                    Comment10 = (s.Comment != null ? s.Comment.Split('~').Length > 9 ? s.Comment.Split('~')[9].ToString() : "" : "")
                }).ToList();

                DataTable table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(obj), (typeof(DataTable)));

                var Followuplist = JsonConvert.SerializeObject(table);
                DataTable dtFollowUp = (DataTable)JsonConvert.DeserializeObject(Followuplist, (typeof(DataTable)));
                dtFollowUp.TableName = "Center Report";

                ds.Tables.Add(dtFollowUp);
            }
            else if (Report == "3")
            {

                //var results = _context.MemberMaster.Where(s => s.MembershipStatus == 4).ToList().Select(a => new NonCoveredExportExcel
                //{
                //    MedicaidID = a.MedicaidID,
                //    Name = a.LastName + ',' + a.FirstName,
                //    LostEligibilityDate = null,
                //    CphlComments = null,
                //});
                var output = new SqlParameter();
                output.ParameterName = "@TotalCount";
                output.SqlDbType = SqlDbType.Int;
                output.Direction = ParameterDirection.Output;
                List<ReportNonCExcelExportModel> obj = new List<ReportNonCExcelExportModel>();
                obj = _context.ReportNonCExcelExportModelSps.FromSqlRaw("SP_CenterReport @Spara,@SearchbyName,@SortColumn,@SortOrder,@Start,@Length,@UserId,@RecertMonth,@TotalCount OUT",
                                     new Object[]
                                      {
                                           new SqlParameter("@Spara", "6"),
                    new SqlParameter("@SearchbyName",DBNull.Value),
                    new SqlParameter("@SortColumn",DBNull.Value),
                    new SqlParameter("@SortOrder",DBNull.Value),
                    new SqlParameter("@Start",DBNull.Value),
                    new SqlParameter("@Length",DBNull.Value),
                    new SqlParameter("@UserId",Convert.ToInt32(HttpContext.Session.GetString("UserID"))),
                    new SqlParameter("@RecertMonth",""),
                    output,
                   }).ToList();

                obj = obj.Select(s => new ReportNonCExcelExportModel
                {
                    MemberID = s.MemberID,
                    Plan = s.Plan,
                    County = s.County,
                    MemberCIN = s.MemberCIN,
                    Member = s.Member,
                    CurrentStatus = Convert.ToInt32(s.CurrentStatus) > 0 ? FollowupStatusData.Where(k => k.Id == Convert.ToInt32(s.CurrentStatus)).FirstOrDefault().Name.Replace("_", " ") : "",
                    NextStepTask = Convert.ToInt32(s.NextStepTask) > 0 ? FollowupStatusData.Where(k => k.Id == Convert.ToInt32(s.NextStepTask)).FirstOrDefault().Name.Replace("_", " ") : "",
                    DatesubReceCon = s.DatesubReceCon,
                    Comment = (s.Comment != null ? s.Comment.Split('~').Length > 0 ? s.Comment.Split('~')[0].ToString() : "" : ""),
                    Comment2 = (s.Comment != null ? s.Comment.Split('~').Length > 1 ? s.Comment.Split('~')[1].ToString() : "" : ""),
                    Comment3 = (s.Comment != null ? s.Comment.Split('~').Length > 2 ? s.Comment.Split('~')[2].ToString() : "" : ""),
                    Comment4 = (s.Comment != null ? s.Comment.Split('~').Length > 3 ? s.Comment.Split('~')[3].ToString() : "" : ""),
                    Comment5 = (s.Comment != null ? s.Comment.Split('~').Length > 4 ? s.Comment.Split('~')[4].ToString() : "" : ""),
                    Comment6 = (s.Comment != null ? s.Comment.Split('~').Length > 5 ? s.Comment.Split('~')[5].ToString() : "" : ""),
                    Comment7 = (s.Comment != null ? s.Comment.Split('~').Length > 6 ? s.Comment.Split('~')[6].ToString() : "" : ""),
                    Comment8 = (s.Comment != null ? s.Comment.Split('~').Length > 7 ? s.Comment.Split('~')[7].ToString() : "" : ""),
                    Comment9 = (s.Comment != null ? s.Comment.Split('~').Length > 8 ? s.Comment.Split('~')[8].ToString() : "" : ""),
                    Comment10 = (s.Comment != null ? s.Comment.Split('~').Length > 9 ? s.Comment.Split('~')[9].ToString() : "" : "")
                }).ToList();

                DataTable table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(obj), (typeof(DataTable)));

                var Followuplist = JsonConvert.SerializeObject(table);
                DataTable dtFollowUp = (DataTable)JsonConvert.DeserializeObject(Followuplist, (typeof(DataTable)));
                dtFollowUp.TableName = "Non Covered Report";




                ds.Tables.Add(dtFollowUp);


            }
            FileContentResult robj;
            byte[] chends = null;
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    var bytesdata = File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MemberWorkflow.xlsx");
                    robj = bytesdata;
                    chends = stream.ToArray();
                }
            }

            //return Json(Convert.ToBase64String(chends));

            return Json(robj);
        }

        public IActionResult CenterReport()
        {
            HttpContext.Session.SetString("followupS", "");
            HttpContext.Session.SetString("followupE", "");
            HttpContext.Session.SetString("Phone", "");
            HttpContext.Session.SetString("ResidenceID", "");
            HttpContext.Session.SetString("MedicaidID", "");
            HttpContext.Session.SetString("RecertMonth", "");
            HttpContext.Session.SetString("Facility", "");
            HttpContext.Session.SetString("Language", "");
            HttpContext.Session.SetString("MembershipStatus", "");
            HttpContext.Session.SetString("RecertMonth", "");
            return View();
        }

        [HttpPost]
        public IActionResult CenterReport(int? length, int? start,string RecertMonth ="")
        {
            if (RecertMonth==null)
            {
                RecertMonth = "";
            }
            RecertMonth = RecertMonth.TrimEnd(',');
            var FollowupStatusData = _context.FollowupStatusMaster.ToList();
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            var sortColumn = HttpContext.Request.Form["columns[" + HttpContext.Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            //var sortColumn = HttpContext.Request.Form["columns[" + HttpContext.Request.Form["orderby[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDir = HttpContext.Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = HttpContext.Request.Form["search[value]"].FirstOrDefault();
            int recordsTotal = 0;
            List<CenterReportModel> obj = new List<CenterReportModel>();
            var output = new SqlParameter();
            output.ParameterName = "@TotalCount";
            output.SqlDbType = SqlDbType.Int;
            output.Direction = ParameterDirection.Output;
            try
            {
                obj = _context.CenterReportModelSPs.FromSqlRaw("SP_CenterReport @Spara,@SearchbyName,@SortColumn,@SortOrder,@Start,@Length,@UserId,@RecertMonth,@TotalCount OUT",
                                      new Object[]
                                      {
                                           new SqlParameter("@Spara", "1"),
                    new SqlParameter("@SearchbyName",(searchValue == "" ? ""  : searchValue)),
                    new SqlParameter("@SortColumn",sortColumn),
                    new SqlParameter("@SortOrder",sortColumnDir),
                    new SqlParameter("@Start",start),
                    new SqlParameter("@Length",length),
                    new SqlParameter("@UserId",DBNull.Value),
                    new SqlParameter("@RecertMonth",RecertMonth),
                    output
                   }).ToList();

                obj = obj.Select(s => new CenterReportModel
                {
                    MemberID = s.MemberID,
                    Plan = s.Plan,
                    County = s.County,
                    MemberCIN = s.MemberCIN,
                    Member = s.Member,
                    CurrentStatus = Convert.ToInt32(s.CurrentStatus) > 0 ? FollowupStatusData.Where(k => k.Id == Convert.ToInt32(s.CurrentStatus)).FirstOrDefault().Name.Replace("_", " ") : "",
                    NextStepTask = Convert.ToInt32(s.NextStepTask) > 0 ? FollowupStatusData.Where(k => k.Id == Convert.ToInt32(s.NextStepTask)).FirstOrDefault().Name.Replace("_", " ") : "",
                    MedicalExpDate = s.MedicalExpDate,
                    RecertDueDate = s.RecertDueDate,
                    DateLettSent = s.DateLettSent,
                    DateofFirstcallAttm = s.DateofFirstcallAttm,
                    DateofFirstSuces = s.DateofFirstSuces,
                    DateofFinalAttm = s.DateofFinalAttm,
                    CountofOut = s.CountofOut,
                    DatesubReceCon = s.DatesubReceCon,
                    SubmittedbyFhs = s.SubmittedbyFhs,
                    ReasonFhs = s.ReasonFhs,
                    SubmittedbyHra = s.SubmittedbyHra,
                    DidMemberLos = s.DidMemberLos,
                    DateofFiout = s.DateofFiout,
                    ReasonMediClose = s.ReasonMediClose,
                    Comment = (s.Comment != null ? s.Comment.Split('~').Length > 0 ? s.Comment.Split('~')[0].ToString() : "" : ""),
                    Comment2 = (s.Comment != null ? s.Comment.Split('~').Length > 1 ? s.Comment.Split('~')[1].ToString() : "" : ""),
                    Comment3 = (s.Comment != null ? s.Comment.Split('~').Length > 2 ? s.Comment.Split('~')[2].ToString() : "" : ""),
                    Comment4 = (s.Comment != null ? s.Comment.Split('~').Length > 3 ? s.Comment.Split('~')[3].ToString() : "" : ""),
                    Comment5 = (s.Comment != null ? s.Comment.Split('~').Length > 4 ? s.Comment.Split('~')[4].ToString() : "" : ""),
                    Comment6 = (s.Comment != null ? s.Comment.Split('~').Length > 5 ? s.Comment.Split('~')[5].ToString() : "" : ""),
                    Comment7 = (s.Comment != null ? s.Comment.Split('~').Length > 6 ? s.Comment.Split('~')[6].ToString() : "" : ""),
                    Comment8 = (s.Comment != null ? s.Comment.Split('~').Length > 7 ? s.Comment.Split('~')[7].ToString() : "" : ""),
                    Comment9 = (s.Comment != null ? s.Comment.Split('~').Length > 8 ? s.Comment.Split('~')[8].ToString() : "" : ""),
                    Comment10 = (s.Comment != null ? s.Comment.Split('~').Length > 9 ? s.Comment.Split('~')[9].ToString() : "" : "")
                }).ToList();

                //var total = _context.CenterReportModelSPs.FromSqlRaw("SP_CenterReport @Spara,@SearchbyName,@SortColumn,@SortOrder",
                //                     new Object[]
                //                     {
                //                           new SqlParameter("@Spara", "1"),
                //    new SqlParameter("@SearchbyName",""),
                //    new SqlParameter("@SortColumn",""),
                //    new SqlParameter("@SortOrder","")
                //  }).ToList();
                recordsTotal = Convert.ToInt32(output.Value);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            var data = obj;
            var response = new { data = data, recordsFiltered = recordsTotal, recordsTotal = recordsTotal };
            return Json(response);
        }

        public IActionResult UsageReport()
        {
            HttpContext.Session.SetString("followupS", "");
            HttpContext.Session.SetString("followupE", "");
            HttpContext.Session.SetString("Phone", "");
            HttpContext.Session.SetString("ResidenceID", "");
            HttpContext.Session.SetString("MedicaidID", "");
            HttpContext.Session.SetString("RecertMonth", "");
            HttpContext.Session.SetString("Facility", "");
            HttpContext.Session.SetString("Language", "");
            HttpContext.Session.SetString("MembershipStatus", "");
            HttpContext.Session.SetString("RecertMonth", "");
            return View();
        }
        public IActionResult UserReport()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UserReportData(string StartDate,string EndDate)
        {
            DataTable dt = null;
            var data = _context.Database.GetDbConnection().ConnectionString;
            SqlCommand sql = new SqlCommand();
            sql.Parameters.AddWithValue("@StartDate", StartDate);
            sql.Parameters.AddWithValue("@EndDate", EndDate);
            sql.CommandText = "SP_UserReport";
            dt = new DataTable();
            SqlConnection conn = new SqlConnection(data);
            sql.Connection = conn;
            sql.CommandType = CommandType.StoredProcedure;

            dt = new DataTable();
            SqlDataReader dr;
            conn.Open();
            dr = sql.ExecuteReader();
            dt.Load(dr);
            conn.Close();
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(dt);
            //var list = DataTableToJsonObj(dt);
            return Json(JSONresult);
        }


        public string DataTableToJsonObj(DataTable dt)
        {
            DataSet ds = new DataSet();
            ds.Merge(dt);
            StringBuilder JsonString = new StringBuilder();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                JsonString.Append("[");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    JsonString.Append("{");
                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        if (j < ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString() + "\",");
                        }
                        else if (j == ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == ds.Tables[0].Rows.Count - 1)
                    {
                        JsonString.Append("}");
                    }
                    else
                    {
                        JsonString.Append("},");
                    }
                }
                JsonString.Append("]");
                return JsonString.ToString();
            }
            else
            {
                return null;
            }
        }

    }
}
