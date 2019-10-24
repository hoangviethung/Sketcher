namespace MainProject.Framework.Constant
{
    public static class CacheConstant
    {
        private const string ResourceKey = "MainProject.Web.Resource";
        public static string GetResourceKey(string resourceKey, string culture)
        {
            return string.Format("{0}.{1}.{2}", ResourceKey, culture.ToLower(), resourceKey.ToLower());
        }
    }
}
