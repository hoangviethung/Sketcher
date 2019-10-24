using System;
using System.Web;

namespace MainProject.Framework.Helper
{
    public class SessionHelper
    {
        public static T Read<T>(SessionItemKey key)
        {
            var sessionKey = key.ToString();
            var level = key.GetSessionType();
            if (level == SessionItemTypeCollection.GlobalSession)
            {
                try
                {
                    return (T)HttpContext.Current.Session[sessionKey];
                }
                catch (Exception exception) {}
            }
            if (level == SessionItemTypeCollection.TempDataSession)
            {
                try
                {
                    var value = (T)HttpContext.Current.Session[sessionKey];
                    HttpContext.Current.Session[sessionKey] = null;

                    return value;
                }
                catch (Exception ex) {}
            }

            return default(T);
        }

        public static void Write<T>(SessionItemKey key, T value)
        {
            string sessionKey = key.ToString();
            HttpContext.Current.Session[sessionKey] = value;
        }
    }
}
