using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
//using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using YSLProject.BAL;
using YSLProject.Models;
using YSLProject.Table;
using static YSLProject.Models.Enumdata;

namespace YSLProject.Controllers
{
    [Autho]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            HttpContext.Session.SetString("followupS","");
            HttpContext.Session.SetString("followupE", "");
            HttpContext.Session.SetString("Phone", "");
            HttpContext.Session.SetString("ResidenceID", "");
            HttpContext.Session.SetString("MedicaidID", "");
            HttpContext.Session.SetString("RecertMonth", "");
            HttpContext.Session.SetString("Facility", "");
            HttpContext.Session.SetString("Language", "");
            HttpContext.Session.SetString("MembershipStatus", "");
            HttpContext.Session.SetString("RecertMonth", "");
            DataTable dt = new DataTable();
            dt = new UserMasterBAL().GetUserList();
            List<UserMasterModel> UserList = new List<UserMasterModel>();
            UserList = (from DataRow dr in dt.Rows
                        select new UserMasterModel()
                        {
                            UserID = Convert.ToInt32(dr["UserID"]),
                            UserName = dr["UserName"].ToString(),
                            UserEmail = dr["UserEmail"].ToString(),
                            LoginDate = dr["LoginDate"].ToString(),
                            IsActives = (dr["IsActive"].ToString() == "True" ? "Yes" : "No"),
                            UserTypes = Enum.GetName(typeof(UserType), Convert.ToInt32(dr["UserType"]))
                        }).ToList();
            return View(UserList);
        }

        public IActionResult Create()
        {
            ViewBag.Usertype = (from UserType e in Enum.GetValues(typeof(UserType))
                                select new SelectListItem
                                {
                                    Value = Convert.ToString((int)e),
                                    Text = e.ToString().Replace("_", " ")
                                }).ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserMasterModel obj)
        {
            ViewBag.Usertype = (from UserType e in Enum.GetValues(typeof(UserType))
                                select new SelectListItem
                                {
                                    Value = Convert.ToString((int)e),
                                    Text = e.ToString().Replace("_", " ")
                                }).ToList();
            if (ModelState.IsValid)
            {
                var username = _context.UserMaster.Where(a => a.UserName == obj.UserName).ToList();
                if (username.Count > 0)
                {
                    ViewBag.ErrorMsg = "This username is already exists";
                    return View(obj);
                }
                obj.Password = PasswordEncrypt.encrypt(obj.Password);
                UserMaster objs = new UserMaster();
                objs.UserName = obj.UserName;
                objs.UserEmail = obj.UserEmail == null ? "" : obj.UserEmail;
                objs.Password = obj.Password;
                objs.IsActive = obj.IsActive;
                objs.LastLoginDate = DateTime.Now;
                objs.UserType = obj.UserType;
                _context.UserMaster.Add(objs);
                await _context.SaveChangesAsync();
                return RedirectToActionPermanent("Index");
            }
            return View(obj);
        }

        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null)
            {
                return RedirectToActionPermanent("Index");
            }
            ViewBag.Usertype = (from UserType e in Enum.GetValues(typeof(UserType))
                                select new SelectListItem
                                {
                                    Value = Convert.ToString((int)e),
                                    Text = e.ToString().Replace("_", " ")
                                }).ToList();
            var data = await _context.UserMaster.FindAsync(Id);
            UserMasterModel obj = new UserMasterModel();
            obj.UserName = data.UserName;
            obj.UserEmail = data.UserEmail;
            obj.Password = PasswordEncrypt.Decrypt(data.Password);
            obj.IsActive = data.IsActive;
            obj.UserType = data.UserType;
            obj.UserID = data.UserID;
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserMasterModel obj)
        {
            ModelState.Remove("ConfirmPassword");
            if (ModelState.IsValid)
            {
                UserMaster objs = new UserMaster();
                objs.UserName = obj.UserName;
                objs.UserEmail = obj.UserEmail;
                objs.Password = PasswordEncrypt.encrypt(obj.Password);
                objs.IsActive = obj.IsActive;
                objs.LastLoginDate = DateTime.Now;
                objs.UserType = obj.UserType;
                objs.UserID = obj.UserID;
                _context.Update(objs);
                await _context.SaveChangesAsync();
                return RedirectToActionPermanent("Index");
            }
            return View(obj);
        }

        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null)
            {
                return RedirectToActionPermanent("Index");
            }
            var data = await _context.UserMaster.FindAsync(Id);
            UserMasterModel obj = new UserMasterModel();
            obj.UserName = data.UserName;
            obj.UserEmail = data.UserEmail;
            obj.Password = PasswordEncrypt.Decrypt(data.Password);
            obj.IsActives = (data.IsActive == true ? "Yes" : "No");
            obj.UserTypes = Enum.GetName(typeof(UserType), Convert.ToInt32(data.UserType));
            obj.UserID = data.UserID;
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int Id)
        {
            var data = await _context.UserMaster.FindAsync(Id);
            _context.UserMaster.Remove(data);
            await _context.SaveChangesAsync();
            return RedirectToActionPermanent("Index");
        }


        //var data = _context.UserMaster.ToList();
        //var user = new SqlParameter("@Spara", "Select");
        //var data= _context.UserMaster.FromSqlRaw("SP_UserMaster @Spara", user).ToList();

        //var data = (from result in _context.UserMaster.FromSqlRaw("SP_UserMaster @Spara", user)
        //            select new UserMasterModel
        //            {
        //                UserID = result.UserID,
        //                UserName = result.UserName,
        //                UserEmail = result.UserEmail,
        //                UserTypes =  result.UserType.ToString(),
        //                IsActive = result.IsActive,
        //                //LoginDate = result.LoginDate
        //            }).ToList();
    }
}
