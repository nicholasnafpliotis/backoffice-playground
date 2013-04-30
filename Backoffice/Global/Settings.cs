using Backoffice.Models;
using Backoffice.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Backoffice
{
    public static class GlobalSettings
    {
        public static class ExigoApiCredentials
        {
            public static string LoginName                          = "API_Username";
            public static string Password                           = "API_Password";
            public static string CompanyKey                         = "exigodemo";
        }

        public static class ExigoPaymentApiCredentials
        {
            public static string LoginName                          = "";
            public static string Password                           = "";
        }

        public static class ExigoApiSettings
        {
            public static ExigoApiContextSource DefaultContextSource = ExigoApiContextSource.Live;

            public static class WebService
            {
                public static string LiveUrl                        = "http://api.exigo.com/3.0/ExigoApi.asmx";
                public static string Sandbox1Url                    = "http://sandboxapi1.exigo.com/3.0/ExigoApi.asmx";
                public static string Sandbox2Url                    = "http://sandboxapi2.exigo.com/3.0/ExigoApi.asmx";
                public static string Sandbox3Url                    = "http://sandboxapi3.exigo.com/3.0/ExigoApi.asmx";
            }

            public static class OData
            {
                public static string LiveUrl                        = "http://api.exigo.com/4.0/" + GlobalSettings.ExigoApiCredentials.CompanyKey;
                public static string Sandbox1Url                    = "http://sandboxapi1.exigo.com/4.0/" + GlobalSettings.ExigoApiCredentials.CompanyKey;
                public static string Sandbox2Url                    = "http://sandboxapi2.exigo.com/4.0/" + GlobalSettings.ExigoApiCredentials.CompanyKey;
                public static string Sandbox3Url                    = "http://sandboxapi3.exigo.com/4.0/" + GlobalSettings.ExigoApiCredentials.CompanyKey;
            }
        }


        public static class LocalSettings
        {
            public static string TestLoginName                      = "www";
            public static string TestPassword                       = "testpsswd";
        }

        public static class ConnectionStrings
        {
            public static string Sql                                = ConfigurationManager.ConnectionStrings["sqlreporting"].ConnectionString;
        }

        public static class Backoffice
        {
            public static int SessionTimeoutInMinutes               = 30;
            public static string LoginNameCookieName                = GlobalSettings.ExigoApiCredentials.CompanyKey + "MobileBackofficeLoginName";
        }

        public static class Markets
        {
            public static string MarketCookieName                   = "SelectedMarket";
            public static List<Market> AvailableMarkets             = new List<Market>
                                                                        {
                                                                            new Market { 
                                                                                Name            = MarketName.UnitedStates,
                                                                                Description     = "United States",     
                                                                                CookieValue     = "US",         
                                                                                CultureCode     = "en-US",
                                                                                IsDefault       = true,
                                                                                Countries       = new List<string> { "US" }
                                                                            },

                                                                            new Market { 
                                                                                Name            = MarketName.Canada,
                                                                                Description     = "Canada",            
                                                                                CookieValue     = "CA",         
                                                                                CultureCode     = "en-US",
                                                                                Countries       = new List<string> { "CA" }
                                                                            }
                                                                        };
        }

        public static class Mail
        {
            public static string SMTPServerUrl                      = "mail.exigo.com";
            public static int SMTPServerPort                        = 26;
            public static bool SMTPSecureConnectionRequired         = false;
            public static string SMTPServerLoginName                = "noreply@exigonow.com";
            public static string SMTPServerPassword                 = "whodaman";
            public static string WebmasterEmailAddress              = "web@exigo.com";
            public static string NoReplyEmailAddress                = "noreply@exigonow.com";
        }

        public static class Shopping
        {
            public static int DefaultPriceTypeID                    = PriceTypes.Distributor;

            public static string ProductImagePath
            {
                get
                {
                    return string.Format(_productImagePath, GlobalSettings.ExigoApiCredentials.CompanyKey);
                }
            }
            private static string _productImagePath                 = "https://api.exigo.com/4.0/{0}/productimages/";
        }

        public static class CustomerImages
        {
            public static string BasePath
            {
                get
                {
                    return string.Format(_basePath, GlobalSettings.ExigoApiCredentials.CompanyKey);
                }
            }
            private static string _basePath                         = "https://api.exigo.com/4.0/{0}/images/";

            public static string CustomerImagesFolderName           = "customers";
            public static string CustomerImagesAvatarFolderName     = "avatars";
            public static string CustomerImagesPhotoFolderName      = "photos";
            public static string CustomerImagesVideoFolderName      = "videos";
            public static string CustomerImagesThumbnailFolderName  = "thumbs";
            public static int MaxThumbnailImageWidth                = 173;
            public static int MaxThumbnailImageHeight               = 117;
            public static int MaxImageWidth                         = 800;
            public static int MaxImageHeight                        = 1000;

            public static string DefaultTinyAvatarAsBase64          = "iVBORw0KGgoAAAANSUhEUgAAADYAAAA2CAYAAACMRWrdAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAA7FpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMC1jMDYxIDY0LjE0MDk0OSwgMjAxMC8xMi8wNy0xMDo1NzowMSAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wUmlnaHRzPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvcmlnaHRzLyIgeG1sbnM6eG1wTU09Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9tbS8iIHhtbG5zOnN0UmVmPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvc1R5cGUvUmVzb3VyY2VSZWYjIiB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iIHhtcFJpZ2h0czpNYXJrZWQ9IkZhbHNlIiB4bXBNTTpPcmlnaW5hbERvY3VtZW50SUQ9InV1aWQ6MEM4OTYxNjUyOUZGREQxMTg2MTNGRTAyMERGNzdFNzMiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6RjM2OTlGNThFN0Y1MTFFMUI1MkJBQjU2OERFOTYxMDAiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6RjM2OTlGNTdFN0Y1MTFFMUI1MkJBQjU2OERFOTYxMDAiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNS4xIE1hY2ludG9zaCI+IDx4bXBNTTpEZXJpdmVkRnJvbSBzdFJlZjppbnN0YW5jZUlEPSJ4bXAuaWlkOjA0NkVDNjY2MTUyMDY4MTE4QTZERTZDREFEOUI0RDhFIiBzdFJlZjpkb2N1bWVudElEPSJ1dWlkOjFDQjZBQ0E3NzRDNkUxMTE4RkVERDY3MTJBMTkyODZCIi8+IDwvcmRmOkRlc2NyaXB0aW9uPiA8L3JkZjpSREY+IDwveDp4bXBtZXRhPiA8P3hwYWNrZXQgZW5kPSJyIj8+eQaBtgAAAoBJREFUeNrsmt9KAkEUxsdaKsMyVtrSgo1KIdguSgq68qo7X6KLnqqLXqIX6KoICiqJ6A+4UEpGUhlYUth+U0l/jHbdYzsOc2BxUXaZ35w538x8Y2j/4KjOJAzt/TMkGVf9A4ytrm9Jkbm1lSWepC4maUgLprXrxT1aN0voEaYP9rNe5z42EObf31Sq7On5hRXLFed66BwwAC3NjLNUQm/6e9yB5TE1ymoO4KF9zXL5Er8XFgxZyS4mOZzbTkg7gBNGlG3snJLCdQUFRfVs28Eys6avhgEuPR0XCyw1pjfEwU9Y5jAbCPeIA2YaQ2Q9bZmGGGAYfih+qmioZtBgscEwowyKIS3syoOizoQEi8gKdnNflROMYgWiti0KTIEpsGDAKKRZSDBIM+UGkcouIBmKBULvovJYEwfMLt2SgdlXt+KAnVyWSYZjpVpj+dKdWKpI0aCTQlk8ud87K/p+B2w44cAwjPwoGrIlpP2G2D0vBprxtoEhY61kLWdf84wLvaTaOr7wPMHvEmerLWA4dEAG3Aa8e2rfHkHi3XMLbiTK/UWcsHhxhOHdz5rDfPWCiT5/dUcC6gsMVpk1Yfx6suKpY4wovzLWm0JC+pH9fwWDqZmeipOZm98DHYULQgSlbUWQPIHB75ufjvvOkJcOzOpJnkFMB16U0zUYgFALlEc9XjKIYQqhcTvfaW56LWOZZKcgfuoQQgPIzZz95/DUgqqjVgMdnF1I/ll/WrMHkSHRgH6rP+wqtp1Fwff6+wLG5dbnyeR/B9qMuXPz0P6ydWqsPHAquTw32VFQn+sPbQfDDzAMv06PzwzKV1RgCkyBKbCmE/Rioi8kI5h0f3h+FWAAYmXlAkyCDB8AAAAASUVORK5CYII=";
            public static string TinyAvatarImageName                = "tiny.png";
            public static int TinyAvatarWidth                       = 54;
            public static int TinyAvatarHeight                      = 54;

            public static string DefaultLargeAvatarAsBase64         = "iVBORw0KGgoAAAANSUhEUgAAAJsAAACuCAIAAACutOjFAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAA7FpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMC1jMDYxIDY0LjE0MDk0OSwgMjAxMC8xMi8wNy0xMDo1NzowMSAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wUmlnaHRzPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvcmlnaHRzLyIgeG1sbnM6eG1wTU09Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9tbS8iIHhtbG5zOnN0UmVmPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvc1R5cGUvUmVzb3VyY2VSZWYjIiB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iIHhtcFJpZ2h0czpNYXJrZWQ9IkZhbHNlIiB4bXBNTTpPcmlnaW5hbERvY3VtZW50SUQ9InV1aWQ6MEM4OTYxNjUyOUZGREQxMTg2MTNGRTAyMERGNzdFNzMiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6QjU3RTkyNDJGRThEMTFFMUJENENDRTQxRTY3MDlBMzIiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6QjU3RTkyNDFGRThEMTFFMUJENENDRTQxRTY3MDlBMzIiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNS4xIE1hY2ludG9zaCI+IDx4bXBNTTpEZXJpdmVkRnJvbSBzdFJlZjppbnN0YW5jZUlEPSJ4bXAuaWlkOjBBODAxMTc0MDcyMDY4MTE4OEM2OEQ2MjNCOUQ1MzUy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";
            public static string LargeAvatarImageName               = "large.png";
            public static int LargeAvatarWidth                      = 155;
            public static int LargeAvatarHeight                     = 174;
        }

        public static class SilentLogins
        {
            public static bool AllowSilentLogins                    = true;
            public static int TokenLifeSpan                         = 15;
            public static string EncryptionKey                      = "KHj4l4k2J2Hi4ug6BMN6vfHG1eJ0T9t1g";
            public static string IVKey                              = "silentloginivkey";
        }

        public static class Websites
        {
            public static string ReplicatedSite                     = "http://www.myexigo.com/public/{0}";
            public static string Signup                             = "http://www.myexigo.com/public/{0}/signup.aspx";
            public static string EmailVerification                  = "http://www.myexigo.com/public/{0}/VerifyOptIn.aspx";
            public static string ResetPassword                      = "http://www.myexigo.com/public/{0}/ResetPassword.aspx";
            public static string Twitter                            = "http://www.twitter.com/" + Company.Twitter;
            public static string Facebook                           = "http://www.facebook.com/" + Company.Facebook;
        }

        public static class Company
        {
            public static string Name                               = "Exigo Office, Inc.";
            public static string Address                            = "8130 John Carpenter Freeway";
            public static string City                               = "Dallas";
            public static string State                              = "Texas";
            public static string Zip                                = "75247";
            public static string Country                            = "United States";
            public static string Phone                              = "(214)367-9999";
            public static string Email                              = "info@exigo.com";
            public static string Twitter                            = "exigo";
            public static string Facebook                           = "platform";
        }
    }

    public static class Warehouses
    {
        public const int Default                                    = 1;
    }
    public static class PeriodTypes
    {
        public const int Default                                    = 1;
        public const int Monthly                                    = 1;
    }
    public static class NewsDepartments
    {
        public const int Backoffice                                 = 10;
    }
    public static class CustomerTypes
    {
        public const int RetailCustomer                             = 1;
        public const int Distributor                                = 2;
    }
    public static class CustomerStatusTypes
    {
        public const int Active                                     = 1;
    }
    public static class PriceTypes
    {
        public const int Distributor                                = 1;
    }
    public static class Languages
    {
        public const int English                                    = 0;
    }
    public enum MarketName
    {
        UnitedStates,
        Canada
    }
    public enum AddressType
    {
        Main    = 1,
        Mailing = 2,
        Other   = 3
    }
    public enum PaymentMethodType
    {
        PrimaryCreditCard   = 1,
        SecondaryCreditCard = 2,
        NewCreditCard       = 3
    }
    public enum DistributorSearchType
    {
        ID      = 1,
        Name    = 2,
        Company = 3,
        Rank    = 4
    }
}