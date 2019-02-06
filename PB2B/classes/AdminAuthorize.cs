using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PB2B.classes
{
    public class AdminAuthorize : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {

                if (httpContext.Session["yetki"].ToString() == "3")
                {
                    return true;
                }
                else
                {
                    httpContext.Response.Redirect("/Home/Index");
                    return false;
                }

            }
            catch (Exception)
            {
                httpContext.Response.Redirect("/Home/Index");
                return false;
            }

        }
    }
}