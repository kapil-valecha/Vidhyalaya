using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vidhyalaya.Controllers
{
    public class SessionController : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if ( HttpContext.Current.Session["RoleId"].Equals(null) &&
                HttpContext.Current.Session["RoleName"].Equals(null))
            {
                filterContext.Result = new RedirectResult("~/UserLogin/Login");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
