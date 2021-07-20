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
using System.Linq.Dynamic;
using static YSLProject.Models.Enumdata;
using Microsoft.EntityFrameworkCore;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

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

        public IActionResult Index()
        {
            //BindReport();
            ViewBag.LanguageList = new SelectList(_context.LanguageMaster.ToList(), "LanguageID", "Language");


            ViewBag.MembershipStatusList = (from MembershipStatus e in Enum.GetValues(typeof(MembershipStatus))
                                            select new SelectListItem
                                            {
                                                Value = Convert.ToString((int)e),
                                                Text = e.ToString().Replace("_", "-")
                                            }).ToList();
            ViewBag.MembershipStatusList.Insert(0, new SelectListItem { Text = "Select", Value = "" });


            ViewBag.FollowupStatusList = (from FollowupFilterStatus e in Enum.GetValues(typeof(FollowupFilterStatus))
                                          select new SelectListItem
                                          {
                                              Value = Convert.ToString((int)e),
                                              Text = e.ToString().Replace("_", " ")
                                          }).ToList();
            ViewBag.FollowupStatusList.Insert(0, new SelectListItem { Text = "Select", Value = "" });


            ViewBag.MonthList = (from RecertMonth e in Enum.GetValues(typeof(RecertMonth))
                                 select new SelectListItem
                                 {
                                     Value = Convert.ToString(e),
                                     Text = e.ToString()
                                 }).ToList();
            ViewBag.MonthList.Insert(0, new SelectListItem { Text = "Select", Value = "" });

            var list = new List<SelectListItem>
            {
                new SelectListItem{ Text="Select", Value = "" , Selected = true },
                new SelectListItem{ Text="MLTC", Value = "MLTC" },
                new SelectListItem{ Text="DSNP", Value = "DSNP" },
                new SelectListItem{ Text="MAP", Value = "MAP"},
            };
            ViewBag.FacilityList = list.ToList();

            var PeriodOfTheDay = new List<SelectListItem>
                    {
                        new SelectListItem{ Text="Select", Value = "" , Selected = true },
                        new SelectListItem{ Text="Morning (8 AM to 11 AM)", Value = "Morning (8 AM to 11 AM)" , Selected = true },
                        new SelectListItem{ Text="Noon (11 AM to 2 PM)", Value = "Noon (11 AM to 2 PM)" },
                        new SelectListItem{ Text="Afternoon (2 PM to 5 PM)", Value = "Afternoon (2 PM to 5 PM)" },
                    };
            ViewBag.FollowuptimesList = PeriodOfTheDay.ToList();

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MemberFilter(string FromDate, string ToDate, string SearchbyName, string MedicaidID, string ResidenceID, string UserName, string Language, string MembershipStatus, string RecertMonth, string Followuptimes, string Facility, string NextStepTask, string Phone, string followupS, string followupE, int UserId)
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
            var FollowupDatefilter = new List<Recertification_Follow_Up>();
            if (followupS != null && followupE != null)
            {
                FollowupDatefilter = _context.Recertification_Follow_Up.Where(s => s.CreatedDate >= Convert.ToDateTime(followupS) && s.CreatedDate <= Convert.ToDateTime(followupE)).ToList();

            }
            var languagelist = _context.LanguageMaster.ToList();
            var NestStepTaskList = _context.Recertification_Follow_Up.Where(s => s.NewStatus == NextStepTask).ToList();
           string UserIdtype= HttpContext.Session.GetString("UserTypeId").ToString();
            string UserIds = HttpContext.Session.GetString("UserID").ToString();
           
            var userlist = _context.UserMaster.ToList();
            //List<SqlParameter> pc = new List<SqlParameter>
            //{

                

            //new SqlParameter("@Spara", "1"),
            //        new SqlParameter("@FromDate",(FromDate == null ? ""  : Convert.ToDateTime(FromDate).ToString())),
            //        new SqlParameter("@ToDate",(ToDate == null ? ""  : Convert.ToDateTime(ToDate).ToString())),
            //        new SqlParameter("@SearchbyName",(SearchbyName == null ? ""  : SearchbyName)),
            //        new SqlParameter("@Facility",(Facility ==null ? "" :Facility)),
            //        new SqlParameter("@MedicaidID",(MedicaidID == null ? ""  : MedicaidID)),
            //        new SqlParameter("@ResidenceID",(ResidenceID == null ? "" : ResidenceID)),
            //        new SqlParameter("@Language",(Language == null ? ""  :Language)),
            //        new SqlParameter("@RecertMonth",(RecertMonth== null ? "" :RecertMonth)),
            //        new SqlParameter("@PrimaryPhone",(Phone == null ? "" :Phone)),
            //        new SqlParameter("@MembershipStatus",(MembershipStatus == null ? "" :MembershipStatus)),
            //        new SqlParameter("@SortColumn",sortColumn),
            //        new SqlParameter("@SortOrder",sortColumnDir),
            //        new SqlParameter("@AssignId",(UserIdtype == "1" ? ""  : UserIds)),
            //        new SqlParameter("@Offset",start),
            //        new SqlParameter("@Limit",length),
            //};


           
            //try
            //{


            //    List<MemberMasterSP> objss = _context.memberMasterSPs.FromSqlRaw("SP_UserMaster @Spara,@FromDate,@ToDate,@SearchbyName",
            //                           param).ToList();
            //    //List<MemberMasterSP> objss = _context.memberMasterSPs.FromSqlRaw("SP_UserMaster @Spara,@ResidenceID",
            //    //                      pc.ToArray()).ToList();

            //}
            //catch (Exception ex)
            //{

                
            //}
            
            var data = _context.MemberMaster.Where(
            a => a.CreatedDate >= (FromDate == null ? a.CreatedDate : Convert.ToDateTime(FromDate)) &&
             a.CreatedDate <= (ToDate == null ? a.CreatedDate : Convert.ToDateTime(ToDate)) &&
             (a.FirstName.Contains(SearchbyName == null ? a.FirstName : SearchbyName) ||
             a.LastName.Contains(SearchbyName == null ? a.LastName : SearchbyName)) &&
             a.Facility == (Facility == null ? a.Facility : Facility) &&
             a.MedicaidID == (MedicaidID == null ? a.MedicaidID : MedicaidID) &&
             a.ResidentID == (ResidenceID == null ? a.ResidentID : ResidenceID) &&
             a.CreatedBy == (UserName == null ? a.CreatedBy : userid) &&
             a.Language == (Language == null ? a.Language : Language) &&
             a.RecertMonth == (RecertMonth == null ? a.RecertMonth : RecertMonth) &&
             a.PrimaryPhone == (Phone == null ? a.PrimaryPhone : Phone) &&
             a.MembershipStatus == (MembershipStatus == null ? a.MembershipStatus : Convert.ToInt32(MembershipStatus))
             //&& a.AssignId == (UserId == 1 ? a.AssignId :  HttpContext.Session.GetString("UserID"))
            ).OrderByDescending(s => s.CreatedDate).
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
                Language = a.Language != "0" ? languagelist.Where(s => s.LanguageID == Convert.ToInt32(a.Language)).FirstOrDefault().Language.ToString() : "",
                RecertMonth = a.RecertMonth,
                MembershipStatu = Enum.GetName(typeof(MembershipStatus), Convert.ToInt32(a.MembershipStatus)),
                CreatedUser = userlist.Count() > 0 ? userlist.Where(s => s.UserID == a.CreatedBy).FirstOrDefault().UserName : "",
                FollowupCount = FollowupData.Count() > 0 ? FollowupData.Where(n => n.MemberId == a.MemberID).Count() > 0 ? Followuptimes : null : null,
                NextStepTask = NestStepTaskList.Count() > 0 ? NestStepTaskList.Where(n => n.MemberId == a.MemberID).Count() > 0 ? NestStepTaskList.Where(n => n.MemberId == a.MemberID).LastOrDefault().NewStatus : null : null,
               // Action = "<a class='btn btn-warning' href='/Member/Workfollow?memberId=' " + a.MemberID + ">View</a>"
            });
            var FltData = FollowupDatefilter.Select(s => s.MemberId).ToList();
            //if (FollowupData.Count() > 0)
            data = data.Where(s => s.FollowupCount == Followuptimes).ToList();
            data = data.Where(s => s.NextStepTask == NextStepTask).ToList();
            if (FltData.Count > 0)
            {
                data = data.Where(s => FltData.Contains(s.MemberID.Value)).ToList();
            }


           

            var recordsTotal= data.Count();

            if(data.Count() > 100) { data= data.Take(100); }
            //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            //{
            //    data = data.OrderBy(sortColumn + " " + sortColumnDir);

            //}
            //Search    
            //if (!string.IsNullOrEmpty(searchValue))
            //{
            //    data = data.Where(m => m.FirstName == searchValue);
            //}
            //data = data.Skip(start.GetValueOrDefault()).Take(length.GetValueOrDefault());
            //var response = new { data = data, recordsFiltered = recordsTotal, recordsTotal = recordsTotal };
            //return Json(response);
            //return Json(new { data = obj });

            return Json(data);
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
        public IActionResult MemberFilterExcel(string FromDate, string ToDate, string SearchbyName, string MedicaidID, string ResidenceID, string UserName, string Language, string MembershipStatus, string RecertMonth, string Followuptimes, string Facility, string NextStepTask, string Phone, string followupS, string followupE,int UserId)
        {
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
            UserId = Convert.ToInt32(HttpContext.Session.GetString("UserTypeId"));
            var userlist = _context.UserMaster.ToList();
            var data = _context.MemberMaster.Where(
            a => a.CreatedDate >= (FromDate == null ? a.CreatedDate : Convert.ToDateTime(FromDate)) &&
             a.CreatedDate <= (ToDate == null ? a.CreatedDate : Convert.ToDateTime(ToDate)) &&
             (a.FirstName.Contains(SearchbyName == null ? a.FirstName : SearchbyName) ||
             a.LastName.Contains(SearchbyName == null ? a.LastName : SearchbyName)) &&
             a.Facility == (Facility == null ? a.Facility : Facility) &&
             a.MedicaidID == (MedicaidID == null ? a.MedicaidID : MedicaidID) &&
             a.ResidentID == (ResidenceID == null ? a.ResidentID : ResidenceID) &&
             a.CreatedBy == (UserName == null ? a.CreatedBy : userid) &&
             a.Language == (Language == null ? a.Language : Language) &&
             a.RecertMonth == (RecertMonth == null ? a.RecertMonth : RecertMonth) &&
             a.PrimaryPhone == (Phone == null ? a.PrimaryPhone : Phone) &&
             a.MembershipStatus == (MembershipStatus == null ? a.MembershipStatus : Convert.ToInt32(MembershipStatus))
             && a.AssignId == (UserId == 1 ? a.AssignId : UserId)
            ).OrderByDescending(s => s.CreatedDate).
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
                Language = a.Language != "0" ? languagelist.Where(s => s.LanguageID == Convert.ToInt32(a.Language)).FirstOrDefault().Language.ToString() : "",
                RecertMonth = a.RecertMonth,
                MembershipStatu = Enum.GetName(typeof(MembershipStatus), Convert.ToInt32(a.MembershipStatus)),
                CreatedUser = userlist.Count() > 0 ? userlist.Where(s => s.UserID == a.CreatedBy).FirstOrDefault().UserName : "",
                FollowupCount = FollowupData.Count() > 0 ? FollowupData.Where(n => n.MemberId == a.MemberID).Count() > 0 ? Followuptimes : null : null,
                NextStepTask = NestStepTaskList.Count() > 0 ? NestStepTaskList.Where(n => n.MemberId == a.MemberID).Count() > 0 ? NestStepTaskList.Where(n => n.MemberId == a.MemberID).LastOrDefault().NewStatus : null : null
            });
            var FltData = FollowupDatefilter.Select(s => s.MemberId).ToList();
            //if (FollowupData.Count() > 0)
            data = data.Where(s => s.FollowupCount == Followuptimes).ToList();
            data = data.Where(s => s.NextStepTask == NextStepTask).ToList();
            if (FltData.Count > 0)
            {
                data = data.Where(s => FltData.Contains(s.MemberID.Value)).ToList();
            }

            if (data.Count() > 100) { data = data.Take(100); }

            DataTable table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(data), (typeof(DataTable)));
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
