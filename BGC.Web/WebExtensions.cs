using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BGC.Web
{
    public static class WebExtensions
    {
        /// <summary>
        /// Gets the cookie with the given <paramref name="cookieName"/>. If it doesn't exist, it will be added with the specified expiration.
        /// </summary>
        /// <param name="cookies"></param>
        /// <param name="cookieName"></param>
        /// <param name="validity">The time from <see cref="DateTime.UtcNow"/> when the cookie will expire. Leave blank to make the cookie last
        /// forever.</param>
        /// <returns></returns>
        public static HttpCookie InitGet(this HttpCookieCollection cookies, string cookieName, TimeSpan validity = default(TimeSpan))
        {
            Shield.ArgumentNotNull(cookies).ThrowOnError();
            Shield.IsNotNullOrEmpty(cookieName).ThrowOnError();

            if (!cookies.AllKeys.Contains(cookieName))
            {
                HttpCookie cookie = new HttpCookie(cookieName)
                {
                    Expires = validity == default(TimeSpan) 
                        ? DateTime.MaxValue
                        : DateTime.UtcNow + validity
                };
                cookies.Add(cookie);
            }

            return cookies[cookieName];
        }
    }
}