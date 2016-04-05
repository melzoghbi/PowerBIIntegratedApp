using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace PowerBIWebApp.Controllers
{
    public class PowerBIController : Controller
    {
        public AuthenticationResult authResult { get; set; }
        string baseUri = "https://api.powerbi.com/beta/myorg/";


        // GET: /<controller>/
        public ActionResult Index()
        {
            //Test for AuthenticationResult
            if (TempData["authResult"] != null )
            {
                //Get the authentication result from the session
                authResult = (AuthenticationResult)TempData["authResult"];

                //Set user and token from authentication result
                ViewBag.Email = authResult.UserInfo.DisplayableId;
                ViewBag.AccessToken = authResult.AccessToken;
            }

            return View();
        }
        public ActionResult Redirect()
        {

            //Redirect uri must match the redirect_uri used when requesting Authorization code.
            string redirectUri = "http://localhost:1109/PowerBI/Redirect";
            string authorityUri = "https://login.windows.net/common/oauth2/authorize/";

            // Get the auth code
            string code = Request.Params.GetValues(0)[0];

            // Get auth token from auth code       
            TokenCache TC = new TokenCache();

            AuthenticationContext AC = new AuthenticationContext(authorityUri, TC);
            ClientCredential cc = new ClientCredential
                (Properties.Settings.Default.clientId,
                Properties.Settings.Default.clientSecret);

            AuthenticationResult AR = AC.AcquireTokenByAuthorizationCode(code, new Uri(redirectUri), cc);

            //Set Session "authResult" index string to the AuthenticationResult
            TempData["authResult"] = AR;

            return RedirectToAction("Index");
        }
        public ActionResult o365Login()
        {
            //Create a query string
            //Create a sign-in NameValueCollection for query string
            var @params = new System.Collections.Specialized.NameValueCollection
            {
                //Azure AD will return an authorization code. 
                //See the Redirect class to see how "code" is used to AcquireTokenByAuthorizationCode
                {"response_type", "code"},

                //Client ID is used by the application to identify themselves to the users that they are requesting permissions from. 
                //You get the client id when you register your Azure app.
                {"client_id", Properties.Settings.Default.clientId},

                //Resource uri to the Power BI resource to be authorized
                {"resource", "https://analysis.windows.net/powerbi/api"},

                //After user authenticates, Azure AD will redirect back to the web app
                {"redirect_uri", "http://localhost:1109/PowerBI/Redirect"}
            };

            //Create sign-in query string
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString.Add(@params);

            //Redirect authority
            //Authority Uri is an Azure resource that takes a client id to get an Access token
            string authorityUri = "https://login.windows.net/common/oauth2/authorize/";
            var authUri = String.Format("{0}?{1}", authorityUri, queryString);

            Response.Redirect(authUri);
                       
            return View("~/Views/PowerBI/Index.cshtml");
        }

    }
}