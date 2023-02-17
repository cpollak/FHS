using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using YSLProject.Models;
using YSLProject.Table;
using static iTextSharp.text.Font;
//using YSLProject.Reports;
using static YSLProject.Models.Enumdata;
using Microsoft.EntityFrameworkCore;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Net;

namespace YSLProject.Controllers
{
    [Autho]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment Environment;
        private IConfiguration Configuration;

        public DashboardController(IHostingEnvironment _environment, ApplicationDbContext context, IConfiguration _configuration)
        {
            Environment = _environment;
            _context = context;
            Configuration = _configuration;
        }
        public IActionResult clearFilter()
        {
            HttpContext.Session.SetString("followupS", "");
            HttpContext.Session.SetString("followupE", "");
            HttpContext.Session.SetString("Phone", "");
            HttpContext.Session.SetString("ResidenceID", "");
            HttpContext.Session.SetString("MedicaidID", "");
            HttpContext.Session.SetString("RecertMonth", "");
            HttpContext.Session.SetString("Facility", "");
            HttpContext.Session.SetString("Followuptimes", "");
            HttpContext.Session.SetString("Language", "");
            HttpContext.Session.SetString("MembershipStatus", "");
            HttpContext.Session.SetString("RecertMonth", "");
            HttpContext.Session.SetString("NextStepTask", "");
            HttpContext.Session.SetString("Followuptimes", "");
            HttpContext.Session.SetString("FirstIniLastName", "");
            return RedirectToAction("Index");
        }
        public IActionResult Index()
        {
            //BindReport();

            ViewBag.LanguageList = new SelectList(_context.LanguageMaster.ToList(), "LanguageID", "Language", HttpContext.Session.GetString("Language"));


            ViewBag.MembershipStatusList = (from MembershipStatus e in Enum.GetValues(typeof(MembershipStatus))
                                            select new SelectListItem
                                            {
                                                Value = Convert.ToString((int)e),
                                                Text = e.ToString().Replace("_", "-")
                                            }).ToList();

            ViewBag.MembershipStatusList.Insert(0, new SelectListItem { Text = "Select", Value = "" });
            if (HttpContext.Session.GetString("MembershipStatus") != null)
            {
                ViewBag.MembershipStatus = HttpContext.Session.GetString("MembershipStatus");
            }


            ViewBag.FollowupStatusList = (from FollowupFilterStatus e in Enum.GetValues(typeof(FollowupFilterStatus))
                                          select new SelectListItem
                                          {
                                              Value = Convert.ToString((int)e),
                                              Text = e.ToString().Replace("_", " ")
                                          }).ToList();
            ViewBag.FollowupStatusList.Insert(0, new SelectListItem { Text = "Select", Value = "" });
            if (HttpContext.Session.GetString("NextStepTask") != null)
            {
                ViewBag.NextStepTask = HttpContext.Session.GetString("NextStepTask");
            }

            ViewBag.MonthList = (from RecertMonth e in Enum.GetValues(typeof(RecertMonth))
                                 select new SelectListItem
                                 {
                                     Value = Convert.ToString((int)e),
                                     Text = e.ToString()
                                 }).ToList();
            ViewBag.MonthList.Insert(0, new SelectListItem { Text = "Select", Value = "" });
            if (HttpContext.Session.GetString("RecertMonth") != null)
            {
                ViewBag.RecertMonth = HttpContext.Session.GetString("RecertMonth");
            }

            var list = new List<SelectListItem>
            {
                new SelectListItem{ Text="Select", Value = "",Selected=true},
                new SelectListItem{ Text="MLTC", Value = "MLTC" },
                new SelectListItem{ Text="DSNP", Value = "DSNP" },
                new SelectListItem{ Text="MAP", Value = "MAP"},
            };
            ViewBag.FacilityList = list.ToList();
            if (HttpContext.Session.GetString("Facility") != null)
            {
                ViewBag.Facility = HttpContext.Session.GetString("Facility");
            }
            
            var PeriodOfTheDay = new List<SelectListItem>
                    {
                        new SelectListItem{ Text="Select", Value = "" , Selected = true },
                        new SelectListItem{ Text="Morning (8 AM to 11 AM)", Value = "Morning (8 AM to 11 AM)"  },
                        new SelectListItem{ Text="Noon (11 AM to 2 PM)", Value = "Noon (11 AM to 2 PM)" },
                        new SelectListItem{ Text="Afternoon (2 PM to 5 PM)", Value = "Afternoon (2 PM to 5 PM)" },
                    };
            ViewBag.FollowuptimesList = PeriodOfTheDay.ToList();

            var Type = new List<SelectListItem>
                    {
                        new SelectListItem{ Text="All", Value = ""  ,Selected=true},
                        new SelectListItem{ Text="Recert", Value = "Recert" },
                        new SelectListItem{ Text="Surplus", Value = "Surplus" },
                    };
            ViewBag.TypeList = Type.ToList();
            var CaseType = new List<SelectListItem>
                    {
                        new SelectListItem{ Text="All", Value = ""  ,Selected=true},
                        new SelectListItem{ Text="SSI", Value = "SSI" },
                        new SelectListItem{ Text="PA", Value = "PA" },
                    };
            ViewBag.CaseType = CaseType.ToList();


            if (HttpContext.Session.GetString("Followuptimes") != null)
            {
                ViewBag.Followuptimes = HttpContext.Session.GetString("Followuptimes");
            }
            ViewBag.ResidenceID = HttpContext.Session.GetString("ResidenceID");
            ViewBag.MedicaidID = HttpContext.Session.GetString("MedicaidID");
            ViewBag.followupS = HttpContext.Session.GetString("followupS"); 
            ViewBag.followupE = HttpContext.Session.GetString("followupE");
            ViewBag.Phone = HttpContext.Session.GetString("Phone");
            ViewBag.FirstIniLastName = HttpContext.Session.GetString("FirstIniLastName");

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserTypeId = HttpContext.Session.GetString("UserTypeId").ToString();
            return View();
        }
      
