using System;
using System.Linq;
using MainProject.Framework.Models;

namespace MainProject.Framework.Helper
{
    public static class UrlRecordHelper
    {
        public static RouterModel GetUrlRecordFromUrl(string request)
        {
            request = request.TrimEnd('/');
            if (request.StartsWith("http://") || request.StartsWith("https://"))
            {
                var protocolLength = request.StartsWith("http://") ? 7 : 8;

                var tempRequest = request.Substring(protocolLength);
                var domainLength = tempRequest.IndexOf("/");
                if (domainLength > 0)
                {
                    request = request.Substring(protocolLength + domainLength);
                }
            }
            var routerModel = new RouterModel();
            var paramValues = request.Split('/').ToList();
            var otherParams = string.Empty;
            var urlWithoutParam = request;
            int pageIndex = 1;
            if (paramValues.Count > 0)
            {
                var lastParamValue = paramValues[paramValues.Count - 1];
                var seName = lastParamValue;
                //incase has param
                if (seName.Contains("?"))
                {
                    otherParams = seName.Substring(seName.IndexOf("?") + 1);
                    seName = seName.Substring(0, seName.IndexOf("?"));
                    urlWithoutParam = request.Replace(string.Format("?{0}", otherParams), "");
                }
                // Incase last param is page number Example: /News/2
                else if (int.TryParse(lastParamValue, out pageIndex))
                {
                    lastParamValue = paramValues[paramValues.Count - 2];
                    seName = lastParamValue;
                    urlWithoutParam = request.Substring(0, request.LastIndexOf("/"));
                }
                var dbContext = DalHelper.InvokeDbContext();
                var urlRecord = dbContext.UrlRecords.FirstOrDefault(c => c.Url.Equals(urlWithoutParam));

                if (urlRecord != null)
                {
                    if (urlRecord.Url.Equals(urlWithoutParam, StringComparison.OrdinalIgnoreCase))
                    {
                        routerModel.HasResult = true;
                        routerModel.PageIndex = pageIndex;
                        routerModel.UrlRecord = urlRecord;
                        routerModel.UrlRequest = request;
                        routerModel.OtherParams = otherParams;
                    }
                }
            }
            return routerModel;
        }
    }
}
