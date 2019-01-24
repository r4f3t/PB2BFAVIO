using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PB2B.classes
{
    public class LoginAuthorize : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {

                if (httpContext.Session["giris"].ToString() != null)
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