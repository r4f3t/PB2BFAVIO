using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PB2B.classes
{
    public class UserAuthorize:AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Request.Cookies["osmankurtnet"] != null)
            {
                return true;
            }
            else
            {
                httpContext.Response.Redirect("/Yonetim/Login/Index");
                return false;
            }

        }
    }
}