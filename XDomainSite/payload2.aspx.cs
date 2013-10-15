using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace XDomainSite
{
    public partial class payload2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var origin = Request.Headers["Origin"];
            //if (!String.IsNullOrEmpty(origin))
            //{
            //    bool match = Regex.IsMatch(origin, "http://[^./]+[.]kudu1[.]antares-test[.]windows-int[.]net/?");

            //    if (match)
            //    {
            //        //Response.Headers.Add("Access-Control-Allow-Origin", origin);
            //        //Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            //        //Response.Headers.Add("Access-Control-Request-Headers", "Authorization");
            //        //Response.Headers.Add("Access-Control-Allow-Methods", "POST,GET,PUT,DELETE,OPTIONS");
            //        //Response.Headers.Add("Access-Control-Allow-Headers", "Authorization");
            //        // Access-Control-Max-Age: 1728000
            //    }

            //    if (!match || Request.HttpMethod == "OPTIONS")
            //    {
            //        Response.End();
            //        return;
            //    }

            //    Response.Write("<p>Rexex = " + match + "</p>");
            //}

            Response.Write("<p>Origin = " + origin + "</p>");
            Response.Write("<p>" + System.Environment.MachineName + "</p>");
            Response.Write("<p>" + Request.Url + "</p>");
            Response.Write("<p>Authorization = " + Request.Headers["Authorization"] + "</p>");
            Response.Write("<p>Authorizationx = " + Request.Headers["Authorizationx"] + "</p>");
            Response.Write("<p>UserAgent = " + Request.UserAgent + "</p>");
            Response.End();
        }
    }
}