using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JeevomShoppingCard.ViewModels
{
    public class SessionManager
    {
        public static class SessionManager
        {

            private static readonly List<UserSession> AllActiveSessions = new List<UserSession>();

            static SessionManager()
            {
                AllActiveSessions = new List<UserSession>();
            }

            public static string UserSessionCookie
            {
                get
                {
                    return HttpContext.Current.Request.Cookies[AppConstants.SessionId] != null
                        ? HttpContext.Current.Request.Cookies[AppConstants.SessionId].Value
                        : null;
                }
            }

            public static UserSession UserActiveSession
            {
                get
                {
                    var curSession = AllActiveSessions.FirstOrDefault(s => s.SessionId == UserSessionCookie);

                    if (curSession == null && UserSessionCookie != null)
                    {
                        curSession = UserSessionDataAccess.GetSessionFromDB(UserSessionCookie);

                        if (curSession != null)
                        {
                            AllActiveSessions.Add(curSession);
                            curSession = AllActiveSessions.FirstOrDefault(s => s.SessionId == UserSessionCookie);
                        }
                    }

                    return curSession;
                }
            }


            public static void CreateSession()
            {
                if (UserActiveSession != null) return;

                var newUserSession = new UserSession(Guid.NewGuid().ToString());

                newUserSession.AddSessionItem("IsAuthenticated", false);
                AllActiveSessions.Add(newUserSession);

                if (HttpContext.Current.Request.Cookies[AppConstants.SessionId] == null && HttpContext.Current.Response.Cookies[AppConstants.SessionId] == null)
                {
                    var cookie = new HttpCookie(AppConstants.SessionId)
                    {
                        Value = newUserSession.SessionId,
                        Expires = DateTime.Now.AddDays(1d)
                    };

                    cookie.HttpOnly = true;
                    cookie.Secure = true;

                    HttpContext.Current.Response.Cookies.Add(cookie);
                    HttpContext.Current.Request.Cookies.Add(cookie);
                }
                else
                {
                    // Need not check for null as we intend to overrite the cookie
                    HttpContext.Current.Response.Cookies[AppConstants.SessionId].Value = newUserSession.SessionId;
                    // Need not check for null as we intend to overrite the cookie
                    HttpContext.Current.Request.Cookies[AppConstants.SessionId].Value = newUserSession.SessionId;
                }

            }
            private static void DeleteCookies(string strCookieName)
            {
                if (HttpContext.Current.Request.Cookies[strCookieName] == null) return;

                var myCookie = new HttpCookie(strCookieName) { Expires = DateTime.Now.AddDays(-1d) };
                HttpContext.Current.Response.Cookies.Add(myCookie);
                HttpContext.Current.Request.Cookies.Add(myCookie);
            }
        }
    }
}