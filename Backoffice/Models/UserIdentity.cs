using  Backoffice;
using Backoffice.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace Backoffice.Models
{
    public static class Identity
    {
        public static UserIdentity Current
        {
            get { return HttpContext.Current.User.Identity as UserIdentity; }
        }
    }

    public class UserIdentity : IIdentity
    {
        public UserIdentity(System.Web.Security.FormsAuthenticationTicket ticket)
        {
            string[] a = ticket.UserData.Split('|');
            Name = ticket.Name;

            // WebIdentity Variables
            CustomerID                     = int.Parse(GlobalUtilities.Coalesce(a[0], "0"));
            FirstName                      = GlobalUtilities.Coalesce(a[1], "");
            LastName                       = GlobalUtilities.Coalesce(a[2], "");
            Company                        = GlobalUtilities.Coalesce(a[3], "");
            Country                        = GlobalUtilities.Coalesce(a[4], "");

            EnrollerID                     = Convert.ToInt32(a[5]);
            SponsorID                      = Convert.ToInt32(a[6]);

            LanguageID                     = int.Parse(GlobalUtilities.Coalesce(a[7], Languages.English.ToString()));
            CustomerTypeID                 = int.Parse(GlobalUtilities.Coalesce(a[8], CustomerTypes.Distributor.ToString()));
            CustomerStatusID               = int.Parse(GlobalUtilities.Coalesce(a[9], CustomerStatusTypes.Active.ToString()));
            DefaultWarehouseID             = int.Parse(GlobalUtilities.Coalesce(a[10], Warehouses.Default.ToString()));
            PriceTypeID                    = int.Parse(GlobalUtilities.Coalesce(a[11], PriceTypes.Distributor.ToString()));
            CurrencyCode                   = GlobalUtilities.Coalesce(a[12], "usd");
            CreatedDate                    = Convert.ToDateTime(a[13]);

            Expires = ticket.Expiration;
        }

        #region Settings
        string IIdentity.AuthenticationType
        {
            get { return "Custom"; }
        }
        bool IIdentity.IsAuthenticated
        {
            get { return true; }
        }
        public string Name { get; set; }
        #endregion

        #region Properties
        public int CustomerID   { get; set; }
        public string FirstName { get; set; }
        public string LastName  { get; set; }
        public string Company   { get; set; }
        public string Country   { get; set; }

        public int EnrollerID { get; set; }
        public int SponsorID  { get; set; }
    
        public int LanguageID         { get;  set; }
        public int CustomerTypeID     { get; set; }
        public int CustomerStatusID   { get; set; }
        public int DefaultWarehouseID { get; set; }
        public int PriceTypeID        { get; set; }
        public string CurrencyCode    { get; set; }
        public DateTime CreatedDate   { get; set; }
    
        public string DisplayName
        {
            get { return GlobalUtilities.Coalesce(this.Company, this.FirstName + " " + this.LastName); }
        }
        public Market Market 
        {
            get { return GlobalSettings.Markets.AvailableMarkets.Where(c => c.Countries.Contains(this.Country)).FirstOrDefault(); }
        }
        public DateTime Expires { get; set; }
        #endregion

        #region Serialization
        public static UserIdentity Deserialize(string data)
        {
            try
            {
                var ticket = FormsAuthentication.Decrypt(data);
                return new UserIdentity(ticket);
            }
            catch
            {
                var service = new UserIdentityAuthenticationService();
                service.SignOut();
                return null;
            }
        }
        #endregion
    }
}