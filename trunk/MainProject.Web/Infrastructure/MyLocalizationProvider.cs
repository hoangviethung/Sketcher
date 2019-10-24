using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MainProject.Framework.Helper;
using ModelMetadata = System.Web.Mvc.ModelMetadata;

namespace MainProject.Web.Infrastructure
{
    public class MyLocalizationProvider : System.Web.Mvc.DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(
                             IEnumerable<Attribute> attributes,
                             Type containerType,
                             Func<object> modelAccessor,
                             Type modelType,
                             string propertyName)
        {
            HttpContext.Current.Application.Lock();
            var enumerable = attributes as Attribute[] ?? attributes.ToArray();
            foreach (var attr in enumerable)
            {
                if (attr != null)
                {
                    var typeName = attr.GetType().Name;
                    string attrAppKey;

                    string sKey;
                    string sLocalizedText;
                    if (typeName.Equals("DisplayAttribute"))
                    {
                        sKey = ((DisplayAttribute)attr).Name;

                        if (!string.IsNullOrEmpty(sKey))
                        {
                            attrAppKey = string.Format("{0}-{1}-{2}",
                            containerType.Name, propertyName, typeName);
                            if (HttpContext.Current.Application[attrAppKey] == null)
                            {
                                HttpContext.Current.Application[attrAppKey] = sKey;
                            }
                            else
                            {
                                sKey = HttpContext.Current.Application[attrAppKey].ToString();
                            }

                            sLocalizedText = ResourceHelper.GetResource(sKey);
                            if (string.IsNullOrEmpty(sLocalizedText))
                            {
                                sLocalizedText = sKey;
                            }

                            ((DisplayAttribute)attr).Name = sLocalizedText;
                        }
                    }
                    else if (attr is ValidationAttribute)
                    {
                        sKey = ((ValidationAttribute)attr).ErrorMessage;

                        if (!string.IsNullOrEmpty(sKey))
                        {
                            attrAppKey = string.Format("{0}-{1}-{2}",
                            containerType.Name, propertyName, typeName);
                            if (HttpContext.Current.Application[attrAppKey] == null)
                            {
                                HttpContext.Current.Application[attrAppKey] = sKey;
                            }
                            else
                            {
                                sKey = HttpContext.Current.Application[attrAppKey].ToString();
                            }

                            sLocalizedText = ResourceHelper.GetResource(sKey);
                            if (string.IsNullOrEmpty(sLocalizedText))
                            {
                                sLocalizedText = sKey;
                            }

                            ((ValidationAttribute)attr).ErrorMessage = sLocalizedText;
                        }
                    }
                }
            }
            HttpContext.Current.Application.UnLock();

            return base.CreateMetadata
              (enumerable, containerType, modelAccessor, modelType, propertyName);
        }
    }
}