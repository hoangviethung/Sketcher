using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using MainProject.Core.Enums;

namespace MainProject.Framework.Helper
{
    public static class EnumHelper
    {
        public static IDictionary<Enum, string> ToDictionary(this Enum enumObj)
        {
            return Enum.GetValues(enumObj.GetType())
                .Cast<Enum>().ToDictionary(t => t, t => t.GetDescription());
        }

        public static string GetDescription(this Enum value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            var description = value.ToString();
            var fieldInfo = value.GetType().GetField(description);
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
            {
                description = attributes[0].Description;
            }

            return description;
        }

        public static string GetName(this Enum value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            string name = value.ToString();
            var fieldInfo = value.GetType().GetField(name);

            return fieldInfo.Name;
        }

        public static IDictionary<Enum, string> ToDictionary(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            var dics = new Dictionary<Enum, string>();
            var enumValues = Enum.GetValues(type);

            foreach (Enum value in enumValues)
            {
                dics.Add(value, GetDescription(value));
            }

            return dics;
        }

        public static List<SelectListItem> ToSelectList(Type type, Enum selectValue = null, SelectListItem root = null)
        {
            var enumValues = Enum.GetValues(type);
            var selectList = new List<SelectListItem>();
            if (root != null)
            {
                selectList.Add(root);
            }
            if (selectValue == null)
            {
                foreach (Enum value in enumValues)
                {
                    selectList.Add(new SelectListItem() { Text = GetDescription(value), Value = value.ToString() });
                }
            }
            else
            {
                foreach (Enum value in enumValues)
                {
                    selectList.Add(new SelectListItem() { Selected = selectValue.ToString() == value.ToString(), Text = GetDescription(value), Value = value.ToString() });
                }
            }

            return selectList;
        }

        /// <summary>
        /// Filter template by attribute for filtering Category
        /// </summary>
        /// <param name="type"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static List<DisplayTemplateCollection> GetFilterTemplates(FilterGroup attribute)
        {
            var enumValues = Enum.GetValues(typeof(DisplayTemplateCollection));
            var selectList = new List<DisplayTemplateCollection>();

            foreach (Enum value in enumValues)
            {
                if (value.GetAttribute<TemplateAttribute>() != null
                 && value.GetAttributes<TemplateAttribute>().Any(x => x.Group == attribute))
                {
                    selectList.Add((DisplayTemplateCollection)value);
                }
            }
            return selectList;
        }

        public static bool IsExcept(this Enum value, FilterGroup attribute)
            => !value.GetAttributes<TemplateAttribute>().Any(x => x.Group == attribute && x.IsExcept);

        public static TAttribute GetAttribute<TAttribute>(this Enum value)
            where TAttribute : Attribute
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            return type.GetField(name) // I prefer to get attributes this way
                .GetCustomAttributes(false)
                .OfType<TAttribute>()
                .FirstOrDefault();
        }

        public static List<TAttribute> GetAttributes<TAttribute>(this Enum value)
            where TAttribute : Attribute
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            return type.GetField(name) // I prefer to get attributes this way
                .GetCustomAttributes(false)
                .OfType<TAttribute>()
                .ToList();
        }
    }
}
