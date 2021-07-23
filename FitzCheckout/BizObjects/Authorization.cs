using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace FitzCheckout.BizObjects
{
    public interface IAuthorization
    {
        bool IsAuthorized(HttpContext context, HttpCookie userCookie);
        bool IsAuthorized(HttpCookie userCookie, UserRole role);
    }
    public class Authorization: IAuthorization
    {
        private readonly IUser _user;

        public Authorization(IUser user)
        {
            _user = user;
        }
        public bool IsAuthorized(HttpContext context, HttpCookie userCookie)
        {
            if (userCookie != null)
            {
                var cookieValue = userCookie.Value;
                NameValueCollection qsCollection = HttpUtility.ParseQueryString(cookieValue);
                var userIDFromCookie = qsCollection["login"].ToString();
                var userNameFromCookie = qsCollection["name"].ToString();

                var currentUser = _user.GetUserByID(userIDFromCookie);
                //var context = new HttpContext(request, response);

                context.Session["currentUser"] = currentUser;
                context.Session["userName"] = currentUser.FullName;
                context.Session["userRole"] = currentUser.UserRole.ToString();
            }
            else
                return false;

            return true;
        }

        public bool IsAuthorized(HttpCookie userCookie, UserRole role)
        {
            throw new NotImplementedException();
        }
    
    }
}