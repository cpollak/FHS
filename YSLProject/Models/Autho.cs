using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YSLProject.Models
{
    public class Autho : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {

        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (filterContext.HttpContext.Session.GetString("UserName") == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary { { "controller", "Login" }, { "action", "Login" } });
                return;
            }

            //var menus = JsonConvert.DeserializeObject<List<PermissionMasterModel>>(filterContext.HttpContext.Session.GetString("menus"));
            //var controllerName = filterContext.RouteData.Values["controller"];
            //var actionName = filterContext.RouteData.Values["action"];
            //string url = "/" + controllerName + "/" + actionName;
            //if (!menus.Where(s => s.URL == url).Any())
            //{
            //    filterContext.Result = new RedirectToRouteResult(
            //        new RouteValueDictionary { { "controller", "Login" }, { "action", "Login" } });
            //    return;
            //}
        }
    }
    
}