        [HttpPost]
        public async Task<IActionResult> MemberFilter(int? length, int? start,string FromDate, string ToDate, string SearchbyName, string MedicaidID, string ResidenceID, string UserName, string Language, string MembershipStatus, string RecertMonth, string Followuptimes, string Facility, string NextStepTask, string Phone, string followupS, string followupE, int UserId,bool chkExcludeDisenrolled,string AssignedFlg,string FirstIniLastName,bool RecertificationProcess,bool Firstcall,string Type,string CaseType)
        {
            
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            //var start = HttpContext.Request.Form["start"].FirstOrDefault();
            //var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortColumn = HttpContext.Request.Form["columns[" + HttpContext.Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDir = HttpContext.Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = HttpContext.Request.Form["search[value]"].FirstOrDefault();


            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Login");
            }
            int userid = 0;
            if (UserName != null)
            {
                userid = _context.UserMaster.Where(a => a.UserName.Trim() == UserName.Trim()).Count() > 0 ? _context.UserMaster.Where(a => a.UserName.Trim() == UserName.Trim()).FirstOrDefault().UserID : 0;
            }
            var FollowupData = _context.Recertification_Follow_Up.Where(s => s.PeriodOfTheDay == Followuptimes).ToList();
            var FollowupDatefilter = new List<Recertification_Follow_Up>();
            if (followupS != null && followupE != null)
            {
                FollowupDatefilter = _context.Recertification_Follow_Up.Where(s => s.CreatedDate >= Convert.ToDateTime(followupS) && s.CreatedDate <= Convert.ToDateTime(followupE)).ToList();
            }
            var languagelist = _context.LanguageMaster.ToList();
            var NestStepTaskList = _context.Recertification_Follow_Up.Where(s => s.NewStatus == NextStepTask).ToList();
           string UserIdtype= HttpContext.Session.GetString("UserTypeId").ToString();
            string UserIds = HttpContext.Session.GetString("UserID").ToString();
            if (AssignedFlg != "1")
            {
                UserIds = "0";
            }
            int couunt = 0;
            //int couunt = _context.MemberCount.FromSqlRaw("SP_UserMaster @Spara,@FromDate,@ToDate,@SearchbyName,@Facility,@MedicaidID,@ResidenceID,@Language,@RecertMonth,@PrimaryPhone,@MembershipStatus,@SortColumn,@SortOrder,@AssignId,@Offset,@Limit,@NextFollwupStatus,@Followuptimes,@chkExcludeDisenrolled,@FirstIniLastName",
            //                           new Object[]
            //                           {
            //                               new SqlParameter("@Spara", "3"),
            //        new SqlParameter("@FromDate",(followupS == null ? ""  : Convert.ToDateTime(followupS).ToString())),
            //        new SqlParameter("@ToDate",(followupE == null ? ""  : Convert.ToDateTime(followupE).ToString())),
            //        new SqlParameter("@SearchbyName",(searchValue == "" ? ""  : searchValue)),
            //        new SqlParameter("@Facility",(Facility ==null ? "" :Facility)),
            //        new SqlParameter("@MedicaidID",(MedicaidID == null ? ""  : MedicaidID)),
            //        new SqlParameter("@ResidenceID",(ResidenceID == null ? "" : ResidenceID)),
            //        new SqlParameter("@Language",(Language == null ? ""  :Language)),
            //        new SqlParameter("@RecertMonth",(RecertMonth== null ? "" :RecertMonth)),
            //        new SqlParameter("@PrimaryPhone",(Phone == null ? "" :Phone)),
            //        new SqlParameter("@MembershipStatus",(MembershipStatus == null ? "" :MembershipStatus)),
            //        new SqlParameter("@SortColumn",sortColumn),
            //        new SqlParameter("@SortOrder",sortColumnDir),
            //        new SqlParameter("@AssignId",(UserIdtype == "1" ? "0"  : UserIds)),
            //        new SqlParameter("@Offset",start),
            //        new SqlParameter("@Limit",length),
            //        new SqlParameter("@NextFollwupStatus",NextStepTask==null?"":NextStepTask),
            //        new SqlParameter("@Followuptimes",Followuptimes==null?"":Followuptimes),
            //        new SqlParameter("@chkExcludeDisenrolled",chkExcludeDisenrolled),
            //        new SqlParameter("@FirstIniLastName",FirstIniLastName ==null ? "" :FirstIniLastName)

