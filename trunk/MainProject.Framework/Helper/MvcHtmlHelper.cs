using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MainProject.Core.Enums;
using MainProject.Framework.Models;

namespace MainProject.Framework.Helper
{
    public static class MvcHtmlHelper
    {
        public static bool CheckHasPermission(this EntityManageTypeCollection entityManageType, CurrentUserHelper currentUserHelper)
        {
            return currentUserHelper.CheckHasPermissionOnFeature(entityManageType);
        }

        public static string GetResource(this HtmlHelper helper, ResourceKeyCollection resourceKey)
        {
            return ResourceHelper.GetResource(resourceKey);
        }

        public static string FormatCurrency(this HtmlHelper helper, decimal value)
        {
            return string.Format("{0:0,0}", value);
        }

        #region Paging
        #region Cusomize by MK
        public static HtmlString RenderPaging(this HtmlHelper helper, PagingModel model, PagingStyleModel styleModel)
        {
            var strBuilder = new StringBuilder();
            if (model != null && model.HasPaging)
            {
                strBuilder.Append("<ul " + styleModel.DivContainerStyle + ">");

                // Check model has valid paging
                if (model.TotalPages > 1)
                {
                    var distancePage = 1;
                    if (model.CurrentPage == 1 || model.CurrentPage == model.TotalPages) distancePage = 2;

                    // Binding dot
                    var hasDot = false;
                    // Check current page must larger than 1 and display button previous
                    if (model.CurrentPage > 1)
                    {
                        strBuilder.Append("<li><a  class='pre' " + BuildActionCode(model.ActionCode, (model.CurrentPage - 1)) + " " + styleModel.ItemStyle + "></a></li>");
                    }
                    // Get all page lower than current page
                    for (var i = 1; i < model.CurrentPage; i++)
                    {
                        // Get 2 page lowest to current page
                        if (i >= model.CurrentPage - distancePage)
                        {
                            strBuilder.Append("<li><a " + styleModel.ItemStyle + " " + BuildActionCode(model.ActionCode, i) + ">" + i + "</a></li>");
                        }
                        // Check lower page must be lower than lowest page is 2
                        else if (i <= 2)
                        {
                            strBuilder.Append("<li><a " + styleModel.ItemStyle + " " + BuildActionCode(model.ActionCode, i) + ">" + i + "</a></li>");
                        }
                        else // These lower page has distance so far
                        {
                            if (!hasDot)
                            {
                                strBuilder.Append("<span>...</span>");
                                hasDot = true;
                            }
                        }
                    }
                    // Current active page
                    strBuilder.Append("<li " + styleModel.CurrentItemStyle + "><a style='color:red' href = 'javascript:void(0)'>" + model.CurrentPage + "</a></li>");

                    hasDot = false;
                    // Get all page larger than current page
                    for (var i = (model.CurrentPage + 1); i <= model.TotalPages; i++)
                    {
                        // Get 2 page larger to current page
                        if (i <= model.CurrentPage + distancePage)
                        {
                            strBuilder.Append("<li><a " + styleModel.ItemStyle + " " + BuildActionCode(model.ActionCode, i) + ">" + i + "</a></li>");
                        }
                        // Check larger page must be larger than largest page is 2
                        else if (i > (model.TotalPages - 2))
                        {
                            strBuilder.Append("<li><a " + styleModel.ItemStyle + " " + BuildActionCode(model.ActionCode, i) + ">" + i + "</a></li>");
                        }
                        else // To many page from current page to largest page
                        {
                            if (!hasDot)
                            {
                                strBuilder.Append("<span>...</span>");
                                hasDot = true;
                            }
                        }
                    }
                    // Check current page must be lower than largest page to display next button
                    if (model.CurrentPage < model.TotalPages)
                    {
                        strBuilder.Append("<li><a class='next'" + BuildActionCode(model.ActionCode, (model.CurrentPage + 1)) + " " + styleModel.ItemStyle + "></a></li>");
                    }
                }
                strBuilder.Append("</ul>");
            }

            return new HtmlString(strBuilder.ToString());
        }
        #endregion

        //#region Brazzers Paging
        //public static HtmlString RenderPaging(this HtmlHelper helper, PagingModel model, PagingStyleModel styleModel)
        //{
        //    var strBuilder = new StringBuilder();
        //    if (model != null && model.HasPaging)
        //    {
        //        strBuilder.Append("<ul " + styleModel.DivContainerStyle + ">");

