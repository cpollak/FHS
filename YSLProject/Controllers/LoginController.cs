using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("UserID");
            HttpContext.Session.Remove("UserType");
            HttpContext.Session.Remove("UserName");
            return RedirectToAction("Login", "Login");
        }
    }
}
