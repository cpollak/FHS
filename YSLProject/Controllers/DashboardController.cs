using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using YSLProject.Models;
using YSLProject.Table;
using static iTextSharp.text.Font;
//using YSLProject.Reports;
using static YSLProject.Models.Enumdata;

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
        public IActionResult MemberFilter(string FromDate, string ToDate, string SearchbyName, string MedicaidID, string ResidenceID, string UserName, string Language, string MembershipStatus, string RecertMonth, string Followuptimes, string Facility, string NextStepTask)
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
            var languagelist = _context.LanguageMaster.ToList();
            var NestStepTaskList = _context.Recertification_Follow_Up.Where(s => s.NewStatus == NextStepTask).ToList();

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
             a.MembershipStatus == (MembershipStatus == null ? a.MembershipStatus : Convert.ToInt32(MembershipStatus))
            ).OrderByDescending(s => s.CreatedDate).Take(100).
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
                s.CreatedBy
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
            //if (FollowupData.Count() > 0)
            data = data.Where(s => s.FollowupCount == Followuptimes).ToList();
            data = data.Where(s => s.NextStepTask == NextStepTask).ToList();
            
            return Json(data);
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