        //        // Check model has valid paging
        //        if (model.TotalPages > 1)
        //        {
        //            var distancePage = 2;
        //            if (model.CurrentPage == 1 || model.CurrentPage == model.TotalPages) distancePage = 4;


        //            // Check current page must larger than 1 and display icon button
        //            if (model.CurrentPage > 1)
        //            {
        //                // Go to first page
        //                strBuilder.Append("<li><a " + styleModel.ItemStyle + " " + BuildActionCode(model.ActionCode, 1) + "> << </a></li>");
        //                // Go to previous page
        //                strBuilder.Append("<li><a  class='pre' " + BuildActionCode(model.ActionCode, (model.CurrentPage + 1)) + " " + styleModel.ItemStyle + "></a></li>");
        //            }
        //            // Get all page lower than current page
        //            for (var i = 1; i < model.CurrentPage; i++)
        //            {
        //                // Get 2 page lowest to current page
        //                if (i >= model.CurrentPage - distancePage)
        //                {
        //                    strBuilder.Append("<li><a " + styleModel.ItemStyle + " " + BuildActionCode(model.ActionCode, i) + ">" + i + "</a></li>");
        //                }
        //            }
        //            // Current active page
        //            strBuilder.Append("<li " + styleModel.CurrentItemStyle + "><a style='color:red' href = 'javascript:void(0)'>" + model.CurrentPage + "</a></li>");

        //            // Get all page larger than current page
        //            for (var i = (model.CurrentPage + 1); i <= model.TotalPages; i++)
        //            {
        //                // Get 2 page larger to current page
        //                if (i <= model.CurrentPage + distancePage)
        //                {
        //                    strBuilder.Append("<li><a " + styleModel.ItemStyle + " " + BuildActionCode(model.ActionCode, i) + ">" + i + "</a></li>");
        //                }
        //            }
        //            // Check current page must be lower than largest page to display icon button
        //            if (model.CurrentPage < model.TotalPages)
        //            {
        //                // Go to next page
        //                strBuilder.Append("<li><a class='next'" + BuildActionCode(model.ActionCode, (model.CurrentPage - 1)) + " " + styleModel.ItemStyle + "></a></li>");
        //                // Go to first page
        //                strBuilder.Append("<li><a " + styleModel.ItemStyle + " " + BuildActionCode(model.ActionCode, model.TotalPages) + "> >> </a></li>");
        //            }
        //        }
        //        strBuilder.Append("</ul>");
        //    }

        //    return new HtmlString(strBuilder.ToString());
        //}
        //#endregion

        //Added by MK
        public static HtmlString RenderPagingAdmin(this HtmlHelper helper, PagingModel model, PagingStyleModel styleModel)
        {
            var strBuilder = new StringBuilder();
            if (model != null && model.HasPaging)
            {
                strBuilder.Append("<ul " + styleModel.DivContainerStyle + " class='pagination'>");

                // Check model has valid paging
                if (model.TotalPages > 1)
                {
                    var distancePage = 1;
                    if (model.CurrentPage == 1 || model.CurrentPage == model.TotalPages) distancePage = 2;

                    // Binding dot
                    var hasDot = false;
                    // Check current page must larger than 1 and display button previous
                    if (model.CurrentPage > 1)
                    {
                        strBuilder.Append("<li class='paginate_button'><a " + BuildActionCode(model.ActionCode, (model.CurrentPage - 1)) + " " + styleModel.ItemStyle + "> < </a></li>");
                    }
                    // Get all page lower than current page
                    for (var i = 1; i < model.CurrentPage; i++)
                    {
                        // Get 2 page lowest to current page
                        if (i >= model.CurrentPage - distancePage)
                        {
                            strBuilder.Append("<li class='paginate_button'><a " + styleModel.ItemStyle + " " + BuildActionCode(model.ActionCode, i) + ">" + i + "</a></li>");
                        }
                        // Check lower page must be lower than lowest page is 2
                        else if (i <= 2)
                        {
                            strBuilder.Append("<li class='paginate_button'><a " + styleModel.ItemStyle + " " + BuildActionCode(model.ActionCode, i) + ">" + i + "</a></li>");
                        }
                        else // These lower page has distance so far
                        {
                            if (!hasDot)
                            {
                                strBuilder.Append("<li class='paginate_button'><span>...</span></li>");
                                hasDot = true;
                            }
                        }
                    }
                    // Current active page
                    strBuilder.Append("<li class='active' " + styleModel.CurrentItemStyle + "><a href = 'javascript:void(0)'>" + model.CurrentPage + "</a></li>");

                    hasDot = false;
                    // Get all page larger than current page
                    for (var i = (model.CurrentPage + 1); i <= model.TotalPages; i++)
                    {
                        // Get 2 page larger to current page
                        if (i <= model.CurrentPage + distancePage)
                        {
                            strBuilder.Append("<li class='paginate_button'><a " + styleModel.ItemStyle + " " + BuildActionCode(model.ActionCode, i) + ">" + i + "</a></li>");
                        }
                        // Check larger page must be larger than largest page is 2
                        else if (i > (model.TotalPages - 2))
                        {
                            strBuilder.Append("<li class='paginate_button'><a " + styleModel.ItemStyle + " " + BuildActionCode(model.ActionCode, i) + ">" + i + "</a></li>");
                        }
                        else // To many page from current page to largest page
                        {
                            if (!hasDot)
                            {
                                strBuilder.Append("<li class='paginate_button'><span>...</span></li>");
                                hasDot = true;
                            }
                        }
                    }
                    // Check current page must be lower than largest page to display next button
                    if (model.CurrentPage < model.TotalPages)
                    {
                        strBuilder.Append("<li><a " + BuildActionCode(model.ActionCode, (model.CurrentPage + 1)) + " " + styleModel.ItemStyle + "> > </a></li>");
                    }
                }
                strBuilder.Append("</ul>");
            }

            return new HtmlString(strBuilder.ToString());
        }

