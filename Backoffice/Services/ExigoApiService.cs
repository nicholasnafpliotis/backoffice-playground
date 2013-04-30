using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Backoffice.Exigo.WebService;
using Backoffice.Exigo.OData;
using System.Text;
using System.Data.Services.Client;
using System.Xml.Linq;
using System.Net;
using System.IO;
using System.Data.SqlClient;

namespace Backoffice.Services
{
    public static class ExigoApiContext
    {
        #region Properties
        private static string LoginName = GlobalSettings.ExigoApiCredentials.LoginName;
        private static string Password = GlobalSettings.ExigoApiCredentials.Password;
        private static string Company = GlobalSettings.ExigoApiCredentials.CompanyKey;
        #endregion Properties

        #region Contexts
        #region Web Service Contexts
        public static ExigoApi CreateWebServiceContext()
        {
            return GetNewWebServiceContext(GlobalSettings.ExigoApiSettings.DefaultContextSource);
        }
        public static ExigoApi CreateWebServiceContext(ExigoApiContextSource source)
        {
            return GetNewWebServiceContext(source);
        }
        private static ExigoApi GetNewWebServiceContext(ExigoApiContextSource source)
        {
            var sourceUrl = "";
            switch(source)
            {
                case ExigoApiContextSource.Live:
                    sourceUrl = GlobalSettings.ExigoApiSettings.WebService.LiveUrl;
                    break;
                case ExigoApiContextSource.Sandbox1:
                    sourceUrl = GlobalSettings.ExigoApiSettings.WebService.Sandbox1Url;
                    break;
                case ExigoApiContextSource.Sandbox2:
                    sourceUrl = GlobalSettings.ExigoApiSettings.WebService.Sandbox2Url;
                    break;
                case ExigoApiContextSource.Sandbox3:
                    sourceUrl = GlobalSettings.ExigoApiSettings.WebService.Sandbox3Url;
                    break;
            }

            return new ExigoApi
            {
                ApiAuthenticationValue = new Exigo.WebService.ApiAuthentication
                {
                    LoginName = LoginName,
                    Password = Password,
                    Company = Company
                },
                Url = sourceUrl
            };
        }
        #endregion

        #region OData Contexts
        public static ExigoContext CreateODataContext()
        {
            return GetNewODataContext(GlobalSettings.ExigoApiSettings.DefaultContextSource);
        }
        public static ExigoContext CreateODataContext(ExigoApiContextSource source)
        {
            return GetNewODataContext(source);
        }
        private static ExigoContext GetNewODataContext(ExigoApiContextSource source)
        {
            var sourceUrl = "";
            switch(source)
            {
                case ExigoApiContextSource.Live:
                    sourceUrl = GlobalSettings.ExigoApiSettings.OData.LiveUrl;
                    break;
                case ExigoApiContextSource.Sandbox1:
                    sourceUrl = GlobalSettings.ExigoApiSettings.OData.Sandbox1Url;
                    break;
                case ExigoApiContextSource.Sandbox2:
                    sourceUrl = GlobalSettings.ExigoApiSettings.OData.Sandbox2Url;
                    break;
                case ExigoApiContextSource.Sandbox3:
                    sourceUrl = GlobalSettings.ExigoApiSettings.OData.Sandbox3Url;
                    break;
            }

            var context = new ExigoContext(new Uri(sourceUrl + "/model"));
            context.IgnoreMissingProperties = true;
            context.IgnoreResourceNotFoundException = true;
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(LoginName + ":" + Password));
            context.SendingRequest +=
                (object s, SendingRequestEventArgs e) =>
                    e.RequestHeaders.Add("Authorization", "Basic " + credentials);
            return context;
        }
        #endregion

        public static SqlConnection CreateSqlDapperContext()
        {
            return new SqlConnection(GlobalSettings.ConnectionStrings.Sql);
        }

        public static ExigoPaymentApi CreatePaymentContext()
        {
            return new ExigoPaymentApi();
        }

        public static ExigoImageApi CreateImagesContext()
        {
            return new ExigoImageApi();
        }
        #endregion
    }

    public class ExigoPaymentApi
    {
        private string PaymentLoginName = GlobalSettings.ExigoPaymentApiCredentials.LoginName;
        private string PaymentPassword  = GlobalSettings.ExigoPaymentApiCredentials.Password;

        /// <summary>
        /// Generate and return a new token to be used in an Exigo credit card transaction.
        /// </summary>
        /// <param name="creditCardNumber">The credit card number you wish to use for this transaction</param>
        /// <param name="expirationMonth">The expiration month of the credit card you wish to use for this transaction</param>
        /// <param name="expirationYear">The expiration year of the credit card you wish to use for this transaction</param>
        /// <returns></returns>
        public string FetchCreditCardToken(string creditCardNumber, int expirationMonth, int expirationYear)
        {
            var ns = "http://payments.exigo.com";
            var xRequest = new XDocument(
                new XElement(ns + "CreditCard",
                    new XElement(ns + "CreditCardNumber", creditCardNumber),
                    new XElement(ns + "ExpirationMonth", expirationMonth),
                    new XElement(ns + "ExpirationYear", expirationYear)
                    ));
            var xResponse = PostRest("https://payments.exigo.com/2.0/token/rest/CreateCreditCardToken", PaymentLoginName, PaymentPassword, xRequest);

            return xResponse.Root.Element(ns + "CreditCardToken").Value;
        }
        private XDocument PostRest(string url, string username, string password, XDocument element)
        {
            string postData = element.ToString();

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password)));
            request.Method = "POST";
            request.ContentLength = postData.Length;
            request.ContentType = "text/xml";

            var writer = new StreamWriter(request.GetRequestStream());
            writer.Write(postData);
            writer.Close();

            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                using(var responseStream = new StreamReader(response.GetResponseStream()))
                {
                    return XDocument.Parse(responseStream.ReadToEnd());
                }
            }
            catch(WebException ex)
            {
                var response = (HttpWebResponse)ex.Response;
                if(response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new Exception("Invalid Credentials");
                using(var responseStream = new StreamReader(ex.Response.GetResponseStream()))
                {
                    XNamespace ns = "http://schemas.microsoft.com/ws/2005/05/envelope/none";
                    XDocument doc = XDocument.Parse(responseStream.ReadToEnd());
                    throw new Exception(doc.Root.Element(ns + "Reason").Element(ns + "Text").Value);
                }
            }
        }
    }

    public class ExigoImageApi
    {
        public void SaveImage(string path, string filename, byte[] contents)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://api.exigo.com/4.0/" + GlobalSettings.ExigoApiCredentials.CompanyKey + "/images" + (path.StartsWith("/") ? "" : "/") + path + "/" + filename);
            request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(GlobalSettings.ExigoApiCredentials.LoginName + ":" + GlobalSettings.ExigoApiCredentials.Password)));
            request.Method = "POST";
            request.ContentLength = contents.Length;
            var writer = request.GetRequestStream();
            writer.Write(contents, 0, contents.Length);
            writer.Close();
            var response = (HttpWebResponse)request.GetResponse();
        }
    }
    public enum ExigoApiContextSource
    {
        Live,
        Sandbox1,
        Sandbox2,
        Sandbox3
    }
}