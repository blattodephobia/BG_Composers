using BGC.Core;
using BGC.Utilities;
using BGC.Web.Controllers;
using CodeShield;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

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

        private BgcUserManager userManager;
        [Dependency]
        public BgcUserManager UserManager
        {
            get
            {
                return this.userManager;
            }

            set
            {
                Shield.Assert(value, this.userManager == null, x => new InvalidOperationException("User Manager cannot be set more than once"));
                this.userManager = value;
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

        private BgcUser user;
        private bool userInitialized;
        public new BgcUser User
        {
            get
            {
                if (!userInitialized && (base.User?.Identity.IsAuthenticated ?? false))
                {
                    this.user = UserManager.FindByName(base.User.Identity.Name);
                    userInitialized = true;
                }

                return this.user;
            }
        }
    }
}