        public static HtmlString RenderPagingSite(this HtmlHelper helper, PagingModel model, PagingStyleModel styleModel)
        {
            var strBuilder = new StringBuilder();
            if (model != null && model.HasPaging)
            {
                var actionForNextPage = (model.CurrentPage < model.TotalPages) ? BuildActionCode(model.ActionCode, (model.CurrentPage + 1)) : "";
                var actionForPreviousPage = (model.CurrentPage > 1) ? BuildActionCode(model.ActionCode, (model.CurrentPage - 1)) : "";
                strBuilder.Append("<ul>");

                if (model.TotalPages > 1)
                {
                    if (model.CurrentPage > 1)
                    {
                        strBuilder.Append("<li class='pre'><a " + actionForPreviousPage + " " + styleModel.ItemStyle + "> <img src='/Content/resources/images/news/arr-left.png'> </a></li>");
                    }
                    if (model.TotalPages > 7)
                    {
                        for (var i = 1; i < model.CurrentPage; i++)
                        {
                            if (i <= 2)
                            {
                                strBuilder.Append("<li><a " + styleModel.ItemStyle + " " + BuildActionCode(model.ActionCode, i) + ">" + i + "</a></li>");
                            }
                            else
                            {
                                strBuilder.Append("<li><a >...</a></li>");
                                break;
                            }
                        }
                        if ((model.TotalPages - model.CurrentPage) < 2)
                        {
                            for (var i = model.TotalPages - 2; i < model.CurrentPage; i++)
                            {
                                strBuilder.Append("<li><a " + styleModel.ItemStyle + " " + BuildActionCode(model.ActionCode, i) + ">" + i + "</a></li>");
                            }
                        }

                        if (model.CurrentPage > 3 && model.CurrentPage <= model.TotalPages - 2)
                        {
                            strBuilder.Append("<li><a" + styleModel.ItemStyle + " " + BuildActionCode(model.ActionCode, model.CurrentPage - 1) + ">" + (model.CurrentPage - 1) + "</a></li>");
                        }
                        strBuilder.Append("<li class='active'><a href='#' " + styleModel.CurrentItemStyle + ">" + model.CurrentPage + "</a></li>");
                        if (model.CurrentPage >= 3 && model.CurrentPage < model.TotalPages - 2)
                        {
                            strBuilder.Append("<li><a " + styleModel.ItemStyle + " " + BuildActionCode(model.ActionCode, model.CurrentPage + 1) + ">" + (model.CurrentPage + 1) + "</a></li>");
                        }

                        if (model.CurrentPage < 3)
                        {
                            for (var i = model.CurrentPage + 1; i <= 3; i++)
                            {
                                strBuilder.Append("<li><a " + styleModel.ItemStyle + " " + BuildActionCode(model.ActionCode, i) + ">" + i + "</a></li>");
                            }
                        }

                        var hasDot = false;
                        for (var i = (model.CurrentPage + 1); i <= model.TotalPages; i++)
                        {
                            if ((model.TotalPages - i) < 2)
                            {
                                strBuilder.Append("<li><a " + styleModel.ItemStyle + " " + BuildActionCode(model.ActionCode, i) + ">" + i + "</a></li>");
                            }
                            else
                            {
                                if (!hasDot)
                                {
                                    strBuilder.Append("<li><a >...</a></li>");
                                    hasDot = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (var i = 1; i < model.CurrentPage; i++)
                        {
                            strBuilder.Append("<li><a" + styleModel.ItemStyle + " " + BuildActionCode(model.ActionCode, i) + ">" + i + "</a></li>");
                        }
                        strBuilder.Append("<li class='active'><a href='#'>" + model.CurrentPage + "</a></li>");
                        for (var i = model.CurrentPage + 1; i <= model.TotalPages; i++)
                        {
                            strBuilder.Append("<li><a" + styleModel.ItemStyle + " " + BuildActionCode(model.ActionCode, i) + ">" + i + "</a></li>");
                        }

                    }
                    if (model.CurrentPage < model.TotalPages)
                    {
                        strBuilder.Append("<li class='next'><a  " + actionForNextPage + " " + styleModel.ItemStyle + "> <img src='/Content/resources/images/news/arr-right.png'> </a></li>");
                    }
                }
                strBuilder.Append("</ul>");
            }

            return new HtmlString(strBuilder.ToString());
        }

        public static HtmlString RenderPagingSiteMobile(this HtmlHelper helper, PagingModel model, PagingStyleModel styleModel)
        {
            var strBuilder = new StringBuilder();
            if (model != null && model.HasPaging)
            {
                var actionForNextPage = (model.CurrentPage < model.TotalPages) ? BuildActionCode(model.ActionCode, (model.CurrentPage + 1)) : "";
                var actionForPreviousPage = (model.CurrentPage > 1) ? BuildActionCode(model.ActionCode, (model.CurrentPage - 1)) : "";
                strBuilder.Append("<ul class='pagination'>");

                if (model.TotalPages > 1)
                {
                    if (model.CurrentPage == 1)
                    {
                        strBuilder.Append("<li class='pre_page' style='opacity: 0.5;'><a href='javascript:void(0)' " + styleModel.ItemStyle + "> < </a></li>");

                    }
                    if (model.CurrentPage > 1)
                    {
                        strBuilder.Append("<li class='pre_page'><a " + actionForPreviousPage + " " + styleModel.ItemStyle + "> < </a></li>");

                    }
                    if (model.TotalPages > 3)
                    {
                        for (var i = 1; i < model.CurrentPage; i++)
                        {
                            //if (i <= 2)
                            //{
                            //    strBuilder.Append("<li><a" + styleModel.ItemStyle + " " + BuildActionCode(model.ActionCode, i) + ">" + i + "</a></li>");
                            //}
                            //else
                            //{
                            //    strBuilder.Append("<li><a>...</a></li>");
                            //    break;
                            //}
                            if (i == 1)
                            {
                                strBuilder.Append("<li><a" + styleModel.ItemStyle + " " + BuildActionCode(model.ActionCode, i) + ">" + i + "</a></li>");
                            }
                            else
                            {
                                strBuilder.Append("<li><a>...</a></li>");
                                break;
                            }
                        }
                        //if ((model.TotalPages - model.CurrentPage) < 2)
                        //{
                        //    for (var i = model.TotalPages - 2; i < model.CurrentPage; i++)
                        //    {
                        //        strBuilder.Append("<li><a" + styleModel.ItemStyle + " " + BuildActionCode(model.ActionCode, i) + ">" + i + "</a></li>");
                        //    }
                        //}

                        //if (model.CurrentPage > 3 && model.CurrentPage <= model.TotalPages - 2)
                        //{
                        //    strBuilder.Append("<li><a" + styleModel.ItemStyle + " " + BuildActionCode(model.ActionCode, model.CurrentPage - 1) + ">" + (model.CurrentPage - 1) + "</a></li>");
                        //}

                        strBuilder.Append("<li class='active'><a href='#' " + styleModel.CurrentItemStyle + ">" + model.CurrentPage + "</a></li>");

                        //if (model.CurrentPage >= 3 && model.CurrentPage < model.TotalPages - 2)
                        //{
                        //    strBuilder.Append("<li><a" + styleModel.ItemStyle + " " + BuildActionCode(model.ActionCode, model.CurrentPage + 1) + ">" + (model.CurrentPage + 1) + "</a></li>");
                        //}

                        //if (model.CurrentPage < 3)
                        //{
                        //    for (var i = model.CurrentPage + 1; i <= 3; i++)
                        //    {
                        //        strBuilder.Append("<li><a" + styleModel.ItemStyle + " " + BuildActionCode(model.ActionCode, i) + ">" + i + "</a></li>");
                        //    }
                        //}

                        var hasDot = false;
                        for (var i = (model.CurrentPage + 1); i <= model.TotalPages; i++)
                        {
                            if ((model.TotalPages - i) < 1)
                            {
                                strBuilder.Append("<li><a" + styleModel.ItemStyle + " " + BuildActionCode(model.ActionCode, i) + ">" + i + "</a></li>");
                            }
                            else
                            {
                                if (!hasDot)
                                {
                                    strBuilder.Append("<li><a>...</a></li>");
                                    hasDot = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (var i = 1; i < model.CurrentPage; i++)
                        {
                            strBuilder.Append("<li><a" + styleModel.ItemStyle + " " + BuildActionCode(model.ActionCode, i) + ">" + i + "</a></li>");
                        }
                        strBuilder.Append("<li class='active'><a href='#'>" + model.CurrentPage + "</a></li>");
                        for (var i = model.CurrentPage + 1; i <= model.TotalPages; i++)
                        {
                            strBuilder.Append("<li><a" + styleModel.ItemStyle + " " + BuildActionCode(model.ActionCode, i) + ">" + i + "</a></li>");
                        }

                    }
                    if (model.CurrentPage < model.TotalPages)
                    {
                        strBuilder.Append("<li class='next_page'><a  " + actionForNextPage + " " + styleModel.ItemStyle + "> > </a></li>");
                    }
                    if (model.CurrentPage == model.TotalPages)
                    {
                        strBuilder.Append("<li class='next_page' style='opacity: 0.5;'><a  href='javascript:void(0)' " + styleModel.ItemStyle + "> > </a></li>");
                    }
                }
                strBuilder.Append("</ul>");
            }

            return new HtmlString(strBuilder.ToString());
        }

        private static string BuildActionCode(string strFormat, int pageIndex)
        {
            return string.Format(strFormat, pageIndex);
        }
        #endregion

        public static string Truncate(this HtmlHelper helper, string input, int length)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            if (input.Length <= length)
            {
                return input;
            }
            else
            {
                return input.Substring(0, length) + "...";
            }
        }

        public static string GenerateBreadCrumb(this HtmlHelper helper, string seoStandardUrl)
        {
            // Check url is valid in db incase some query url can't get breadcrumb
            // ex: paging /News?page=1
            var urlRecord = UrlRecordHelper.GetUrlRecordFromUrl(seoStandardUrl).UrlRecord;
            if (urlRecord == null) return string.Empty;

            seoStandardUrl = urlRecord.OriginUrl;
            // Initialize dbContext for some query
            var dbContext = DalHelper.InvokeDbContext();
            // Get home category title
            var homeCategory = dbContext.Categories.FirstOrDefault(
                                        x => x.DisplayTemplate == DisplayTemplateCollection.Home 
                                                    && x.Language.Id == urlRecord.Language.Id);
            var homePage = homeCategory != null ? homeCategory.Title + " /" : "";
            // Bind breadcrumb
            string result = "";
            while (!string.IsNullOrEmpty(seoStandardUrl) && urlRecord != null)
            {
                // Bind current url to breadcrumb
                if (string.IsNullOrEmpty(result))
                {
                    result = string.Format("<a href='{0}'>&nbsp; {1}</a>", urlRecord.Url, urlRecord.Title);
                }
                else
                {
                    result = string.Format("<a href='{0}'>&nbsp; {1}</a>{2}", urlRecord.Url, urlRecord.Title, result);
                }
                // Bind parent url to breadcrumb
                seoStandardUrl = seoStandardUrl.Substring(0, seoStandardUrl.LastIndexOf("/"));
                urlRecord = dbContext.UrlRecords.FirstOrDefault(x => x.Url == seoStandardUrl);
            }

            return string.Format("<ul><li><a href='/'>{0}</a></li>{1}</ul>", homePage, result);
        }
    }
}
