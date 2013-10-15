<% Response.AddHeader("Access-Control-Allow-Origin","http://sso02.kudu1.antares-test.windows-int.net") %>
"<%= System.Environment.MachineName %>, <%= Request.Url.AbsoluteUri %>"