            //                           }).ToList().Count();


            List<MemberMasterSP> objss = new List<MemberMasterSP>();
            var FollwupStatusMaster = _context.FollowupStatusMaster.ToList();
            try
            {
                //SqlParameter TotalCount = new SqlParameter("@TotalCount", SqlDbType.Int) { Direction = ParameterDirection.Output };

                _context.Database.SetCommandTimeout(0);
                objss = _context.memberMasterSPs.FromSqlRaw("SP_UserMasterS @Spara,@FromDate,@ToDate,@SearchbyName,@Facility,@MedicaidID,@ResidenceID,@Language,@RecertMonth,@PrimaryPhone,@MembershipStatus,@SortColumn,@SortOrder,@AssignId,@Offset,@Limit,@NextFollwupStatus,@Followuptimes,@chkExcludeDisenrolled,@FirstIniLastName,@RecertificationProcess,@FirstCall,@Type,@CaseType",
                                       new Object[]
                                       {
                                           new SqlParameter("@Spara", "5"),
                    new SqlParameter("@FromDate",(followupS == null ? ""  : followupS)),
                    new SqlParameter("@ToDate",(followupE == null ? ""  : followupE)),
                    new SqlParameter("@SearchbyName",(searchValue == "" ? ""  : searchValue)),
                    new SqlParameter("@Facility",(Facility ==null ? "" :Facility)),
                    new SqlParameter("@MedicaidID",(MedicaidID == null ? ""  : MedicaidID)),
                    new SqlParameter("@ResidenceID",(ResidenceID == null ? "" : ResidenceID)),
                    new SqlParameter("@Language",(Language == null ? ""  :Language)),
                    new SqlParameter("@RecertMonth",(RecertMonth== null ? "" :RecertMonth)),
                    new SqlParameter("@PrimaryPhone",(Phone == null ? "" :Phone)),
                    new SqlParameter("@MembershipStatus",(MembershipStatus == null ? "" :MembershipStatus)),
                    new SqlParameter("@SortColumn",sortColumn),
                    new SqlParameter("@SortOrder",sortColumnDir),
                    new SqlParameter("@AssignId",(UserIdtype == "1" ? "0"  : UserIds)),
                    new SqlParameter("@Offset",start),
                    new SqlParameter("@Limit",length),
                    new SqlParameter("@NextFollwupStatus",NextStepTask==null? "":NextStepTask),
                    new SqlParameter("@Followuptimes",Followuptimes== null ? "":Followuptimes),
                    new SqlParameter("@chkExcludeDisenrolled",chkExcludeDisenrolled),
                    new SqlParameter("@FirstIniLastName",FirstIniLastName == null ? "" :FirstIniLastName),
                    new SqlParameter("@RecertificationProcess",RecertificationProcess),
                    new SqlParameter("@FirstCall",Firstcall),
                    new SqlParameter("@Type",(Type == null?"":Type)),
                    new SqlParameter("@CaseType",(CaseType == null?"":CaseType)),
                                       }).ToList();
                //couunt = Convert.ToInt32(TotalCount.Value);
                HttpContext.Session.SetString("followupS", followupS == null ? "" : followupS);
                HttpContext.Session.SetString("SortColumn", sortColumn == null ? "" : sortColumn);
                HttpContext.Session.SetString("SortOrder", sortColumnDir == null ? "" : sortColumnDir);
                HttpContext.Session.SetString("followupE", followupE == null ? "" : followupE);
                HttpContext.Session.SetString("Phone", Phone == null ? "" : Phone);
                HttpContext.Session.SetString("ResidenceID", ResidenceID == null ? "" : ResidenceID);
                HttpContext.Session.SetString("MedicaidID", MedicaidID == null ? "" : MedicaidID);
                HttpContext.Session.SetString("RecertMonth", RecertMonth == null ? "" : RecertMonth);
                HttpContext.Session.SetString("Facility", Facility == null ? "" : Facility);
                HttpContext.Session.SetString("Language", Language == null ? "" : Language);
                HttpContext.Session.SetString("MembershipStatus", MembershipStatus == null ? "" : MembershipStatus);
                HttpContext.Session.SetString("RecertMonth", RecertMonth == null ? "" : RecertMonth);
                HttpContext.Session.SetString("NextStepTask", NextStepTask == null ? "" : NextStepTask);
                HttpContext.Session.SetString("Followuptimes", Followuptimes == null ? "" : Followuptimes);
                HttpContext.Session.SetString("FirstIniLastName", FirstIniLastName == null ? "" : FirstIniLastName);

            }
            catch (Exception ex)
            {
            }
            //List<UserMaster> userlist = await _context.UserMaster.ToListAsync();

            couunt = objss.Select(s => s.RecordCount).FirstOrDefault();

