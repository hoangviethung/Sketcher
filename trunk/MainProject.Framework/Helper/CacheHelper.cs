using System;
using System.Runtime.Caching;

namespace MainProject.Framework.Helper
{
    public static class CacheHelper
    {
        private const int HourCacheTime = 24;

        private static MemoryCache _mCache;
        private static MemoryCache MCache
        {
            get { return _mCache ?? (_mCache = new MemoryCache("cache")); }
        }

        public static void InsertOrUpdate(string key, object objectValue)
        {
            if (!MCache.Contains(key))
            {
                var policy = new CacheItemPolicy {AbsoluteExpiration = DateTime.Now.AddHours(HourCacheTime)};
                MCache.Add(key, objectValue, policy);
            }
            else
            {
                MCache[key] = objectValue;
            }
        }

        public static Object GetValueCache(string key)
        {
            var objectValue = MCache.Get(key);

            return objectValue;
        }

        public static void ReleaseCache(string key)
        {
            MCache.Remove(key);
        }

        public static void Dispose()
        {
            MCache.Dispose();
        }
    }
}
