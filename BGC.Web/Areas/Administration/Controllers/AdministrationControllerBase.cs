using BGC.Core;
using BGC.Utilities;
using BGC.Web.Areas.Administration.Models;
using BGC.Web.Controllers;
using BGC.Web.Services;
using CodeShield;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using static BGC.Web.WebApiApplication;

namespace BGC.Web.Areas.Administration.Controllers
{
    [AdminAreaAuthorization]
    public class AdministrationControllerBase : BgcControllerBase
    {
        private string _encryptionKey;
        internal string EncryptionKey
        {
            get
            {
                return _encryptionKey ?? (_encryptionKey = ConfigurationManager.AppSettings["EncryptionKey"]);
            }

            set
            {
                _encryptionKey = value;
            }
        }

        public override CultureInfo CurrentLocale => UserLocale.EffectiveValue;

        private UserLocaleDependencyValue _userLocale;
        public UserLocaleDependencyValue UserLocale
        {
            get
            {
                if (_userLocale == null)
                {
                    HttpContext.Response.Cookies[LocaleCookieName][LocaleRouteTokenName] = HttpContext.Request.Cookies[LocaleCookieName][LocaleRouteTokenName];
                    _userLocale = new UserLocaleDependencyValue(ApplicationProfile.SupportedLanguages, HttpContext.Response.Cookies[LocaleCookieName], LocaleRouteTokenName);
                    _userLocale.DbSetting.SetValue(UserProfile?.PreferredLocale);
                }

                return _userLocale;
            }

            protected set
            {
                Shield.ValueNotNull(value).ThrowOnError();
                _userLocale = value;
            }
        }

        protected string Encrypt(string text)
        {
            Shield.ArgumentNotNull(text, nameof(text));

            string result = Encoding.Unicode.GetBytes(text).Encrypt(EncryptionKey).ToBase62();
            return result;
        }

        protected string Decrypt(string base62String)
        {
            Shield.ArgumentNotNull(base62String, nameof(base62String));

            string result = Encoding.Unicode.GetString(base62String.FromBase62().Decrypt(EncryptionKey));
            return result;
        }

        private BgcUserManager _userManager;
        [Dependency]
        public BgcUserManager UserManager
        {
            get
            {
                return _userManager;
            }

            set
            {
                Shield.Assert(value, _userManager == null, x => new InvalidOperationException("User Manager cannot be set more than once"));
                _userManager = value;
            }
        }

        private SignInManager<BgcUser, long> _signInManager;
        [Dependency]
        public SignInManager<BgcUser, long> SignInManager
        {
            get
            {
                return _signInManager;
            }

            set
            {
                _signInManager = value.ValueNotNull(nameof(SignInManager)).GetValueOrThrow();
            }
        }

        private BgcUser _user;
        private bool _userInitialized;
        public new BgcUser User
        {
            get
            {
                if (!_userInitialized && (base.User?.Identity.IsAuthenticated ?? false))
                {
                    _user = UserManager.FindByName(base.User.Identity.Name);
                    _userInitialized = true;
                }

                return _user;
            }
        }

        private UserProfile _userProfile;
        public UserProfile UserProfile =>
            User != null
                ? _userProfile ?? (_userProfile = new UserProfile(User))
                : null;
    }
}