            var data = objss.OrderByDescending(s => s.CreatedDate).
            Select(s => new
            {
                s.MemberID,
                s.FirstName,
                s.LastName,
                s.CreatedDate,
                s.Facility,
                s.MedicaidID,
                s.ResidentID,
                s.MembershipStatus,
                s.Language,
                s.RecertMonth,
                s.CreatedBy,
                s.MembershipString,
                s.Languagestring,
                s.Outcome,
                s.NextStepTask,
                s.Type
            }).ToList()
            .Select(a => new MemberMasterModel
            {
                MemberID = a.MemberID,
                FirstName = a.FirstName,
                LastName = a.LastName,
                CreatedDate = a.CreatedDate.Value.ToString("MM/dd/yyyy"),
                Facility = a.Facility,
                MedicaidID = a.MedicaidID,
                ResidentID = a.ResidentID,
                Outcome=a.Outcome == null? "":a.Outcome,
                NextStepTask= a.NextStepTask != null ? (a.NextStepTask != "0" ? FollwupStatusMaster.Where(k => k.Id == Convert.ToInt32(a.NextStepTask)).FirstOrDefault().Name.Replace("_", " ") : "N/A") : "N/A",
                Language = a.Languagestring, // a.Language == "" ? "" : a.Language == "0" ?"": languagelist.Where(s => s.LanguageID == Convert.ToInt32(a.Language)).FirstOrDefault().Language.ToString(),
                RecertMonth = a.RecertMonth,
                
                MembershipStatu =a.MembershipString,
                Type = a.Type,//(a.MembershipStatus !=null ?  Enum.GetName(typeof(MembershipStatus), Convert.ToInt32(a.MembershipStatus)) : ""),
                //CreatedUser = (a.CreatedBy !=null ? (userlist.Count() > 0 ? userlist.Where(s => s.UserID == a.CreatedBy).FirstOrDefault().UserName : "") : ""),
                //FollowupCount =( a.MemberID !=null ? (FollowupData.Count() > 0 ? FollowupData.Where(n => n.MemberId == a.MemberID).Count() > 0 ? Followuptimes : null : null) : ""),
                //NextStepTask =(a.MemberID !=null ? (NestStepTaskList.Count() > 0 ? NestStepTaskList.Where(n => n.MemberId == a.MemberID).Count() > 0 ? NestStepTaskList.Where(n => n.MemberId == a.MemberID).LastOrDefault().NewStatus : null : null) : ""),
                Action = "<a class='btn btn-warning' href=/Member/Workfollow?memberId=" +a.MemberID + ">View</a> <a class='btn btn-warning' data-toggle='modal' data-target='#default' style='background-color:lightblue!important; border-color:#45b7dd !important;' onclick='LoadModule("+ a.MemberID + ")'>Note</a>"
            });
            //var FltData = FollowupDatefilter.Select(s => s.MemberId).ToList();
            //if (Followuptimes !=null)
            //data = data.Where(s => s.FollowupCount == Followuptimes).ToList();
            //if (NextStepTask != null)
            //    data = data.Where(s => s.NextStepTask == NextStepTask).ToList();
            //if (FltData.Count > 0)
            //{
            //    data = data.Where(s => FltData.Contains(s.MemberID.Value)).ToList();
            //}




            var recordsTotal= couunt;

            //if(data.Count() > 100) { data= data.Take(100); }
            //if (sortColumn == "Language")
            //{
            //    if(sortColumnDir == "asc")
            //        data = data.OrderBy(a=>a.Language);
            //    else
            //        data = data.OrderByDescending(a => a.Language);
            //}
            //Search    
            //if (!string.IsNullOrEmpty(searchValue))
            //{
            //    data = data.Where(m => m.FirstName == searchValue);
            //}
            //data = data.Skip(start.GetValueOrDefault()).Take(length.GetValueOrDefault());
            var response = new { data = data, recordsFiltered = recordsTotal, recordsTotal = recordsTotal };
            return Json(response);
            //return Json(new { data = obj });

