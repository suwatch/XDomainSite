﻿namespace XDomainSite
{
    using System;
    using System.Web;
    using System.Collections;
    using System.Text.RegularExpressions;

    public class CrossOriginModule : IHttpModule
    {
        public String ModuleName
        {
            get { return "CrossOriginModule"; }
        }

        public void Init(HttpApplication application)
        {
            application.BeginRequest += (new EventHandler(this.Application_BeginRequest));
        }

        private void Application_BeginRequest(Object source, EventArgs e)
        {
            HttpApplication application = (HttpApplication)source;
            HttpContext context = application.Context;
            if (CrossOriginHandler.SetAllowCrossSiteRequestOrigin(context))
            {
                if (context.Request.HttpMethod == "OPTIONS")
                {
                    CrossOriginHandler.SetAllowCrossSiteRequestHeaders(context);
                    context.Response.End();
                    context.ApplicationInstance.CompleteRequest();
                    return;
                }
            }
        }

        public void Dispose()
        {
        }
    }

    public class CrossOriginHandler : IHttpHandler
    {
        #region IHttpHandler Members
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            //Clear the response (just in case)
            ClearResponse(context);

            //Checking the method
            switch (context.Request.HttpMethod.ToUpper())
            {
                //Cross-Origin preflight request
                case "OPTIONS":
                    SetAllowCrossSiteRequestHeaders(context);
                    SetAllowCrossSiteRequestOrigin(context);
                    context.Response.End();
                    break;

                default:
                    context.Response.Headers.Add("Allow", "OPTIONS");
                    context.Response.StatusCode = 405;
                    break;
            }

            context.ApplicationInstance.CompleteRequest();
        }
        #endregion

        #region Methods
        protected void ClearResponse(HttpContext context)
        {
            context.Response.ClearHeaders();
            context.Response.ClearContent();
            context.Response.Clear();
        }

        protected void SetNoCacheHeaders(HttpContext context)
        {
            context.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            context.Response.Cache.SetValidUntilExpires(false);
            context.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.Cache.SetNoStore();
        }
        #endregion

        public static void SetAllowCrossSiteRequestHeaders(HttpContext context)
        {
            string requestMethod = context.Request.Headers["Access-Control-Request-Method"];

            context.Response.AppendHeader("Access-Control-Allow-Methods", "GET,POST");

            string requestHeaders = context.Request.Headers["Access-Control-Request-Headers"];
            if (!String.IsNullOrEmpty(requestHeaders))
                context.Response.AppendHeader("Access-Control-Allow-Headers", requestHeaders);
        }

        public static bool SetAllowCrossSiteRequestOrigin(HttpContext context)
        {
            string origin = context.Request.Headers["Origin"];
            if (!String.IsNullOrEmpty(origin))
            {
                bool match = Regex.IsMatch(origin, "http://sso[^./]+[.]kudu1[.]antares-test[.]windows-int[.]net/?");
                if (!match)
                {
                    context.Response.End();
                    context.ApplicationInstance.CompleteRequest();
                    return false;
                }

                context.Response.AppendHeader("Access-Control-Allow-Origin", origin);
            }

            return true;
        }
    }
}