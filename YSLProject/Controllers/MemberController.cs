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
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data.Entity;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Reflection;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.StaticFiles;
using ClosedXML.Excel;

namespace YSLProject.Controllers
{
    [Autho]
    public class MemberController : Controller
    {
        private readonly ApplicationDbContext _context;

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
            
            // var x = DateTime.ParseExact("DEC", "MMM", CultureInfo.CurrentCulture).Month;
            if (HttpContext.Session.GetString("successmessage") != null)
            {
                ViewBag.successmessage = HttpContext.Session.GetString("successmessage").ToString();
                HttpContext.Session.Remove("successmessage");
            }
            var data = _context.MemberMaster.OrderByDescending(a => a.MemberID).ToList();
            return View(data);
        }
        [HttpPost]
        public IActionResult AjaxMethod()
        {
            List<MemberMaster> customers = (from customer in _context.MemberMaster
                                            select customer).ToList();
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
                obj.MembershipStatus = mob.MembershipStatus.Value;
                obj.Phone = mob.Phone;
                obj.WorkFlowStatus = mob.WorkFlowStatus.Value;
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
            ViewBag.MemberStatusList = (from MemberStatus e in Enum.GetValues(typeof(MemberStatus))
                                        select new SelectListItem
                                        {
                                            Value = Convert.ToString((int)e),
                                            Text = e.ToString().Replace("_", "-")
                                        }).ToList();

            var FacilityType = new List<SelectListItem>
                    {
                        new SelectListItem{ Text="MLTC", Value = "MLTC" , Selected = true },
                        new SelectListItem{ Text="DSNP", Value = "DSNP" },
                        new SelectListItem{ Text="MAP", Value = "MAP" },
                    };
            ViewBag.FacilityType = FacilityType.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadAddMember(IFormFile postedFile, MemberMasterModel objM)
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
                else
                {
                    SaveDischarge(dt, objM);

                }
                HttpContext.Session.SetString("successmessage", "Record saved successfully");
                return RedirectToAction("Index", "Member");

            }
            return RedirectToAction("Index", "Member");
        }

        public async void SaveNewAddmition(DataTable dt, MemberMasterModel objM)
        {
            foreach (DataRow dr in dt.Rows)
            {
                MemberMaster obj1 = new MemberMaster();
                obj1.FollowUpDate = objM.FollowUpDate;
                string checkfiletype = objM.Facility;

                bool ischeck = true;
                obj1.RecertMonth = (dt.Columns.Contains("RecertMonth") ? dr["RecertMonth"].ToString() : null);
                int Recertmonth = 0;

                obj1.FirstName = (dt.Columns.Contains("FirstName") ? dr["FirstName"].ToString() : null);
                obj1.LastName = (dt.Columns.Contains("LastName") ? dr["LastName"].ToString() : null);
                //obj1.EnrollmentDate = DateTime.Now;
                if (dt.Columns.Contains("CountyCode"))
                {
                    try
                    {
                        if (dr["CountyCode"].ToString() != "")
                        {
                            string[] counr = dr["CountyCode"].ToString().Split(' ');
                            if (counr.Length > 1)
                            {
                                bool CountryCo = counr[1].StartsWith("0");
                                if (CountryCo == true)
                                {
                                    if (dt.Columns.Contains("RecertMonth"))
                                    {
                                        bool RecertM = dr["RecertMonth"].ToString().Contains("DEC");
                                        if (RecertM == true)
                                        {
                                            ischeck = false;
                                        }
                                    }
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
                }
                obj1.CountyCode = (dt.Columns.Contains("CountyCode") ? dr["CountyCode"].ToString() : null);
                obj1.MedicaidID = (dt.Columns.Contains("CIN#") ? dr["CIN#"].ToString() : null);
                obj1.ResidentID = (dt.Columns.Contains("ID#") ? dr["ID#"].ToString() : null);
                obj1.ChartsID = (dt.Columns.Contains("ID#") ? dr["ID#"].ToString() : null);
                Recertmonth = obj1.RecertMonth != "" ? DateTime.ParseExact(obj1.RecertMonth, "MMM", CultureInfo.CurrentCulture).Month : 0;
                if (Recertmonth > 0)
                {
                    int cur_month = DateTime.Now.Month;
                    if (cur_month <= 8)
                    {
                        int dif = Recertmonth - cur_month;
                        if (dif <= 3)
                        { obj1.FollowUpDate = DateTime.Now.AddDays(1); }
                    }
                }
                if (ischeck == true)
                {
                    DateTime? DOB = null;
                    if (dt.Columns.Contains("DateOfBirth"))
                    {
                        if (dr["DateOfBirth"].ToString() != "")
                        {
                            string dateYear = dr["DateOfBirth"].ToString().Substring(0, 4);
                            string dateMonth = dr["DateOfBirth"].ToString().Substring(4, 2);
                            string dateDay = dr["DateOfBirth"].ToString().Substring(6, 2);

                            DOB = Convert.ToDateTime(dateYear + "-" + dateMonth + "-" + dateDay);
                        }
                    }

                    obj1.Address = (dt.Columns.Contains("Address") ? dr["Address"].ToString() : null);
                    obj1.PrimaryPhone = (dt.Columns.Contains("PrimaryPhone") ? dr["PrimaryPhone"].ToString() : null);
                    obj1.Email = (dt.Columns.Contains("Email") ? dr["Email"].ToString() : null);
                    //obj1.Gender = (dt.Columns.Contains("Gender") ? (dr["Gender"].ToString().Contains("F") ? "2" : "1") : null);
                    obj1.Language = "0";
                    if (dt.Columns.Contains("Language"))
                    {
                        if (dr["Language"].ToString() != "")
                        {
                            var Lang = _context.LanguageMaster.Where(a => a.Language.Trim().ToLower() == dr["Language"].ToString().Trim().ToLower()).Count() > 0 ? _context.LanguageMaster.Where(a => a.Language.Trim().ToLower() == dr["Language"].ToString().Trim().ToLower()).FirstOrDefault().LanguageID : 0;
                            if (Lang == 0)
                            {
                                LanguageMaster objlan = new LanguageMaster();
                                objlan.Language = dr["Language"].ToString();
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
                    obj1.DateOfBirth = DOB;

                    obj1.Gender = (dt.Columns.Contains("M/F") ? (dr["M/F"].ToString().Contains("F") ? "2" : "1") : null);
                    obj1.DisenrolmentDate = (dt.Columns.Contains("DisenrolmentDate") ? (dr["DisenrolmentDate"] == null ? null : (DateTime?)Convert.ToDateTime(dr["DisenrolmentDate"])) : null);
                    obj1.Phone = (dt.Columns.Contains("Phone") ? dr["Phone"].ToString() : null);


                    obj1.WorkFlowStatus = (dt.Columns.Contains("WorkFlowStatus") ? (dr["WorkFlowStatus"].ToString() == "" ? 0 : Convert.ToInt32(dr["WorkFlowStatus"])) : 0);
                    obj1.InternalNotes = (dt.Columns.Contains("InternalNotes") ? dr["InternalNotes"].ToString() : null);
                    obj1.ExternalNotes = (dt.Columns.Contains("ExternalNotes") ? dr["ExternalNotes"].ToString() : null);
                    obj1.CreatedDate = DateTime.Now;
                    obj1.Facility = checkfiletype;
                    obj1.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
                    obj1.CIN = (dt.Columns.Contains("CIN") ? dr["CIN"].ToString() : null);
                    obj1.Comment = (dt.Columns.Contains("Comment") ? dr["Comment"].ToString() : null);
                    obj1.Coverage = (dt.Columns.Contains("Coverage") ? dr["Coverage"].ToString() : null);
                    obj1.Payer = (dt.Columns.Contains("Payer") ? dr["Payer"].ToString() : null);
                    obj1.PlanCode = (dt.Columns.Contains("PlanCode") ? dr["PlanCode"].ToString() : null);
                    obj1.MedicarePayer = (dt.Columns.Contains("MedicarePayer") ? dr["MedicarePayer"].ToString() : null);
                    obj1.CarrierCode = (dt.Columns.Contains("CarrierCode") ? dr["CarrierCode"].ToString() : null);
                    obj1.ExceptionCodes = (dt.Columns.Contains("ExceptionCodes") ? dr["ExceptionCodes"].ToString() : null);


                    MemberMaster ObjCheck = new MemberMaster();
                    MemberMaster ObjCheck1 = new MemberMaster();
                    string mid = obj1.MedicaidID;
                    string cid = obj1.ChartsID;
                    ObjCheck = _context.MemberMaster.AsNoTracking().Where(s => s.MedicaidID == mid && s.ChartsID == cid).AsNoTracking().FirstOrDefault();
                    ObjCheck1 = _context.MemberMaster.AsNoTracking().Where(s => s.MedicaidID == mid).AsNoTracking().FirstOrDefault();

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
                        ObjCheck1.MembershipStatus = 4;
                        ObjCheck1.DisenrolmentDate = DateTime.Now;
                        ObjCheck1.MemberID = oldmemid;
                        _context.MemberMaster.Update(ObjCheck1);
                        _context.SaveChanges();

                        Logs objlog = new Logs();
                        objlog.CreatedDate = DateTime.Now;
                        objlog.MemberID = oldmemid;
                        objlog.ActionName = "Disenrolled  member";
                        _context.Logs.Add(objlog);
                        _context.SaveChanges();

                        #endregion

                        #region Add new Recorde with new chartsid 
                        obj1.EnrollmentDate = dtoldenrolldate;
                        obj1.NewEnrollmentDate = (dt.Columns.Contains("RunDate") ? (dr["RunDate"] == null ? DateTime.Now : (DateTime?)Convert.ToDateTime(dr["RunDate"])) : DateTime.Now);
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
                    else
                    {
                        obj1.EnrollmentDate = (dt.Columns.Contains("RunDate") ? (dr["RunDate"] == null ? DateTime.Now : (DateTime?)Convert.ToDateTime(dr["RunDate"])) : DateTime.Now);
                        obj1.NewEnrollmentDate = (dt.Columns.Contains("RunDate") ? (dr["RunDate"] == null ? DateTime.Now : (DateTime?)Convert.ToDateTime(dr["RunDate"])) : DateTime.Now);
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
                        objlog.ActionName = "New member added";
                        _context.Logs.Add(objlog);
                        _context.SaveChanges();


                        Recertification_Follow_Up objf = new Recertification_Follow_Up();
                        if (Recertmonth > 0)
                        {
                            int cur_month = DateTime.Now.Month;
                            //DateTimeFormatInfo df = new DateTimeFormatInfo();
                            if (cur_month <= 8)
                            {
                                int dif = Recertmonth - cur_month;
                                if (dif <= 3)
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
                            }


                        }
                    }




                }

            }
        }

        public async void SaveRecertification(DataTable dt, MemberMasterModel objM)
        {

            foreach (DataRow dr in dt.Rows)
            {
                MemberMaster obj1 = new MemberMaster();
                obj1.MedicaidID = (dt.Columns.Contains("MEDICAID#") ? dr["MEDICAID#"].ToString() : null);
                string mid = obj1.MedicaidID;

                MemberMaster ObjCheck = _context.MemberMaster.AsNoTracking().Where(s => s.MedicaidID == mid).AsNoTracking().FirstOrDefault();
                if (ObjCheck == null)
                {

                    obj1.FollowUpDate = objM.FollowUpDate;
                    obj1.Facility = objM.Facility;

                    obj1.FirstName = null;
                    obj1.LastName = null;
                    if (dt.Columns.Contains("Resident"))
                    {
                        string[] sn = dr["Resident"].ToString().Split(' ');
                        obj1.FirstName = sn[0].ToString();
                        obj1.LastName = sn[1].ToString();
                    }
                    else
                    {
                        obj1.FirstName = (dt.Columns.Contains("FirstName") ? dr["FirstName"].ToString() : null);
                        obj1.LastName = (dt.Columns.Contains("LastName") ? dr["LastName"].ToString() : null);
                    }

                    obj1.RecertMonth = (dt.Columns.Contains("RecertMonth") ? Convert.ToDateTime("01-" + dr["RecertMonth"].ToString() + "-" + DateTime.Now.Year).ToString("MMM") : null);
                    obj1.RecertMonth = obj1.RecertMonth.ToUpper();
                    obj1.CountyCode = (dt.Columns.Contains("Location") ? dr["Location"].ToString() : null);
                    obj1.ResidentID = (dt.Columns.Contains("ID#") ? dr["ID#"].ToString() : null);
                    obj1.ChartsID = (dt.Columns.Contains("ID#") ? dr["ID#"].ToString() : null);
                    obj1.EnrollmentDate = (dt.Columns.Contains("Rundate") ? (dr["Rundate"] == null ? DateTime.Now : (DateTime?)Convert.ToDateTime(dr["Rundate"])) : DateTime.Now);
                    obj1.NewEnrollmentDate = (dt.Columns.Contains("Rundate") ? (dr["Rundate"] == null ? DateTime.Now : (DateTime?)Convert.ToDateTime(dr["Rundate"])) : DateTime.Now);
                    DateTime endate = Convert.ToDateTime(obj1.EnrollmentDate);
                    DateTime newendate = Convert.ToDateTime(obj1.NewEnrollmentDate);
                    obj1.EnrollmentDate = new DateTime(endate.Year, endate.Month, 1);
                    obj1.NewEnrollmentDate = new DateTime(newendate.Year, newendate.Month, 1);

                    obj1.Address = (dt.Columns.Contains("RESPADDRESS1") ? dr["RESPADDRESS1"].ToString() : null);
                    obj1.Language = "0";
                    if (dt.Columns.Contains("Language"))
                    {
                        if (dr["Language"].ToString() != "")
                        {
                            var Lang = _context.LanguageMaster.Where(a => a.Language.Trim().ToLower() == dr["Language"].ToString().Trim().ToLower()).Count() > 0 ? _context.LanguageMaster.Where(a => a.Language.Trim().ToLower() == dr["Language"].ToString().Trim().ToLower()).FirstOrDefault().LanguageID : 0;
                            if (Lang == 0)
                            {
                                LanguageMaster objlan = new LanguageMaster();
                                objlan.Language = dr["Language"].ToString();
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
                    obj1.MembershipStatus = 3;
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
                else
                {
                    obj1.MemberID = ObjCheck.MemberID;
                    obj1.RecertMonth = ObjCheck.RecertMonth;
                }
                int Recertmonth = 0;
                if (obj1.RecertMonth != null)
                {
                    Recertmonth = obj1.RecertMonth != "" ? DateTime.ParseExact(obj1.RecertMonth, "MMM", CultureInfo.CurrentCulture).Month : 0;

                    Recertification_Follow_Up objf = new Recertification_Follow_Up();
                    if (Recertmonth > 0)
                    {
                        int cur_month = DateTime.Now.Month;
                        //DateTimeFormatInfo df = new DateTimeFormatInfo();
                        if (cur_month <= 8)
                        {
                            int dif = Recertmonth - cur_month;
                            if (dif <= 3)
                            {
                                objf.MemberId = obj1.MemberID;
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
                            }
                        }


                    }
                }
            }
        }

        public async void SaveNoncovered(DataTable dt, MemberMasterModel objM)
        {
            foreach (DataRow dr in dt.Rows)
            {

                MemberMaster obj1 = new MemberMaster();
                string Medicaid = (dt.Columns.Contains("CIN#") ? dr["CIN#"].ToString() : null);
                obj1 = _context.MemberMaster.AsNoTracking().Where(s => s.MedicaidID == Medicaid).AsNoTracking().FirstOrDefault();
                if (obj1 != null)
                {
                    int mID = obj1.MemberID;
                    obj1.Status = objM.MemberStatus;
                    obj1.MembershipStatus = 4;

                    _context.MemberMaster.Update(obj1);
                    _context.SaveChanges();

                    Logs objlog1 = new Logs();
                    objlog1.CreatedDate = DateTime.Now;
                    objlog1.MemberID = mID;
                    objlog1.ActionName = "Status to non covered";
                    _context.Logs.Add(objlog1);
                    _context.SaveChanges();
                }
                else
                {
                    obj1 = new MemberMaster();
                    obj1.FirstName = null;
                    obj1.LastName = null;
                    obj1.MedicaidID = Medicaid;
                    if (dt.Columns.Contains("Resident"))
                    {
                        string[] sn = dr["Resident"].ToString().Split(' ');
                        obj1.FirstName = sn[0].ToString();
                        obj1.LastName = sn[1].ToString();
                    }
                    else
                    {
                        obj1.FirstName = (dt.Columns.Contains("FirstName") ? dr["FirstName"].ToString() : null);
                        obj1.LastName = (dt.Columns.Contains("LastName") ? dr["LastName"].ToString() : null);
                    }
                    obj1.ResidentID = (dt.Columns.Contains("ID#") ? dr["ID#"].ToString() : null);

                    obj1.CountyCode = (dt.Columns.Contains("Location") ? dr["Location"].ToString() : null);
                    obj1.ChartsID = (dt.Columns.Contains("ID#") ? dr["ID#"].ToString() : null);
                    obj1.Comment = (dt.Columns.Contains("Comment") ? dr["Comment"].ToString() : null);
                    obj1.EnrollmentDate = (dt.Columns.Contains("Rundate") ? (dr["Rundate"] == null ? DateTime.Now : (DateTime?)Convert.ToDateTime(dr["Rundate"])) : DateTime.Now);
                    obj1.NewEnrollmentDate = (dt.Columns.Contains("Rundate") ? (dr["Rundate"] == null ? DateTime.Now : (DateTime?)Convert.ToDateTime(dr["Rundate"])) : DateTime.Now);
                    DateTime endate = Convert.ToDateTime(obj1.EnrollmentDate);
                    DateTime newendate = Convert.ToDateTime(obj1.NewEnrollmentDate);
                    obj1.EnrollmentDate = new DateTime(endate.Year, endate.Month, 1);
                    obj1.NewEnrollmentDate = new DateTime(newendate.Year, newendate.Month, 1);

                    obj1.Address = (dt.Columns.Contains("RESPADDRESS1") ? dr["RESPADDRESS1"].ToString() : null);
                    obj1.Language = "0";
                    if (dt.Columns.Contains("Language"))
                    {
                        if (dr["Language"].ToString() != "")
                        {
                            var Lang = _context.LanguageMaster.Where(a => a.Language.Trim().ToLower() == dr["Language"].ToString().Trim().ToLower()).Count() > 0 ? _context.LanguageMaster.Where(a => a.Language.Trim().ToLower() == dr["Language"].ToString().Trim().ToLower()).FirstOrDefault().LanguageID : 0;
                            if (Lang == 0)
                            {
                                LanguageMaster objlan = new LanguageMaster();
                                objlan.Language = dr["Language"].ToString();
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

                    obj1.MembershipStatus = 4;
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
                    objlog1.ActionName = "Status to non covered";
                    _context.Logs.Add(objlog1);
                    _context.SaveChanges();


                }


            }
        }

        public async void SaveDischarge(DataTable dt, MemberMasterModel objM)
        {
            foreach (DataRow dr in dt.Rows)
            {

                MemberMaster obj1 = new MemberMaster();
                string Medicaid = (dt.Columns.Contains("Medicaid#") ? dr["Medicaid#"].ToString() : null);
                string Date = (dt.Columns.Contains("DateDischarged") ? dr["DateDischarged"].ToString() : null);
                obj1 = _context.MemberMaster.AsNoTracking().Where(s => s.MedicaidID == Medicaid).AsNoTracking().FirstOrDefault();
                if (obj1 != null)
                {
                    DateTime dtnew = new DateTime();
                    if (Date != null)
                    {
                        dtnew = Convert.ToDateTime(Date);
                        dtnew = dtnew.AddMonths(1);
                        //dtnew = new DateTime(dtnew.Year, dtnew.Month + 1, 1);
                    }
                    int mID = obj1.MemberID;
                    obj1.Status = objM.MemberStatus;
                    obj1.MembershipStatus = 5;
                    obj1.DischargeDate = dtnew;

                    _context.MemberMaster.Update(obj1);
                    _context.SaveChanges();

                    List<Recertification_Follow_Up> objr = new List<Recertification_Follow_Up>();

                    objr = _context.Recertification_Follow_Up.AsNoTracking().Where(a => a.MemberId == mID).OrderByDescending(a => a.CreatedDate).ToList();

                    foreach (Recertification_Follow_Up r in objr)
                    {
                        //var folloid = r.FollowUpID;
                        //Recertification_Follow_Up rfu = new Recertification_Follow_Up();
                        //rfu.MemberId = mID;
                        //rfu.FollowUpID = folloid;
                        _context.Recertification_Follow_Up.Remove(r);
                        _context.SaveChanges();
                    }


                    Logs objlog1 = new Logs();
                    objlog1.CreatedDate = DateTime.Now;
                    objlog1.MemberID = mID;
                    objlog1.ActionName = "Discharge";
                    _context.Logs.Add(objlog1);
                    _context.SaveChanges();
                }


            }
        }


        public IActionResult UploadAddMemberPIT()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadAddMemberPIT(IFormFile postedFile)
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
                    string Medicaid = (dt.Columns.Contains("MedicaidID") ? dr["MedicaidID"].ToString().Trim() : null);

                    obj1 = _context.MemberMaster.AsNoTracking().Where(s => s.MedicaidID == Medicaid).AsNoTracking().FirstOrDefault();
                    if (obj1 != null)
                    {
                        PIT p = new PIT();
                        int mID = obj1.MemberID;
                        p = _context.PIT.AsNoTracking().Where(s => s.MemberID == mID).AsNoTracking().FirstOrDefault();

                        if (p != null)
                        {
                            // p.MemberID = mID;
                            p.PITName = (dt.Columns.Contains("PITName") ? dr["PITName"].ToString().Trim() : null);
                            p.PITStatus = (dt.Columns.Contains("PIT") ? dr["PIT"].ToString().Trim() : null);
                            if (p.PITName.Trim() == "PIT PENDING" || p.PITStatus.Trim() == "PIT PENDING") { p.PITStatus = "PENDING"; }
                            else if (p.PITStatus.Trim() == "PIT") { p.PITStatus = "APPROVED"; }
                            p.PITEffective = (dt.Columns.Contains("EffectiveDate") ? dr["EffectiveDate"].ToString() : null);
                            p.PITNotes = (dt.Columns.Contains("Comments") ? dr["Comments"].ToString() : null);

                            if (p.PITStatus.Trim() != "NO PIT")
                            {
                                _context.PIT.Update(p);
                                _context.SaveChanges();
                            }
                        }
                        else
                        {
                            p = new PIT();
                            p.MemberID = Convert.ToInt32(mID);
                            p.PITName = (dt.Columns.Contains("PITName") ? dr["PITName"].ToString() : null);
                            p.PITStatus = (dt.Columns.Contains("PIT") ? dr["PIT"].ToString() : null);
                            if (p.PITName.Trim() == "PIT PENDING" || p.PITStatus.Trim() == "PIT PENDING") { p.PITStatus = "PENDING"; }
                            else if (p.PITStatus.Trim() == "PIT") { p.PITStatus = "APPROVED"; }
                            p.PITEffective = (dt.Columns.Contains("EffectiveDate") ? dr["EffectiveDate"].ToString() : null);
                            string Notes = "";
                            Notes += (dt.Columns.Contains("Comments") ? dr["Comments"].ToString() : null);
                            Notes += (dt.Columns.Contains("Comments2") ? dr["Comments2"].ToString() : null);
                            p.PITNotes = Notes;

                            if (p.PITStatus.Trim() != "NO PIT")
                            {
                                _context.PIT.Add(p);
                                _context.SaveChanges();
                            }
                        }
                    }
                }

                return RedirectToAction("Index", "Dashboard");

            }
            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult UploadAddMemberContacts()
        {
            return View();
        }

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

                    obj1 = _context.MemberMaster.AsNoTracking().Where(s => s.MedicaidID == Medicaid).AsNoTracking().FirstOrDefault();
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

                foreach (DataRow dr in dt.Rows)
                {

                    MemberMaster obj1 = new MemberMaster();
                    string Medicaid = (dt.Columns.Contains("OldMedicalID") ? dr["OldMedicalID"].ToString() : null);

                    obj1 = _context.MemberMaster.AsNoTracking().Where(s => s.MedicaidID == Medicaid).AsNoTracking().FirstOrDefault();
                    if (obj1 != null)
                    {
                        
                        int mID = obj1.MemberID;


                        
                        obj1.MedicaidID = (dt.Columns.Contains("NewMedicalID") ? dr["NewMedicalID"].ToString() : obj1.MedicaidID);

                        _context.Update(obj1);
                        _context.SaveChanges();

                    }
                }

                return RedirectToAction("Index", "Dashboard");

            }
            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult UploadAddMemberSpousal()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadAddMemberSpousal(IFormFile postedFile)
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

                    List<MemberMaster> obj1 = new List<MemberMaster>();
                    string Medicaid = (dt.Columns.Contains("Medicaid #") ? dr["Medicaid #"].ToString() : null);

                    obj1 = _context.MemberMaster.AsNoTracking().Where(s => s.MedicaidID == Medicaid).AsNoTracking().ToList();
                    if (obj1 != null)
                    {
                        foreach (var x in obj1)
                        {
                            Spousal C = new Spousal();
                            int mID = x.MemberID;

                            C.MemberID = mID;
                            C.Name = (dt.Columns.Contains("Name") ? dr["Name"].ToString() : null);
                            C.CaseName = (dt.Columns.Contains("Duplicate Case Name") ? dr["Duplicate Case Name"].ToString() : null);
                            C.MA = (dt.Columns.Contains("MA#") ? dr["MA#"].ToString() : null);

                            _context.Spousal.Add(C);
                            _context.SaveChanges();
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
            ViewBag.LanguageList = new SelectList(_context.LanguageMaster.ToList(), "LanguageID", "Language");

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

            MemberMasterModel obj = new MemberMasterModel();
            var datas = _context.MemberMaster.AsNoTracking().Where(a => a.MemberID == memberId).ToList();

            foreach (var dt in datas.ToList())
            {
                var dataf = _context.MemberMaster.Where(a => a.MedicaidID == dt.MedicaidID).OrderByDescending(a => a.MemberID).ToList();
                obj.CurrentFacility = dataf.FirstOrDefault().Facility;


                obj.MemberID = dt.MemberID;
                obj.FirstName = dt.FirstName + " " + dt.LastName;
                obj.LastName = dt.LastName;
                obj.Address = dt.Address;
                obj.PrimaryPhone = dt.PrimaryPhone;
                obj.Email = dt.Email;
                obj.Comment = dt.Comment;

                string languname = "";
                if (dt.Language != "")
                {
                    var langid = Convert.ToInt32(dt.Language);
                    languname = _context.LanguageMaster.Where(a => a.LanguageID == langid).Count() > 0 ? _context.LanguageMaster.Where(a => a.LanguageID == langid).FirstOrDefault().Language : "";
                    //languname = Enum.GetName(typeof(Language), Convert.ToInt32(dt.Language));
                }
                obj.Language = languname;
                obj.CountyCode = dt.CountyCode;
                obj.ResidentID = dt.ResidentID;
                obj.MedicaidID = dt.MedicaidID;
                obj.MemberID = dt.MemberID;
                obj.RecertMonth = dt.RecertMonth;
                obj.TempRecertMonth = dt.TempRecertMonth;
                obj.DOB = (dt.DateOfBirth == null ? "" : Convert.ToDateTime(dt.DateOfBirth).ToString("MM/dd/yyyy"));
                obj.EnrollmentDa = (dt.EnrollmentDate == null ? "" : Convert.ToDateTime(dt.EnrollmentDate).ToString("MM/dd/yyyy"));
                obj.DisenrolmentDa = (dt.DisenrolmentDate == null ? "" : Convert.ToDateTime(dt.DisenrolmentDate).ToString("MM/dd/yyyy"));
                obj.NewEnrollmentDa = (dt.NewEnrollmentDate == null ? "" : Convert.ToDateTime(dt.NewEnrollmentDate).ToString("MM/dd/yyyy"));
                if (dt.MembershipStatus != null)
                {
                    obj.MembershipStatu = Enum.GetName(typeof(MembershipStatus), Convert.ToInt32(dt.MembershipStatus));
                }
            }


            obj.Recertification_Follow_UpModels = _context.Recertification_Follow_Up.Where(a => a.MemberId == memberId).OrderByDescending(a => a.CreatedDate).ToList().Select(s => new Recertification_Follow_UpModel
            {
                FollowUpID = s.FollowUpID,
                MemberId = s.MemberId,
                CurrentStatus = Convert.ToInt32(s.CurrentStatus) > 0 ? Enum.GetName(typeof(FollowupStatus), Convert.ToInt32(s.CurrentStatus)).Replace("_", " ") : "",
                Outcome = s.Outcome,
                AttemptCount = s.AttemptCount,
                Notes = s.Notes,
                NextStepTask = Convert.ToInt32(s.NextStepTask) > 0 ? Enum.GetName(typeof(FollowupStatus), Convert.ToInt32(s.NextStepTask)).Replace("_", " ") : "",
                NextStepDueNotes = s.NextStepDueNotes,
                NewStatus = Convert.ToInt32(s.NewStatus) > 0 ? Enum.GetName(typeof(FollowupStatus), Convert.ToInt32(s.NewStatus)).Replace("_", " ") : "",
                CallOutcome = s.CallOutcome,
                CreatedDt = s.CreatedDate.ToString("MM/dd/yyyy"),
                CreatedBy = s.CreatedBy,
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
            obj.SpousalModels = _context.Spousal.Where(a => a.MemberID == memberId).ToList().Select(s => new SpousalModel
            {
                MemberID = s.MemberID,
                Name = s.Name,
                CaseName = s.CaseName,
                MA = s.MA,

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
                                     where l.MemberId == memberId
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

            PITModel pmodel = new PITModel();
            pmodel.MemberID = memberId;
            var datapit = _context.PIT.Where(a => a.MemberID == memberId).FirstOrDefault();
            if (datapit != null)
            {
                pmodel.ID = datapit.ID;
                pmodel.PITName = datapit.PITName;
                pmodel.PITStatus = datapit.PITStatus;
                pmodel.PITEffective = datapit.PITEffective != null ? Convert.ToDateTime(datapit.PITEffective).ToString("MM/dd/yyyy") : null;
                pmodel.PITNotes = datapit.PITNotes;
            }
            obj.pITModel = pmodel;

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

            MemberMaster model = _context.MemberMaster.Where(m => m.MemberID == MemberID).FirstOrDefault();
            string medicid = model.MedicaidID;
            var data = (from l in _context.Logs
                        join m in _context.MemberMaster
                        on l.MemberID equals m.MemberID
                        where m.MedicaidID == medicid && m.Facility == (Facility == null ? m.Facility : Facility)
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
                        where m.MedicaidID == medicid && m.Facility == (Facility == null ? m.Facility : Facility)
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
            var data = (from l in _context.Recertification_Follow_Up
                        join m in _context.MemberMaster
                        on l.MemberId equals m.MemberID
                        where m.MedicaidID == medicid && m.Facility == (Facility == null ? m.Facility : Facility)
                        select new
                        {
                            l.MemberId,
                            l.FollowUpID,
                            l.CurrentStatus,
                            l.Outcome,
                            l.Notes,
                            l.NextStepTask,
                            l.NewStatus,
                            l.CreatedDate
                        }).ToList()
                        .Select(s => new Recertification_Follow_UpModel
                        {
                            FollowUpID = s.FollowUpID,
                            MemberId = s.MemberId,
                            CurrentStatus = Convert.ToInt32(s.CurrentStatus) > 0 ? Enum.GetName(typeof(FollowupStatus), Convert.ToInt32(s.CurrentStatus)).Replace("_", " ") : "",
                            Outcome = s.Outcome,
                            Notes = s.Notes,
                            NextStepTask = Convert.ToInt32(s.NextStepTask) > 0 ? Enum.GetName(typeof(FollowupStatus), Convert.ToInt32(s.NextStepTask)).Replace("_", " ") : "",
                            NewStatus = Convert.ToInt32(s.NewStatus) > 0 ? Enum.GetName(typeof(FollowupStatus), Convert.ToInt32(s.NewStatus)).Replace("_", " ") : "",
                            CreatedDt = s.CreatedDate.ToString("MM/dd/yyyy"),
                        });

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
                        where m.MedicaidID == medicid && m.Facility == (Facility == null ? m.Facility : Facility)
                        select new
                        {
                            l.MemberId,
                            l.FollowUpID,
                            l.CurrentStatus,
                            l.Outcome,
                            l.Notes,
                            l.NextStepTask,
                            l.NewStatus,
                            l.CreatedDate
                        }).ToList()
                        .Select(s => new Recertification_Follow_UpModel
                        {
                            FollowUpID = s.FollowUpID,
                            MemberId = s.MemberId,
                            CurrentStatus = Convert.ToInt32(s.CurrentStatus) > 0 ? Enum.GetName(typeof(FollowupStatus), Convert.ToInt32(s.CurrentStatus)).Replace("_", " ") : "",
                            Outcome = s.Outcome,
                            Notes = s.Notes,
                            NextStepTask = Convert.ToInt32(s.NextStepTask) > 0 ? Enum.GetName(typeof(FollowupStatus), Convert.ToInt32(s.NextStepTask)).Replace("_", " ") : "",
                            NewStatus = Convert.ToInt32(s.NewStatus) > 0 ? Enum.GetName(typeof(FollowupStatus), Convert.ToInt32(s.NewStatus)).Replace("_", " ") : "",
                            CreatedDt = s.CreatedDate.ToString("MM/dd/yyyy"),
                        });
            DataTable table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(data), (typeof(DataTable)));
            Parallel.ForEach(table.Rows.Cast<DataRow>(), (row) =>
            {
                string Createddate = row["CreatedDate"].ToString();
                if(Createddate.Contains("01/01/0001 00:00:00"))
                {
                    row["CreatedDate"] = DBNull.Value;
                }
                string Nextduedate = row["Nextduedate"].ToString();
                if (Createddate.Contains("01/01/0001 00:00:00"))
                {
                    row["Nextduedate"] = DBNull.Value;
                }
            });
                var Emplist = JsonConvert.SerializeObject(table);
            DataTable dt11 = (DataTable)JsonConvert.DeserializeObject(Emplist, (typeof(DataTable)));
            dt11.TableName = "MemberWorkflow";
            FileContentResult robj;
            byte[] chends = null;
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt11);
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
                RelationShip = s.RelationShip,
                Address = s.Address,
                Name = s.Name,
                Email = s.Email,
                Phone = s.Phone
            }).ToList();

            return Json(data);
        }

        public IActionResult FollowupS(int Memberid, int Id)
        {
            //ViewBag.CuurentStatusList = (from CuurentStatus e in Enum.GetValues(typeof(CuurentStatus))
            //                             select new SelectListItem
            //                             {
            //                                 Value = Convert.ToString((int)e),
            //                                 Text = e.ToString().Replace("_", "-")
            //                             }).ToList();
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

            RecertificationFollowUp obj = new RecertificationFollowUp();

            var data = _context.Recertification_Follow_Up.Where(s => s.MemberId == Memberid).OrderByDescending(a => a.FollowUpID).FirstOrDefault();
            if (data != null)
            {
                obj.Nextduedate = DateTime.Now;

                //RecertificationFollowUpList objd = new RecertificationFollowUpList();
                if (data.NewStatus != "")
                {
                    string currunt_status = Enum.GetName(typeof(FollowupStatus), Convert.ToInt32(data.NewStatus)).Replace("_", " ");
                    if (data.NewStatus == "2")//Follow_up
                    {
                        var list = new List<SelectListItem>
                    {
                        new SelectListItem{ Text="Wrong Number", Value = "Wrong Number" , Selected = true },
                        new SelectListItem{ Text="Left Voice Mail", Value = "Left Voice Mail"  },
                        new SelectListItem{ Text="Called five times", Value = "Called five times"},
                        new SelectListItem{ Text="FHS mailed/e-mailed recert to member", Value = "FHS mailed/e-mailed recert to member" },
                        new SelectListItem{ Text="Not compliant", Value = "Not compliant"},
                        new SelectListItem{ Text="Excess Resource – CPHL to advise member", Value = "Excess Resource – CPHL to advise member"},
                        new SelectListItem{ Text="Recert sent to FHS", Value = "Recert sent to FHS"},
                        new SelectListItem{ Text="Recert sent to Medicaid by FHS", Value = "Recert sent to Medicaid by FHS"},
                        new SelectListItem{ Text="Recert Packet sent to Medicaid by member", Value = "Recert Packet sent to Medicaid by member"},
                        new SelectListItem{ Text="Hospitalized", Value = "Hospitalized"},
                        new SelectListItem{ Text="OOSA (out of service area)", Value = "OOSA (out of service area)"},
                        new SelectListItem{ Text="Nursing home Long Term", Value = "Nursing home Long Term"},
                        new SelectListItem{ Text="Medicaid Approved", Value = "Medicaid Approved"  },
                        new SelectListItem{ Text="Medicaid Rejected", Value = "Medicaid Rejected" },
                        new SelectListItem{ Text="Recertification Deferred", Value = "Recertification Deferred"},
                        };
                        ViewBag.Outcome = list.ToList();
                        obj.NextStepTask = Enum.GetName(typeof(FollowupStatus), Convert.ToInt32(8)).Replace("_", " ");
                        obj.NewStatus = Enum.GetName(typeof(FollowupStatus), Convert.ToInt32(5)).Replace("_", " ");
                    }
                    else if (data.NewStatus == "3") //Packet_in_Review
                    {
                        var list = new List<SelectListItem>
                    {
                        new SelectListItem{ Text="Check If Packet Approved", Value = "Check If Packet Approved" , Selected = true },

                    };
                        ViewBag.Outcome = list.ToList();
                        obj.NextStepTask = Enum.GetName(typeof(FollowupStatus), Convert.ToInt32(20)).Replace("_", " ");
                        obj.NewStatus = Enum.GetName(typeof(FollowupStatus), Convert.ToInt32(15)).Replace("_", " ");
                        obj.Nextduedate = DateTime.Now.AddDays(7);
                    }
                    //else if (data.NewStatus == "4")//Follow_up_with_Medicaid
                    //{
                    //    var list = new List<SelectListItem>
                    //{

                    //};
                    //    ViewBag.Outcome = list.ToList();
                    //    obj.NextStepTask = Enum.GetName(typeof(FollowupStatus), Convert.ToInt32(8)).Replace("_", " ");
                    //    obj.NewStatus = Enum.GetName(typeof(FollowupStatus), Convert.ToInt32(16)).Replace("_", " ");
                    //}

                    else if (data.NewStatus == "5")//Requires_CPHL_Assistance
                    {
                        var list = new List<SelectListItem>
                    {
                        new SelectListItem{ Text="Left Voice Mail", Value = "Left Voice Mail", Selected = true },
                        new SelectListItem{ Text="Not compliant", Value = "Not compliant"},
                        new SelectListItem{ Text="Excess Resource – CPHL to advise member", Value = "Excess Resource – CPHL to advise member"},
                    };
                        ViewBag.Outcome = list.ToList();
                        obj.NextStepTask = Enum.GetName(typeof(FollowupStatus), Convert.ToInt32(9)).Replace("_", " ");
                        obj.NewStatus = Enum.GetName(typeof(FollowupStatus), Convert.ToInt32(2)).Replace("_", " ");
                    }

                    else if (data.NewStatus == "6") //Follow_up_Through_Edits
                    {
                        var list = new List<SelectListItem>
                    {
                        new SelectListItem{ Text="Rejected", Value = "Rejected" , Selected = true },
                        new SelectListItem{ Text="Deferred", Value = "Deferred"},

                    };
                        ViewBag.Outcome = list.ToList();
                        obj.NextStepTask = Enum.GetName(typeof(FollowupStatus), Convert.ToInt32(21)).Replace("_", " ");
                        obj.NewStatus = Enum.GetName(typeof(FollowupStatus), Convert.ToInt32(2)).Replace("_", " ");
                        obj.Nextduedate = DateTime.Now.AddDays(1);
                    }
                    else if (data.NewStatus == "22") //NonCovered
                    {
                        var list = new List<SelectListItem>
                    {
                        new SelectListItem{ Text="Deceased", Value = "Deceased" , Selected = true },
                        new SelectListItem{ Text="PA case", Value = "PA case"},
                        new SelectListItem{ Text="SSI case", Value = "SSI case"},
                        new SelectListItem{ Text="Relink", Value = "Relink"},
                        new SelectListItem{ Text="OOSA", Value = "OOSA"},

                    };
                        ViewBag.Outcome = list.ToList();
                        obj.NextStepTask = Enum.GetName(typeof(FollowupStatus), Convert.ToInt32(23)).Replace("_", " ");
                        obj.NewStatus = "";
                        obj.Nextduedate = DateTime.Now.AddDays(1);
                    }
                    else
                    {
                        //var list = new List<SelectListItem>();

                        ViewBag.Outcome = null;
                    }
                    obj.MemberId = data.MemberId;
                    obj.CurrentStatus = currunt_status;
                }


                //obj.recertificLists.Add(objd);
            }
            return View(obj);
            //return RedirectToAction("ViewAsset", "AssetManagement", new { id = assetComment.AssetId });
        }

        [HttpPost]
        public async Task<IActionResult> FollowupS(RecertificationFollowUp obj)
        {

            var statuslist = (from FollowupStatus e in Enum.GetValues(typeof(FollowupStatus))
                              select new SelectListItem
                              {
                                  Value = Convert.ToString((int)e),
                                  Text = e.ToString()
                              }).ToList();

            var currnts = obj.CurrentStatus != null ? statuslist.Where(s => s.Text == obj.CurrentStatus.Replace(" ", "_")).FirstOrDefault() : null;
            var newstep = obj.NextStepTask != null ? statuslist.Where(s => s.Text == obj.NextStepTask.Replace(" ", "_")).FirstOrDefault() : null;
            var newstatus = obj.NewStatus != null ? statuslist.Where(s => s.Text == obj.NewStatus.Replace(" ", "_")).FirstOrDefault() : null;


            Recertification_Follow_Up obj1 = new Recertification_Follow_Up();
            obj1.FollowUpID = obj.FollowUpID;
            obj1.MemberId = obj.MemberId;
            obj1.CurrentStatus = currnts != null ? currnts.Value : "0";
            obj1.Outcome = obj.Outcome;
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
            //}
            return RedirectToAction("Workfollow", "Member", new
            {
                memberId = obj.MemberId,
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

            _context.GeneralNotes.Add(obj1);
            await _context.SaveChangesAsync();

            return RedirectToAction("Workfollow", "Member", new
            {
                memberId = obj.MemberId,
            });

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
        public async Task<IActionResult> SaveContacts(int memberid, string name, string phone, string email, string address)
        {
            Contacts model = new Contacts();
            model.MemberID = memberid;
            model.Name = name;
            model.Phone = phone;
            model.Email = email;
            model.Address = address;

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

            var ReportTypes = new List<SelectListItem>
                    {
                        new SelectListItem{ Text="Non-Covered Members Activity", Value = "Non-Covered Members Activity" , Selected = true },
                        new SelectListItem{ Text="Recertification Activity", Value = "Recertification Activity" },
                    };
            ViewBag.ReportTypes = ReportTypes.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Reports(MemberMasterModel obj)
        {
            var ReportTypes = new List<SelectListItem>
                    {
                        new SelectListItem{ Text="Non-Covered Members Activity", Value = "Non-Covered Members Activity" , Selected = true },
                        new SelectListItem{ Text="Recertification Activity", Value = "Recertification Activity" },
                    };
            ViewBag.ReportTypes = ReportTypes.ToList();
            if (obj.ReportsType == "Non-Covered Members Activity")
            {
                string isdata = BindNonCoveredReport();

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
                return View();
            }

        }
        private string BindNonCoveredReport()
        {
            List<MemberMaster> ObjCheck = new List<MemberMaster>();

             int   UserId = Convert.ToInt32(HttpContext.Session.GetString("UserTypeId"));
            
            ObjCheck = _context.MemberMaster.AsNoTracking().Where(s => s.MembershipStatus == 4 && s.AssignId == (UserId == 1 ?  s.AssignId : UserId)).AsNoTracking().ToList();
            if (ObjCheck != null && ObjCheck.Count > 0)
            {

                DataTable dt = ToDataTable(ObjCheck);
                DataView dv = new DataView(dt);
                dt = dv.ToTable(true, "MemberID", "MedicaidID", "ChartsID", "FirstName", "LastName");
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

                PdfPTable PdfTable = new PdfPTable(5);
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
                            if (col == 1) { data = Convert.ToDateTime(dtlog.Rows[i][col].ToString()).ToString("MM/dd/yyyy hh:mm tt"); }
                            PdfPCell1 = new PdfPCell(new Phrase(new Chunk(data, font8)));
                            PdfTable1.AddCell(PdfPCell1);
                        }
                    }
                    PdfPCell PdfPCelld = new PdfPCell();
                    PdfPCelld.Colspan = 5;
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
            ViewBag.UserList = new SelectList(_context.UserMaster.Where(a=>a.IsActive == true && a.UserType == 2).ToList(), "UserID", "UserName");
            return View();
        }

        [HttpGet]
        public IActionResult AssignMembersFilter()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var data = _context.MemberMaster.OrderByDescending(s => s.CreatedDate).
            Select(s => new
            {
                s.MemberID,
                s.FirstName,
                s.LastName,
                s.CreatedDate,
                s.Facility,
                s.MedicaidID,
                s.ResidentID,
            }).ToList()
            .Select(a => new MemberMasterModel
            {
                MemberID = a.MemberID,
                FirstName = a.FirstName,
                LastName = a.LastName,
                CreatedDate = a.CreatedDate.Value.ToString("MM/dd/yyyy"),
                Facility = a.Facility,
                MedicaidID = a.MedicaidID,
                ResidentID = a.ResidentID
            });
            if (data.Count() > 100) { data = data.Take(100); }
            return Json(data);
        }

        public IActionResult AssignMemId(int MemberId,string ischeck,int UserId)
        {
            int member =( _context.MemberMaster.Where(a => a.MemberID == MemberId).Count() > 0  ? _context.MemberMaster.Where(a => a.MemberID == MemberId).FirstOrDefault().MemberID : 0);
            if(member > 0 )
            {
                MemberMaster MemberMa =  _context.MemberMaster.Where(a => a.MemberID == MemberId).FirstOrDefault();
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
    }
}