            //return Json(data);
        }
        public IActionResult getFollowupList(string id)
        {
            int MemberId = Convert.ToInt32(id);
            List<Recertification_Follow_UpModel> FollowupModel = new List<Recertification_Follow_UpModel>();

            var FollwupStatusMaster = _context.FollowupStatusMaster.ToList();

            FollowupModel = (from l in _context.Recertification_Follow_Up
                             join m in _context.MemberMaster
                             on l.MemberId equals m.MemberID
                             join u in _context.UserMaster
                             on l.CreatedBy equals u.UserID
                             where m.MemberID == MemberId 
                             select new
                             {
                                 l.MemberId,
                                 l.FollowUpID,
                                 l.CurrentStatus,
                                 l.Outcome,
                                 l.Notes,
                                 l.NextStepTask,
                                 l.NewStatus,
                                 l.CreatedDate,
                                 u.UserName
                             }).ToList()
                        .Select(s => new Recertification_Follow_UpModel
                        {
                            FollowUpID = s.FollowUpID,
                            MemberId = s.MemberId,
                            CurrentStatus = FollwupStatusMaster.Where(k => k.Id == Convert.ToInt32(s.CurrentStatus)).FirstOrDefault().Name.Replace("_", " "),
                            //CurrentStatus = Convert.ToInt32(s.CurrentStatus) > 0 ? Enum.GetName(typeof(FollowupStatus), Convert.ToInt32(s.CurrentStatus)).Replace("_", " ") : "",
                            Outcome = s.Outcome,
                            Notes = s.Notes,
                            NextStepTask = s.NextStepTask != "0" ? FollwupStatusMaster.Where(k => k.Id == Convert.ToInt32(s.NextStepTask)).FirstOrDefault().Name.Replace("_", " ") : "N/A",
                            //NextStepTask = Convert.ToInt32(s.NextStepTask) > 0 ? Enum.GetName(typeof(FollowupStatus), Convert.ToInt32(s.NextStepTask)).Replace("_", " ") : "",
                            NewStatus = s.NewStatus != "0" ? FollwupStatusMaster.Where(k => k.Id == Convert.ToInt32(s.NewStatus)).FirstOrDefault().Name.Replace("_", " ") : "N/A",
                            //NewStatus = Convert.ToInt32(s.NewStatus) > 0 ? Enum.GetName(typeof(FollowupStatus), Convert.ToInt32(s.NewStatus)).Replace("_", " ") : "",
                            CreatedDt = s.CreatedDate.ToString("MM/dd/yyyy"),
                            CreatedByName = s.UserName
                        }).ToList();
            return Json(FollowupModel);
        }
        public IActionResult getNoteList(string id)
        {
            int MemberId = Convert.ToInt32(id);
            List<GeneralNotesModel> GeneralNotesModel = new List<GeneralNotesModel>();
            GeneralNotesModel = (from l in _context.GeneralNotes
                                     join m in _context.UserMaster
                                     on l.CreatedBy equals m.UserID
                                     where l.MemberId == MemberId
                                     select new
                                     {
                                         l.ID,
                                         l.MemberId,
                                         l.Type,
                                         l.Notes,
                                         l.CreatedDate,
                                         m.UserName
                                     }).OrderByDescending(a => a.ID).ToList()
                       .Select(a => new GeneralNotesModel
                       {
                           MemberId = a.MemberId,
                           Type = a.Type,
                           Notes = a.Notes,
                           CreatedDat = a.CreatedDate.ToString("MM/dd/yyyy hh:mm tt"),
                           CreatedBys = a.UserName,
                       }).ToList();
            return Json(GeneralNotesModel);
        }

        public IActionResult getOldNoteList(string id)
        {
            int MemberId = Convert.ToInt32(id);
            List<GeneralNotesModel> GeneralNotesModel = new List<GeneralNotesModel>();
            GeneralNotesModel = (from l in _context.GeneralNotes_Old
                                 where l.MemberId == MemberId
                                 select new
                                 {
                                     l.ID,
                                     l.MemberId,
                                     l.Type,
                                     l.Notes,
                                     l.CreatedDate,
                                 }).OrderByDescending(a => a.ID).ToList()
                       .Select(a => new GeneralNotesModel
                       {
                           MemberId = a.MemberId,
                           Type = a.Type,
                           Notes = a.Notes,
                           CreatedDat = a.CreatedDate!=null? a.CreatedDate.Value.ToString("MM/dd/yyyy hh:mm tt"):"",
                       }).ToList();
            return Json(GeneralNotesModel);
        }
        public class checkmodl
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string MedicaidID { get; set; }
            public string ResidentID { get; set; }
            public string MembershipStatu { get; set; }
            public string Language { get; set; }
            public string RecertMonth { get; set; }
            public string Facility { get; set; }
            public string Action { get; set; }
        }

        [HttpPost]
        public IActionResult MemberFilterExcel(string FromDate, string ToDate, string SearchbyName, string MedicaidID, string ResidenceID, string UserName, string Language, string MembershipStatus, string RecertMonth, string Followuptimes, string Facility, string NextStepTask, string Phone, string followupS, string followupE, int UserId, bool chkExcludeDisenrolled, string AssignedFlg, string FirstIniLastName, bool RecertificationProcess,bool Firstcall,string Type,string CaseType)
        {
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            //var start = HttpContext.Request.Form["start"].FirstOrDefault();
            //var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortColumn = HttpContext.Request.Form["columns[" + HttpContext.Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDir = HttpContext.Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = HttpContext.Request.Form["search[value]"].FirstOrDefault();
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Login");
            }
            int userid = 0;
            if (UserName != null)
            {
                userid = _context.UserMaster.Where(a => a.UserName.Trim() == UserName.Trim()).Count() > 0 ? _context.UserMaster.Where(a => a.UserName.Trim() == UserName.Trim()).FirstOrDefault().UserID : 0;
            }
            var FollowupData = _context.Recertification_Follow_Up.Where(s => s.PeriodOfTheDay == Followuptimes).ToList();
            //var FollowupDatefilter = new List<Recertification_Follow_Up>();
            //if (followupS != null && followupE != null)
            //{
            //    FollowupDatefilter = _context.Recertification_Follow_Up.Where(s => s.CreatedDate >= Convert.ToDateTime(followupS) && s.CreatedDate <= Convert.ToDateTime(followupE)).ToList();

            //}
            var languagelist = _context.LanguageMaster.ToList();
            var NestStepTaskList = _context.Recertification_Follow_Up.Where(s => s.NewStatus == NextStepTask).ToList();
            UserId = Convert.ToInt32(HttpContext.Session.GetString("UserTypeId"));
            var userlist = _context.UserMaster.ToList();
           
            string UserIdtype = HttpContext.Session.GetString("UserTypeId").ToString();
            string UserIds = HttpContext.Session.GetString("UserID").ToString();

            if (searchValue == null)
            {
                searchValue = "";
            }
            List<MemberMasterSP> objss = new List<MemberMasterSP>();
            try
            {
                //SqlParameter TotalCount = new SqlParameter("@TotalCount", SqlDbType.Int) { Direction = ParameterDirection.Output };
                //objss = _context.memberMasterSPs.FromSqlRaw("SP_UserMaster @Spara,@FromDate,@ToDate,@SearchbyName,@Facility,@MedicaidID,@ResidenceID,@Language,@RecertMonth,@PrimaryPhone,@MembershipStatus,@SortColumn,@SortOrder,@AssignId,@Offset,@Limit",
                //                       new Object[]
                //                       {
                //    new SqlParameter("@Spara", "4"),
                //    new SqlParameter("@FromDate",(followupS == null ? ""  : Convert.ToDateTime(followupS).ToString())),
                //    new SqlParameter("@ToDate",(followupE == null ? ""  : Convert.ToDateTime(followupE).ToString())),
                //    new SqlParameter("@SearchbyName",(searchValue == "" ? ""  : searchValue)),
                //    new SqlParameter("@Facility",(Facility ==null ? "" :Facility)),
                //    new SqlParameter("@MedicaidID",(MedicaidID == null ? ""  : MedicaidID)),
                //    new SqlParameter("@ResidenceID",(ResidenceID == null ? "" : ResidenceID)),
                //    new SqlParameter("@Language",(Language == null ? ""  :Language)),
                //    new SqlParameter("@RecertMonth",(RecertMonth== null ? "" :RecertMonth)),
                //    new SqlParameter("@PrimaryPhone",(Phone == null ? "" :Phone)),
                //    new SqlParameter("@MembershipStatus",(MembershipStatus == null ? "" :MembershipStatus)),
                //    new SqlParameter("@SortColumn",""),
                //    new SqlParameter("@SortOrder",""),
                //    new SqlParameter("@AssignId",(UserIdtype == "1" ? "0"  : "0")),
                //    new SqlParameter("@Offset",1),
                //    new SqlParameter("@Limit",100)}).ToList();
                sortColumn = HttpContext.Session.GetString("SortColumn");
                sortColumnDir= HttpContext.Session.GetString("SortOrder");

                objss = _context.memberMasterSPs.FromSqlRaw("SP_UserMasterS @Spara,@FromDate,@ToDate,@SearchbyName,@Facility,@MedicaidID,@ResidenceID,@Language,@RecertMonth,@PrimaryPhone,@MembershipStatus,@SortColumn,@SortOrder,@AssignId,@Offset,@Limit,@NextFollwupStatus,@Followuptimes,@chkExcludeDisenrolled,@FirstIniLastName,@RecertificationProcess,@Firstcall,@Type,@CaseType",
                                       new Object[]
                                       {
                                           new SqlParameter("@Spara", "5"),
                    new SqlParameter("@FromDate",(followupS == null ? ""  : followupS)),
                    new SqlParameter("@ToDate",(followupE == null ? ""  : followupE)),
                    new SqlParameter("@SearchbyName",(searchValue == "" ? ""  : searchValue)),
                    new SqlParameter("@Facility",(Facility ==null ? "" :Facility)),
                    new SqlParameter("@MedicaidID",(MedicaidID == null ? ""  : MedicaidID)),
                    new SqlParameter("@ResidenceID",(ResidenceID == null ? "" : ResidenceID)),
                    new SqlParameter("@Language",(Language == null ? ""  :Language)),
                    new SqlParameter("@RecertMonth",(RecertMonth== null ? "" :RecertMonth)),
                    new SqlParameter("@PrimaryPhone",(Phone == null ? "" :Phone)),
                    new SqlParameter("@MembershipStatus",(MembershipStatus == null ? "" :MembershipStatus)),
                    new SqlParameter("@SortColumn",sortColumn),
                    new SqlParameter("@SortOrder",sortColumnDir),
                    new SqlParameter("@AssignId","0"),
                    new SqlParameter("@Offset","0"),
                    new SqlParameter("@Limit","100000"),
                    new SqlParameter("@NextFollwupStatus",NextStepTask==null? "":NextStepTask),
                    new SqlParameter("@Followuptimes",Followuptimes== null ? "":Followuptimes),
                    new SqlParameter("@chkExcludeDisenrolled",chkExcludeDisenrolled),
                    new SqlParameter("@FirstIniLastName",FirstIniLastName == null ? "" :FirstIniLastName),
                    new SqlParameter("@RecertificationProcess",RecertificationProcess),
                    new SqlParameter("@Firstcall",Firstcall),
                    new SqlParameter("@Type",(Type == null?"":Type)),
                    new SqlParameter("@CaseType",(CaseType == null?"":CaseType)),
                                       }).ToList();

            }
            catch (Exception ex)
            {
            }

            var data = objss.OrderByDescending(s => s.CreatedDate).
            Select(s => new
            {
                s.MemberID,
                s.FirstName,
                s.LastName,
                s.Type,
                s.CreatedDate,
                s.Facility,
                s.MedicaidID,
                s.ResidentID,
                s.MembershipStatus,
                s.Language,
                s.RecertMonth,
                s.CreatedBy,
                s.InternalNotes,
                s.ExternalNotes,
                s.FollowCreatedDate
            }).ToList()
            .Select(a => new MemberMasterModel
            {
                MemberID = a.MemberID,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Type = a.Type,
                CreatedDate = a.CreatedDate.Value.ToString("dd/MM/yyyy"),
                Facility = a.Facility,
                MedicaidID = a.MedicaidID,
                ResidentID = a.ResidentID,
                InternalNotes=a.InternalNotes,
                ExternalNotes=a.ExternalNotes,
                Language = a.Language == "" ? "" : a.Language == "0" ? "" : languagelist.Where(s => s.LanguageID == Convert.ToInt32(a.Language)).FirstOrDefault().Language.ToString(),
                RecertMonth = a.RecertMonth,
                MembershipStatu = (a.MembershipStatus != null ? Enum.GetName(typeof(MembershipStatus), Convert.ToInt32(a.MembershipStatus)) : ""),
                CreatedUser = (a.CreatedBy != null ? (userlist.Count() > 0 ? userlist.Where(s => s.UserID == a.CreatedBy).FirstOrDefault().UserName : "") : ""),
                FollowupCount = (a.MemberID != null ? (FollowupData.Count() > 0 ? FollowupData.Where(n => n.MemberId == a.MemberID).Count() > 0 ? Followuptimes : null : null) : ""),
                NextStepTask = (a.MemberID != null ? (NestStepTaskList.Count() > 0 ? NestStepTaskList.Where(n => n.MemberId == a.MemberID).Count() > 0 ? NestStepTaskList.Where(n => n.MemberId == a.MemberID).LastOrDefault().NewStatus : null : null) : ""),
                FollowUpDate= a.FollowCreatedDate == null ? null : Convert.ToDateTime(a.FollowCreatedDate).Date == Convert.ToDateTime("01/01/0001").Date ? null :  a.FollowCreatedDate
                //Action = "<a class='btn btn-warning' href=/Member/Workfollow?memberId=" + a.MemberID + ">View</a>"
            });
            //var FltData = FollowupDatefilter.Select(s => s.MemberId).ToList();
            //if (Followuptimes !=null)
            //data = data.Where(s => s.FollowupCount == Followuptimes).ToList();
            //if (NextStepTask != null)
            //    data = data.Where(s => s.NextStepTask == NextStepTask).ToList();
            //if (FltData.Count > 0)
            //{
            //    data = data.Where(s => FltData.Contains(s.MemberID.Value)).ToList();
            //}


            //var recordsTotal = couunt;

            ////if(data.Count() > 100) { data= data.Take(100); }
            //if (sortColumn == "Language")
            //{
            //    if (sortColumnDir == "asc")
            //        data = data.OrderBy(a => a.Language);
            //    else
            //        data = data.OrderByDescending(a => a.Language);
            //}
            //var data = _context.MemberMaster.Where(
            //a => a.CreatedDate >= (FromDate == null ? a.CreatedDate : Convert.ToDateTime(FromDate)) &&
            // a.CreatedDate <= (ToDate == null ? a.CreatedDate : Convert.ToDateTime(ToDate)) &&
            // (a.FirstName.Contains(SearchbyName == null ? a.FirstName : SearchbyName) ||
            // a.LastName.Contains(SearchbyName == null ? a.LastName : SearchbyName)) &&
            // a.Facility == (Facility == null ? a.Facility : Facility) &&
            // a.MedicaidID == (MedicaidID == null ? a.MedicaidID : MedicaidID) &&
            // a.ResidentID == (ResidenceID == null ? a.ResidentID : ResidenceID) &&
            // a.CreatedBy == (UserName == null ? a.CreatedBy : userid) &&
            // a.Language == (Language == null ? a.Language : Language) &&
            // a.RecertMonth == (RecertMonth == null ? a.RecertMonth : RecertMonth) &&
            // a.PrimaryPhone == (Phone == null ? a.PrimaryPhone : Phone) &&
            // a.MembershipStatus == (MembershipStatus == null ? a.MembershipStatus : Convert.ToInt32(MembershipStatus))
            // && a.AssignId == (UserId == 1 ? a.AssignId : UserId)
            //).OrderByDescending(s => s.CreatedDate).
            //Select(s => new
            //{
            //    s.MemberID,
            //    s.FirstName,
            //    s.LastName,
            //    s.CreatedDate,
            //    s.Facility,
            //    s.MedicaidID,
            //    s.ResidentID,
            //    s.MembershipStatus,
            //    s.Language,
            //    s.RecertMonth,
            //    s.CreatedBy,
            //}).ToList()
            //.Select(a => new MemberMasterModel
            // {
            //     MemberID = a.MemberID,
            //     FirstName = a.FirstName,
            //     LastName = a.LastName,
            //     CreatedDate = a.CreatedDate.Value.ToString("MM/dd/yyyy"),
            //     Facility = a.Facility,
            //     MedicaidID = a.MedicaidID,
            //     ResidentID = a.ResidentID,
            //     Language = a.Language == "" ? "" : a.Language == "0" ? "" : a.Language == null ? "" : languagelist.Where(s => s.LanguageID == Convert.ToInt32(a.Language)).FirstOrDefault().Language.ToString(),
            //     RecertMonth = a.RecertMonth,
            //     MembershipStatu = (a.MembershipStatus != null ? Enum.GetName(typeof(MembershipStatus), Convert.ToInt32(a.MembershipStatus)) : ""),
            //     CreatedUser = (a.CreatedBy != null ? userlist.Count() > 0 ? userlist.Where(s => s.UserID == a.CreatedBy).FirstOrDefault().UserName : "" : ""),
            //     FollowupCount = (FollowupData.Count() > 0 ? FollowupData.Where(n => n.MemberId == a.MemberID).Count() > 0 ? Followuptimes : null : null),
            //     NextStepTask = (NestStepTaskList.Count() > 0 ? NestStepTaskList.Where(n => n.MemberId == a.MemberID).Count() > 0 ? NestStepTaskList.Where(n => n.MemberId == a.MemberID).LastOrDefault().NewStatus : null : null)
            // }).ToList();
            //var FltData = FollowupDatefilter.Select(s => s.MemberId).ToList();
            //if (FollowupData.Count() > 0)
            //    data = data.Where(s => s.FollowupCount == Followuptimes).ToList();
            //data = data.Where(s => s.NextStepTask == NextStepTask).ToList();
            //if (FltData.Count > 0)
            //{
            //    data = data.Where(s => FltData.Contains(s.MemberID.Value)).ToList();
            //}

            //if (data.Count() > 100) { data = data.Take(100).ToList(); }

            DataTable table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(data), (typeof(DataTable)));
            //Parallel.ForEach(table.AsEnumerable(), dr =>
            //{
            //    if (dr["FollowUpDate"] != null)
            //    {
            //        dr["FollowUpDate"] = Convert.ToDateTime(dr["FollowUpDate"]).ToString("MM/dd/yyyy");
            //    }

            //});
            table.Columns["FollowupCount"].ColumnName = "Follow-Up times";
            //table.Columns.Remove("Address");
            table.Columns.Remove("NewEnrollmentDa");
            table.Columns.Remove("MembershipStatus");
            table.Columns.Remove("PrimaryPhone");
            table.Columns.Remove("Email");
            table.Columns.Remove("LanguageId");
            table.Columns.Remove("CountyCode");
            table.Columns.Remove("EnrollmentDate");
            table.Columns.Remove("NewEnrollmentDate");
            table.Columns.Remove("EnrollmentDa");
            table.Columns.Remove("DisenrolmentDate");
            table.Columns.Remove("DisenrolmentDa");
            table.Columns.Remove("Phone");
            table.Columns.Remove("Comment");
            table.Columns.Remove("WorkFlowStatus");
            table.Columns.Remove("DischargeDate");
            table.Columns.Remove("CurrentStatus");
            table.Columns.Remove("InternalNotes");
            table.Columns.Remove("ExternalNotes");
            table.Columns.Remove("ChartsID");
            table.Columns.Remove("DateOfBirth");
            table.Columns.Remove("TempRecertMonth");
            table.Columns.Remove("DOB");
            table.Columns.Remove("CreatedUser");
            table.Columns.Remove("CreatedBy");
            table.Columns.Remove("CreatedDate");
            table.Columns.Remove("CDate");
            //table.Columns.Remove("FollowUpDate");
            //table.Columns.Remove("Followupdaterange");
            //table.Columns.Remove("ReportsType");
            //table.Columns.Remove("NextStepTask");
            //table.Columns.Remove("MemberStatus");
            //table.Columns.Remove("CurrentFacility");
            //table.Columns.Remove("LostEligibilityDate");
            //table.Columns.Remove("CphlComments");
            //table.Columns.Remove("Action");
            //table.Columns.Remove("IsNoHeader");
            //table.Columns.Remove("Recertification_Follow_UpModels");
            //table.Columns.Remove("contactModels");
            //table.Columns.Remove("SpousalModels");
            //table.Columns.Remove("LogsModels");
            //table.Columns.Remove("pITModel");
            //table.Columns.Remove("PITUploadsModel");
            //table.Columns.Remove("GeneralNotesModel");
            //table.Columns.Remove("GeneralNotesModelOld");
            //table.Columns.Remove("NewEnrollmentDa");
            //table.Columns.Remove("MembershipStatus");
            //table.Columns.Remove("FollowupCount");
            var Emplist = JsonConvert.SerializeObject(table);
            DataTable dt11 = (DataTable)JsonConvert.DeserializeObject(Emplist, (typeof(DataTable)));
            dt11.TableName = "Member";
            FileContentResult robj;
            byte[] chends = null;
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt11);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    var bytesdata = File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Member.xlsx");
                    robj = bytesdata;
                    chends = stream.ToArray();
                }
            }
            return Json(robj);
        }


        public static class MyServer
        {
            public static string MapPath(string path)
            {
                return Path.Combine(
                    (string)AppDomain.CurrentDomain.GetData("WebRootPath"),
                    path);
            }
        }

    }
}
