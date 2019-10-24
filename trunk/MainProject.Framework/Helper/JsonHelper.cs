using Newtonsoft.Json;
using System;

namespace MainProject.Framework.Helper
{
    public static class JsonHelper
    {
        public static string Serialize(object obj)
            => JsonConvert.SerializeObject(obj);

        public static T Deserialize<T>(string str)
            => JsonConvert.DeserializeObject<T>(str);
    }
}
