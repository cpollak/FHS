using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using YSLProject.Models;
using YSLProject.Table;
using static YSLProject.Models.Enumdata;

namespace YSLProject.Controllers
{
    
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            UserMaster obj = new UserMaster();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserMaster obj)
        {
            ModelState.Remove("UserEmail");
            if (ModelState.IsValid)
            {
                
                var data = _context.UserMaster.Where(a => a.UserName.ToLower() == obj.UserName.ToLower() && a.Password == PasswordEncrypt.encrypt(obj.Password)).ToList();
                if (data.Count > 0)
                {
                    int UserId = data.FirstOrDefault().UserID;
                    var UserData = _context.UserMaster.AsNoTracking().Where(s => s.UserID == UserId).FirstOrDefault();
                    if (UserData != null)
                    {


                        UserData.LastLoginDate = DateTime.Now;

                        _context.Update(UserData);
                        _context.SaveChanges();
                    }
                    
                    //["UserId"] = UserId;
                    HttpContext.Session.SetString("UserID", UserId.ToString());
                    HttpContext.Session.SetString("UserType", Enum.GetName(typeof(UserType), Convert.ToInt32(data.FirstOrDefault().UserType)));
                    HttpContext.Session.SetString("UserName", data.FirstOrDefault().UserName);
                    HttpContext.Session.SetString("UserTypeId",data.FirstOrDefault().UserType.ToString());
                    //var Permissiondate = _context.PermissionMaster.Where(a => a.UserId == Convert.ToInt32(UserId)).ToList();
                    //List<PermissionMasterModel> objs = new List<PermissionMasterModel>();
                    //if (Permissiondate.Count > 0)
                    //{
                    //    foreach(var ms in Permissiondate)
                    //    {
                    //        PermissionMasterModel uo = new PermissionMasterModel();
                    //        uo.ControllerName = ms.ControllerName;
                    //        uo.FunctionName = ms.FunctionName;
                    //        uo.URL = ms.URL;
                    //        objs.Add(uo);
                    //    }
                    //}
                    //HttpContext.Session.SetString("menus", JsonConvert.SerializeObject(objs));
                    return RedirectToAction("Index", "Dashboard");
                }

            }
            ViewBag.ErrorMsg = "Username and password does not match ";
            return View(obj);
        }

        public IActionResult ForgetPassword()
        {
            UserMasterModel obj = new UserMasterModel();
            obj.UserID = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(UserMasterModel obj)
        {
            ModelState.Remove("UserName");
            ModelState.Remove("UserEmail");
            ModelState.Remove("IsActive");
            if (ModelState.IsValid)
            {
                var data = _context.UserMaster.Where(a=>a.UserID == obj.UserID).FirstOrDefault();
                if (data !=null)
                {
                    data.Password = PasswordEncrypt.encrypt(obj.Password);
                    data.LastLoginDate = DateTime.Now;
                    _context.UserMaster.Update(data);
                    await _context.SaveChangesAsync();
                    HttpContext.Session.Remove("UserID");
                    HttpContext.Session.Remove("UserType");
                    HttpContext.Session.Remove("UserName");
                }
            }
            return RedirectToAction("Login", "Login");
        }
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("UserID");
            HttpContext.Session.Remove("UserType");
            HttpContext.Session.Remove("UserName");

            HttpContext.Session.Remove("followupS");
            HttpContext.Session.Remove("followupE");
            HttpContext.Session.Remove("Phone");
            HttpContext.Session.Remove("ResidenceID");
            HttpContext.Session.Remove("MedicaidID");
            HttpContext.Session.Remove("RecertMonth");
            HttpContext.Session.Remove("Facility");
            HttpContext.Session.Remove("Language");
            HttpContext.Session.Remove("MembershipStatus");
            HttpContext.Session.Remove("RecertMonth");

            return RedirectToAction("Login", "Login");
        }

        public IActionResult ResetPassword()
        {
            UserMasterModel obj = new UserMasterModel();
            obj.UserID = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(UserMasterModel obj)
        {
            ModelState.Remove("UserName");
            ModelState.Remove("Name");
            ModelState.Remove("IsActive");
            ModelState.Remove("UserEmail");
            if (ModelState.IsValid)
            {
                try
                {
                    var data = _context.UserMaster.Where(a => a.UserID == obj.UserID).FirstOrDefault();
                    if (data != null)
                    {
                        data.Password = PasswordEncrypt.encrypt(obj.Password);
                        data.LastLoginDate = DateTime.Now;
                        _context.UserMaster.Update(data);
                        await _context.SaveChangesAsync();
                        HttpContext.Session.Remove("UserID");
                        HttpContext.Session.Remove("UserType");
                        HttpContext.Session.Remove("UserName");
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }


            }
            return RedirectToAction("Login", "Login");
        }
    }
}
