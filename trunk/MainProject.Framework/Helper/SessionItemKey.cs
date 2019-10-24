using System;

namespace MainProject.Framework.Helper
{
    public enum SessionItemKey
    {
        [SessionItemTypeAttribute(SessionItemTypeCollection.TempDataSession)]
        [SessionItemDataTypeAttribute(typeof(string))]
        Title,

        [SessionItemTypeAttribute(SessionItemTypeCollection.TempDataSession)]
        [SessionItemDataTypeAttribute(typeof(string))]
        Message
    }

    public static class SessionItemKeyExtension
    {
        public static SessionItemTypeCollection GetSessionType(this SessionItemKey sessionItemKey)
        {
            var t = sessionItemKey.GetType();
            var memInfo = t.GetMember(sessionItemKey.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(SessionItemTypeAttribute), false);
            return ((SessionItemTypeAttribute)attributes[0]).SessionItemType;
        }
    }

    public enum SessionItemTypeCollection
    {
        GlobalSession,

        TempDataSession
    }

    public class SessionItemTypeAttribute : Attribute
    {
        private SessionItemTypeCollection _sessiontypeValue;

        public SessionItemTypeAttribute(SessionItemTypeCollection sessionType) : base()
        {
            _sessiontypeValue = sessionType;
        }

        public SessionItemTypeCollection SessionItemType
        {
            get { return _sessiontypeValue; }
        }
    }

    public class SessionItemDataTypeAttribute : Attribute
    {
        private Type _typeValue;

        public SessionItemDataTypeAttribute(Type type) : base()
        {
            _typeValue = type;
        }

        public Type SessionItemDataType
        {
            get { return _typeValue; }
        }
    }
}
