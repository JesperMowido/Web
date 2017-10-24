using Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace BC.Web.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string cultureName = null;
            string currentCulture = string.Empty;
            string host = Request.Url.Host;

            if (host.Contains("mowido.se"))
            {
                currentCulture = "sv";
            }
            else
            {
                currentCulture = "en-US";
            }

            // Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = Request.Cookies["_culture" + currentCulture];
            if (cultureCookie != null)
                cultureName = cultureCookie.Value;
            else
            {
                cultureName = currentCulture;
                var cookie = new HttpCookie("_culture" + currentCulture);
                cookie.Value = currentCulture;
            }
                //cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ?
                //        Request.UserLanguages[0] :  // obtain it from HTTP header AcceptLanguages
                //        null;
            // Validate culture name
            cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            return base.BeginExecuteCore(callback, state);
        }
    }
}