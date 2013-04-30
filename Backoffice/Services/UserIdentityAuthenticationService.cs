using Backoffice.Exigo.OData;
using Backoffice.Exigo.WebService;
using Backoffice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Backoffice.Services
{
    public interface IAuthenticationService
    {
        bool SignIn(string loginName, string password);
        void SignOut();
    }
    public class UserIdentityAuthenticationService : IAuthenticationService
    {
        /// <summary>
        /// Signs the customer into the backoffice.
        /// </summary>
        /// <param name="loginName">The customer's login name</param>
        /// <param name="password">The customer's password</param>
        /// <returns>Whether or not the customer was successfully signed in.</returns>
        public bool SignIn(string loginName, string password)
        {
            object oCustomerID  = null;

            var command = new SqlHelper();
            oCustomerID = command.GetField("AuthenticateCustomer {0}, {1}", loginName, password);

            if (oCustomerID == null) return false;
            else
            {
                // Save the login name into a cookie.
                SetLoginNameCookie(loginName);
                return CreateFormsAuthenticationTicket((int)oCustomerID);
            }
        }

        /// <summary>
        /// Signs the customer into the backoffice.
        /// </summary>
        /// <param name="customerID">The customer's ID.</param>
        /// <param name="loginName">The customer's login name.</param>
        /// <returns>Whether or not the customer was successfully signed in.</returns>
        public bool SilentLogin(int customerID, string loginName)
        {
            var cust = (from c in ExigoApiContext.CreateODataContext().Customers
                        where c.CustomerID == customerID
                        where c.LoginName == loginName
                        select new Backoffice.Exigo.OData.Customer { CustomerID = c.CustomerID }).FirstOrDefault();

            if (cust != null) return CreateFormsAuthenticationTicket(cust.CustomerID);
            else return false;
        }

        /// <summary>
        /// Signs the customer into the backoffice.
        /// </summary>
        /// <param name="sessionID">A SessionID created by the Exigo web service's LoginCustomer method.</param>
        /// <returns>Whether or not the customer was successfully signed in.</returns>
        public bool SilentLogin(string sessionID)
        {
            var response = ExigoApiContext.CreateWebServiceContext().GetLoginSession(new GetLoginSessionRequest
            {
                SessionID = sessionID
            });

            if (response.Result.Status == ResultStatus.Success && response.CustomerID > 0) return CreateFormsAuthenticationTicket(response.CustomerID);
            else return false;
        }

        /// <summary>
        /// Refreshes the current identity.
        /// </summary>
        /// <returns>Whether or not the customer was successfully refreshed.</returns>
        public bool RefreshIdentity()
        {
            var identity = HttpContext.Current.User.Identity as UserIdentity;
            return CreateFormsAuthenticationTicket(identity.CustomerID);
        }

        /// <summary>
        /// Signs the user out of the backoffice
        /// </summary>
        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        /// <summary>
        /// Saves the login name provided in a cookie.
        /// </summary>
        /// <param name="loginName">The user's login name.</param>
        private void SetLoginNameCookie(string loginName)
        {
            // Save the login name provided
            var cookie = HttpContext.Current.Request.Cookies[GlobalSettings.Backoffice.LoginNameCookieName];
            if (cookie == null)
            {
                cookie = new HttpCookie(GlobalSettings.Backoffice.LoginNameCookieName, loginName);
                cookie.HttpOnly=true;
                cookie.Expires = DateTime.Now.AddYears(3);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            else
            {
                cookie.Value = loginName;
                cookie.Expires = DateTime.Now.AddYears(3);
                HttpContext.Current.Response.Cookies.Set(cookie);
            }
        }


        


        /// <summary>
        /// Creates the forms authentication ticket
        /// </summary>
        /// <param name="customerID">The customer ID</param>
        /// <returns>Whether or not the ticket was created successfully.</returns>
        public bool CreateFormsAuthenticationTicket(int customerID)
        {
            var command = new SqlHelper();
            var row = command.GetRow(@"
                    SELECT 
                        c.CustomerID,
                        c.FirstName,
                        c.LastName,
                        c.Company,
                        c.MainCountry,
                        c.EnrollerID,
                        c.SponsorID,
                        c.LanguageID,
                        c.CustomerTypeID,
                        c.CustomerStatusID,
                        c.DefaultWarehouseID,
                        c.CurrencyCode,
                        c.CreatedDate
                    FROM 
                        Customers c
                    WHERE 
                        c.CustomerID = {0}
                ", customerID);


            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                customerID.ToString(),
                DateTime.Now,
                DateTime.Now.AddMinutes(GlobalSettings.Backoffice.SessionTimeoutInMinutes),
                false,
                string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}",
                    customerID,
                    row["FirstName"].ToString(),
                    row["LastName"].ToString(),
                    row["Company"].ToString(),
                    row["MainCountry"].ToString(),

                    (!Convert.IsDBNull(row["EnrollerID"])) ? Convert.ToInt32(row["EnrollerID"]) : 0,
                    (!Convert.IsDBNull(row["SponsorID"])) ? Convert.ToInt32(row["SponsorID"]) : 0,

                    Convert.ToInt32(row["LanguageID"]),
                    Convert.ToInt32(row["CustomerTypeID"]),
                    Convert.ToInt32(row["CustomerStatusID"]),
                    Convert.ToInt32(row["DefaultWarehouseID"]),
                    "",
                    row["CurrencyCode"].ToString(),
                    Convert.ToDateTime(row["CreatedDate"]).ToShortDateString()));

            // encrypt the ticket
            string encTicket = FormsAuthentication.Encrypt(ticket);

            // create the cookie.
            HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName]; //saved user
            if (cookie == null)
            {
                HttpContext.Current.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
            }
            else
            {
                cookie.Value = encTicket;
                HttpContext.Current.Response.Cookies.Set(cookie);
            }

            return true;
        }
    }
}