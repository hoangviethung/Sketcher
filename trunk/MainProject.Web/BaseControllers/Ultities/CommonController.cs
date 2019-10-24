using System;
using System.Linq;
using System.Web.Mvc;
using MainProject.Core.Enums;
using MainProject.Framework.ActionResults;
using MainProject.Framework.Helper;

namespace MainProject.Web.BaseControllers.Ultities
{
    public class CommonController : BaseController
    {
        public CommonController()
        {
            cultureKey = CultureHelper.GetCurrentLanguage();
        }

        public ActionResult ChangeCulture(string culture, string url)
        {
            var newCurrentLanguage = DbContext.Languages.FirstOrDefault(c => c.LanguageKey.Equals(culture, StringComparison.OrdinalIgnoreCase));
            if (newCurrentLanguage == null)
            {
                return Redirect("/");
            }
            CultureHelper.SaveCurrentLanguage(newCurrentLanguage.LanguageKey);

            //var routerModel = UrlRecordHelper.GetUrlRecordFromUrl(WebUtility.UrlDecode(url));
            return Redirect("/");
        }

        public void UpdateCulture(string culture)
        {
            var newCurrentLanguage = DbContext.Languages.FirstOrDefault(c => c.LanguageKey.Equals(culture, StringComparison.OrdinalIgnoreCase));
            if (newCurrentLanguage != null)
            {
                CultureHelper.SaveCurrentLanguage(newCurrentLanguage.LanguageKey);
            }
        }

        public ActionResult Redirect()
        {
            var request = Request.Url.PathAndQuery;
            if (request.Equals("/_common-settings/ReGenerateDb", StringComparison.OrdinalIgnoreCase))
            {
                return
                    new MVCTransferResult(
                        new
                        {
                            controller = "Service",
                            action = "ReGenerateDb"
                        });
            }
            // Ecommerce prevent customer account can pass maintain page
            //else if (ConfigItemHelper.IsMaintainance() && !AccountHelper.IsAdmin())
            else if (ConfigItemHelper.IsMaintainance() && !Request.IsAuthenticated)
            {
                return
                    new MVCTransferResult(
                        new
                        {
                            controller = "Home",
                            action = "Maintainance"
                        });
            }
            if (request.StartsWith("/_common-settings/", StringComparison.OrdinalIgnoreCase))
            {
                if (request.StartsWith("/_common-settings/ChangeCulture", StringComparison.OrdinalIgnoreCase))
                {
                    var parameters = request.Substring(request.IndexOf("?") + 1).Split('&');
                    if (parameters.Count() == 2)
                    {
                        var isValid = true;
                        var culture = string.Empty;
                        var newUrl = string.Empty;
                        foreach (var parameter in parameters)
                        {
                            var dictionary = parameter.Split('=');
                            if (dictionary.Count() == 2)
                            {
                                if (!dictionary[0].Equals("culture") && !dictionary[0].Equals("url"))
                                {
                                    isValid = false;
                                }
                                else
                                {
                                    if (dictionary[0].Equals("culture")) culture = dictionary[1];
                                    if (dictionary[0].Equals("url")) newUrl = dictionary[1];
                                }
                            }
                        }
                        if (isValid)
                        {
                            return
                                new MVCTransferResult(
                                    new { controller = "Common", action = "ChangeCulture", culture, url = newUrl });
                        }
                    }
                }
            }
            else
            {
                var paramValues = request.Split('/').ToList();
                if (paramValues.Count > 0)
                {
                    var lastParamValue = paramValues[paramValues.Count - 1];
                    if (!lastParamValue.Contains("."))
                    {
                        var dbContext = DalHelper.InitDbContext();
                        DalHelper.SaveDbContextToRequest(dbContext);

                        var routerModel = UrlRecordHelper.GetUrlRecordFromUrl(request);

                        if (routerModel.HasResult)
                        {
                            var urlRecord = routerModel.UrlRecord;

                            // Switch language by url
                            CultureHelper.SaveCurrentLanguage(urlRecord.Language.LanguageKey);
                            var controllerName = string.Empty;
                            var actionName = string.Empty;
                            switch (
                                (EntityTypeCollection)
                                Enum.Parse(typeof(EntityTypeCollection), urlRecord.EntityType.ToString()))
                            {
                                case EntityTypeCollection.Articles:
                                    controllerName = "Home";
                                    actionName = "Article";
                                    break;
                                //case EntityTypeCollection.Attachments:
                                //    controllerName = "Home";
                                //    actionName = "Download";
                                //    break;
                                case EntityTypeCollection.Categories:
                                    controllerName = "Home";
                                    actionName = "Category";
                                    break;
                                default:
                                    break;
                            }
                            if (!string.IsNullOrEmpty(controllerName) && !string.IsNullOrEmpty(actionName))
                            {
                                return
                                    new MVCTransferResult(
                                        new
                                        {
                                            controller = controllerName,
                                            action = actionName,
                                            id = urlRecord.EntityId,
                                            page = routerModel.PageIndex,
                                            breadcumUrl = urlRecord.Url,
                                            otherParams = routerModel.OtherParams
                                        });
                            }
                        }
                        // Incase url equal "/" at the end of non param-url
                        else if (!Request.Url.AbsolutePath.Equals("/", StringComparison.OrdinalIgnoreCase))
                        {
                            //In this case, may be there is a controller handle this request
                            //return
                            //    new MVCTransferResult(
                            //        new { controller = lastParamValue, action = "Index", id = 0 });
                            ViewBag.IsHome = "Error";
                            return View("~/Views/Shared/Error.cshtml");
                        }
                    }
                }
            }

            return new MVCTransferResult(new { controller = "Home", action = "Index" });
        }
    }
}
