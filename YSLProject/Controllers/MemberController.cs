using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Web;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YSLProject.Models;
using YSLProject.Table;
using static YSLProject.Models.Enumdata;
using Microsoft.AspNetCore.Hosting;
using System.Data.OleDb;
//using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
//using System.Data.Entity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Reflection;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.StaticFiles;
using ClosedXML.Excel;
using System.Configuration;

namespace YSLProject.Controllers
{
    [Autho]
    public class MemberController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ApplicationDbContext _context2;

        private IHostingEnvironment Environment;

        private IConfiguration Configuration;


        public MemberController(IHostingEnvironment _environment, ApplicationDbContext context, IConfiguration _configuration)
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
            if (HttpContext.Session.GetString("successmessage") != null)
            {
                ViewBag.successmessage = HttpContext.Session.GetString("successmessage").ToString();
                HttpContext.Session.Remove("successmessage");
            }
            // var date = _context.MemberMaster.ToList().Take(10);
            return View();
        }

        [HttpPost]
        public IActionResult MembersList(int? length, int? start)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var sortColumn = HttpContext.Request.Form["columns[" + HttpContext.Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDir = HttpContext.Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = HttpContext.Request.Form["search[value]"].FirstOrDefault();

            var results = _context.MemberMaster.Where(a => a.MembershipStatus != 99 && a.DisenrolmentDate.ToString() == null).ToList().Select(a => new MemberMasterModel
            {
                FirstName = a.FirstName,
                LastName = a.LastName,
                Email = a.Email,
                PrimaryPhone = a.PrimaryPhone,
                Action = "<a class='btn btn - warning' href='/Member/EditMember/" + a.MemberID + "'>Edit</a> | <a class='btn btn - danger' href='/Member/DeleteMember/" + a.MemberID + "'>Delete</a>"
            });
            int recordsTot = results.Count();
            results = results.Where(c => (c.FirstName != null && c.FirstName.ToLower().Contains(searchValue.ToLower())) || (c.LastName != null && c.LastName.ToLower().Contains(searchValue.ToLower())) || (c.Email != null && c.Email.Contains(searchValue)) || (c.PrimaryPhone != null && c.PrimaryPhone.Contains(searchValue))).ToList();
            int recordsFilter = results.Count();
            if (sortColumnDir == "asc")
            {
                if (sortColumn == "FirstName")
                    results = results.OrderBy(x => x.FirstName).ToList();
                else if (sortColumn == "LastName")
                    results = results.OrderBy(x => x.LastName).ToList();
                else if (sortColumn == "Email")
                    results = results.OrderBy(x => x.Email).ToList();
                else
                    results = results.OrderBy(x => x.PrimaryPhone).ToList();
            }
            else
            {
                if (sortColumn == "FirstName")
                    results = results.OrderByDescending(x => x.FirstName).ToList();
                else if (sortColumn == "LastName")
                    results = results.OrderByDescending(x => x.LastName).ToList();
                else if (sortColumn == "Email")
                    results = results.OrderByDescending(x => x.Email).ToList();
                else
                    results = results.OrderByDescending(x => x.PrimaryPhone).ToList();
            }
            var data = results.Skip(start.GetValueOrDefault()).Take(length.GetValueOrDefault()).ToList();

            var response = new { data = data, recordsFiltered = recordsFilter, recordsTotal = recordsTot };
            return Json(response);
            // return Json(data);
        }



        [HttpPost]
        public IActionResult AjaxMethod()
        {
            List<MemberMaster> customers = (from customer in _context.MemberMaster
                                            select customer).Where(a => a.MembershipStatus != 99 && a.DisenrolmentDate.ToString() == null).ToList();
            return Json(JsonConvert.SerializeObject(customers));
        }

        public IActionResult AddMember()
        {
            ViewBag.LanguageList = new SelectList(_context.LanguageMaster.ToList(), "LanguageID", "Language");

            ViewBag.MembershipStatusList = (from MembershipStatus e in Enum.GetValues(typeof(MembershipStatus))
                                            select new SelectListItem
                                            {
                                                Value = Convert.ToString((int)e),
                                                Text = e.ToString().Replace("_", "-")
                                            }).ToList();

            var list = new List<SelectListItem>
    {
        new SelectListItem{ Text="MLTC", Value = "MLTC" , Selected = true },
        new SelectListItem{ Text="DSNP", Value = "DSNP" },
        new SelectListItem{ Text="MAP", Value = "MAP"},
    };
            ViewBag.Facility = list.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddMember(MemberMasterModel obj)
        {
            ViewBag.LanguageList = new SelectList(_context.LanguageMaster.ToList(), "LanguageID", "Language");

            ViewBag.MembershipStatusList = (from MembershipStatus e in Enum.GetValues(typeof(MembershipStatus))
                                            select new SelectListItem
                                            {
                                                Value = Convert.ToString((int)e),
                                                Text = e.ToString().Replace("_", "-")
                                            }).ToList();
            var list = new List<SelectListItem>
            {
                new SelectListItem{ Text="MLTC", Value = "MLTC" , Selected = true },
                new SelectListItem{ Text="DSNP", Value = "DSNP" },
                new SelectListItem{ Text="MAP", Value = "MAP"},
            };
            ViewBag.Facility = list.ToList();
            for (int i = 0; i < obj.contactModels.Count; i++)
            {
                ModelState.Remove("contactModels[" + i + "].RelationShip");
            }
            obj.contactModels.RemoveAt(0);

            if (ModelState.IsValid)
            {

                MemberMaster obj1 = new MemberMaster();
                obj1.FirstName = obj.FirstName;
                obj1.LastName = obj.LastName;
                obj1.Address = obj.Address;
                obj1.PrimaryPhone = obj.PrimaryPhone;
                obj1.Email = obj.Email;
                obj1.Language = obj.Language;
                obj1.CountyCode = obj.CountyCode;
                obj1.ResidentID = obj.ResidentID;
                obj1.MedicaidID = obj.MedicaidID;
                obj1.EnrollmentDate = obj.EnrollmentDate;
                obj1.DisenrolmentDate = obj.DisenrolmentDate;
                obj1.MembershipStatus = obj.MembershipStatus;
                obj1.Phone = obj.Phone;
                obj1.WorkFlowStatus = obj.WorkFlowStatus;
                obj1.InternalNotes = obj.InternalNotes;
                obj1.ExternalNotes = obj.ExternalNotes;
                obj1.ChartsID = obj.ChartsID;
                obj1.DateOfBirth = obj.DateOfBirth;
                obj1.CreatedDate = DateTime.Now;
                obj1.Facility = obj.Facility;
                obj1.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
                _context.MemberMaster.Add(obj1);
                await _context.SaveChangesAsync();
                int MemberId = obj1.MemberID;

                Logs objlog = new Logs();
                objlog.CreatedDate = DateTime.Now;
                objlog.MemberID = MemberId;
                objlog.ActionName = "New member added";
                _context.Logs.Add(objlog);
                await _context.SaveChangesAsync();

                List<Contacts> Contacts = new List<Contacts>();
                foreach (ContactModel mon in obj.contactModels)
                {
                    Contacts objc = new Contacts();
                    objc.MemberID = MemberId;
                    objc.RelationShip = mon.RelationShip;
                    objc.Name = mon.Name;
                    objc.Phone = mon.Phone;
                    objc.Email = mon.Email;
                    objc.Address = mon.Address;
                    Contacts.Add(objc);
                }
                if (Contacts.Count > 0)
                {
                    _context.Contacts.AddRange(Contacts);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("Index", "Member");
            }
            return View(obj);
        }

        public IActionResult EditMember(int Id)
        {
            ViewBag.LanguageList = new SelectList(_context.LanguageMaster.ToList(), "LanguageID", "Language");

            ViewBag.MembershipStatusList = (from MembershipStatus e in Enum.GetValues(typeof(MembershipStatus))
                                            select new SelectListItem
                                            {
                                                Value = Convert.ToString((int)e),
                                                Text = e.ToString().Replace("_", "-")
                                            }).ToList();

            var list = new List<SelectListItem>
            {
                new SelectListItem{ Text="MLTC", Value = "MLTC" , Selected = true },
                new SelectListItem{ Text="DSNP", Value = "DSNP" },
                new SelectListItem{ Text="MAP", Value = "MAP"},
            };
            ViewBag.Facility = list.ToList();
            MemberMasterModel obj = new MemberMasterModel();
            var MemberMasters = _context.MemberMaster.Where(a => a.MemberID == Id).ToList();
            foreach (var mob in MemberMasters.ToList())
            {
                obj.MemberID = mob.MemberID;
                obj.FirstName = mob.FirstName;
                obj.LastName = mob.LastName;
                obj.Address = mob.Address;
                obj.PrimaryPhone = mob.PrimaryPhone;
                obj.Email = mob.Email;
                obj.Language = mob.Language;
                obj.CountyCode = mob.CountyCode;
                obj.ResidentID = mob.ResidentID;
                obj.MedicaidID = mob.MedicaidID;
                obj.EnrollmentDate = mob.EnrollmentDate;
                obj.DisenrolmentDate = mob.DisenrolmentDate;
                obj.MembershipStatus = mob.MembershipStatus != null ? mob.MembershipStatus.Value : 0;
                obj.Phone = mob.Phone;
                obj.WorkFlowStatus = mob.WorkFlowStatus != null ? mob.WorkFlowStatus.Value : 0;
                obj.InternalNotes = mob.InternalNotes;
                obj.ExternalNotes = mob.ExternalNotes;
                obj.CDate = mob.CreatedDate;
                obj.CreatedBy = mob.CreatedBy;
                obj.ChartsID = mob.ChartsID;
                obj.DateOfBirth = mob.DateOfBirth;
                obj.Facility = mob.Facility;
                var data = _context.Recertification_Follow_Up.OrderByDescending(a => a.FollowUpID).FirstOrDefault();
                if (data != null)
                {
                    //RecertificationFollowUpList objd = new RecertificationFollowUpList();
                    obj.CurrentStatus = Enum.GetName(typeof(FollowupStatus), Convert.ToInt32(data.CurrentStatus)).Replace("_", " ");
                }


            }

            var Contact = _context.Contacts.Where(a => a.MemberID == Id).ToList();
            if (Contact.Count > 0)
            {
                obj.contactModels = new List<ContactModel>();
            }
            foreach (var mons in Contact.ToList())
            {
                ContactModel or = new ContactModel();
                or.ContactID = mons.ContactID;
                or.MemberID = mons.MemberID;
                or.RelationShip = mons.RelationShip;
                or.Name = mons.Name;
                or.Phone = mons.Phone;
                or.Email = mons.Email;
                or.Address = mons.Address;
                obj.contactModels.Add(or);
            }
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> EditMember(MemberMasterModel obj)
        {
            ViewBag.LanguageList = new SelectList(_context.LanguageMaster.ToList(), "LanguageID", "Language");

            ViewBag.MembershipStatusList = (from MembershipStatus e in Enum.GetValues(typeof(MembershipStatus))
                                            select new SelectListItem
                                            {
                                                Value = Convert.ToString((int)e),
                                                Text = e.ToString().Replace("_", "-")
                                            }).ToList();
            var list = new List<SelectListItem>
            {
                new SelectListItem{ Text="MLTC", Value = "MLTC" , Selected = true },
                new SelectListItem{ Text="DSNP", Value = "DSNP" },
                new SelectListItem{ Text="MAP", Value = "MAP"},
            };
            ViewBag.Facility = list.ToList();

            for (int i = 0; i < obj.contactModels.Count; i++)
            {
                ModelState.Remove("contactModels[" + i + "].RelationShip");
            }
            obj.contactModels.RemoveAt(0);

            if (ModelState.IsValid)
            {

                //var MemberData = _context.MemberMaster.AsNoTracking().Where(s=>s.MemberID== obj.MemberID).FirstOrDefault();
                MemberMaster obj1 = new MemberMaster();
                obj1.MemberID = obj.MemberID.Value;
                obj1.FirstName = obj.FirstName;
                obj1.LastName = obj.LastName;
                obj1.Address = obj.Address;
                obj1.PrimaryPhone = obj.PrimaryPhone;
                obj1.Email = obj.Email;
                obj1.Language = obj.Language;
                obj1.CountyCode = obj.CountyCode;
                obj1.ResidentID = obj.ResidentID;
                obj1.MedicaidID = obj.MedicaidID;
                obj1.EnrollmentDate = obj.EnrollmentDate;
                obj1.DisenrolmentDate = obj.DisenrolmentDate;
                obj1.MembershipStatus = obj.MembershipStatus;
                obj1.Phone = obj.Phone;
                obj1.WorkFlowStatus = obj.WorkFlowStatus;
                obj1.InternalNotes = obj.InternalNotes;
                obj1.ExternalNotes = obj.ExternalNotes;
                obj1.ChartsID = obj.ChartsID;
                obj1.DateOfBirth = obj.DateOfBirth;
                obj1.Facility = obj.Facility;
                obj1.CreatedDate = obj.CDate;
                obj1.CreatedBy = obj.CreatedBy;
                obj1.TempRecertMonth = obj.TempRecertMonth;

                _context.MemberMaster.Update(obj1);
                int MemberId = obj.MemberID.Value;
                await _context.SaveChangesAsync();

                Logs objlog = new Logs();
                objlog.CreatedDate = DateTime.Now;
                objlog.MemberID = MemberId;
                objlog.ActionName = "Member Updated";
                _context.Logs.Add(objlog);
                await _context.SaveChangesAsync();

                List<Contacts> Contacts = new List<Contacts>();
                var data = _context.Contacts.Where(a => a.MemberID == obj1.MemberID).ToList();
                _context.Contacts.RemoveRange(data);
                await _context.SaveChangesAsync();
                foreach (ContactModel mon in obj.contactModels)
                {
                    Contacts objc = new Contacts();
                    objc.MemberID = MemberId;
                    objc.RelationShip = mon.RelationShip;
                    objc.Name = mon.Name;
                    objc.Phone = mon.Phone;
                    objc.Email = mon.Email;
                    objc.Address = mon.Address;
                    Contacts.Add(objc);
                }
                if (Contacts.Count > 0)
                {
                    _context.Contacts.AddRange(Contacts);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("Index", "Member");
            }
            return RedirectToAction("Index", "Member");
        }


        [HttpPost]
        public async Task<IActionResult> UpdateMember(MemberMasterModel obj)
        {
            ViewBag.LanguageList = new SelectList(_context.LanguageMaster.ToList(), "LanguageID", "Language");

            ViewBag.MembershipStatusList = (from MembershipStatus e in Enum.GetValues(typeof(MembershipStatus))
                                            select new SelectListItem
                                            {
                                                Value = Convert.ToString((int)e),
                                                Text = e.ToString().Replace("_", "-")
                                            }).ToList();
            var list = new List<SelectListItem>
            {
                new SelectListItem{ Text="MLTC", Value = "MLTC" , Selected = true },
                new SelectListItem{ Text="DSNP", Value = "DSNP" },
                new SelectListItem{ Text="MAP", Value = "MAP"},
            };
            ViewBag.Facility = list.ToList();

            ViewBag.MonthList = (from RecertMonth e in Enum.GetValues(typeof(RecertMonth))
                                 select new SelectListItem
                                 {
                                     Value = e.ToString(),
                                     Text = e.ToString()
                                 }).ToList();
            ViewBag.MonthList.Insert(0, new SelectListItem { Text = "Select", Value = "" });
            
            ModelState.Remove("MemberStatus");
            ModelState.Remove("EnrollmentDate");
            ModelState.Remove("NewEnrollmentDate");
            ModelState.Remove("DisenrolmentDate");
            ModelState.Remove("Facility");
            if (ModelState.IsValid)
            {

                var MemberData = _context.MemberMaster.AsNoTracking().Where(s => s.MemberID == obj.MemberID).FirstOrDefault();
                MemberMaster obj1 = new MemberMaster();
                obj1.MemberID = obj.MemberID.Value;
                obj1.FirstName = obj.FirstName;
                obj1.LastName = obj.LastName;
                obj1.Address = obj.Address;
                obj1.PrimaryPhone = obj.PrimaryPhone;
                obj1.Email = obj.Email;
                obj1.Language = obj.Language;
                obj1.CountyCode = obj.CountyCode;
                obj1.ResidentID = obj.ResidentID;
                obj1.MedicaidID = obj.MedicaidID;
                obj1.EnrollmentDate = MemberData.EnrollmentDate;
                obj1.DisenrolmentDate = MemberData.DisenrolmentDate;
                obj1.MembershipStatus = obj.MembershipStatus;
                obj1.Phone = obj.Phone;
                obj1.WorkFlowStatus = obj.WorkFlowStatus;
                obj1.InternalNotes = obj.InternalNotes;
                obj1.ExternalNotes = obj.ExternalNotes;
                obj1.ChartsID = obj.ChartsID;
                obj1.DateOfBirth = obj.DateOfBirth;
                obj1.Facility = obj.Facility;
                obj1.CreatedDate = obj.CDate;
                obj1.CreatedBy = obj.CreatedBy;
                obj1.NewEnrollmentDate = MemberData.NewEnrollmentDate;
                obj1.Comment = MemberData.Comment;
                obj1.TempRecertMonth = MemberData.TempRecertMonth;
                obj1.RecertMonth = obj.RecertMonth != null ? (obj.RecertMonth != "" ? DateTime.ParseExact(obj.RecertMonth, "MMM", CultureInfo.CurrentCulture).Month.ToString() : "0") : "0";
                string[] counrs = obj.CountyCode.ToString().Split(' ');
                bool ischeck = true;
                if (counrs.Length > 1)
                {
                    bool CountryCo = counrs[1].StartsWith("0");
                    if (CountryCo == true)
                    {
                        ischeck = false;
                    }
                }

                if (!String.IsNullOrEmpty(obj.RecertMonth))
                {
                    if (obj.RecertMonth.ToLower() == "dec")
                    {
                        obj1.CaseType = "SSI";
                    }
                    else if (ischeck == false)
                    {
                        obj1.CaseType = "PA";
                    }
                    else
                        obj1.CaseType = "";
                }
                else
                {
                    obj1.CaseType = "SSI";
                }

                _context.MemberMaster.Update(obj1);
                int MemberId = obj.MemberID.Value;
                await _context.SaveChangesAsync();

                Logs objlog = new Logs();
                objlog.CreatedDate = DateTime.Now;
                objlog.MemberID = MemberId;
                objlog.ActionName = "Member Updated";
                _context.Logs.Add(objlog);
                await _context.SaveChangesAsync();


                return RedirectToAction("Workfollow", "Member", new { memberId = MemberId });
            }
            return RedirectToAction("Index", "Member");
        }
        public async Task<IActionResult> DeleteMember(int Id)
        {
            var MemberM = await _context.MemberMaster.FindAsync(Id);
            _context.MemberMaster.Remove(MemberM);
            await _context.SaveChangesAsync();

            var Contac = _context.Contacts.Where(a => a.MemberID == Id).ToList();
            _context.Contacts.RemoveRange(Contac);
            await _context.SaveChangesAsync();
            return RedirectToActionPermanent("Index");
        }


        public IActionResult UploadAddMember()
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
            ViewBag.MemberStatusList = (from MemberStatus e in Enum.GetValues(typeof(MemberStatus))
                                        select new SelectListItem
                                        {
                                            Value = Convert.ToString((int)e),
                                            Text = e.ToString().Replace("_", "-")
                                        }).ToList();


            var FacilityType = new List<SelectListItem>
                    {
                new SelectListItem{ Text="Select", Value = "" , Selected = true },
                        new SelectListItem{ Text="MLTC", Value = "MLTC"},
                        new SelectListItem{ Text="DSNP", Value = "DSNP" },
                        new SelectListItem{ Text="MAP", Value = "MAP" },
                        //new SelectListItem{ Text="Centers", Value = "Centers" },
                        //new SelectListItem{ Text="Centers FIDA", Value = "Centers FIDA" },
                        //new SelectListItem{ Text="Centers MAP", Value = "Centers MAP" },
                        //new SelectListItem{ Text="Centers Surplus", Value = "Centers Surplus" },
                        //new SelectListItem{ Text="Centers DSNP", Value = "Centers DSNP" },
                    };
            ViewBag.FacilityType = FacilityType.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadAddMember(IFormFile postedFile, MemberMasterModel objM)
        {
            string JSONresult = "Done";
            try
            {
                if (postedFile != null)
                {
                    //Create a Folder.
                    string path = Path.Combine(this.Environment.WebRootPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    else
                    {
                        //string[] Files = Directory.GetFiles(path);
                        //foreach (string file in Files)
                        //{
                        //    System.IO.File.Delete(file);
                        //    //Directory.Delete(path);
                        //}
                        //Directory.Delete(path);
                    }

                    //Save the uploaded Excel file.
                    string fileName = Path.GetFileName(postedFile.FileName);
                    string filePath = Path.Combine(path, fileName);
                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                    }

                    //Read the connection string for the Excel file.
                    //string conString = this.Configuration.GetConnectionString("ExcelConString");
                    string conString = "";
                    string extension = Path.GetExtension(postedFile.FileName);
                    DataTable dt = new DataTable();
                    DataTable dtData = new DataTable();
                    if (extension == ".xls" || extension == ".xlsx")
                    {
                        switch (extension)
                        {
                            case ".xls": //Excel 97-03.
                                conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                                break;
                            case ".xlsx": //Excel 07 and above.
                                conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                                break;
                        }

                        conString = string.Format(conString, filePath);
                        try
                        {
                            using (OleDbConnection connExcel = new OleDbConnection(conString))
                            {
                                using (OleDbCommand cmdExcel = new OleDbCommand())
                                {
                                    using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                                    {
                                        cmdExcel.Connection = connExcel;

                                        //Get the name of First Sheet.
                                        connExcel.Open();
                                        DataTable dtExcelSchema;
                                        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                        string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                        connExcel.Close();

                                        //Read Data from First Sheet.
                                        connExcel.Open();
                                        cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                        odaExcel.SelectCommand = cmdExcel;
                                        odaExcel.Fill(dt);
                                        connExcel.Close();
                                        if (objM.IsNoHeader == true)
                                        {
                                            DataRow dr = dt.NewRow();
                                            for (int i = 0; i < dt.Columns.Count; i++)
                                            {

                                                dr[i] = dt.Columns[i].ToString();
                                            }
                                            dt.Rows.Add(dr);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            return Content("<script language='javascript' type='text/javascript'>alert('" + ex.Message + "');</script>");
                        }
                    }
                    else
                    {
                        string csvData = System.IO.File.ReadAllText(filePath);
                        bool firstRow = true;
                        foreach (string row in csvData.Split('\n'))
                        {
                            if (!string.IsNullOrEmpty(row))
                            {
                                if (!string.IsNullOrEmpty(row))
                                {
                                    if (objM.IsNoHeader == false)
                                    {
                                        if (firstRow)
                                        {
                                            foreach (string cell in row.Split(','))
                                            {
                                                dt.Columns.Add(Regex.Replace(cell.Trim(), @"\s+", ""));
                                            }
                                            firstRow = false;
                                        }
                                        else
                                        {
                                            dt.Rows.Add();
                                            int i = 0;
                                            foreach (string cell in row.Split(','))
                                            {
                                                dt.Rows[dt.Rows.Count - 1][i] = cell.Trim();
                                                i++;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        dt.Rows.Add();
                                        int i = 0;
                                        foreach (string cell in row.Split(','))
                                        {
                                            dt.Rows[dt.Rows.Count - 1][i] = cell.Trim();
                                            i++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //***--Delete Empty row from Datatable --***//
                    dt = dt.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(f => f is DBNull)).CopyToDataTable();
                    if (objM.MemberStatus == 1)//New-Admissions
                    {
                        SaveNewAddmition(dt, objM);
                    }
                    else if (objM.MemberStatus == 2)//Recertification
                    {
                        SaveRecertification(dt, objM);
                    }
                    else if (objM.MemberStatus == 3)//Noncovered 
                    {
                        SaveNoncovered(dt, objM);
                    }
                    else if (objM.MemberStatus == 4)//Discharged 
                    {
                        SaveDischarge(dt, objM);
                    }
                    else if (objM.MemberStatus == 6)
                    {
                        SaveRsFile(dt, objM);
                    }
                    else
                    {
                        SaveRelink(dt, objM);
                    }
                    HttpContext.Session.SetString("successmessage", "Record saved successfully");
                    return RedirectToAction("Index", "Member");

                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return RedirectToAction("Index", "Member");
        }

        public async void SaveNewAddmition(DataTable dt, MemberMasterModel objM)
        {
            var ResidentId = "";
            foreach (DataRow dr in dt.Rows)
            {
                MemberMaster obj1 = new MemberMaster();
                obj1.FollowUpDate = objM.FollowUpDate;
                string checkfiletype = objM.Facility;
                if (dr.ItemArray != null)
                {
                    bool ischeck = true;
                    obj1.RecertMonth = dr.ItemArray[8].ToString();
                    string rm = dr.ItemArray[8].ToString();
                    int Recertmonth = 0;

                    obj1.FirstName = (dr.ItemArray != null ? dr.ItemArray[3].ToString().Trim() : null);
                    obj1.LastName = (dr.ItemArray != null ? dr.ItemArray[2].ToString().Trim() : null);
                    ResidentId = dr.ItemArray[1].ToString().Trim();
                    //obj1.EnrollmentDate = DateTime.Now;
                    var ObjMem = _context.MemberMaster.AsTracking().Where(s => s.ResidentID.Trim() == ResidentId && s.Facility == objM.Facility && s.MembershipStatus != 99 && s.DisenrolmentDate.ToString() == null ).AsNoTracking().Count();
                    if (ObjMem == 0)
                    {
                        try
                        {
                            if (dr.ItemArray != null)
                            {
                                string[] counr = dr.ItemArray[16].ToString().Split(' ');
                                if (counr.Length > 1)
                                {
                                    bool CountryCo = counr[1].StartsWith("0");
                                    if (CountryCo == true)
                                    {
                                        //bool RecertM = dr.ItemArray[8].ToString().Contains("DEC");
                                        //if (RecertM == true)
                                        //{
                                        ischeck = false;
                                        //}
                                    }
                                    CountryCo = counr[1].Contains("500");
                                    if (CountryCo == true)
                                    {
                                        ischeck = false;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                        obj1.CountyCode = (dr.ItemArray != null ? dr.ItemArray[16].ToString() : null);//CountyCode
                        obj1.MedicaidID = (dr.ItemArray != null ? dr.ItemArray[4].ToString().Trim() : null);//CIN#
                        obj1.ResidentID = (dr.ItemArray != null ? dr.ItemArray[1].ToString().Trim() : null);//ID#
                        obj1.ChartsID = (dr.ItemArray != null ? dr.ItemArray[1].ToString().Trim() : null);
                        Recertmonth = obj1.RecertMonth != "" ? DateTime.ParseExact(obj1.RecertMonth, "MMM", CultureInfo.CurrentCulture).Month : 0;

                        if (Recertmonth > 0)
                        {
                            obj1.RecertMonth = Recertmonth.ToString();
                            int cur_month = DateTime.Now.Month;
                            if (cur_month <= 8)
                            {
                                int dif = Recertmonth - cur_month;
                                if (dif <= 3)
                                { obj1.FollowUpDate = DateTime.Now.AddDays(1); }
                            }
                        }
                        //if (ischeck == true)
                        //{
                            DateTime? DOB = null;

                            if (dr.ItemArray[6].ToString() != "")
                            {
                                string dateYear = dr.ItemArray[6].ToString().Substring(0, 4);
                                string dateMonth = dr.ItemArray[6].ToString().Substring(4, 2);
                                string dateDay = dr.ItemArray[6].ToString().Substring(6, 2);

                                DOB = Convert.ToDateTime(dateYear + "-" + dateMonth + "-" + dateDay);
                            }

                            obj1.Address = null;
                            obj1.PrimaryPhone = dr.ItemArray[20].ToString();
                            obj1.Email = null;
                            //obj1.Gender = (dt.Columns.Contains("Gender") ? (dr["Gender"].ToString().Contains("F") ? "2" : "1") : null);
                            obj1.Language = "0";

                            if (dr.ItemArray[18].ToString() != "")
                            {
                                var Lang = _context.LanguageMaster.Where(a => a.Language.Trim().ToLower() == dr.ItemArray[18].ToString().Trim().ToLower()).Count() > 0 ? _context.LanguageMaster.Where(a => a.Language.Trim().ToLower() == dr.ItemArray[18].ToString().Trim().ToLower()).FirstOrDefault().LanguageID : 0;
                                if (Lang == 0)
                                {
                                    LanguageMaster objlan = new LanguageMaster();
                                    objlan.Language = dr.ItemArray[18].ToString();
                                    _context.LanguageMaster.Add(objlan);
                                    _context.SaveChanges();
                                    Logs objlog = new Logs();
                                    objlog.CreatedDate = DateTime.Now;
                                    objlog.MemberID = obj1.MemberID;
                                    objlog.ActionName = "Closing";
                                    _context.Logs.Add(objlog);
                                    var langid = objlan.LanguageID;
                                    obj1.Language = langid.ToString();
                                }
                                else
                                {
                                    obj1.Language = Lang.ToString();
                                }
                            }

                            obj1.DateOfBirth = DOB;

                            obj1.Gender = (dr.ItemArray[6].ToString() != "" ? (dr.ItemArray[6].ToString().Contains("F") ? "2" : "1") : null);
                            obj1.DisenrolmentDate = null;
                            //obj1.DisenrolmentDate = (dt.Columns.Contains("DisenrolmentDate") ? (dr["DisenrolmentDate"] == null ? null : (DateTime?)Convert.ToDateTime(dr["DisenrolmentDate"])) : null);
                            obj1.Phone = (dr.ItemArray[21].ToString() != "" ? dr.ItemArray[21].ToString() : null);


                            obj1.WorkFlowStatus = 0;
                            obj1.InternalNotes = null;
                            obj1.ExternalNotes = null;
                            obj1.CreatedDate = DateTime.Now;
                            obj1.Facility = checkfiletype;
                            obj1.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
                            obj1.CIN = (dr.ItemArray[4].ToString() != "" ? dr.ItemArray[4].ToString() : null);
                            obj1.Comment = null;
                            obj1.Coverage = dr.ItemArray[9].ToString();
                            obj1.Payer = dr.ItemArray[10].ToString();
                            obj1.PlanCode = dr.ItemArray[11].ToString();
                            obj1.MedicarePayer = (dr.ItemArray[13] != null ? dr.ItemArray[13].ToString() : null);
                            obj1.CarrierCode = (dr.ItemArray[14] != null ? dr.ItemArray[14].ToString() : null);
                            obj1.ExceptionCodes = (dr.ItemArray[15] != null ? dr.ItemArray[15].ToString() : null);
                        string[] counrs = dr.ItemArray[16].ToString().Split(' ');
                        bool ischecked = true;
                        if (counrs.Length > 1)
                        {
                            bool CountryCo = counrs[1].StartsWith("0");
                            if (CountryCo == true)
                            {
                                ischecked = false;
                            }
                        }

                        if (!String.IsNullOrEmpty(rm))
                        {
                            if (rm.ToLower() == "dec")
                            {
                                obj1.CaseType = "SSI";
                            }
                            else if (ischecked == false)
                            {
                                obj1.CaseType = "PA";
                            }
                            else
                                obj1.CaseType = "";
                        }
                        else
                        {
                            obj1.CaseType = "SSI";
                        }


                        MemberMaster ObjCheck = new MemberMaster();
                            MemberMaster ObjCheck1 = new MemberMaster();
                            MemberMaster ObjCheck2 = new MemberMaster();
                            MemberMaster ObjCheck3 = new MemberMaster();
                            string mid = obj1.MedicaidID;
                            string cid = obj1.ResidentID;
                            ObjCheck = _context.MemberMaster.AsNoTracking().Where(s => s.MedicaidID == mid && s.ResidentID == cid && s.MembershipStatus != 99 && s.DisenrolmentDate.ToString() == null).AsNoTracking().FirstOrDefault();
                            ObjCheck1 = _context.MemberMaster.AsNoTracking().Where(s => s.MedicaidID == mid && s.MembershipStatus != 99 && s.DisenrolmentDate.ToString() == null).AsNoTracking().FirstOrDefault();
                            ObjCheck2 = _context.MemberMaster.AsNoTracking().Where(s => s.MedicaidID == mid && s.Facility == checkfiletype && s.MembershipStatus != 99 && s.DisenrolmentDate.ToString() == null).AsNoTracking().FirstOrDefault();
                            ObjCheck3 = _context.MemberMaster.AsNoTracking().Where(s => s.ResidentID == ResidentId && s.Facility == checkfiletype && s.MembershipStatus != 99 && s.DisenrolmentDate.ToString() == null).AsNoTracking().FirstOrDefault();

                            if (ObjCheck != null)
                            {
                                                              Recertification_Follow_Up objf = new Recertification_Follow_Up();

                                objf.MemberId = ObjCheck.MemberID;
                                objf.CurrentStatus = "2";
                                objf.Outcome = "Left Voice Mail";
                                objf.Notes = "";
                                objf.NextStepTask = "9";
                                objf.NextStepDueNotes = "";
                                objf.Nextduedate = DateTime.Now.AddDays(1);
                                objf.CreatedDate = DateTime.Now;
                                objf.AttemptCount = "";
                                objf.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
                                objf.NewStatus = "2";
                                _context.Recertification_Follow_Up.Add(objf);
                                _context.SaveChanges();


                            }
                            else if (ObjCheck1 != null)
                            {
                                int oldmemid = ObjCheck1.MemberID;
                                DateTime? dtoldenrolldate = ObjCheck1.EnrollmentDate;

                                #region Update Old record with log entry                        
                                ObjCheck1.MembershipStatus = 8;
                                ObjCheck1.DisenrolmentDate = DateTime.Now;
                                ObjCheck1.MemberID = oldmemid;
                                _context.MemberMaster.Update(ObjCheck1);
                                _context.SaveChanges();
                                _context.Entry(ObjCheck1).State = EntityState.Detached;

                                Logs objlog = new Logs();
                                objlog.CreatedDate = DateTime.Now;
                                objlog.MemberID = oldmemid;
                                objlog.ActionName = "Disenrolled  member";
                                _context.Logs.Add(objlog);
                                _context.SaveChanges();

                                #endregion

                                #region Add new Recorde with new chartsid 
                                obj1.EnrollmentDate = dtoldenrolldate;
                                obj1.NewEnrollmentDate = (dr.ItemArray[0] != null ? (DateTime?)Convert.ToDateTime(dr.ItemArray[0]) : DateTime.Now);
                                //obj1.NewEnrollmentDate = (dt.Columns.Contains("RunDate") ? (dr["RunDate"] == null ? DateTime.Now : (DateTime?)Convert.ToDateTime(dr["RunDate"])) : DateTime.Now);
                                DateTime newendate = Convert.ToDateTime(obj1.NewEnrollmentDate);
                                obj1.NewEnrollmentDate = new DateTime(newendate.Year, newendate.Month, 1);

                                obj1.MembershipStatus = 3;
                                obj1.Status = objM.MemberStatus;
                                _context.MemberMaster.Add(obj1);
                                _context.SaveChanges();
                                int MemberId = obj1.MemberID;

                                Logs objlog1 = new Logs();
                                objlog1.CreatedDate = DateTime.Now;
                                objlog1.MemberID = MemberId;
                                objlog1.ActionName = "New facility enrollment";
                                _context.Logs.Add(objlog1);
                                _context.SaveChanges();

                                #endregion

                            }
                            else if (ObjCheck2 != null)
                            {
                                ObjCheck2.ResidentID = ResidentId;
                                //ObjCheck2.MembershipStatus = 3;
                                _context.MemberMaster.Update(ObjCheck2);
                                _context.SaveChanges();
                                _context.Entry(ObjCheck2).State = EntityState.Detached;
                                Logs objlog1 = new Logs();
                                objlog1.CreatedDate = DateTime.Now;
                                objlog1.MemberID = ObjCheck2.MemberID;
                                objlog1.ActionName = "Patient is New admission’";
                                _context.Logs.Add(objlog1);
                                _context.SaveChanges();
                            }
                            else if (ObjCheck3 != null)
                            {
                                ObjCheck3.MedicaidID = mid;
                                //ObjCheck3.MembershipStatus = 3;
                                _context.MemberMaster.Update(ObjCheck3);
                                _context.SaveChanges();
                                _context.Entry(ObjCheck3).State = EntityState.Detached;
                                Logs objlog1 = new Logs();
                                objlog1.CreatedDate = DateTime.Now;
                                objlog1.MemberID = ObjCheck3.MemberID;
                                objlog1.ActionName = "Patient is New admission’";
                                _context.Logs.Add(objlog1);
                                _context.SaveChanges();
                            }
                            else
                            {
                                obj1.EnrollmentDate = (dr.ItemArray[0] != null ? (DateTime?)Convert.ToDateTime(dr.ItemArray[0]) : DateTime.Now);
                                obj1.NewEnrollmentDate = (dr.ItemArray[0] != null ? (DateTime?)Convert.ToDateTime(dr.ItemArray[0]) : DateTime.Now);
                                DateTime endate = Convert.ToDateTime(obj1.EnrollmentDate);
                                DateTime newendate = Convert.ToDateTime(obj1.NewEnrollmentDate);
                                obj1.EnrollmentDate = new DateTime(endate.Year, endate.Month, 1);
                                obj1.NewEnrollmentDate = new DateTime(newendate.Year, newendate.Month, 1);

                                obj1.MembershipStatus = 3;
                                obj1.Status = objM.MemberStatus;
                                //obj.Add(obj1);
                                _context.MemberMaster.Add(obj1);
                                _context.SaveChanges();
                                int MemberId = obj1.MemberID;

                                Logs objlog = new Logs();
                                objlog.CreatedDate = DateTime.Now;
                                objlog.MemberID = MemberId;
                                objlog.ActionName = "Patient is New admission";
                                _context.Logs.Add(objlog);
                                _context.SaveChanges();


                                Recertification_Follow_Up objf = new Recertification_Follow_Up();
                                if (Recertmonth > 0)
                                {
                                    int cur_month = DateTime.Now.Month;
                                    //DateTimeFormatInfo df = new DateTimeFormatInfo();
                                    //if (cur_month <= 8)
                                    //{
                                    int dif = Recertmonth - cur_month;
                                    if (dif <= 3 && dif > 0)
                                    {
                                        objf.MemberId = MemberId;
                                        objf.CurrentStatus = "1";
                                        objf.Outcome = "N/A";
                                        objf.Notes = "";
                                        objf.NextStepTask = "7";
                                        objf.NextStepDueNotes = "";
                                        objf.Nextduedate = DateTime.Now.AddDays(1);
                                        objf.CreatedDate = DateTime.Now;
                                        objf.AttemptCount = "";
                                        objf.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
                                        objf.NewStatus = "2";
                                        _context.Recertification_Follow_Up.Add(objf);
                                        _context.SaveChanges();
                                    }
                                    //}


                                }
                            }



                        //}



                    }
                    else
                    {
                        var memb = _context.MemberMaster.AsTracking().Where(s => s.ResidentID.Trim() == ResidentId && s.Facility == objM.Facility && s.MembershipStatus != 99 && s.DisenrolmentDate.ToString() == null).AsNoTracking().FirstOrDefault();
                        string[] counrs = memb.CountyCode.Split(' ');
                        bool ischecked = true;
                        if (counrs.Length > 1)
                        {
                            bool CountryCo = counrs[1].StartsWith("0");
                            if (CountryCo == true)
                            {
                                ischecked = false;
                            }
                        }

                        if (!String.IsNullOrEmpty(memb.RecertMonth))
                        {
                            if (memb.RecertMonth == "12")
                            {
                                obj1.CaseType = "SSI";
                            }
                            else if (ischecked == false)
                            {
                                obj1.CaseType = "PA";
                            }
                            else
                                obj1.CaseType = "";
                        }
                        else
                        {
                            obj1.CaseType = "SSI";
                        }
                        memb.MembershipStatus = 3;
                        memb.Status = null;
                        _context.MemberMaster.Update(memb);
                        _context.SaveChanges();
                    }
                }
            }
        }

        public async void SaveRecertification(DataTable dt, MemberMasterModel objM)
        {
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    MemberMaster obj1 = new MemberMaster();
                    obj1.MedicaidID = (dr.ItemArray != null ? dr.ItemArray[3].ToString() : null);
                    var ResidentId = (dr.ItemArray != null ? dr.ItemArray[0].ToString().Trim() : null);
                    string mid = obj1.MedicaidID;

                    //MemberMaster ObjCheck = _context.MemberMaster.AsTracking().Where(s => s.MedicaidID == mid).AsNoTracking().FirstOrDefault();
                    MemberMaster ObjCheck1 = _context.MemberMaster.AsTracking().Where(s => s.MedicaidID.Trim() == mid && s.Facility == objM.Facility && s.MembershipStatus != 99 && s.DisenrolmentDate.ToString() == null).AsNoTracking().FirstOrDefault();
                    MemberMaster ObjCheck = _context.MemberMaster.AsTracking().Where(s => s.ResidentID.Trim() == ResidentId && s.Facility == objM.Facility && s.MembershipStatus != 99 && s.DisenrolmentDate.ToString() == null).AsNoTracking().FirstOrDefault();
                    if (ObjCheck1 == null)
                    {
                        if (ObjCheck != null)
                        {
                            if (ObjCheck.MembershipStatus == 6)
                                ObjCheck.Status = 3;

                            ObjCheck.RecertMonth = (dr.ItemArray != null ? Convert.ToDateTime("01-" + dr.ItemArray[5].ToString() + "-" + DateTime.Now.Year).ToString("MMM") : null);
                            if (ObjCheck.RecertMonth != null && ObjCheck.RecertMonth != "")
                            {
                                ObjCheck.RecertMonth = ObjCheck.RecertMonth.ToUpper();
                                ObjCheck.RecertMonth = Convert.ToString(DateTime.ParseExact(ObjCheck.RecertMonth, "MMM", CultureInfo.CurrentCulture).Month);
                            }
                            ObjCheck.MedicaidID = mid;
                            ObjCheck.RecertificationProcess = true;
                            _context.Update(ObjCheck);
                            _context.SaveChanges();
                            Logs objlog = new Logs();
                            objlog.CreatedDate = DateTime.Now;
                            objlog.MemberID = ObjCheck.MemberID;
                            objlog.ActionName = "There is a recertification due";
                            _context.Logs.Add(objlog);
                            _context.SaveChanges();

                        }
                        else
                        {
                            obj1.FollowUpDate = objM.FollowUpDate;
                            obj1.Facility = objM.Facility;

                            obj1.FirstName = null;
                            obj1.LastName = null;
                            //if (dt.Columns.Contains("Resident"))
                            //{
                            //    string[] sn = dr["Resident"].ToString().Split(' ');
                            //    obj1.FirstName = sn[0].ToString();
                            //    obj1.LastName = sn[1].ToString();
                            //}
                            //else
                            //{
                            obj1.FirstName = (dr.ItemArray != null ? dr.ItemArray[2].ToString().Trim() : null);
                            obj1.LastName = (dr.ItemArray != null ? dr.ItemArray[1].ToString().Trim() : null);
                            //}

                            obj1.RecertMonth = (dr.ItemArray != null ? Convert.ToDateTime("01-" + dr.ItemArray[5].ToString() + "-" + DateTime.Now.Year).ToString("MMM") : null);
                            if (obj1.RecertMonth != null && obj1.RecertMonth != "")
                            {
                                obj1.RecertMonth = obj1.RecertMonth.ToUpper();
                                obj1.RecertMonth = Convert.ToString(DateTime.ParseExact(obj1.RecertMonth, "MMM", CultureInfo.CurrentCulture).Month);
                            }

                            obj1.CountyCode = (dr.ItemArray != null ? dr.ItemArray[12].ToString().Trim() : null);
                            obj1.ResidentID = ResidentId;
                            obj1.ChartsID = (dr.ItemArray != null ? dr.ItemArray[0].ToString().Trim() : null);
                            obj1.EnrollmentDate = DateTime.Now;
                            obj1.NewEnrollmentDate = DateTime.Now;
                            DateTime endate = Convert.ToDateTime(obj1.EnrollmentDate);
                            DateTime newendate = Convert.ToDateTime(obj1.NewEnrollmentDate);
                            obj1.EnrollmentDate = new DateTime(endate.Year, endate.Month, 1);
                            obj1.NewEnrollmentDate = new DateTime(newendate.Year, newendate.Month, 1);

                            obj1.Address = (dr.ItemArray != null ? dr.ItemArray[7].ToString().Trim() : null);
                            obj1.Language = "0";
                            if (dr.ItemArray != null)
                            {
                                if (dr.ItemArray[13].ToString().Trim() != "")
                                {
                                    var Lang = _context.LanguageMaster.Where(a => a.Language.Trim().ToLower() == dr.ItemArray[13].ToString().Trim().ToLower()).Count() > 0 ? _context.LanguageMaster.Where(a => a.Language.Trim().ToLower() == dr.ItemArray[13].ToString().Trim().ToLower()).FirstOrDefault().LanguageID : 0;
                                    if (Lang == 0)
                                    {
                                        LanguageMaster objlan = new LanguageMaster();
                                        objlan.Language = dr.ItemArray[13].ToString().Trim();
                                        _context.LanguageMaster.Add(objlan);
                                        _context.SaveChanges();
                                        var langid = objlan.LanguageID;
                                        obj1.Language = langid.ToString();
                                    }
                                    else
                                    {
                                        obj1.Language = Lang.ToString();
                                    }
                                }
                            }
                            obj1.CreatedDate = DateTime.Now;
                            obj1.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
                            obj1.MembershipStatus = 6;
                            obj1.Status = objM.MemberStatus;

                            _context.MemberMaster.Add(obj1);
                            _context.SaveChanges();
                            int MemberId = obj1.MemberID;

                            Logs objlog = new Logs();
                            objlog.CreatedDate = DateTime.Now;
                            objlog.MemberID = MemberId;
                            objlog.ActionName = "There is a recertification due";
                            _context.Logs.Add(objlog);
                            _context.SaveChanges();
                        }
                    }
                    else
                    {
                        if (ObjCheck1.MembershipStatus == 6)
                            ObjCheck1.Status = 3;
                        ObjCheck1.RecertMonth = (dr.ItemArray != null ? Convert.ToDateTime("01-" + dr.ItemArray[5].ToString() + "-" + DateTime.Now.Year).ToString("MMM") : null);
                        if (ObjCheck1.RecertMonth != null && ObjCheck1.RecertMonth != "")
                        {
                            ObjCheck1.RecertMonth = ObjCheck1.RecertMonth.ToUpper();
                            ObjCheck1.RecertMonth = Convert.ToString(DateTime.ParseExact(ObjCheck1.RecertMonth, "MMM", CultureInfo.CurrentCulture).Month);
                        }
                        ObjCheck1.RecertificationProcess = true;
                        _context.Update(ObjCheck1);
                        _context.SaveChanges();
                        Logs objlog = new Logs();
                        objlog.CreatedDate = DateTime.Now;
                        objlog.MemberID = ObjCheck1.MemberID;
                        objlog.ActionName = "There is a recertification due";
                        _context.Logs.Add(objlog);
                        _context.SaveChanges();

                    }

                    //if (ObjCheck1 == null)
                    //{

                    //}

                    //else
                    //{
                    //    obj1.MemberID = ObjCheck.MemberID;
                    //    obj1.RecertMonth = ObjCheck.RecertMonth;
                    //    obj1.MembershipStatus = 6;
                    //    if (ObjCheck.MembershipStatus == 6)
                    //    {
                    //         ObjCheck.Status = 2;
                    //        _context.Update(ObjCheck);
                    //        _context.SaveChanges();
                    //    }
                    //}
                    int Recertmonth = 0;


                    if ((dr.ItemArray != null ? Convert.ToDateTime("01-" + dr.ItemArray[5].ToString() + "-" + DateTime.Now.Year).ToString("MMM") : null) != null)
                    {
                        //Recertmonth = obj1.RecertMonth != "" ? Convert.ToInt32(obj1.RecertMonth) : 0;

                        Recertification_Follow_Up objf = new Recertification_Follow_Up();
                        // if (Recertmonth > 0)
                        // {
                        int cur_month = DateTime.Now.Month;
                        //DateTimeFormatInfo df = new DateTimeFormatInfo();
                        //if (cur_month <= 8)
                        //{
                        int dif = Recertmonth - cur_month;
                        // if (dif <= 3 && dif > 0)
                        //{
                        if (obj1.MemberID == 0)
                        {
                            if (ObjCheck1 != null)
                            {
                                if (ObjCheck1.MemberID == 0)
                                {
                                    objf.MemberId = ObjCheck.MemberID;
                                }
                                else
                                {
                                    objf.MemberId = ObjCheck1.MemberID;
                                }
                            }
                            else
                            {
                                if (ObjCheck.MemberID == 0)
                                {
                                    objf.MemberId = ObjCheck1.MemberID;
                                }
                                else
                                {
                                    objf.MemberId = ObjCheck.MemberID;
                                }
                            }
                        }
                        else
                        {
                            objf.MemberId = obj1.MemberID;
                        }
                        //objf.MemberId = (obj1.MemberID == 0 ? ( ObjCheck1.MemberID == 0 ? ObjCheck.MemberID :ObjCheck1.MemberID):obj1.MemberID);
                        objf.CurrentStatus = "1";
                        objf.Outcome = "N/A";
                        objf.Notes = "";
                        objf.NextStepTask = "7";
                        objf.NextStepDueNotes = "";
                        objf.Nextduedate = objM.FollowUpDate == null ? DateTime.Now.AddDays(10) : Convert.ToDateTime(objM.FollowUpDate);
                        objf.CreatedDate = DateTime.Now;
                        objf.AttemptCount = "";
                        objf.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
                        objf.NewStatus = "2";
                        _context.Recertification_Follow_Up.Add(objf);
                        _context.SaveChanges();

                        Logs objlog = new Logs();
                        objlog.CreatedDate = DateTime.Now;
                        objlog.MemberID = obj1.MemberID;
                        objlog.ActionName = "There is a recertification due";
                        _context.Logs.Add(objlog);
                        _context.SaveChanges();
                        //}
                        //}
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }

        public async void SaveNoncovered(DataTable dt, MemberMasterModel objM)
        {

            try
            {
                dt.Columns.Add("Facility", typeof(string));
                foreach (DataColumn col in dt.Columns)
                    col.ColumnName = col.ColumnName.Trim();
                foreach (DataRow row in dt.Rows)
                {
                    //need to set value to NewColumn column
                    row["Facility"] = objM.Facility;   // or set it to some other value
                }
                SqlBulkCopy oCopy = new SqlBulkCopy(this.Configuration.GetConnectionString("Myconnection"), SqlBulkCopyOptions.KeepNulls);
                try
                {
                    oCopy.BatchSize = 500;
                    oCopy.BulkCopyTimeout = 0;
                    oCopy.DestinationTableName = "NonCoveredBulk";
                    oCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Run date", "RunDate"));
                    oCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ID #", "ResidentID"));
                    oCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Last Name", "LastName"));
                    oCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("First Name", "FirstName"));
                    oCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("CIN #", "MedicaidID"));
                    oCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Comment", "Comment"));
                    oCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Facility", "Facility"));
                    oCopy.WriteToServer(dt);
                }
                catch (Exception ex)
                {
                    oCopy.Close();
                    oCopy = null;
                }
                finally
                {
                    oCopy.Close();
                    oCopy = null;
                }

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SP_NonCoverdFileUpload";
                cmd.Parameters.AddWithValue("@Facility", objM.Facility);
                cmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(HttpContext.Session.GetString("UserID")));

                string strConn = _context.Database.GetDbConnection().ConnectionString;
                SqlConnection Con = new SqlConnection(strConn);
                cmd.Connection = Con;
                cmd.CommandType = CommandType.StoredProcedure;
                Con.Open();
                cmd.ExecuteNonQuery();
                Con.Close();
            }
            catch (Exception)
            {
                throw;
            }


            //foreach (DataRow dr in dt.Rows)
            //{
            //    MemberMaster obj1 = new MemberMaster();
            //    MemberMaster obj2 = new MemberMaster();
            //    string Medicaid = (dr.ItemArray != null ? dr.ItemArray[4].ToString().Trim() : null);
            //    string ResidentId = (dr.ItemArray != null ? dr.ItemArray[1].ToString().Trim() : null);
            //    obj2 = _context.MemberMaster.AsNoTracking().Where(s => s.ResidentID.Trim() == ResidentId && s.Facility == objM.Facility && s.MembershipStatus != 99).AsNoTracking().FirstOrDefault();
            //    obj1 = _context.MemberMaster.AsNoTracking().Where(s => s.MedicaidID.Trim() == Medicaid && s.Facility == objM.Facility && s.MembershipStatus != 99).AsNoTracking().FirstOrDefault();
            //    if (obj1 == null)
            //    {
            //        if (obj2 != null)
            //        {
            //            if (obj2.MembershipStatus == 4)
            //                obj2.Status = 3;
            //            obj2.MembershipStatus = 4;
            //            obj2.MedicaidID = Medicaid;
            //            obj2.ResidentID = ResidentId;
            //            _context.Update(obj2);
            //            _context.SaveChanges();
            //            Logs objlog = new Logs();
            //            objlog.MemberID = obj2.MemberID;
            //            objlog.ActionName = "Patient is noncovered";
            //            _context.Logs.Add(objlog);
            //            _context.SaveChanges();
            //        }
            //        else
            //        {
            //            obj1 = new MemberMaster();
            //            obj1.FirstName = null;
            //            obj1.LastName = null;
            //            obj1.MedicaidID = Medicaid;
            //            //if (dt.Columns.Contains("Resident"))
            //            //{
            //            //    string[] sn = dr["Resident"].ToString().Split(' ');
            //            //    obj1.FirstName = sn[0].ToString();
            //            //    obj1.LastName = sn[1].ToString();
            //            //}
            //            //else
            //            //{
            //            obj1.FirstName = (dr.ItemArray != null ? dr.ItemArray[3].ToString() : null);
            //            obj1.LastName = (dr.ItemArray != null ? dr.ItemArray[2].ToString() : null);
            //            //}
            //            obj1.ResidentID = (dr.ItemArray != null ? dr.ItemArray[1].ToString() : null);

            //            //obj1.CountyCode = (dt.Columns.Contains("Location") ? dr["Location"].ToString() : null);
            //            obj1.ChartsID = (dr.ItemArray != null ? dr.ItemArray[1].ToString() : null);
            //            obj1.Comment = (dr.ItemArray != null ? dr.ItemArray[6].ToString().Trim() : null);
            //            obj1.EnrollmentDate = dr.ItemArray == null ? DateTime.Now : (DateTime?)Convert.ToDateTime(dr.ItemArray[0]);
            //            obj1.NewEnrollmentDate = dr.ItemArray == null ? DateTime.Now : (DateTime?)Convert.ToDateTime(dr.ItemArray[0]);
            //            DateTime endate = Convert.ToDateTime(obj1.EnrollmentDate);
            //            DateTime newendate = Convert.ToDateTime(obj1.NewEnrollmentDate);
            //            obj1.EnrollmentDate = new DateTime(endate.Year, endate.Month, 1);
            //            obj1.NewEnrollmentDate = new DateTime(newendate.Year, newendate.Month, 1);
            //            obj1.Facility = objM.Facility;
            //            obj1.Address = null;
            //            //obj1.Address = (dt.Columns.Contains("RESPADDRESS1") ? dr["RESPADDRESS1"].ToString() : null);
            //            obj1.Language = "0";
            //            //if (dt.Columns.Contains("Language"))
            //            //{
            //            //    if (dr["Language"].ToString() != "")
            //            //    {
            //            //        var Lang = _context.LanguageMaster.Where(a => a.Language.Trim().ToLower() == dr["Language"].ToString().Trim().ToLower()).Count() > 0 ? _context.LanguageMaster.Where(a => a.Language.Trim().ToLower() == dr["Language"].ToString().Trim().ToLower()).FirstOrDefault().LanguageID : 0;
            //            //        if (Lang == 0)
            //            //        {
            //            //            LanguageMaster objlan = new LanguageMaster();
            //            //            objlan.Language = dr["Language"].ToString();
            //            //            _context.LanguageMaster.Add(objlan);
            //            //            _context.SaveChanges();
            //            //            var langid = objlan.LanguageID;
            //            //            obj1.Language = langid.ToString();
            //            //        }
            //            //        else
            //            //        {
            //            //            obj1.Language = Lang.ToString();
            //            //        }
            //            //    }
            //            //}
            //            obj1.CreatedDate = DateTime.Now;
            //            obj1.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserID"));

            //            obj1.MembershipStatus = 4;
            //            obj1.Status = objM.MemberStatus;
            //            _context.MemberMaster.Add(obj1);
            //            _context.SaveChanges();
            //            int MemberId = obj1.MemberID;


            //            Logs objlog = new Logs();
            //            objlog.CreatedDate = DateTime.Now;
            //            objlog.MemberID = MemberId;
            //            objlog.ActionName = "Patient is noncovered";
            //            _context.Logs.Add(objlog);
            //            _context.SaveChanges();

            //            Logs objlog1 = new Logs();
            //            objlog1.CreatedDate = DateTime.Now;
            //            objlog1.MemberID = MemberId;
            //            objlog1.ActionName = "Status to non covered";
            //            _context.Logs.Add(objlog1);
            //            _context.SaveChanges();
            //        }

            //    }
            //    else
            //    {
            //        if (obj1.MembershipStatus == 4)
            //            obj1.Status = 3;
            //        obj1.MembershipStatus = 4;
            //        obj1.ResidentID = ResidentId;
            //        obj1.MedicaidID = Medicaid;
            //        //obj1.RecertificationProcess = true;
            //        _context.Update(obj1);
            //        _context.SaveChanges();
            //        Logs objlog = new Logs();
            //        objlog.MemberID = obj1.MemberID;
            //        objlog.ActionName = "Patient is noncovered";
            //        _context.Logs.Add(objlog);
            //        _context.SaveChanges();

            //        if (obj1.Status != objM.MemberStatus)
            //        {
            //            obj1.Status = objM.MemberStatus;
            //            obj1.MembershipStatus = 4;

            //            _context.MemberMaster.Update(obj1);
            //            _context.SaveChanges();
            //            _context.Entry(obj1).State = EntityState.Detached;


            //            Logs objlog1 = new Logs();
            //            objlog1.CreatedDate = DateTime.Now;
            //            objlog1.MemberID = obj1.MemberID;
            //            objlog1.ActionName = "Status to non covered";
            //            _context.Logs.Add(objlog1);
            //            _context.SaveChanges();
            //        }
            //    }
            //}
        }

        public async void SaveDischarge(DataTable dt, MemberMasterModel objM)
        {
            foreach (DataRow dr in dt.Rows)
            {

                MemberMaster obj1 = new MemberMaster();
                MemberMaster obj2 = new MemberMaster();
                string Medicaid = (dr.ItemArray != null ? dr.ItemArray[3].ToString().Trim() : null);
                string ResidentId = (dr.ItemArray != null ? dr.ItemArray[0].ToString().Trim() : null);
                string Date = (dr.ItemArray != null ? dr.ItemArray[4].ToString() : null);
                //obj1 = _context.MemberMaster.AsNoTracking().Where(s => s.MedicaidID.Trim() == Medicaid).AsNoTracking().FirstOrDefault();
                obj1 = _context.MemberMaster.AsNoTracking().Where(s => s.ResidentID.Trim() == ResidentId && s.Facility == objM.Facility && s.MembershipStatus != 99 && s.DisenrolmentDate.ToString() == null).AsNoTracking().FirstOrDefault();
                obj2 = _context.MemberMaster.AsNoTracking().Where(s => s.MedicaidID.Trim() == Medicaid && s.Facility == objM.Facility && s.MembershipStatus != 99 && s.DisenrolmentDate.ToString() == null).AsNoTracking().FirstOrDefault();
                if (obj2 == null)
                {
                    if (obj1 != null)
                    {
                        //if (obj1.MembershipStatus == 6)
                        //    obj1.Status = 3;
                        obj1.MembershipStatus = 5;
                        obj1.MedicaidID = Medicaid;
                        obj1.RecertificationProcess = true;
                        _context.Update(obj1);
                        _context.SaveChanges();
                        Logs objlog = new Logs();
                        objlog.CreatedDate = DateTime.Now;
                        objlog.MemberID = obj1.MemberID;
                        objlog.ActionName = "Patient is discharged";
                        _context.Logs.Add(objlog);
                        _context.SaveChanges();

                    }
                    else
                    {
                        DateTime dtnew = new DateTime();
                        if (Date != null)
                        {
                            dtnew = Convert.ToDateTime(Date);
                            dtnew = dtnew.AddMonths(1);
                            //dtnew = new DateTime(dtnew.Year, dtnew.Month + 1, 1);
                        }

                        obj1 = new MemberMaster();
                        obj1.FirstName = null;
                        obj1.LastName = null;
                        obj1.MedicaidID = Medicaid;
                        obj1.FirstName = (dr.ItemArray != null ? dr.ItemArray[1].ToString().Trim() : null);
                        obj1.LastName = (dr.ItemArray != null ? dr.ItemArray[2].ToString().Trim() : null);
                        obj1.ResidentID = ResidentId;
                        obj1.ChartsID = (dr.ItemArray != null ? dr.ItemArray[0].ToString().Trim() : null);
                        obj1.Comment = (dr.ItemArray != null ? dr.ItemArray[5].ToString().Trim() : null);
                        //obj1.EnrollmentDate = dr.ItemArray == null ? DateTime.Now : (DateTime?)Convert.ToDateTime(dr.ItemArray[0]);
                        //obj1.NewEnrollmentDate = dr.ItemArray == null ? DateTime.Now : (DateTime?)Convert.ToDateTime(dr.ItemArray[0]);
                        //DateTime endate = Convert.ToDateTime(obj1.EnrollmentDate);
                        //DateTime newendate = Convert.ToDateTime(obj1.NewEnrollmentDate);
                        //obj1.EnrollmentDate = new DateTime(endate.Year, endate.Month, 1);
                        //obj1.NewEnrollmentDate = new DateTime(newendate.Year, newendate.Month, 1);
                        obj1.EnrollmentDate = null;
                        obj1.NewEnrollmentDate = null;
                        obj1.Facility = objM.Facility;
                        obj1.Address = null;
                        //obj1.Address = (dt.Columns.Contains("RESPADDRESS1") ? dr["RESPADDRESS1"].ToString() : null);
                        obj1.Language = "0";

                        obj1.CreatedDate = DateTime.Now;
                        obj1.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserID"));

                        obj1.MembershipStatus = 5;
                        obj1.DischargeDate = dtnew;
                        obj1.RecertificationProcess = true;
                        obj1.Status = objM.MemberStatus;
                        _context.MemberMaster.Add(obj1);
                        _context.SaveChanges();
                        int MemberId = obj1.MemberID;


                        Logs objlog = new Logs();
                        objlog.CreatedDate = DateTime.Now;
                        objlog.MemberID = MemberId;
                        objlog.ActionName = "Add new member";
                        _context.Logs.Add(objlog);
                        _context.SaveChanges();

                        Logs objlog1 = new Logs();
                        objlog1.CreatedDate = DateTime.Now;
                        objlog1.MemberID = MemberId;
                        objlog1.ActionName = "Patient is discharged";
                        _context.Logs.Add(objlog1);
                        _context.SaveChanges();

                        //int mID = obj1.MemberID;
                        //obj1.Status = objM.MemberStatus;
                        //obj1.MembershipStatus = 5;
                        //obj1.DischargeDate = dtnew;

                        //_context.MemberMaster.Update(obj1);
                        //_context.SaveChanges();
                        //_context.Entry(obj1).State = EntityState.Detached;

                        //List<Recertification_Follow_Up> objr = new List<Recertification_Follow_Up>();

                        //objr = _context.Recertification_Follow_Up.AsNoTracking().Where(a => a.MemberId == mID).OrderByDescending(a => a.CreatedDate).ToList();

                        //foreach (Recertification_Follow_Up r in objr)
                        //{
                        //    //var folloid = r.FollowUpID;
                        //    //Recertification_Follow_Up rfu = new Recertification_Follow_Up();
                        //    //rfu.MemberId = mID;
                        //    //rfu.FollowUpID = folloid;
                        //    _context.Recertification_Follow_Up.Remove(r);
                        //    _context.SaveChanges();
                        //}


                        //Logs objlog1 = new Logs();
                        //objlog1.CreatedDate = DateTime.Now;
                        //objlog1.MemberID = mID;
                        //objlog1.ActionName = "Patient is discharged";
                        //_context.Logs.Add(objlog1);
                        //_context.SaveChanges();
                    }
                }
                else
                {
                    //if (obj2.MembershipStatus == 6)
                    //    obj2.Status = 3;
                    obj2.MembershipStatus = 5;
                    obj2.ResidentID = ResidentId;
                    obj2.RecertificationProcess = true;
                    _context.Update(obj2);
                    _context.SaveChanges();
                    Logs objlog = new Logs();
                    objlog.CreatedDate = DateTime.Now;
                    objlog.MemberID = obj2.MemberID;
                    objlog.ActionName = "Patient is discharged";
                    _context.Logs.Add(objlog);
                    _context.SaveChanges();
                }
            }
        }


        public async void SaveRelink(DataTable dt, MemberMasterModel objM)
        {
            foreach (DataRow dr in dt.Rows)
            {

                MemberMaster obj1 = new MemberMaster();
                string Medicaid = (dr.ItemArray != null ? dr.ItemArray[4].ToString().Trim() : null);
                string ResidentId = (dr.ItemArray != null ? dr.ItemArray[1].ToString().Trim() : null);
                obj1 = _context.MemberMaster.AsNoTracking().Where(s => s.ResidentID.Trim() == ResidentId && s.Facility == objM.Facility && s.MembershipStatus != 99 && s.DisenrolmentDate.ToString() == null).AsNoTracking().FirstOrDefault();
                if (obj1 != null)
                {
                    int mID = obj1.MemberID;
                    if (obj1.Status != objM.MemberStatus)
                    {
                        obj1.Status = objM.MemberStatus;
                        obj1.MembershipStatus = 7;

                        _context.MemberMaster.Update(obj1);
                        _context.SaveChanges();
                        _context.Entry(obj1).State = EntityState.Detached;


                        Logs objlog1 = new Logs();
                        objlog1.CreatedDate = DateTime.Now;
                        objlog1.MemberID = mID;
                        objlog1.ActionName = "Status to Relink";
                        _context.Logs.Add(objlog1);
                        _context.SaveChanges();
                    }
                }
                else
                {
                    obj1 = new MemberMaster();
                    obj1.FirstName = null;
                    obj1.LastName = null;
                    obj1.MedicaidID = Medicaid;
                    //if (dt.Columns.Contains("Resident"))
                    //{
                    //    string[] sn = dr["Resident"].ToString().Split(' ');
                    //    obj1.FirstName = sn[0].ToString();
                    //    obj1.LastName = sn[1].ToString();
                    //}
                    //else
                    //{
                    obj1.FirstName = (dr.ItemArray != null ? dr.ItemArray[3].ToString() : null);
                    obj1.LastName = (dr.ItemArray != null ? dr.ItemArray[2].ToString() : null);
                    //}
                    obj1.ResidentID = (dr.ItemArray != null ? dr.ItemArray[1].ToString() : null);

                    //obj1.CountyCode = (dt.Columns.Contains("Location") ? dr["Location"].ToString() : null);
                    obj1.ChartsID = (dr.ItemArray != null ? dr.ItemArray[1].ToString() : null);
                    obj1.Comment = (dr.ItemArray != null ? dr.ItemArray[5].ToString().Trim() : null);
                    obj1.EnrollmentDate = dr.ItemArray == null ? DateTime.Now : (DateTime?)Convert.ToDateTime(dr.ItemArray[0]);
                    obj1.NewEnrollmentDate = dr.ItemArray == null ? DateTime.Now : (DateTime?)Convert.ToDateTime(dr.ItemArray[0]);
                    DateTime endate = Convert.ToDateTime(obj1.EnrollmentDate);
                    DateTime newendate = Convert.ToDateTime(obj1.NewEnrollmentDate);
                    obj1.EnrollmentDate = new DateTime(endate.Year, endate.Month, 1);
                    obj1.NewEnrollmentDate = new DateTime(newendate.Year, newendate.Month, 1);
                    obj1.Facility = objM.Facility;
                    obj1.Address = null;
                    //obj1.Address = (dt.Columns.Contains("RESPADDRESS1") ? dr["RESPADDRESS1"].ToString() : null);
                    obj1.Language = "0";

                    obj1.CreatedDate = DateTime.Now;
                    obj1.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserID"));

                    obj1.MembershipStatus = 7;
                    obj1.Status = objM.MemberStatus;
                    _context.MemberMaster.Add(obj1);
                    _context.SaveChanges();
                    int MemberId = obj1.MemberID;


                    Logs objlog = new Logs();
                    objlog.CreatedDate = DateTime.Now;
                    objlog.MemberID = MemberId;
                    objlog.ActionName = "Add new member";
                    _context.Logs.Add(objlog);
                    _context.SaveChanges();

                    Logs objlog1 = new Logs();
                    objlog1.CreatedDate = DateTime.Now;
                    objlog1.MemberID = MemberId;
                    objlog1.ActionName = "Status to Relink";
                    _context.Logs.Add(objlog1);
                    _context.SaveChanges();
                }
            }
        }

        public async void SaveRsFile(DataTable dt, MemberMasterModel objM)
        {

            int Count = 1;
            //  SqlBulkCopy oCopy = new SqlBulkCopy(ConfigurationManager.AppSettings["Myconnection"].ToString(), SqlBulkCopyOptions.KeepNulls);
            SqlBulkCopy oCopy = new SqlBulkCopy(this.Configuration.GetConnectionString("Myconnection"), SqlBulkCopyOptions.KeepNulls);
            try
            {
                oCopy.BatchSize = 500;
                oCopy.BulkCopyTimeout = 0;
                oCopy.DestinationTableName = "RsBulkUpload";
                //for (int i = 0; i < dt.Columns.Count; i++)
                //{
                oCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Full Name", "FullName"));
                oCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("CIN", "CIN"));
                oCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Recert Status", "RecertStatus"));
                oCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Telephone", "Telephone"));
                //}
                oCopy.WriteToServer(dt);
            }
            catch (Exception ex)
            {
                oCopy.Close();
                oCopy = null;
            }
            finally
            {
                oCopy.Close();
                oCopy = null;
            }

            try
            {


                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SP_RsFileUpload";
                cmd.Parameters.AddWithValue("@Facility", objM.Facility);
                cmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(HttpContext.Session.GetString("UserID")));
                cmd.Parameters.AddWithValue("@MemberStatus", objM.MemberStatus);
                string strConn = _context.Database.GetDbConnection().ConnectionString;
                SqlConnection Con = new SqlConnection(strConn);
                cmd.Connection = Con;
                cmd.CommandType = CommandType.StoredProcedure;
                Con.Open();
                cmd.ExecuteNonQuery();
                Con.Close();
            }
            catch (Exception)
            {
            }
            //foreach (DataRow dr in dt.Rows)
            //{

            //    MemberMaster obj1 = new MemberMaster();
            //    string Medicaid = (dr.ItemArray != null ? dr["CIN"].ToString().Trim() : null);
            //    //string ResidentId = (dr.ItemArray != null ? dr.ItemArray[1].ToString().Trim() : null);
            //    obj1 = _context.MemberMaster.AsNoTracking().Where(s => s.MedicaidID.Trim() == Medicaid && s.Facility == objM.Facility && s.MembershipStatus != 99 && s.DisenrolmentDate.ToString() == null).AsNoTracking().FirstOrDefault();
            //    if (obj1 != null)
            //    {
            //        int mID = obj1.MemberID;

            //        obj1.MembershipStatus = 3;
            //        if (dr["Recert Status"].ToString() == "5")
            //        {
            //            obj1.RecertificationProcess = false;
            //        }
            //        _context.MemberMaster.Update(obj1);
            //        _context.SaveChanges();
            //        _context.Entry(obj1).State = EntityState.Detached;


            //        Logs objlog1 = new Logs();
            //        objlog1.CreatedDate = DateTime.Now;
            //        objlog1.MemberID = mID;
            //        objlog1.ActionName = "Status to Rs";
            //        _context.Logs.Add(objlog1);
            //        _context.SaveChanges();
            //    }
            //    else
            //    {
            //        obj1 = new MemberMaster();
            //        obj1.FirstName = null;
            //        obj1.LastName = null;
            //        obj1.MedicaidID = Medicaid;
            //        if (dr["Full Name"].ToString().Contains("."))
            //        {
            //            obj1.FirstName = (dr.ItemArray != null ? dr["Full Name"].ToString().Split(".")[0].ToString() : null);
            //            obj1.LastName = (dr.ItemArray != null ? dr["Full Name"].ToString().Split(".")[1].ToString() : null);
            //        }
            //        else if (dr.ItemArray[0].ToString().Contains(","))
            //        {
            //            obj1.FirstName = (dr.ItemArray != null ? dr["Full Name"].ToString().Split(",")[0].ToString() : null);
            //            obj1.LastName = (dr.ItemArray != null ? dr["Full Name"].ToString().Split(",")[1].ToString() : null);
            //        }
            //        //}
            //        obj1.ResidentID = null; //(dr.ItemArray != null ? dr.ItemArray[1].ToString() : null);

            //        //obj1.CountyCode = (dt.Columns.Contains("Location") ? dr["Location"].ToString() : null);
            //        obj1.ChartsID = null; // (dr.ItemArray != null ? dr.ItemArray[1].ToString() : null);
            //        obj1.Comment = null; // (dr.ItemArray != null ? dr.ItemArray[6].ToString().Trim() : null);
            //        obj1.EnrollmentDate = DateTime.Now; // dr.ItemArray == null ? DateTime.Now : (DateTime?)Convert.ToDateTime(dr.ItemArray[0]);
            //        obj1.NewEnrollmentDate = DateTime.Now; // dr.ItemArray == null ? DateTime.Now : (DateTime?)Convert.ToDateTime(dr.ItemArray[0]);
            //        DateTime endate = Convert.ToDateTime(obj1.EnrollmentDate);
            //        DateTime newendate = Convert.ToDateTime(obj1.NewEnrollmentDate);
            //        obj1.EnrollmentDate = new DateTime(endate.Year, endate.Month, 1);
            //        obj1.NewEnrollmentDate = new DateTime(newendate.Year, newendate.Month, 1);
            //        obj1.Facility = objM.Facility;
            //        obj1.Address = null;
            //        obj1.Language = "0";
            //        obj1.CreatedDate = DateTime.Now;
            //        obj1.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserID"));

            //        obj1.MembershipStatus = 3;
            //        obj1.Status = objM.MemberStatus;
            //        _context.MemberMaster.Add(obj1);
            //        _context.SaveChanges();
            //        int MemberId = obj1.MemberID;

            //        Logs objlog = new Logs();
            //        objlog.CreatedDate = DateTime.Now;
            //        objlog.MemberID = MemberId;
            //        objlog.ActionName = "Add new member";
            //        _context.Logs.Add(objlog);
            //        _context.SaveChanges();

            //        Logs objlog1 = new Logs();
            //        objlog1.CreatedDate = DateTime.Now;
            //        objlog1.MemberID = MemberId;
            //        objlog1.ActionName = "Status to Rs";
            //        _context.Logs.Add(objlog1);
            //        _context.SaveChanges();
            //    }


            //    if (dr["Recert Status"].ToString() == "2")
            //    {
            //        Recertification_Follow_Up objf = new Recertification_Follow_Up();
            //        objf.MemberId = obj1.MemberID;
            //        objf.CurrentStatus = "1";
            //        //objf.Outcome = "RS FILE - Renewal Received – not renewed";
            //        objf.Outcome = "RECERTIFICATION PENDING";
            //        objf.Notes = "";
            //        objf.NextStepTask = "2";
            //        objf.NextStepDueNotes = "";
            //        objf.Nextduedate = DateTime.Now.AddDays(21);
            //        objf.CreatedDate = DateTime.Now;
            //        objf.AttemptCount = "";
            //        objf.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            //        objf.NewStatus = "2";
            //        _context.Recertification_Follow_Up.Add(objf);
            //        _context.SaveChanges();
            //        Logs objlog = new Logs();
            //        objlog.CreatedDate = DateTime.Now;
            //        objlog.MemberID = obj1.MemberID;
            //        objlog.ActionName = "Closing";
            //        _context.Logs.Add(objlog);
            //        _context.SaveChanges();
            //    }
            //    else if (dr["Recert Status"].ToString() == "5")
            //    {
            //        Recertification_Follow_Up objf = new Recertification_Follow_Up();
            //        objf.MemberId = obj1.MemberID;
            //        objf.CurrentStatus = "1";
            //        objf.Outcome = "RECERTIFICATION APPROVED";
            //        objf.Notes = "";
            //        objf.NextStepTask = "0";
            //        objf.NextStepDueNotes = "";
            //        objf.Nextduedate = DateTime.Now.AddYears(1);
            //        objf.CreatedDate = DateTime.Now;
            //        objf.AttemptCount = "";
            //        objf.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            //        objf.NewStatus = "16";

            //        _context.Recertification_Follow_Up.Add(objf);
            //        _context.SaveChanges();
            //        Logs objlog = new Logs();
            //        objlog.CreatedDate = DateTime.Now;
            //        objlog.MemberID = obj1.MemberID;
            //        objlog.ActionName = "Renewed";
            //        _context.Logs.Add(objlog);
            //        _context.SaveChanges();
            //    }
            //    else if (dr["Recert Status"].ToString() == "6")
            //    {
            //        Recertification_Follow_Up objf = new Recertification_Follow_Up();
            //        objf.MemberId = obj1.MemberID;
            //        objf.CurrentStatus = "1";
            //        objf.Outcome = "RECERTIFICATION REJECTED";
            //        objf.Notes = "RS Files Upload";
            //        objf.NextStepTask = "2";
            //        objf.NextStepDueNotes = "";
            //        objf.Nextduedate = DateTime.Now.AddDays(1);
            //        objf.CreatedDate = DateTime.Now;
            //        objf.AttemptCount = "";
            //        objf.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            //        objf.NewStatus = "2";
            //        _context.Recertification_Follow_Up.Add(objf);
            //        _context.SaveChanges();
            //    }
            //    else
            //    {

            //    }

            //}
        }

        public IActionResult UploadAddMemberPIT()
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

            var FacilityType = new List<SelectListItem>
                    {
                new SelectListItem{ Text="Select", Value = "" , Selected = true },
                        new SelectListItem{ Text="MLTC", Value = "MLTC"},
                        new SelectListItem{ Text="DSNP", Value = "DSNP" },
                        new SelectListItem{ Text="MAP", Value = "MAP" },
                        //new SelectListItem{ Text="Centers", Value = "Centers" },
                        //new SelectListItem{ Text="Centers FIDA", Value = "Centers FIDA" },
                        //new SelectListItem{ Text="Centers MAP", Value = "Centers MAP" },
                        //new SelectListItem{ Text="Centers Surplus", Value = "Centers Surplus" },
                        //new SelectListItem{ Text="Centers DSNP", Value = "Centers DSNP" },
                    };
            ViewBag.FacilityType = FacilityType.ToList();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadAddMemberPIT(IFormFile postedFile, MemberMasterModel objM)
        {
            string JSONresult = "Done";

            if (postedFile != null)
            {
                //Create a Folder.
                string path = Path.Combine(this.Environment.WebRootPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                else
                {
                    string[] Files = Directory.GetFiles(path);
                    foreach (string file in Files)
                    {
                        System.IO.File.Delete(file);
                        //Directory.Delete(path);
                    }
                    //Directory.Delete(path);
                }

                //Save the uploaded Excel file.
                string fileName = Path.GetFileName(postedFile.FileName);
                string filePath = Path.Combine(path, fileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                //Read the connection string for the Excel file.
                //string conString = this.Configuration.GetConnectionString("ExcelConString");
                string conString = "";
                string extension = Path.GetExtension(postedFile.FileName);
                DataTable dt = new DataTable();
                if (extension == ".xls" || extension == ".xlsx")
                {
                    switch (extension)
                    {
                        case ".xls": //Excel 97-03.
                            conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                            break;

                    }

                    conString = string.Format(conString, filePath);
                    try
                    {
                        using (OleDbConnection connExcel = new OleDbConnection(conString))
                        {
                            using (OleDbCommand cmdExcel = new OleDbCommand())
                            {
                                using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                                {
                                    cmdExcel.Connection = connExcel;

                                    //Get the name of First Sheet.
                                    connExcel.Open();
                                    DataTable dtExcelSchema;
                                    dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                    string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                    connExcel.Close();

                                    //Read Data from First Sheet.
                                    connExcel.Open();
                                    cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                    odaExcel.SelectCommand = cmdExcel;
                                    odaExcel.Fill(dt);
                                    connExcel.Close();

                                    if (objM.IsNoHeader == true)
                                    {
                                        DataRow dr = dt.NewRow();
                                        for (int i = 0; i < dt.Columns.Count; i++)
                                        {

                                            dr[i] = dt.Columns[i].ToString();
                                        }
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('" + ex.Message + "');</script>");
                    }
                }
                else
                {
                    try
                    {


                        int Count = 0;
                        string csvData = System.IO.File.ReadAllText(filePath);

                        bool firstRow = true;
                        foreach (string row in csvData.Split('\n'))
                        {
                            if (!string.IsNullOrEmpty(row))
                            {
                                if (!string.IsNullOrEmpty(row))
                                {
                                    if (firstRow)
                                    {
                                        foreach (string cell in row.Split(','))
                                        {
                                            dt.Columns.Add(Regex.Replace(cell.Trim(), @"\s+", ""));
                                            Count += 1;
                                        }
                                        firstRow = false;

                                    }
                                    else
                                    {
                                        dt.Rows.Add();
                                        int i = 0;
                                        foreach (string cell in row.Split(','))
                                        {
                                            dt.Rows[dt.Rows.Count - 1][i] = cell.Trim();
                                            i++;
                                        }
                                    }
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
                //***--Delete Empty row from Datatable --***//
                dt = dt.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(f => f is DBNull)).CopyToDataTable();
                try
                {

                    int Count = 1;
                    //  SqlBulkCopy oCopy = new SqlBulkCopy(ConfigurationManager.AppSettings["Myconnection"].ToString(), SqlBulkCopyOptions.KeepNulls);
                    SqlBulkCopy oCopy = new SqlBulkCopy(this.Configuration.GetConnectionString("Myconnection"), SqlBulkCopyOptions.KeepNulls);
                    try
                    {
                        oCopy.BatchSize = 500;
                        oCopy.BulkCopyTimeout = 0;
                        oCopy.DestinationTableName = "PITBulk";
                        //for (int i = 0; i < dt.Columns.Count; i++)
                        //{
                        oCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("ChartsID", "ChartsID"));
                        oCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("MedicaidID", "MedicaidID"));
                        oCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("PITName", "PITName"));
                        oCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("PIT", "PITStatus"));
                        oCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("EffectiveDate", "PITEffective"));
                        oCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Comments", "PITNotes"));
                        oCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping("Comments2", "PITNotes1"));
                        //}
                        oCopy.WriteToServer(dt);
                    }
                    catch (Exception ex)
                    {
                        oCopy.Close();
                        oCopy = null;
                    }
                    finally
                    {
                        oCopy.Close();
                        oCopy = null;
                    }

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "SP_PITFileUpload";
                    string strConn = _context.Database.GetDbConnection().ConnectionString;
                    SqlConnection Con = new SqlConnection(strConn);
                    cmd.Connection = Con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    Con.Open();
                    cmd.ExecuteNonQuery();
                    Con.Close();
                    //int id = _context.Database.ExecuteSqlCommand("SP_PITFileUpload");
                    //foreach (DataRow dr in dt.Rows)
                    //{

                    //    MemberMaster obj1 = new MemberMaster();

                    //    string Medicaid = (dr.ItemArray != null ? dr.ItemArray[4].ToString().Trim() : null);
                    //    string ResidenceId = (dr.ItemArray != null ? dr.ItemArray[0].ToString().Trim() : null);

                    //    obj1 = _context.MemberMaster.AsNoTracking().Where(s => s.ResidentID.Trim() == ResidenceId && s.MembershipStatus != 99).AsNoTracking().FirstOrDefault();
                    //    if (obj1 != null)
                    //    {
                    //        PIT p = new PIT();
                    //        int mID = obj1.MemberID;
                    //        p = _context.PIT.AsNoTracking().Where(s => s.MemberID == mID).AsNoTracking().FirstOrDefault();

                    //        if (p != null)
                    //        {
                    //            // p.MemberID = mID;
                    //            p.PITName = (dr.ItemArray != null ? dr.ItemArray[6].ToString().Trim() : null);
                    //            p.PITStatus = (dr.ItemArray != null ? dr.ItemArray[5].ToString().Trim() : null);
                    //            //if (p.PITName.Trim() == "PIT PENDING" || p.PITStatus.Trim() == "PIT PENDING") { p.PITStatus = "PENDING"; }
                    //            //else if (p.PITStatus.Trim() == "PIT" || p.PITStatus.Trim() == "APPROVED") { p.PITStatus = "APPROVED"; }
                    //            p.PITEffective = (dr.ItemArray != null ? dr.ItemArray[7].ToString().Trim() : null);
                    //            p.PITNotes = (dr.ItemArray != null ? dr.ItemArray[14].ToString().Trim() : null);

                    //            if (p.PITStatus.Trim() != "NO PIT")
                    //            {
                    //                _context.PIT.Update(p);
                    //                _context.SaveChanges();
                    //                _context.Entry(p).State = EntityState.Detached;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            p = new PIT();
                    //            p.MemberID = Convert.ToInt32(mID);
                    //            p.PITName = (dr.ItemArray != null ? dr.ItemArray[6].ToString().Trim() : null);
                    //            p.PITStatus = (dr.ItemArray != null ? dr.ItemArray[5].ToString().Trim() : null);
                    //            //if (p.PITName.Trim() == "PIT PENDING" || p.PITStatus.Trim() == "PIT PENDING") { p.PITStatus = "PENDING"; }
                    //            //else if (p.PITStatus.Trim() == "PIT"|| p.PITStatus.Trim() == "APPROVED") { p.PITStatus = "APPROVED"; }
                    //            p.PITEffective = (dr.ItemArray != null ? dr.ItemArray[7].ToString().Trim() : null);
                    //            string Notes = "";
                    //            Notes += (dr.ItemArray != null ? dr.ItemArray[14].ToString().Trim() : null);
                    //            Notes += (dr.ItemArray != null ? dr.ItemArray[15].ToString().Trim() : null);
                    //            p.PITNotes = Notes;

                    //            if (p.PITStatus.Trim() != "NO PIT")
                    //            {
                    //                _context.PIT.Add(p);
                    //                _context.SaveChanges();
                    //                _context.Entry(p).State = EntityState.Detached;
                    //            }
                    //        }
                    //    }
                    //    Count += 1;
                    //}
                }
                catch (Exception ex)
                {

                    throw;
                }
                return RedirectToAction("Index", "Dashboard");

            }
            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult UploadAddMemberContacts()
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
        //public static void WriteErrorLogs_Master(string Message)
        //{
        //    try
        //    {
        //        string Date = DateTime.Today.Year + "-" + DateTime.Today.Month + "-" + DateTime.Today.Day;
        //        string logPath = "~/Log/";
        //        StreamWriter sw = File.AppendText(HttpContext.Current.Server.MapPath(logPath) + Date + "-Master-Log.txt");
        //        sw.WriteLine(DateTime.Now + "  " + Message);
        //        sw.Close();
        //        sw.Dispose();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        [HttpPost]
        public async Task<IActionResult> UploadAddMemberContacts(IFormFile postedFile)
        {
            string JSONresult = "Done";

            if (postedFile != null)
            {
                //Create a Folder.
                string path = Path.Combine(this.Environment.WebRootPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                else
                {
                    string[] Files = Directory.GetFiles(path);
                    foreach (string file in Files)
                    {
                        System.IO.File.Delete(file);
                        //Directory.Delete(path);
                    }
                    //Directory.Delete(path);
                }

                //Save the uploaded Excel file.
                string fileName = Path.GetFileName(postedFile.FileName);
                string filePath = Path.Combine(path, fileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                //Read the connection string for the Excel file.
                //string conString = this.Configuration.GetConnectionString("ExcelConString");
                string conString = "";
                string extension = Path.GetExtension(postedFile.FileName);
                DataTable dt = new DataTable();
                if (extension == ".xls" || extension == ".xlsx")
                {
                    switch (extension)
                    {
                        case ".xls": //Excel 97-03.
                            conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                            break;

                    }

                    conString = string.Format(conString, filePath);
                    try
                    {
                        using (OleDbConnection connExcel = new OleDbConnection(conString))
                        {
                            using (OleDbCommand cmdExcel = new OleDbCommand())
                            {
                                using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                                {
                                    cmdExcel.Connection = connExcel;

                                    //Get the name of First Sheet.
                                    connExcel.Open();
                                    DataTable dtExcelSchema;
                                    dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                    string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                    connExcel.Close();

                                    //Read Data from First Sheet.
                                    connExcel.Open();
                                    cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                    odaExcel.SelectCommand = cmdExcel;
                                    odaExcel.Fill(dt);
                                    connExcel.Close();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('" + ex.Message + "');</script>");
                    }
                }
                else
                {
                    string csvData = System.IO.File.ReadAllText(filePath);
                    bool firstRow = true;
                    foreach (string row in csvData.Split('\n'))
                    {
                        if (!string.IsNullOrEmpty(row))
                        {
                            if (!string.IsNullOrEmpty(row))
                            {
                                if (firstRow)
                                {
                                    foreach (string cell in row.Split(','))
                                    {
                                        dt.Columns.Add(Regex.Replace(cell.Trim(), @"\s+", ""));
                                    }
                                    firstRow = false;
                                }
                                else
                                {
                                    dt.Rows.Add();
                                    int i = 0;
                                    foreach (string cell in row.Split(','))
                                    {
                                        dt.Rows[dt.Rows.Count - 1][i] = cell.Trim();
                                        i++;
                                    }
                                }
                            }
                        }
                    }
                }
                //***--Delete Empty row from Datatable --***//
                dt = dt.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(f => f is DBNull)).CopyToDataTable();

                foreach (DataRow dr in dt.Rows)
                {

                    MemberMaster obj1 = new MemberMaster();
                    string Medicaid = (dt.Columns.Contains("Medicaid#") ? dr["Medicaid#"].ToString() : null);

                    obj1 = _context.MemberMaster.AsNoTracking().Where(s => s.MedicaidID == Medicaid && s.MembershipStatus != 99 && s.DisenrolmentDate.ToString() == null).AsNoTracking().FirstOrDefault();
                    if (obj1 != null)
                    {
                        Contacts C = new Contacts();
                        int mID = obj1.MemberID;


                        C.MemberID = mID;
                        C.Name = (dt.Columns.Contains("Name") ? dr["Name"].ToString() : null);

                        _context.Contacts.Add(C);
                        _context.SaveChanges();

                    }
                }

                return RedirectToAction("Index", "Dashboard");

            }
            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult UpdateMedicalId()
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
        public async Task<IActionResult> UpdateMedicalId(IFormFile postedFile)
        {
            string JSONresult = "Done";

            if (postedFile != null)
            {
                //Create a Folder.
                string path = Path.Combine(this.Environment.WebRootPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                else
                {
                    string[] Files = Directory.GetFiles(path);
                    foreach (string file in Files)
                    {
                        System.IO.File.Delete(file);
                        //Directory.Delete(path);
                    }
                    //Directory.Delete(path);
                }

                //Save the uploaded Excel file.
                string fileName = Path.GetFileName(postedFile.FileName);
                string filePath = Path.Combine(path, fileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                //Read the connection string for the Excel file.
                //string conString = this.Configuration.GetConnectionString("ExcelConString");
                string conString = "";
                string extension = Path.GetExtension(postedFile.FileName);
                DataTable dt = new DataTable();
                if (extension == ".xls" || extension == ".xlsx")
                {
                    switch (extension)
                    {
                        case ".xls": //Excel 97-03.
                            conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                            break;

                    }

                    conString = string.Format(conString, filePath);
                    try
                    {
                        using (OleDbConnection connExcel = new OleDbConnection(conString))
                        {
                            using (OleDbCommand cmdExcel = new OleDbCommand())
                            {
                                using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                                {
                                    cmdExcel.Connection = connExcel;

                                    //Get the name of First Sheet.
                                    connExcel.Open();
                                    DataTable dtExcelSchema;
                                    dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                    string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                    connExcel.Close();

                                    //Read Data from First Sheet.
                                    connExcel.Open();
                                    cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                    odaExcel.SelectCommand = cmdExcel;
                                    odaExcel.Fill(dt);
                                    connExcel.Close();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('" + ex.Message + "');</script>");
                    }
                }
                dt = dt.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(f => f is DBNull)).CopyToDataTable();
                string ChartsId = null;
                foreach (DataRow dr in dt.Rows)
                {
                    MemberMaster obj1 = new MemberMaster();
                    string Medicaid = (dt.Columns.Contains("OldMedicalID") ? dr["OldMedicalID"].ToString() : null);
                    if (Medicaid != null)
                    {
                        obj1 = _context.MemberMaster.AsNoTracking().Where(s => s.MedicaidID == Medicaid && s.MembershipStatus != 99 && s.DisenrolmentDate.ToString() == null).AsNoTracking().FirstOrDefault();
                        if (obj1 != null)
                        {
                            int mID = obj1.MemberID;
                            obj1.MedicaidID = (dt.Columns.Contains("Medicaid No") ? dr["Medicaid No"].ToString() : obj1.MedicaidID);

                            _context.Update(obj1);
                            _context.SaveChanges();
                            _context.Entry(obj1).State = EntityState.Detached;

                        }
                    }
                    else
                    {
                        ChartsId = (dt.Columns.Contains("Charts ID") ? dr["Charts ID"].ToString() : null);
                        if (ChartsId != null)
                        {
                            obj1 = _context.MemberMaster.AsNoTracking().Where(s => (s.ChartsID == ChartsId || s.ResidentID == ChartsId) && s.MembershipStatus != 99 && s.DisenrolmentDate.ToString() == null).AsNoTracking().FirstOrDefault();
                            if (obj1 != null)
                            {
                                int mID = obj1.MemberID;
                                obj1.MedicaidID = (dt.Columns.Contains("Medicaid No") ? dr["Medicaid No"].ToString().Trim() : obj1.MedicaidID);

                                _context.Update(obj1);
                                _context.SaveChanges();
                                _context.Entry(obj1).State = EntityState.Detached;

                            }
                        }
                        else
                        {
                            string FirstName = dt.Columns.Contains("First Name") ? dr["First Name"].ToString() : null;
                            string LastName = dt.Columns.Contains("Last Name") ? dr["Last Name"].ToString() : null;
                            if (FirstName != null || LastName != null)
                            {
                                obj1 = _context.MemberMaster.AsNoTracking().Where(s => s.FirstName == FirstName && s.LastName == LastName && s.MembershipStatus != 99 && s.DisenrolmentDate.ToString() == null).AsNoTracking().FirstOrDefault();
                                if (obj1 != null)
                                {
                                    int mID = obj1.MemberID;
                                    obj1.MedicaidID = (dt.Columns.Contains("Medicaid No") ? dr["Medicaid No"].ToString() : obj1.MedicaidID);

                                    _context.Update(obj1);
                                    _context.SaveChanges();
                                    _context.Entry(obj1).State = EntityState.Detached;

                                }
                            }
                        }
                    }
                }

                return RedirectToAction("Index", "Dashboard");

            }
            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult UploadAddMemberSpousal()
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
            var FacilityType = new List<SelectListItem>
                    {
                new SelectListItem{ Text="Select", Value = "" , Selected = true },
                        new SelectListItem{ Text="MLTC", Value = "MLTC"},
                        new SelectListItem{ Text="DSNP", Value = "DSNP" },
                        new SelectListItem{ Text="MAP", Value = "MAP" },
                        //new SelectListItem{ Text="Centers", Value = "Centers" },
                        //new SelectListItem{ Text="Centers FIDA", Value = "Centers FIDA" },
                        //new SelectListItem{ Text="Centers MAP", Value = "Centers MAP" },
                        //new SelectListItem{ Text="Centers Surplus", Value = "Centers Surplus" },
                        //new SelectListItem{ Text="Centers DSNP", Value = "Centers DSNP" },
                    };
            ViewBag.FacilityType = FacilityType.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadAddMemberSpousal(IFormFile postedFile, MemberMasterModel objM)
        {
            string JSONresult = "Done";

            if (postedFile != null)
            {
                //Create a Folder.
                string path = Path.Combine(this.Environment.WebRootPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                else
                {
                    string[] Files = Directory.GetFiles(path);
                    foreach (string file in Files)
                    {
                        System.IO.File.Delete(file);
                        //Directory.Delete(path);
                    }
                    //Directory.Delete(path);
                }

                //Save the uploaded Excel file.
                string fileName = Path.GetFileName(postedFile.FileName);
                string filePath = Path.Combine(path, fileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                //Read the connection string for the Excel file.
                //string conString = this.Configuration.GetConnectionString("ExcelConString");
                string conString = "";
                string extension = Path.GetExtension(postedFile.FileName);
                DataTable dt = new DataTable();
                if (extension == ".xls" || extension == ".xlsx")
                {
                    switch (extension)
                    {
                        case ".xls": //Excel 97-03.
                            conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                            break;
                        case ".xlsx": //Excel 07 and above.
                            conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                            break;

                    }

                    conString = string.Format(conString, filePath);
                    try
                    {
                        using (OleDbConnection connExcel = new OleDbConnection(conString))
                        {
                            using (OleDbCommand cmdExcel = new OleDbCommand())
                            {
                                using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                                {
                                    cmdExcel.Connection = connExcel;

                                    //Get the name of First Sheet.
                                    connExcel.Open();
                                    DataTable dtExcelSchema;
                                    dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                    string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                    connExcel.Close();

                                    //Read Data from First Sheet.
                                    connExcel.Open();
                                    cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                    odaExcel.SelectCommand = cmdExcel;
                                    odaExcel.Fill(dt);
                                    connExcel.Close();

                                    if (objM.IsNoHeader == true)
                                    {
                                        DataRow dr = dt.NewRow();
                                        for (int i = 0; i < dt.Columns.Count; i++)
                                        {

                                            dr[i] = dt.Columns[i].ToString();
                                        }
                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('" + ex.Message + "');</script>");
                    }
                }
                else
                {
                    string csvData = System.IO.File.ReadAllText(filePath);
                    bool firstRow = true;
                    foreach (string row in csvData.Split('\n'))
                    {
                        if (!string.IsNullOrEmpty(row))
                        {
                            if (!string.IsNullOrEmpty(row))
                            {
                                if (firstRow)
                                {
                                    foreach (string cell in row.Split(','))
                                    {
                                        dt.Columns.Add(Regex.Replace(cell.Trim(), @"\s+", ""));
                                    }
                                    firstRow = false;
                                }
                                else
                                {
                                    dt.Rows.Add();
                                    int i = 0;
                                    foreach (string cell in row.Split(','))
                                    {
                                        dt.Rows[dt.Rows.Count - 1][i] = cell.Trim();
                                        i++;
                                    }
                                }
                            }
                        }
                    }
                }
                //***--Delete Empty row from Datatable --***//
                dt = dt.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(f => f is DBNull)).CopyToDataTable();

                foreach (DataRow dr in dt.Rows)
                {

                    List<MemberMaster> obj1 = new List<MemberMaster>();
                    string Medicaid = (dr.ItemArray != null ? dr.ItemArray[3].ToString().Trim() : null);
                    string MedicaidSpousal = (dr.ItemArray != null ? dr.ItemArray[6].ToString().Trim() : null);

                    obj1 = _context.MemberMaster.AsNoTracking().Where(s => s.MedicaidID == Medicaid && s.Facility == objM.Facility && s.MembershipStatus != 99 && s.DisenrolmentDate.ToString() == null).AsNoTracking().ToList();
                    var SposalData = _context.MemberMaster.AsNoTracking().Where(s => s.MedicaidID == MedicaidSpousal && s.Facility == objM.Facility && s.MembershipStatus != 99 && s.DisenrolmentDate.ToString() == null).AsNoTracking().FirstOrDefault();
                    if (obj1 != null)
                    {
                        foreach (var x in obj1)
                        {
                            Spousal C = new Spousal();
                            int mID = x.MemberID;
                            var Name = (x.FirstName != null ? x.FirstName : null) + (x.LastName != null ? (" " + x.LastName) : null);
                            C.MemberID = mID;
                            C.Name = Name;
                            C.CaseName = (SposalData != null ? SposalData.FirstName.ToString().Trim() : null) + (SposalData != null ? (SposalData.LastName != null ? (" " + SposalData.LastName) : null) : null);
                            C.MA = (dr.ItemArray[6].ToString() != null ? dr.ItemArray[6].ToString().Trim() : null);
                            C.SpousalMemberId = (SposalData != null ? (int?)SposalData.MemberID : null);
                            _context.Spousal.Add(C);
                            _context.SaveChanges();
                            C = null;


                            if (SposalData != null)
                            {
                                C = new Spousal();
                                C.MemberID = SposalData.MemberID;
                                C.Name = (SposalData != null ? SposalData.FirstName.ToString().Trim() : null) + (SposalData != null ? (SposalData.LastName != null ? (" " + SposalData.LastName) : null) : null);
                                C.CaseName = Name;
                                C.MA = x.MedicaidID;
                                C.SpousalMemberId = mID;
                                _context.Spousal.Add(C);
                                _context.SaveChanges();
                            }
                        }


                    }
                }

                return RedirectToAction("Index", "Dashboard");

            }
            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult Followup(int Id)
        {
            RecertificationFollowUp obj = new RecertificationFollowUp();
            obj.MemberId = Id;
            obj.recertificLists = new List<RecertificationFollowUpList>();
            var data = _context.Recertification_Follow_Up.Where(a => a.MemberId == Id).OrderByDescending(a => a.FollowUpID).ToList();
            foreach (var dr in data.ToList())
            {
                RecertificationFollowUpList objd = new RecertificationFollowUpList();
                objd.MemberId = dr.MemberId;
                objd.CurrentStatus = dr.CurrentStatus;
                objd.Outcome = dr.Outcome;
                objd.NextStepDueNotes = dr.NextStepDueNotes;
                objd.NextStepTask = dr.NextStepTask;
                objd.Nextduedate = dr.Nextduedate.ToString("MM/dd/yyyy");
                objd.NewStatus = dr.NewStatus;
                obj.recertificLists.Add(objd);
            }
            return View(obj);
            //return RedirectToAction("ViewAsset", "AssetManagement", new { id = assetComment.AssetId });
        }

        [HttpPost]
        public async Task<IActionResult> Followup(RecertificationFollowUp obj)
        {
            Recertification_Follow_Up obj1 = new Recertification_Follow_Up();
            obj1.MemberId = obj.MemberId;
            obj1.CurrentStatus = obj.CurrentStatus;
            obj1.Outcome = obj.Outcome;
            obj1.Notes = obj.Notes;
            obj1.NextStepTask = obj.NextStepTask;
            obj1.NextStepDueNotes = obj.NextStepDueNotes;
            obj1.Nextduedate = Convert.ToDateTime(obj.Nextduedate);
            obj1.CreatedDate = DateTime.Now;
            obj1.AttemptCount = "";
            obj1.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            obj1.NewStatus = obj.NewStatus;
            _context.Add(obj1);
            await _context.SaveChangesAsync();
            return RedirectToAction("Followup", "Member", new { id = obj.MemberId });
            //return View(obj);
        }

        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult getRatioList(string StartDate, string ToDate, int? ShiftId, int? StoreId)
        //{
        //    List<OpenratingRatioList> List = new List<OpenratingRatioList>();
        //    //var vData = db.Database.SqlQuery<OpenratingRatioList>("SP_Get_Terminal_Trace_data @StartDate = {0},@EndDate={1},@ShiftId={2},@StoreId = {3}", StartDate, ToDate,ShiftId,StoreId).ToList();
        //    List.Add(new OpenratingRatioList { Department = "BAKERY", Sales = 0, TotalSalPercentage = 0, COgs = 0, SalPercentage = 0 });
        //    return PartialView("_OperatingRatioList", List);
        //}

        public IActionResult Workfollow(int memberId)
        {
            ViewBag.RelationshipList = new SelectList(_context.Relationships.ToList(), "RelationshipID", "Relationship");


            ViewBag.MonthList = (from RecertMonth e in Enum.GetValues(typeof(RecertMonth))
                                 select new SelectListItem
                                 {
                                     Value = Convert.ToString(e),
                                     Text = e.ToString()
                                 }).ToList();


            ViewBag.MonthList.Insert(0, new SelectListItem { Text = "Select", Value = "" });
            var FacilityType = new List<SelectListItem>
                    {
                        new SelectListItem{ Text="All", Value = "" , Selected = true },
                        new SelectListItem{ Text="MLTC", Value = "MLTC" },
                        new SelectListItem{ Text="DSNP", Value = "DSNP" },
                        new SelectListItem{ Text="MAP", Value = "MAP" },
                    };
            ViewBag.FacilityType = FacilityType.ToList();
            ViewBag.MembershipStatusList = (from MembershipStatus e in Enum.GetValues(typeof(MembershipStatus))
                                            select new SelectListItem
                                            {
                                                Value = Convert.ToString((int)e),
                                                Text = e.ToString().Replace("_", "-")
                                            }).ToList();
            ViewBag.MonthList = (from RecertMonth e in Enum.GetValues(typeof(RecertMonth))
                                 select new SelectListItem
                                 {
                                     Value = e.ToString(),
                                     Text = e.ToString()
                                 }).ToList();
            ViewBag.MonthList.Insert(0, new SelectListItem { Text = "Select", Value = "" });
            MemberMasterModel obj = new MemberMasterModel();
            var datas = _context.MemberMaster.AsNoTracking().Where(a => a.MemberID == memberId).ToList();
            
            foreach (var dt in datas.ToList())
            {
                //var dataf = _context.MemberMaster.Where(a => a.MedicaidID == dt.MedicaidID).OrderByDescending(a => a.MemberID).ToList();
                //obj.CurrentFacility = dataf.FirstOrDefault().Facility;
                if (String.IsNullOrEmpty(dt.CaseType))
                {
                    obj.value = "0";
                }
                else
                {
                    if (dt.MembershipStatus == 3 || dt.MembershipStatus == 4)
                    {
                        obj.value = "1";
                    }
                    else
                    {
                        obj.value = "0";
                    }
                }
                obj.CurrentFacility = dt.Facility;

                obj.MemberID = dt.MemberID;
                obj.FirstName = dt.FirstName;
                obj.LastName = dt.LastName;
                obj.Address = dt.Address;
                obj.PrimaryPhone = dt.PrimaryPhone;
                obj.Email = dt.Email;
                obj.Comment = dt.Comment;

                string languname = "";
                if (dt.Language != "")
                {

                    var langid = Convert.ToInt32(dt.Language);
                    obj.Language = dt.Language;
                    obj.LanguageId = langid;
                    languname = _context.LanguageMaster.Where(a => a.LanguageID == langid).Count() > 0 ? _context.LanguageMaster.Where(a => a.LanguageID == langid).FirstOrDefault().Language : "";
                    //languname = Enum.GetName(typeof(Language), Convert.ToInt32(dt.Language));
                }
                obj.CountyCode = dt.CountyCode;
                obj.Facility = dt.Facility;
                obj.WorkFlowStatus = dt.WorkFlowStatus != null ? dt.WorkFlowStatus.Value : 0;
                obj.CreatedBy = dt.CreatedBy;
                obj.CDate = dt.CreatedDate;
                obj.Phone = dt.Phone;
                obj.InternalNotes = dt.InternalNotes;
                obj.ExternalNotes = dt.ExternalNotes;
                obj.ChartsID = dt.ChartsID;
                obj.ResidentID = dt.ResidentID;
                obj.MedicaidID = dt.MedicaidID;
                obj.MemberID = dt.MemberID;
                if (dt.RecertMonth != "" && dt.RecertMonth != null)
                {
                    obj.RecertMonth = dt.RecertMonth != "0" ? Enum.GetName(typeof(RecertMonth), Convert.ToInt32(dt.RecertMonth)) : "";
                }

                obj.TempRecertMonth = dt.TempRecertMonth;
                obj.DOB = (dt.DateOfBirth == null ? "" : Convert.ToDateTime(dt.DateOfBirth).ToString("MM/dd/yyyy"));
                obj.DateOfBirth = dt.DateOfBirth;
                //obj.EnrollmentDa = (dt.EnrollmentDate == null ? "" : Convert.ToDateTime(dt.EnrollmentDate).ToString("MM/dd/yyyy"));
                //obj.DisenrolmentDa = (dt.DisenrolmentDate == null ? "" : Convert.ToDateTime(dt.DisenrolmentDate).ToString("MM/dd/yyyy"));
                obj.EnrollmentDate = dt.EnrollmentDate;
                obj.DisenrolmentDate = dt.DisenrolmentDate;
                obj.NewEnrollmentDate = dt.NewEnrollmentDate;
                obj.MembershipStatus = Convert.ToInt32(dt.MembershipStatus);
                //if (dt.MembershipStatus != null)
                //{
                //    obj.MembershipStatu = Enum.GetName(typeof(MembershipStatus), Convert.ToInt32(dt.MembershipStatus));
                //}
            }

            var CurStatusData = _context.CurrentStatusMaster.ToList();
            var FollwupStatusMaster = _context.FollowupStatusMaster.ToList();
            var UserList = _context.UserMaster.Select(s => new { s.UserID, s.UserName }).ToList();
            obj.Recertification_Follow_UpModels = _context.Recertification_Follow_Up.Where(a => a.MemberId == memberId && a.IsDeleted == false).OrderByDescending(a => a.CreatedDate).ToList().Select(s => new Recertification_Follow_UpModel
            {
                FollowUpID = s.FollowUpID,
                MemberId = s.MemberId,
                CurrentStatus = FollwupStatusMaster.Where(k => k.Id == Convert.ToInt32(s.CurrentStatus)).FirstOrDefault().Name.Replace("_", " "),
                Outcome = s.Outcome,
                AttemptCount = s.AttemptCount,
                Notes = s.Notes,
                NextStepTask = s.NextStepTask != "" ? (s.NextStepTask != "0" ? FollwupStatusMaster.Where(k => k.Id == Convert.ToInt32(s.NextStepTask)).FirstOrDefault().Name.Replace("_", " ") : "N/A") : "N/A",
                NextStepDueNotes = s.NextStepDueNotes,
                NewStatus = s.NewStatus != "" ? (s.NewStatus != "0" ? FollwupStatusMaster.Where(k => k.Id == Convert.ToInt32(s.NewStatus)).FirstOrDefault().Name.Replace("_", " ") : "N/A") : "N/A",

                CallOutcome = s.CallOutcome,
                CreatedDt = s.CreatedDate.ToString("MM/dd/yyyy"),
                CreatedBy = s.CreatedBy,
                CreatedByName = s.CreatedBy != 0 ? UserList.Where(k => k.UserID == s.CreatedBy).FirstOrDefault().UserName : "",
                Nextduedate = s.Nextduedate
            }).ToList();



            //obj.contactModels = _context.Contacts.Where(a => a.MemberID == memberId).ToList().Select(s => new ContactModel
            //{
            //    MemberID = s.MemberID,
            //    ContactID = s.ContactID,
            //    RelationShip = s.RelationShip,
            //    Address = s.Address,
            //    Name = s.Name,
            //    Email = s.Email,
            //    Phone = s.Phone
            //}).ToList();
            obj.SpousalModels = (from l in _context.Spousal
                                 join m in _context.MemberMaster
                                 on l.MemberID equals m.MemberID
                                 join s in _context.MemberMaster
                               on l.SpousalMemberId equals s.MemberID
                                 where l.MemberID == memberId

                                 select new
                                 {
                                     MemberID = l.MemberID,
                                     Name = l.Name,
                                     CaseName = l.CaseName,
                                     MA = l.MA,
                                     ResidentId = s.ResidentID
                                 }).ToList()
            .Select(s => new SpousalModel
            {
                MemberID = s.MemberID,
                Name = s.Name,
                CaseName = s.CaseName,
                MA = s.MA,
                ResidentId = s.ResidentId
            }).ToList();

            //obj.LogsModels = _context.Logs.Where(a => a.MemberID == memberId).OrderByDescending(a => a.CreatedDate).ToList().Select(s => new LogsModel
            //{
            //    MemberID = s.MemberID,
            //    ActionName = s.ActionName,
            //    CreatedDate = s.CreatedDate
            //}).ToList();


            //obj.PITUploadsModel = _context.PITUploads.Where(a => a.MemberID == memberId).ToList().Select(s => new PITUploadsModel
            //{
            //    MemberID = s.MemberID,
            //    ID = s.ID,
            //    FileName = s.FileName
            //}).ToList();

            obj.GeneralNotesModel = (from l in _context.GeneralNotes
                                     join m in _context.UserMaster
                                     on l.CreatedBy equals m.UserID
                                     where l.MemberId == memberId && l.IsDeleted == false
                                     select new
                                     {
                                         l.ID,
                                         l.MemberId,
                                         l.Type,
                                         l.Notes,
                                         l.CreatedDate,
                                         m.UserName
                                     }).OrderByDescending(a => a.CreatedDate).ToList()
                        .Select(a => new GeneralNotesModel
                        {
                            ID = a.ID,
                            MemberId = a.MemberId,
                            Type = a.Type,
                            Notes = a.Notes,
                            CreatedDat = a.CreatedDate.ToString("MM/dd/yyyy hh:mm tt"),
                            CreatedBys = a.UserName,
                        }).ToList();
            var DT = (from l in _context.GeneralNotes_Old
                      join m in _context.UserMaster
                      on l.CreatedBy equals m.UserID
                      where l.MemberId == memberId
                      select new
                      {
                          l.ID,
                          l.MemberId,
                          l.Type,
                          l.Notes,
                          l.CreatedDate,
                          m.UserName
                      }).OrderByDescending(a => a.CreatedDate).ToList();

            obj.GeneralNotesModelOld = (from l in _context.GeneralNotes_Old
                                        where l.MemberId == memberId
                                        select new
                                        {
                                            l.ID,
                                            l.MemberId,
                                            l.Type,
                                            l.Notes,
                                            l.CreatedDate

                                        }).OrderByDescending(a => a.CreatedDate).ToList()
                       .Select(a => new GeneralNotesModel
                       {
                           MemberId = a.MemberId,
                           Type = a.Type,
                           Notes = a.Notes,
                           CreatedDat = a.CreatedDate != null ? a.CreatedDate.Value.ToString("MM/dd/yyyy hh:mm tt") : "",
                       }).ToList();

            PITModel pmodel = new PITModel();
            pmodel.MemberID = memberId;
            var datapit = _context.PIT.Where(a => a.MemberID == memberId).FirstOrDefault();
            if (datapit != null)
            {
                pmodel.ID = datapit.ID;
                pmodel.PITName = datapit.PITName;
                pmodel.PITStatus = datapit.PITStatus;
                if (datapit.PITEffective != null && datapit.PITEffective.Trim() != "")
                {
                    pmodel.PITEffective = datapit.PITEffective != null ? Convert.ToDateTime(datapit.PITEffective).ToString("MM/dd/yyyy") : null;
                    pmodel.PITEffectiveDt = Convert.ToDateTime(datapit.PITEffective);
                }
                pmodel.PITNotes = datapit.PITNotes;
            }
            obj.pITModel = pmodel;
            ViewBag.LanguageList = new SelectList(_context.LanguageMaster.ToList(), "LanguageID", "Language", obj.Language);
            return View(obj);
            //return View(obj);
        }

        //[HttpPost]
        //public async Task<IActionResult> Workfollow(IList<IFormFile> postedFile, MemberMasterModel obj)
        //{
        //    PIT pmodel = new PIT();
        //    pmodel.ID = obj.pITModel.ID;
        //    pmodel.MemberID = obj.pITModel.MemberID;
        //    pmodel.PITName = obj.pITModel.PITName;
        //    pmodel.PITStatus = obj.pITModel.PITStatus;
        //    pmodel.PITEffective = obj.pITModel.PITEffective;
        //    pmodel.PITNotes = obj.pITModel.PITNotes;
        //    if (obj.pITModel.ID != null)
        //    {
        //        _context.Update(pmodel);
        //        await _context.SaveChangesAsync();
        //    }
        //    else
        //    {
        //        if (obj.pITModel.PITName != "")
        //        {
        //            _context.Add(pmodel);
        //            await _context.SaveChangesAsync();
        //        }
        //    }


        //    if (postedFile != null)
        //    {
        //        //Create a Folder.
        //        string path = Path.Combine(this.Environment.WebRootPath, "PITUploads");
        //        if (!Directory.Exists(path))
        //        {
        //            Directory.CreateDirectory(path);
        //        }

        //        foreach (IFormFile file in postedFile)
        //        {

        //            if (file.Length > 0)
        //            {
        //                PITUploads pfilemodel = new PITUploads();
        //                string extension = Path.GetExtension(file.FileName);
        //                string name = Path.GetFileNameWithoutExtension(file.FileName);
        //                string newname = name + Guid.NewGuid().ToString().Substring(0, 4) + extension;
        //                string filePath = Path.Combine(path, newname);
        //                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
        //                {
        //                    await file.CopyToAsync(fileStream);
        //                }
        //                pfilemodel.MemberID = obj.pITModel.MemberID;
        //                pfilemodel.FileName = newname;
        //                _context.PITUploads.Add(pfilemodel);
        //                await _context.SaveChangesAsync();

        //            }

        //        }
        //    }
        //    return RedirectToAction("Workfollow", "Member", new
        //    {
        //        memberId = obj.pITModel.MemberID
        //    });
        //}

        //[HttpPost]
        //public async Task<IActionResult> DeletePITFile(int Id)
        //{
        //    var data = await _context.PITUploads.FindAsync(Id);
        //    _context.PITUploads.Remove(data);
        //    await _context.SaveChangesAsync();
        //    return RedirectToActionPermanent("Workfollow");
        //}

        [HttpPost]
        public IActionResult LogFilter(int? MemberID, string Facility)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Login");
            }

            //MemberMaster model = _context.MemberMaster.Where(m => m.MemberID == MemberID).FirstOrDefault();
            //string medicid = model.MedicaidID;
            var data = (from l in _context.Logs
                        join m in _context.MemberMaster
                        on l.MemberID equals m.MemberID
                        where m.MemberID == MemberID && m.Facility == Facility && m.MembershipStatus != 99 && m.DisenrolmentDate.ToString() == null
                        orderby l.CreatedDate descending
                        select new
                        {
                            l.MemberID,
                            l.ActionName,
                            l.CreatedDate,
                        }).ToList()
                        .Select(a => new LogsModel
                        {
                            MemberID = (a.MemberID == null ? 0 : a.MemberID),
                            ActionName = a.ActionName == null ? "" : a.ActionName,
                            CreatedDt = (a.CreatedDate == null ? "" : a.CreatedDate.Value.ToString("MM/dd/yyyy"))
                        });
            var Dt = data.Count();
            return Json(data);
        }

        [HttpPost]
        public IActionResult LogFilterExcel(int? MemberID, string Facility)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Login");
            }

            MemberMaster model = _context.MemberMaster.Where(m => m.MemberID == MemberID).FirstOrDefault();
            string medicid = model.MedicaidID;
            var data = (from l in _context.Logs
                        join m in _context.MemberMaster
                        on l.MemberID equals m.MemberID
                        where m.MedicaidID == medicid && m.Facility == (Facility == null ? m.Facility : Facility) && m.MembershipStatus != 99 && m.DisenrolmentDate.ToString() == null
                        select new
                        {
                            l.MemberID,
                            l.ActionName,
                            l.CreatedDate,
                        }).ToList()
                        .Select(a => new LogsModel
                        {
                            MemberID = a.MemberID,
                            ActionName = a.ActionName,
                            CreatedDt = a.CreatedDate.Value.ToString("MM/dd/yyyy")
                        });
            DataTable table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(data), (typeof(DataTable)));
            var Emplist = JsonConvert.SerializeObject(table);
            DataTable dt11 = (DataTable)JsonConvert.DeserializeObject(Emplist, (typeof(DataTable)));
            dt11.TableName = "Log";
            FileContentResult robj;
            byte[] chends = null;
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt11);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    var bytesdata = File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Log.xlsx");
                    robj = bytesdata;
                    chends = stream.ToArray();
                }
            }

            //return Json(Convert.ToBase64String(chends));

            return Json(robj);
        }


        [HttpPost]
        public IActionResult WorkflowFilter(int? MemberID, string Facility)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Login");
            }

            MemberMaster model = _context.MemberMaster.Where(m => m.MemberID == MemberID).FirstOrDefault();
            string medicid = model.MedicaidID;
            var OutComeData = _context.FollowupStatusMaster.ToList();
            var data = (from l in _context.Recertification_Follow_Up
                        join m in _context.MemberMaster
                        on l.MemberId equals m.MemberID
                        join u in _context.UserMaster
                        on l.CreatedBy equals u.UserID
                        where m.MemberID == MemberID && m.Facility == (Facility == null ? m.Facility : Facility) && l.IsDeleted == false && m.MembershipStatus != 99
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
                            u.UserName,
                            l.NextStepDueNotes
                        }).ToList()
                        .Select(s => new Recertification_Follow_UpModel
                        {
                            FollowUpID = s.FollowUpID,
                            MemberId = s.MemberId,
                            CurrentStatus = OutComeData.Where(k => k.Id == Convert.ToInt32(s.CurrentStatus)).FirstOrDefault().Name.Replace("_", " "),
                            Outcome = s.Outcome,
                            Notes = s.Notes,
                            NextStepTask = s.NextStepTask != "" ? (s.NextStepTask != "0" ? OutComeData.Where(k => k.Id == Convert.ToInt32(s.NextStepTask)).FirstOrDefault().Name.Replace("_", " ") : "N/A") : "N/A",
                            NewStatus = s.NewStatus != "" ? (s.NewStatus != "0" ? OutComeData.Where(k => k.Id == Convert.ToInt32(s.NewStatus)).FirstOrDefault().Name.Replace("_", " ") : "N/A") : "N/A",
                            CreatedDt = s.CreatedDate.ToString("MM/dd/yyyy"),
                            CreatedByName = s.UserName,
                            NextStepDueNotes = s.NextStepDueNotes,
                            CreatedDate = s.CreatedDate,
                            Action = "<a class='btn btn-warning' onclick='DeleteFollowup(" + s.FollowUpID + "," + s.MemberId + ")'>Delete</a>"
                        }).OrderByDescending(a=>a.CreatedDate);
            var DataCount = data.Count();

            return Json(data);
        }

        [HttpGet]
        public IActionResult WorkflowFilterEXCEL(int? MemberID, string Facility)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Login");
            }

            MemberMaster model = _context.MemberMaster.Where(m => m.MemberID == MemberID).FirstOrDefault();
            string medicid = model.MedicaidID;
            var data = (from l in _context.Recertification_Follow_Up
                        join m in _context.MemberMaster
                        on l.MemberId equals m.MemberID
                        where m.MedicaidID == medicid && m.Facility == (Facility == null ? m.Facility : Facility) && m.MembershipStatus != 99 && m.DisenrolmentDate.ToString() == null
                        select new
                        {
                            l.MemberId,
                            Name = m.FirstName + " " + m.LastName,
                            l.FollowUpID,
                            l.CurrentStatus,
                            l.Outcome,
                            l.Notes,
                            l.NextStepTask,
                            l.NewStatus,
                            l.CreatedDate,
                            MedicaidID = m.MedicaidID
                        }).ToList()
                        .Select(s => new Recertification_Follow_UpModel_ExcelExport
                        {
                            FollowUpID = s.FollowUpID,
                            MemberId = s.MemberId,
                            CurrentStatus = Convert.ToInt32(s.CurrentStatus) > 0 ? Enum.GetName(typeof(FollowupStatus), Convert.ToInt32(s.CurrentStatus)).Replace("_", " ") : "",
                            Outcome = s.Outcome,
                            Notes = s.Notes,
                            NextStepTask = Convert.ToInt32(s.NextStepTask) > 0 ? Enum.GetName(typeof(FollowupStatus), Convert.ToInt32(s.NextStepTask)).Replace("_", " ") : "",
                            NewStatus = Convert.ToInt32(s.NewStatus) > 0 ? Enum.GetName(typeof(FollowupStatus), Convert.ToInt32(s.NewStatus)).Replace("_", " ") : "",
                            CreatedDt = s.CreatedDate.ToString("MM/dd/yyyy"),
                            Name = s.Name,
                            MedicaidId = s.MedicaidID
                        });

            var NewNotesData = (from l in _context.GeneralNotes
                                join m in _context.UserMaster
                                on l.CreatedBy equals m.UserID
                                join u in _context.MemberMaster
                        on l.MemberId equals u.MemberID
                                where l.MemberId == MemberID && l.IsDeleted == false
                                select new
                                {
                                    l.ID,
                                    l.MemberId,
                                    l.Type,
                                    l.Notes,
                                    l.CreatedDate,
                                    m.UserName,
                                    Name = u.FirstName + " " + u.LastName,
                                    MedicaidID = u.MedicaidID
                                }).OrderByDescending(a => a.ID).ToList()
                        .Select(a => new GeneralNotesExcelExport
                        {
                            MemberId = a.MemberId,
                            Type = a.Type,
                            Notes = a.Notes,
                            CreatedDat = a.CreatedDate.ToString("MM/dd/yyyy hh:mm tt"),
                            CreatedBys = a.UserName,
                            Name = a.Name,
                            MedicaidID = a.MedicaidID
                        }).ToList();

            var OldNotesData = (from l in _context.GeneralNotes_Old
                                    // join m in _context.UserMaster
                                    //on l.CreatedBy equals m.UserID
                                join u in _context.MemberMaster
                        on l.MemberId equals u.MemberID
                                where l.MemberId == MemberID
                                select new
                                {
                                    l.ID,
                                    l.MemberId,
                                    l.Type,
                                    l.Notes,
                                    l.CreatedDate,
                                    Name = u.FirstName + " " + u.LastName,
                                    MedicaidID = u.MedicaidID
                                }).OrderByDescending(a => a.ID).ToList()
                       .Select(a => new GeneralNotesExcelExport
                       {
                           MemberId = a.MemberId,
                           Type = a.Type,
                           Notes = a.Notes,
                           CreatedDat = a.CreatedDate != null ? a.CreatedDate.Value.ToString("MM/dd/yyyy hh:mm tt") : "",
                           CreatedBys = "",
                           Name = a.Name,
                           MedicaidID = a.MedicaidID
                       }).ToList();

            DataTable table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(data), (typeof(DataTable)));
            //Parallel.ForEach(table.Rows.Cast<DataRow>(), (row) =>
            //{
            //    string Createddate = row["CreatedDate"].ToString();
            //    if(Createddate.Contains("01/01/0001 00:00:00"))
            //    {
            //        row["CreatedDate"] = DBNull.Value;
            //    }
            //    string Nextduedate = row["Nextduedate"].ToString();
            //    if (Createddate.Contains("01/01/0001 00:00:00"))
            //    {
            //        row["Nextduedate"] = DBNull.Value;
            //    }
            //});

            DataTable tableNewNotes = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(NewNotesData), (typeof(DataTable)));
            //Parallel.ForEach(table1.Rows.Cast<DataRow>(), (row) =>
            //{
            //    string Createddate = row["CreatedDate"].ToString();
            //    if (Createddate.Contains("01/01/0001 00:00:00"))
            //    {
            //        row["CreatedDate"] = DBNull.Value;
            //    }
            //});
            var Followuplist = JsonConvert.SerializeObject(table);
            DataTable dtFollowUp = (DataTable)JsonConvert.DeserializeObject(Followuplist, (typeof(DataTable)));
            dtFollowUp.TableName = "FollowUp";

            var NewNotes = JsonConvert.SerializeObject(tableNewNotes);
            DataTable dtNewNotes = (DataTable)JsonConvert.DeserializeObject(NewNotes, (typeof(DataTable)));
            dtNewNotes.TableName = "NewNotes";

            var OldNotesTbl = JsonConvert.SerializeObject(OldNotesData);
            DataTable dtOldNotes = (DataTable)JsonConvert.DeserializeObject(OldNotesTbl, (typeof(DataTable)));
            dtOldNotes.TableName = "OldNotes";

            DataSet ds = new DataSet();
            ds.Tables.Add(dtFollowUp);
            ds.Tables.Add(dtNewNotes);
            ds.Tables.Add(dtOldNotes);

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


        [HttpPost]
        public IActionResult ContactList(int? MemberID)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var data = _context.Contacts.Where(a => a.MemberID == MemberID).OrderByDescending(a => a.ContactID).ToList().Select(s => new ContactModel
            {
                MemberID = s.MemberID,
                ContactID = s.ContactID,
                RelationShipstring = s.RelationShip != 0 ? _context.Relationships.Where(a => a.RelationshipID == s.RelationShip).FirstOrDefault().Relationship : "",
                Address = s.Address,
                Name = s.Name,
                Email = s.Email,
                Phone = s.Phone
            }).ToList();

            return Json(data);
        }

        public IActionResult FollowupS(int Memberid, int Id)
        {

            //ViewBag.NextStepTaskList = (from NextStepTask e in Enum.GetValues(typeof(NextStepTask))
            //                            select new SelectListItem
            //                            {
            //                                Value = Convert.ToString((int)e),
            //                                Text = e.ToString().Replace("_", "-")
            //                            }).ToList();
            //ViewBag.NewStatusList = (from NewStatus e in Enum.GetValues(typeof(NewStatus))
            //                         select new SelectListItem
            //                         {
            //                             Value = Convert.ToString((int)e),
            //                             Text = e.ToString().Replace("_", "-")
            //                         }).ToList();


            var PeriodOfTheDay = new List<SelectListItem>
                    {
                        new SelectListItem{ Text="Morning (8 AM to 11 AM)", Value = "Morning (8 AM to 11 AM)" , Selected = true },
                        new SelectListItem{ Text="Noon (11 AM to 2 PM)", Value = "Noon (11 AM to 2 PM)" },
                        new SelectListItem{ Text="Afternoon (2 PM to 5 PM)", Value = "Afternoon (2 PM to 5 PM)" },
                    };
            ViewBag.PeriodOfTheDay = PeriodOfTheDay.ToList();
            var CurrentStatusSelect = "";
            RecertificationFollowUp obj = new RecertificationFollowUp();

            var Memberstatus = _context.MemberMaster.Where(a => a.MemberID == Memberid && a.MembershipStatus != 99).ToList().FirstOrDefault();
            obj.MembershipStatus = Memberstatus.MembershipStatus;
            var data = _context.Recertification_Follow_Up.Where(s => s.MemberId == Memberid && s.IsDeleted == false).OrderByDescending(a => a.FollowUpID).FirstOrDefault();
            if (data != null)
            {
                obj.Nextduedate = DateTime.Now;

                //RecertificationFollowUpList objd = new RecertificationFollowUpList();
                if (data.NewStatus != "" && data.NewStatus != "0")
                {
                    string currunt_status = _context.FollowupStatusMaster.Where(s => s.Id == Convert.ToInt32(data.NewStatus)).FirstOrDefault().Name.Replace("_", " ");
                    CurrentStatusSelect = data.NewStatus;
                    //if (data.NewStatus == "2")//Follow_up
                    //{

                    //    var list = _context.OutcomeMaster.Where(a=>a.CurrentStatusId == Convert.ToInt32(data.NewStatus)).Select(u => new SelectListItem
                    //    {
                    //        Text = u.Name,
                    //        Value = u.Id.ToString()
                    //    }).ToList();
                    //    if (Memberstatus.MembershipStatus == 4)
                    //    {
                    //        obj.MembershipStatus = 4;
                    //        list.Clear();

                    //        list = new List<SelectListItem>
                    //        {
                    //            new SelectListItem{ Text="Wrong Number", Value = "Wrong Number"},
                    //            new SelectListItem{ Text="Left Voice Mail", Value = "Left Voice Mail"  },
                    //            new SelectListItem{ Text="FHS Mailed/ emailed Docs for App", Value = "FHS Mailed/ emailed Docs for App" },
                    //            new SelectListItem{ Text="New App submitted to Medicaid by FHS", Value = "New App submitted to Medicaid by FHS" },
                    //            new SelectListItem{ Text="New App sent to Medicaid by Member", Value = "New App sent to Medicaid by Member" },
                    //            new SelectListItem{ Text="Hospitalized", Value = "Hospitalized" },
                    //            new SelectListItem{ Text="OOSA (Out of Service Area)", Value = "OOSA (Out of Service Area)" },
                    //            new SelectListItem{ Text="Nursing Home Long Term", Value = "Nursing Home Long Term" },
                    //            new SelectListItem{ Text="Medicaid Approved", Value = "Medicaid Approved"  },
                    //            new SelectListItem{ Text="Medicaid Rejected", Value = "Medicaid Rejected" },
                    //            new SelectListItem{ Text="New App Deferred", Value = "New App Deferred" }
                    //        };
                    //    }
                    //    ViewBag.Outcome = list.ToList();
                    //}
                    //else
                    //{

                    //    var list = new List<SelectListItem>
                    //    {
                    //    new SelectListItem{ Text="N/A", Value = "NA"},

                    //    };
                    //    ViewBag.Outcome = list.ToList();
                    //    obj.Nextduedate = DateTime.Now.AddDays(1);
                    //}
                    if (obj.MembershipStatus != 4)//Noncovered
                    {
                        var list = _context.OutcomeMaster.Where(a => a.CurrentStatusId == Convert.ToInt32(data.NewStatus)).Select(u => new SelectListItem
                        {
                            Text = u.Name,
                            Value = u.Id.ToString()
                        }).OrderBy(o => o.Text).ToList();
                        ViewBag.Outcome = list.ToList();

                    }
                    else
                    {
                        //if (Convert.ToInt32(data.NewStatus) == 2)
                        //{
                        var list = _context.OutcomeMaster.Where(a => a.CurrentStatusId == 7).Select(u => new SelectListItem
                        {
                            Text = u.Name,
                            Value = u.Id.ToString()
                        }).OrderBy(o => o.Text).ToList();
                        ViewBag.Outcome = list.ToList();
                        //}
                        //else
                        //{
                        //    var list = _context.OutcomeMaster.Where(a => a.CurrentStatusId == Convert.ToInt32(data.NewStatus)).Select(u => new SelectListItem
                        //    {
                        //        Text = u.Name,
                        //        Value = u.Id.ToString()
                        //    }).OrderBy(o => o.Text).ToList();
                        //    ViewBag.Outcome = list.ToList();
                        //}
                    }
                    obj.MemberId = data.MemberId;
                    obj.CurrentStatus = currunt_status;
                }
                else
                {
                    if (obj.MembershipStatus != 4)//Noncovered
                    {
                        //var list = new List<SelectListItem> { new SelectListItem { Text = "N/A", Value = "NA" } };
                        var list = _context.OutcomeMaster.Where(a => a.CurrentStatusId == Convert.ToInt32(data.NewStatus != "0" ? data.NewStatus : "1")).Select(u => new SelectListItem
                        {
                            Text = u.Name,
                            Value = u.Id.ToString()
                        }).OrderBy(o => o.Text).ToList();
                        ViewBag.Outcome = list.ToList();
                    }
                    else
                    {
                        var list = _context.OutcomeMaster.Where(a => a.CurrentStatusId == 7).Select(u => new SelectListItem
                        {
                            Text = u.Name,
                            Value = u.Id.ToString()
                        }).OrderBy(o => o.Text).ToList();
                        ViewBag.Outcome = list.ToList();
                    }
                    obj.Nextduedate = DateTime.Now.AddDays(1);
                    obj.MemberId = data.MemberId;
                    obj.CurrentStatus = Enum.GetName(typeof(FollowupStatus), 1).Replace("_", " "); ;
                }
            }
            else
            {
                if (obj.MembershipStatus != 4)//Noncovered
                {
                    var list = _context.OutcomeMaster.Where(a => a.CurrentStatusId == 1).Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }).OrderBy(o => o.Text).ToList();
                    ViewBag.Outcome = list.ToList();
                }
                else
                {
                    var list = _context.OutcomeMaster.Where(a => a.CurrentStatusId == 7).Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }).OrderBy(o => o.Text).ToList();
                    ViewBag.Outcome = list.ToList();
                }
                obj.Nextduedate = DateTime.Now.AddDays(1);
                obj.CurrentStatusFlg = true;
            }
            var CurrentStatus = _context.CurrentStatusMaster.Where(s => s.Id != 7).Select(u => new SelectListItem
            {
                Text = u.Name.Replace("_", " "),
                Value = u.Id.ToString()
            }).ToList();
            //(from FollowupCurrentStatus e in Enum.GetValues(typeof(FollowupCurrentStatus))
            //                     select new SelectListItem
            //                     {
            //                         Value = Convert.ToString((int)e),
            //                         Text = e.ToString().Replace("_", "-")
            //                     }).ToList();

            ViewBag.CurrentStatus = new SelectList(CurrentStatus, "Value", "Text", CurrentStatusSelect);
            return View(obj);
            //return RedirectToAction("ViewAsset", "AssetManagement", new { id = assetComment.AssetId });
        }
        //public IActionResult getOutComeList(int id, int membershipSt)
        //{

        //    var list = new List<SelectListItem>
        //            {
        //                new SelectListItem{ Text="N/A", Value = "NA" },
        //            };
        //    if (id == 2 && membershipSt == 0)//Follow_up
        //    {
        //        list = new List<SelectListItem>
        //            {
        //            new SelectListItem{ Text="Initial Follow-up Call", Value = "Initial Follow-up Call" },
        //                new SelectListItem{ Text="Wrong Number", Value = "Wrong Number" },
        //                new SelectListItem{ Text="Left Voice Mail", Value = "Left Voice Mail"  },
        //                new SelectListItem{ Text="Called five times", Value = "Called five times"},
        //                new SelectListItem{ Text="FHS mailed/e-mailed recert to member", Value = "FHS mailed/e-mailed recert to member" },
        //                new SelectListItem{ Text="Not compliant", Value = "Not compliant"},
        //                new SelectListItem{ Text="Excess Resource – CPHL to advise member", Value = "Excess Resource – CPHL to advise member"},
        //                new SelectListItem{ Text="Recert sent to FHS", Value = "Recert sent to FHS"},
        //                new SelectListItem{ Text="Recert submitted to Medicaid by FHS", Value = "Recert submitted to Medicaid by FHS"},
        //                //new SelectListItem{ Text="Call hotline/ Access Submitted", Value = "Call hotline/ Access Submitted"},
        //                new SelectListItem{ Text="Call hotline/ Access Received", Value = "Call hotline/ Access Received"},
        //                new SelectListItem{ Text="Call hotline/ Access Renewed", Value = "Call hotline/ Access Renewed"},
        //                new SelectListItem{ Text="Call hotline/ Access Reprint not available", Value = "Call hotline/ Reprint not available"},
        //                new SelectListItem{ Text="Recert Packet sent to Medicaid by member", Value = "Recert Packet sent to Medicaid by member"},
        //                new SelectListItem{ Text="Hospitalized", Value = "Hospitalized"},
        //                new SelectListItem{ Text="OOSA (out of service area)", Value = "OOSA (out of service area)"},
        //                new SelectListItem{ Text="Nursing home Long Term", Value = "Nursing home Long Term"},
        //                new SelectListItem{ Text="Medicaid Approved", Value = "Medicaid Approved"  },
        //                new SelectListItem{ Text="Medicaid Rejected", Value = "Medicaid Rejected" },
        //                new SelectListItem{ Text="Recertification Deferred", Value = "Recertification Deferred"},
        //                 new SelectListItem{ Text="Spoke To Member", Value = "Spoke To Member"},
        //                };
        //        ViewBag.Outcome = list.ToList();
        //    }
        //    else if (id == 3) //Packet_in_Review
        //    {
        //        list = new List<SelectListItem>
        //            {
        //                new SelectListItem{ Text="Check If Packet Approved", Value = "Check If Packet Approved" },

        //            };
        //        ViewBag.Outcome = list.ToList();

        //    }
        //    else if (id == 2 && membershipSt == 4) //Packet_in_Review
        //    {
        //        list = new List<SelectListItem>
        //                    {
        //                        new SelectListItem{ Text="Wrong Number", Value = "Wrong Number"},
        //                        new SelectListItem{ Text="Left Voice Mail", Value = "Left Voice Mail"  },
        //                        new SelectListItem{ Text="FHS Mailed/ emailed Docs for App", Value = "FHS Mailed/ emailed Docs for App" },
        //                        new SelectListItem{ Text="New App submitted to Medicaid by FHS", Value = "New App submitted to Medicaid by FHS" },
        //                        new SelectListItem{ Text="New App sent to Medicaid by Member", Value = "New App sent to Medicaid by Member" },
        //                        new SelectListItem{ Text="Hospitalized", Value = "Hospitalized" },
        //                        new SelectListItem{ Text="OOSA (Out of Service Area)", Value = "OOSA (Out of Service Area)" },
        //                        new SelectListItem{ Text="Nursing Home Long Term", Value = "Nursing Home Long Term" },
        //                        new SelectListItem{ Text="Medicaid Approved", Value = "Medicaid Approved"  },
        //                        new SelectListItem{ Text="Medicaid Rejected", Value = "Medicaid Rejected" },
        //                        new SelectListItem{ Text="New App Deferred", Value = "New App Deferred" }
        //                    };
        //        ViewBag.Outcome = list.ToList();

        //    }
        //    //else if (data.NewStatus == "4")//Follow_up_with_Medicaid
        //    //{
        //    //    var list = new List<SelectListItem>
        //    //{

        //    //};
        //    //    ViewBag.Outcome = list.ToList();
        //    //    obj.NextStepTask = Enum.GetName(typeof(FollowupStatus), Convert.ToInt32(8)).Replace("_", " ");
        //    //    obj.NewStatus = Enum.GetName(typeof(FollowupStatus), Convert.ToInt32(16)).Replace("_", " ");
        //    //}

        //    else if (id == 5)//Requires_CPHL_Assistance
        //    {
        //        list = new List<SelectListItem>
        //            {
        //                new SelectListItem{ Text="Left Voice Mail", Value = "Left Voice Mail"},
        //                new SelectListItem{ Text="Not compliant", Value = "Not compliant"},
        //                new SelectListItem{ Text="Excess Resource – CPHL to advise member", Value = "Excess Resource – CPHL to advise member"},
        //            };
        //        ViewBag.Outcome = list.ToList();
        //    }

        //    else if (id == 6) //Follow_up_Through_Edits
        //    {
        //        list = new List<SelectListItem>
        //            {
        //                new SelectListItem{ Text="Rejected", Value = "Rejected"},
        //                new SelectListItem{ Text="Deferred", Value = "Deferred"},

        //            };
        //        ViewBag.Outcome = list.ToList();
        //    }
        //    else if (id == 22) //NonCovered
        //    {
        //        list = new List<SelectListItem>
        //            {
        //                new SelectListItem{ Text="Deceased", Value = "Deceased"},
        //                new SelectListItem{ Text="PA case", Value = "PA case"},
        //                new SelectListItem{ Text="SSI case", Value = "SSI case"},
        //                new SelectListItem{ Text="Relink", Value = "Relink"},
        //                new SelectListItem{ Text="OOSA", Value = "OOSA"},

        //            };
        //        ViewBag.Outcome = list.ToList();
        //    }
        //    else
        //    {
        //        //var list = new List<SelectListItem>();

        //        list = new List<SelectListItem>
        //            {
        //                new SelectListItem{ Text="N/A", Value = "NA"},

        //            };
        //        ViewBag.Outcome = list.ToList();
        //    }




        //    //obj.recertificLists.Add(objd);

        //    return Json(list);
        //}

        public IActionResult getOutComeList(int id, int membershipSt, int memberid)
        {


            var list = new List<SelectListItem>
                    {
                        new SelectListItem{ Text="N/A", Value = "NA" },
                    };

            if (id > 0)//Follow_up
            {
                var MemStatus = _context.MemberMaster.Where(s => s.MemberID == memberid).FirstOrDefault().MembershipStatus;
                if (MemStatus != 4)
                {
                    list = _context.OutcomeMaster.Where(a => a.CurrentStatusId == Convert.ToInt32(id)).Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }).OrderBy(o => o.Text).ToList();
                }
                else
                {
                    //if (id == 2)
                    //{
                    list = _context.OutcomeMaster.Where(a => a.CurrentStatusId == 7).Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }).OrderBy(o => o.Text).ToList();
                    //}
                    //else
                    //{
                    //    list = _context.OutcomeMaster.Where(a => a.CurrentStatusId == Convert.ToInt32(id)).Select(u => new SelectListItem
                    //    {
                    //        Text = u.Name,
                    //        Value = u.Id.ToString()
                    //    }).OrderBy(o => o.Text).ToList();

                    //}

                }
                ViewBag.Outcome = list.ToList();
            }

            else
            {
                //var list = new List<SelectListItem>();

                list = new List<SelectListItem>
                    {
                        new SelectListItem{ Text="N/A", Value = "NA"},

                    };
                ViewBag.Outcome = list.ToList();
            }



            //obj.recertificLists.Add(objd);

            return Json(list);
        }

        public IActionResult OutComeChange(int id)
        {
            try
            {
                var OutComeList = _context.OutcomeMaster.Where(s => s.Id == id).FirstOrDefault();
                return Json(OutComeList);
            }
            catch (Exception ex)
            {

                throw;
            }
            return Json(null);
        }

        [HttpPost]
        public async Task<IActionResult> FollowupS(RecertificationFollowUp obj)
        {

            //var statuslist = (from FollowupStatus e in Enum.GetValues(typeof(FollowupStatus))
            //                  select new SelectListItem
            //                  {
            //                      Value = Convert.ToString((int)e),
            //                      Text = e.ToString()
            //                  }).ToList();
            var statuslist = _context.FollowupStatusMaster.Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            var currnts = obj.CurrentStatus != null ? statuslist.Where(s => s.Text == obj.CurrentStatus.Replace(" ", "_")).FirstOrDefault() : null;
            if (obj.CurrentStatus.Any(char.IsDigit))
            {
                currnts = statuslist.Where(s => s.Value == obj.CurrentStatus).FirstOrDefault();
            }
            var newstep = obj.NextStepTask != null ? statuslist.Where(s => s.Text == obj.NextStepTask.Replace(" ", "_").Replace("-", "_")).FirstOrDefault() : null;
            var newstatus = obj.NewStatus != null ? statuslist.Where(s => s.Text == obj.NewStatus.Replace(" ", "_")).FirstOrDefault() : null;
            var OutCome = _context.OutcomeMaster.Where(s => s.Id == Convert.ToInt32(obj.Outcome)).Count() > 0 ? _context.OutcomeMaster.Where(s => s.Id == Convert.ToInt32(obj.Outcome)).FirstOrDefault().Name : "";

            Recertification_Follow_Up obj1 = new Recertification_Follow_Up();
            obj1.FollowUpID = obj.FollowUpID;
            obj1.MemberId = obj.MemberId;
            obj1.CurrentStatus = currnts != null ? currnts.Value : "0";
            obj1.Outcome = OutCome;
            obj1.Notes = obj.Notes;
            obj1.NextStepTask = newstep != null ? newstep.Value : "0";
            obj1.NextStepDueNotes = obj.NextStepDueNotes;
            obj1.Nextduedate = Convert.ToDateTime(obj.Nextduedate);
            obj1.CreatedDate = DateTime.Now;
            obj1.AttemptCount = "";
            obj1.PeriodOfTheDay = obj.PeriodOfTheDay;
            obj1.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            obj1.NewStatus = newstatus != null ? newstatus.Value : "0";

            Logs objlog = new Logs();
            objlog.CreatedDate = DateTime.Now;
            objlog.MemberID = obj.MemberId;

            //if (obj.FollowUpID > 0)
            //{
            //    _context.Update(obj1);
            //    await _context.SaveChangesAsync();

            //    objlog.ActionName = "Update followup";
            //    _context.Add(objlog);
            //    await _context.SaveChangesAsync();
            //}
            //else
            //{
            _context.Recertification_Follow_Up.Add(obj1);
            await _context.SaveChangesAsync();

            objlog.ActionName = "Add followup";
            _context.Add(objlog);
            await _context.SaveChangesAsync();

            if (obj.NewStatus == "Recertification_Approved")
            {
                var MemberMaster = _context.MemberMaster.Where(a => a.MemberID == obj1.MemberId).ToList().FirstOrDefault();
                MemberMaster.RecertificationProcess = false;
                _context.MemberMaster.Update(MemberMaster);
                _context.SaveChanges();
            }


            //}
            return RedirectToAction("Workfollow", "Member", new
            {
                memberId = obj.MemberId,
            });

            //return View(obj);
        }
        [HttpPost]
        public async Task<IActionResult> AddAutoFollowup(int? id)
        {
            Recertification_Follow_Up obj1 = new Recertification_Follow_Up();

            obj1.MemberId = id.Value;
            obj1.CurrentStatus = "1";
            obj1.Outcome = "NA";
            obj1.Notes = "";
            obj1.NextStepTask = "7";
            obj1.NextStepDueNotes = null;
            obj1.Nextduedate = DateTime.Today.AddDays(10);
            obj1.CreatedDate = DateTime.Now;
            obj1.AttemptCount = "";
            obj1.PeriodOfTheDay = "Morning (8 AM to 11 AM)";
            obj1.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            obj1.NewStatus = "2";

            Logs objlog = new Logs();
            objlog.CreatedDate = DateTime.Now;
            objlog.MemberID = id.Value;

            //if (obj.FollowUpID > 0)
            //{
            //    _context.Update(obj1);
            //    await _context.SaveChangesAsync();

            //    objlog.ActionName = "Update followup";
            //    _context.Add(objlog);
            //    await _context.SaveChangesAsync();
            //}
            //else
            //{
            _context.Recertification_Follow_Up.Add(obj1);
            await _context.SaveChangesAsync();

            objlog.ActionName = "Add followup";
            _context.Add(objlog);
            await _context.SaveChangesAsync();

            var MemberMaster = _context.MemberMaster.Where(a => a.MemberID == obj1.MemberId).ToList().FirstOrDefault();
            MemberMaster.Status = 3;
            MemberMaster.RecertificationProcess = true;
            _context.MemberMaster.Update(MemberMaster);
            _context.SaveChanges();
            //_context.Entry(obj1).State = EntityState.Detached;


            //pmodel.ID = id;
            //pmodel.MemberID = memberid;
            //pmodel.PITName = name;
            //pmodel.PITStatus = status;
            //pmodel.PITEffective = effective;
            //pmodel.PITNotes = notes;
            //if (id != null)
            //{
            //    _context.Update(pmodel);
            //    await _context.SaveChangesAsync();

            //}
            //else
            //{
            //    if (name != "")
            //    {
            //        _context.Add(pmodel);
            //        await _context.SaveChangesAsync();
            //    }
            //}

            return RedirectToAction("Workfollow", "Member", new
            {
                //memberId = pmodel.MemberID,
            });

            //return View(obj);
        }
        public IActionResult GeneralNotes(int Memberid)
        {
            GeneralNotesModel model = new GeneralNotesModel();
            model.MemberId = Memberid;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GeneralNotes(GeneralNotesModel obj)
        {

            GeneralNotes obj1 = new GeneralNotes();
            obj1.CreatedDate = DateTime.Now;
            obj1.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            obj1.MemberId = obj.MemberId;
            obj1.Type = obj.Type;
            obj1.Notes = obj.Notes;
            obj1.FollowUpDate = obj.FollowUpDate;

            _context.GeneralNotes.Add(obj1);
            await _context.SaveChangesAsync();

            return RedirectToAction("Workfollow", "Member", new
            {
                memberId = obj.MemberId,
            });

        }
        public IActionResult DeleteFollowup(int FollowUpID, int MemberId)
        {
            AuditTrail model = new AuditTrail();
            model.NoteId = FollowUpID;
            model.MemberId = MemberId;
            model.Type = "F";
            return View("AuditTrail", model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFollowup(AuditTrail obj)
        {

            AuditTrail obj1 = new AuditTrail();
            obj1.DeletedOn = DateTime.Now;
            obj1.DeleteBy = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            obj1.NoteId = obj.NoteId;
            obj1.Type = obj.Type;
            obj1.ReasonForDelete = obj.ReasonForDelete;
            obj1.MemberId = obj.MemberId;

            _context.AuditTrail.Add(obj1);
            await _context.SaveChangesAsync();
            if (obj.Type == "F")
            {
                var followup = _context.Recertification_Follow_Up.Find(obj.NoteId);
                if (followup != null)
                {
                    followup.IsDeleted = true;
                    _context.Update(followup);
                    await _context.SaveChangesAsync();
                }
            }
            else if (obj.Type == "N")
            {
                var followup = _context.GeneralNotes.Find(obj.NoteId);
                if (followup != null)
                {
                    followup.IsDeleted = true;
                    _context.Update(followup);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("Workfollow", "Member", new
            {
                memberId = obj.MemberId,
            });

        }

        public IActionResult DeleteGeneralNotes(int NoteId, int MemberId)
        {
            AuditTrail model = new AuditTrail();
            model.NoteId = NoteId;
            model.MemberId = MemberId;
            model.Type = "N";
            return View("AuditTrail", model);
        }



        [HttpPost]
        public async Task<IActionResult> SavePIT(int? id, int? memberid, string name, string status, string effective, string notes)
        {
            PIT pmodel = new PIT();
            pmodel.ID = id;
            pmodel.MemberID = memberid;
            pmodel.PITName = name;
            pmodel.PITStatus = status;
            pmodel.PITEffective = effective;
            pmodel.PITNotes = notes;
            if (id != null)
            {
                _context.Update(pmodel);
                await _context.SaveChangesAsync();

            }
            else
            {
                if (name != "")
                {
                    _context.Add(pmodel);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Workfollow", "Member", new
            {
                memberId = pmodel.MemberID,
            });

            //return View(obj);
        }


        [HttpPost]
        public async Task<IActionResult> SaveContacts(int Relationship, int memberid, string name, string phone, string email, string address)
        {
            Contacts model = new Contacts();
            model.MemberID = memberid;
            model.Name = name;
            model.Phone = phone;
            model.Email = email;
            model.Address = address;
            model.RelationShip = Relationship;


            if (name != "")
            {
                _context.Contacts.Add(model);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Workfollow", "Member", new
            {
                memberId = model.MemberID,
            });

            //return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> SaveTempRecerMonth(int? memberid, string month)
        {
            MemberMaster model = new MemberMaster();
            model = _context.MemberMaster.Where(s => s.MemberID == memberid).FirstOrDefault();
            if (model != null)
            {
                model.TempRecertMonth = month;

                _context.MemberMaster.Update(model);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction("Workfollow", "Member", new
            {
                memberId = model.MemberID,
            });

            //return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMemberData(int? memberid, string Email, string PrimaryPhone, string Language)
        {
            MemberMaster model = new MemberMaster();
            model = _context.MemberMaster.Where(s => s.MemberID == Convert.ToInt32(memberid)).FirstOrDefault();
            if (model != null)
            {
                model.PrimaryPhone = PrimaryPhone;
                model.Email = Email;
                model.Language = Language;
                _context.MemberMaster.Update(model);
                await _context.SaveChangesAsync();
            }

            //return RedirectToAction("Workfollow", "Member", new
            //{
            //    memberId = model.MemberID,
            //});
            return Json("success");
            //return View(obj);
        }


        [HttpPost]
        public async Task<IActionResult> SaveComment(int? memberid, string Comment)
        {
            MemberMaster model = new MemberMaster();
            model = _context.MemberMaster.Where(s => s.MemberID == memberid).FirstOrDefault();
            if (model != null)
            {
                model.Comment = Comment;

                _context.MemberMaster.Update(model);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction("Workfollow", "Member", new
            {
                memberId = model.MemberID,
            });

            //return View(obj);
        }


        public IActionResult Reports()
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
            var ReportTypes = new List<SelectListItem>
                    {
                        new SelectListItem{ Text="Non-Covered Members Activity", Value = "Non-Covered Members Activity" , Selected = true },
                        new SelectListItem{ Text="Recertification Activity", Value = "Recertification Activity" },
                    };
            ViewBag.ReportTypes = ReportTypes.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Reports(MemberMasterModel obj, string StartDate, string EndDate)
        {

            var ReportTypes = new List<SelectListItem>
                    {
                        new SelectListItem{ Text="Non-Covered Members Activity", Value = "Non-Covered Members Activity" , Selected = true },
                        new SelectListItem{ Text="Recertification Activity", Value = "Recertification Activity" },
                    };
            ViewBag.ReportTypes = ReportTypes.ToList();
            if (obj.ReportsType == "Non-Covered Members Activity")
            {
                string isdata = BindNonCoveredReport(StartDate, EndDate);

                //string path = Path.Combine(this.Environment.WebRootPath, "Reports");
                //path = Path.Combine(path, "NoncoverdActivity.pdf");
                var path = Path.Combine(
                          Directory.GetCurrentDirectory(),
                          "wwwroot", "Reports/NoncoverdActivity.pdf");
                if (isdata == "true")
                {
                    var memory = new MemoryStream();
                    using (var stream = new FileStream(path, FileMode.Open))
                    {
                        await stream.CopyToAsync(memory);
                    }
                    memory.Position = 0;
                    return File(memory, "application/pdf", Path.GetFileName(path));
                }
                else
                {

                }
                return View();
            }
            else
            {
                string isdata = BindRecertificationReport(StartDate, EndDate);

                //string path = Path.Combine(this.Environment.WebRootPath, "Reports");
                //path = Path.Combine(path, "NoncoverdActivity.pdf");
                var path = Path.Combine(
                          Directory.GetCurrentDirectory(),
                          "wwwroot", "Reports/RecertificationActivity.pdf");
                if (isdata == "true")
                {
                    var memory = new MemoryStream();
                    using (var stream = new FileStream(path, FileMode.Open))
                    {
                        await stream.CopyToAsync(memory);
                    }
                    memory.Position = 0;
                    return File(memory, "application/pdf", Path.GetFileName(path));
                }
                else
                {

                }
                return View();
            }

        }
        private string BindNonCoveredReport(string StartDate, string EndDate)
        {
            DateTime dtfrom = new DateTime();
            DateTime dtto = new DateTime();

            if (StartDate != null)
            {
                dtfrom = Convert.ToDateTime(StartDate);
            }
            if (EndDate != null)
            {
                dtto = Convert.ToDateTime(EndDate);
            }
            List<MemberMaster> ObjCheck = new List<MemberMaster>();

            int UserId = Convert.ToInt32(HttpContext.Session.GetString("UserTypeId"));

            if (StartDate != null || EndDate != null)
            {
                ObjCheck = _context.MemberMaster.AsNoTracking().Where(s => s.MembershipStatus == 4 && s.AssignId == (UserId == 1 ? s.AssignId : UserId) && s.CreatedDate >= Convert.ToDateTime(StartDate) && s.CreatedDate <= Convert.ToDateTime(EndDate)).AsNoTracking().ToList();
            }
            else
            {
                ObjCheck = _context.MemberMaster.AsNoTracking().Where(s => s.MembershipStatus == 4 && s.AssignId == (UserId == 1 ? s.AssignId : UserId)).AsNoTracking().ToList();
            }
            if (ObjCheck != null && ObjCheck.Count > 0)
            {

                DataTable dt = ToDataTable(ObjCheck);
                DataView dv = new DataView(dt);
                dt = dv.ToTable(true, "MemberID", "MedicaidID", "ChartsID", "FirstName", "LastName", "ExternalNotes");
                dt.AcceptChanges();
                System.Data.DataColumn newColumn = new System.Data.DataColumn("Status", typeof(System.String));
                newColumn.DefaultValue = "Non Covered";
                dt.Columns.Add(newColumn);
                dt.AcceptChanges();

                string path = Path.Combine(this.Environment.WebRootPath, "Reports");

                FileStream fs = new FileStream(path + "/NoncoverdActivity.pdf", FileMode.Create, FileAccess.Write, FileShare.None);
                Document doc = new Document();
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                //doc.Add(new Paragraph("Non-Covered Members Activity Report"));

                Paragraph para = new Paragraph("Non-Covered Members Activity Report", new Font(Font.FontFamily.HELVETICA, 22));
                para.Alignment = Element.ALIGN_CENTER;
                doc.Add(para);
                doc.Add(new Paragraph("\r\n"));

                PdfPTable PdfTable = new PdfPTable(6);
                PdfPCell PdfPCell = null;

                Font fonth = FontFactory.GetFont("ARIAL", 10);
                Font font8 = FontFactory.GetFont("ARIAL", 7);
                //Add Header of the pdf table
                PdfPCell = new PdfPCell(new Phrase(new Chunk("Medicaid ID", fonth))) { BackgroundColor = BaseColor.GRAY };
                PdfTable.AddCell(PdfPCell);
                PdfPCell = new PdfPCell(new Phrase(new Chunk("ID", fonth))) { BackgroundColor = BaseColor.GRAY };
                PdfTable.AddCell(PdfPCell);
                PdfPCell = new PdfPCell(new Phrase(new Chunk("First Name", fonth))) { BackgroundColor = BaseColor.GRAY };
                PdfTable.AddCell(PdfPCell);
                PdfPCell = new PdfPCell(new Phrase(new Chunk("Last Name", fonth))) { BackgroundColor = BaseColor.GRAY };
                PdfTable.AddCell(PdfPCell);
                PdfPCell = new PdfPCell(new Phrase(new Chunk("External Notes", fonth))) { BackgroundColor = BaseColor.GRAY };
                PdfTable.AddCell(PdfPCell);
                PdfPCell = new PdfPCell(new Phrase(new Chunk("Status", fonth))) { BackgroundColor = BaseColor.GRAY };
                PdfTable.AddCell(PdfPCell);

                //How add the data from datatable to pdf table
                for (int rows = 0; rows < dt.Rows.Count; rows++)
                {

                    for (int column = 1; column < dt.Columns.Count; column++)
                    {
                        PdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Rows[rows][column].ToString(), font8)));
                        PdfTable.AddCell(PdfPCell);
                    }


                    List<Logs> logactivity = new List<Logs>();
                    int memid = Convert.ToInt32(dt.Rows[rows]["MemberID"].ToString());
                    //logactivity = _context.Logs.ToList().Where(n=>n.MemberID==memid).ToList();
                    logactivity = _context.Logs.AsNoTracking().Where(s => s.MemberID == memid).AsNoTracking().ToList();
                    DataTable dtlog = ToDataTable(logactivity);
                    DataView dv1 = new DataView(dtlog);
                    dtlog = dv1.ToTable(true, "ActionName", "CreatedDate");
                    dtlog.AcceptChanges();

                    PdfPTable PdfTable1 = new PdfPTable(2);
                    PdfPCell PdfPCell1 = null;

                    PdfPCell1 = new PdfPCell(new Phrase(new Chunk("Activity", fonth))) { BackgroundColor = BaseColor.GRAY };
                    PdfTable1.AddCell(PdfPCell1);
                    PdfPCell1 = new PdfPCell(new Phrase(new Chunk("Date", fonth))) { BackgroundColor = BaseColor.GRAY };
                    PdfTable1.AddCell(PdfPCell1);
                    for (int i = 0; i < dtlog.Rows.Count; i++)
                    {
                        for (int col = 0; col < dtlog.Columns.Count; col++)
                        {
                            string data = dtlog.Rows[i][col].ToString();
                            if (col == 1) { data = dtlog.Rows[i][col].ToString() != "" ? Convert.ToDateTime(dtlog.Rows[i][col].ToString()).ToString("MM/dd/yyyy hh:mm tt") : ""; }
                            PdfPCell1 = new PdfPCell(new Phrase(new Chunk(data, font8)));
                            PdfTable1.AddCell(PdfPCell1);
                        }
                    }
                    PdfPCell PdfPCelld = new PdfPCell();
                    PdfPCelld.Colspan = 6;
                    PdfPCelld.AddElement(PdfTable1);
                    PdfTable.AddCell(PdfPCelld);
                }


                PdfTable.SpacingBefore = 15f; // Give some space after the text or it may overlap the table

                doc.Add(PdfTable); // add pdf table to the document

                doc.Close();

                return "true";
            }
            else
            {
                return "false";
            }

        }
        private string BindRecertificationReport(string StartDate, string EndDate)
        {
            List<MemberMaster> ObjCheck = new List<MemberMaster>();

            int UserId = Convert.ToInt32(HttpContext.Session.GetString("UserTypeId"));

            var Memberids = _context.Recertification_Follow_Up.Where(s => s.CurrentStatus == "1").Select(s => s.MemberId).ToList();
            if (StartDate != null || EndDate != null)
            {
                Memberids = _context.Recertification_Follow_Up.Where(s => s.CurrentStatus == "1" && s.CreatedDate >= Convert.ToDateTime(StartDate) && s.CreatedDate <= Convert.ToDateTime(EndDate)).Select(s => s.MemberId).ToList();
            }

            //s.CreatedDate >= Convert.ToDateTime(followupS) && s.CreatedDate <= Convert.ToDateTime(followupE)
            ObjCheck = _context.MemberMaster.AsNoTracking().Where(s => Memberids.Contains(s.MemberID) && s.AssignId == (UserId == 1 ? s.AssignId : UserId)).AsNoTracking().ToList();
            if (ObjCheck != null && ObjCheck.Count > 0)
            {

                DataTable dt = ToDataTable(ObjCheck);
                DataView dv = new DataView(dt);
                dt = dv.ToTable(true, "MemberID", "MedicaidID", "ChartsID", "FirstName", "LastName", "ExternalNotes");
                dt.AcceptChanges();
                System.Data.DataColumn newColumn = new System.Data.DataColumn("Status", typeof(System.String));
                newColumn.DefaultValue = "Recertification";
                dt.Columns.Add(newColumn);
                dt.AcceptChanges();

                string path = Path.Combine(this.Environment.WebRootPath, "Reports");

                FileStream fs = new FileStream(path + "/RecertificationActivity.pdf", FileMode.Create, FileAccess.Write, FileShare.None);
                Document doc = new Document();
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                //doc.Add(new Paragraph("Non-Covered Members Activity Report"));

                Paragraph para = new Paragraph("Recertification Members Activity Report", new Font(Font.FontFamily.HELVETICA, 22));
                para.Alignment = Element.ALIGN_CENTER;
                doc.Add(para);
                doc.Add(new Paragraph("\r\n"));

                PdfPTable PdfTable = new PdfPTable(6);
                PdfPCell PdfPCell = null;

                Font fonth = FontFactory.GetFont("ARIAL", 10);
                Font font8 = FontFactory.GetFont("ARIAL", 7);
                //Add Header of the pdf table
                PdfPCell = new PdfPCell(new Phrase(new Chunk("Medicaid ID", fonth))) { BackgroundColor = BaseColor.GRAY };
                PdfTable.AddCell(PdfPCell);
                PdfPCell = new PdfPCell(new Phrase(new Chunk("ID", fonth))) { BackgroundColor = BaseColor.GRAY };
                PdfTable.AddCell(PdfPCell);
                PdfPCell = new PdfPCell(new Phrase(new Chunk("First Name", fonth))) { BackgroundColor = BaseColor.GRAY };
                PdfTable.AddCell(PdfPCell);
                PdfPCell = new PdfPCell(new Phrase(new Chunk("Last Name", fonth))) { BackgroundColor = BaseColor.GRAY };
                PdfTable.AddCell(PdfPCell);

                PdfPCell = new PdfPCell(new Phrase(new Chunk("External Notes", fonth))) { BackgroundColor = BaseColor.GRAY };
                PdfTable.AddCell(PdfPCell);
                PdfPCell = new PdfPCell(new Phrase(new Chunk("Status", fonth))) { BackgroundColor = BaseColor.GRAY };
                PdfTable.AddCell(PdfPCell);

                //How add the data from datatable to pdf table
                for (int rows = 0; rows < dt.Rows.Count; rows++)
                {

                    for (int column = 1; column < dt.Columns.Count; column++)
                    {
                        PdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Rows[rows][column].ToString(), font8)));
                        PdfTable.AddCell(PdfPCell);
                    }


                    List<Logs> logactivity = new List<Logs>();

                    int memid = Convert.ToInt32(dt.Rows[rows]["MemberID"].ToString());
                    //logactivity = _context.Logs.ToList().Where(n=>n.MemberID==memid).ToList();
                    logactivity = _context.Logs.AsNoTracking().Where(s => s.MemberID == memid).AsNoTracking().ToList();
                    var oldNotesActivity = _context.GeneralNotes_Old.AsNoTracking().Where(s => s.MemberId == memid && s.Notes != "").Select(s => new { Notes = s.Notes, CreatedDate = s.CreatedDate }).AsNoTracking().ToList();
                    DataTable dtlog = ToDataTable(logactivity);
                    DataTable dtOldlog = ToDataTable(oldNotesActivity);
                    DataView dv1 = new DataView(dtlog);
                    dtlog = dv1.ToTable(true, "ActionName", "CreatedDate");
                    dtlog.AcceptChanges();

                    PdfPTable PdfTable1 = new PdfPTable(2);
                    PdfPCell PdfPCell1 = null;

                    PdfPCell1 = new PdfPCell(new Phrase(new Chunk("Activity", fonth))) { BackgroundColor = BaseColor.GRAY };
                    PdfTable1.AddCell(PdfPCell1);
                    PdfPCell1 = new PdfPCell(new Phrase(new Chunk("Date", fonth))) { BackgroundColor = BaseColor.GRAY };
                    PdfTable1.AddCell(PdfPCell1);
                    for (int i = 0; i < dtOldlog.Rows.Count; i++)
                    {
                        for (int col = 0; col < dtOldlog.Columns.Count; col++)
                        {
                            string data = dtOldlog.Rows[i][col].ToString();
                            if (col == 1) { data = dtOldlog.Rows[i][col].ToString() != "" ? Convert.ToDateTime(dtOldlog.Rows[i][col].ToString()).ToString("MM/dd/yyyy hh:mm tt") : ""; }
                            PdfPCell1 = new PdfPCell(new Phrase(new Chunk(data, font8)));
                            PdfTable1.AddCell(PdfPCell1);
                        }
                    }
                    for (int i = 0; i < dtlog.Rows.Count; i++)
                    {
                        for (int col = 0; col < dtlog.Columns.Count; col++)
                        {
                            string data = dtlog.Rows[i][col].ToString();
                            if (col == 1) { data = dtlog.Rows[i][col].ToString() != "" ? Convert.ToDateTime(dtlog.Rows[i][col].ToString()).ToString("MM/dd/yyyy hh:mm tt") : ""; }
                            PdfPCell1 = new PdfPCell(new Phrase(new Chunk(data, font8)));
                            PdfTable1.AddCell(PdfPCell1);
                        }
                    }
                    PdfPCell PdfPCelld = new PdfPCell();
                    PdfPCelld.Colspan = 6;
                    PdfPCelld.AddElement(PdfTable1);
                    PdfTable.AddCell(PdfPCelld);
                }


                PdfTable.SpacingBefore = 15f; // Give some space after the text or it may overlap the table

                doc.Add(PdfTable); // add pdf table to the document

                doc.Close();

                return "true";
            }
            else
            {
                return "false";
            }

        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public IActionResult AssignMembers()
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
            ViewBag.UserList = new SelectList(_context.UserMaster.Where(a => a.IsActive == true && a.UserType == 2).ToList(), "UserID", "UserName");
            return View();
        }

        [HttpPost]
        public IActionResult AssignMembersFilter(int? length, int? start, int AssignID)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var sortColumn = HttpContext.Request.Form["columns[" + HttpContext.Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDir = HttpContext.Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = HttpContext.Request.Form["search[value]"].FirstOrDefault();
            var List = _context.memberMasterSPs.FromSqlRaw("SP_AssignMaster @Spara,@SearchbyName,@SortColumn,@SortOrder,@AssignId,@Offset,@Limit",
                                       new Object[]
                                       {
                                           new SqlParameter("@Spara", "1"),
                    new SqlParameter("@SearchbyName",(searchValue == "" ? ""  : searchValue)),
                    new SqlParameter("@SortColumn",sortColumn),
                    new SqlParameter("@SortOrder",sortColumnDir),
                    new SqlParameter("@AssignId",AssignID),
                     new SqlParameter("@Offset",start),
                    new SqlParameter("@Limit",length)}).ToList();
            var data = List.Select(s => new
            {
                s.MemberID,
                s.FirstName,
                s.LastName,
                s.CreatedDate,
                s.Facility,
                s.MedicaidID,
                s.ResidentID
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
                Action = "<input id='ReClose' name='ReClose' style='width: 26px; height: 22px; display: inline; opacity: unset; margin - top: 2px;' type='checkbox' value=" + a.MemberID + ">"
            });

            var count = _context.MemberMaster.Where(a => a.AssignId != AssignID).Count();
            var recordsFiltere = _context.memberMasterSPs.FromSqlRaw("SP_AssignMaster @Spara,@SearchbyName,@SortColumn,@SortOrder,@AssignId,@Offset,@Limit",

                                      new Object[]
                                      {
                                           new SqlParameter("@Spara", "2"),
                    new SqlParameter("@SearchbyName",(searchValue == "" ? ""  : searchValue)),
                    new SqlParameter("@SortColumn",sortColumn),
                    new SqlParameter("@SortOrder",sortColumnDir),
                    new SqlParameter("@AssignId",AssignID),
                     new SqlParameter("@Offset",start),
                    new SqlParameter("@Limit",length)}).ToList();

            var response = new { data = data, recordsFiltered = recordsFiltere.Count(), recordsTotal = count };
            return Json(response);
            // return Json(data);
        }

        [HttpPost]
        public IActionResult AssignedMembersFilter(int? length, int? start, int AssignID)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var sortColumn = HttpContext.Request.Form["columns[" + HttpContext.Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDir = HttpContext.Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = HttpContext.Request.Form["search[value]"].FirstOrDefault();
            var List = _context.memberMasterSPs.FromSqlRaw("SP_GetAssignedMember @Spara,@SearchbyName,@SortColumn,@SortOrder,@AssignId,@Offset,@Limit",
                                       new Object[]
                                       {
                                           new SqlParameter("@Spara", "1"),
                    new SqlParameter("@SearchbyName",(searchValue == "" ? ""  : searchValue)),
                    new SqlParameter("@SortColumn",sortColumn),
                    new SqlParameter("@SortOrder",sortColumnDir),
                    new SqlParameter("@AssignId",AssignID),
                     new SqlParameter("@Offset",start),
                    new SqlParameter("@Limit",length)}).ToList();
            var data = List.Select(s => new
            {
                s.MemberID,
                s.FirstName,
                s.LastName,
                s.CreatedDate,
                s.Facility,
                s.MedicaidID,
                s.ResidentID
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
                Action = "<input id='ReOpen' name='ReOpen' style='width: 26px; height: 22px; display: inline; opacity: unset; margin - top: 2px;' type='checkbox' value=" + a.MemberID + ">"
            });

            var count = _context.MemberMaster.Where(a => a.AssignId == AssignID).Count();
            var recordsFiltere = _context.memberMasterSPs.FromSqlRaw("SP_GetAssignedMember @Spara,@SearchbyName,@SortColumn,@SortOrder,@AssignId,@Offset,@Limit",

                                      new Object[]
                                      {
                                           new SqlParameter("@Spara", "2"),
                    new SqlParameter("@SearchbyName",(searchValue == "" ? ""  : searchValue)),
                    new SqlParameter("@SortColumn",sortColumn),
                    new SqlParameter("@SortOrder",sortColumnDir),
                    new SqlParameter("@AssignId",AssignID),
                     new SqlParameter("@Offset",start),
                    new SqlParameter("@Limit",length)}).ToList();

            var response = new { data = data, recordsFiltered = recordsFiltere.Count(), recordsTotal = count };
            return Json(response);
            // return Json(data);
        }

        public IActionResult AssignMemId(int MemberId, string ischeck, int UserId)
        {
            int member = (_context.MemberMaster.Where(a => a.MemberID == MemberId).Count() > 0 ? _context.MemberMaster.Where(a => a.MemberID == MemberId).FirstOrDefault().MemberID : 0);
            if (member > 0)
            {
                MemberMaster MemberMa = _context.MemberMaster.Where(a => a.MemberID == MemberId).FirstOrDefault();
                if (ischeck == "0")
                {
                    MemberMa.AssignId = 0;
                    _context.MemberMaster.Update(MemberMa);
                    _context.SaveChanges();
                }
                else
                {
                    MemberMa.AssignId = UserId;
                    _context.MemberMaster.Update(MemberMa);
                    _context.SaveChanges();
                }
            }
            return Json(member);
        }
        public IActionResult ReAssignMemId(int MemberId, string ischeck, int UserId)
        {
            int member = (_context.MemberMaster.Where(a => a.MemberID == MemberId).Count() > 0 ? _context.MemberMaster.Where(a => a.MemberID == MemberId).FirstOrDefault().MemberID : 0);
            if (member > 0)
            {
                MemberMaster MemberMa = _context.MemberMaster.Where(a => a.MemberID == MemberId).FirstOrDefault();
                if (ischeck != "0")
                {
                    MemberMa.AssignId = 0;
                    _context.MemberMaster.Update(MemberMa);
                    _context.SaveChanges();
                }
                else
                {
                    MemberMa.AssignId = UserId;
                    _context.MemberMaster.Update(MemberMa);
                    _context.SaveChanges();
                }
            }
            return Json(member);
        }


    }
}
