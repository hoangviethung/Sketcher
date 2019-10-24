using Newtonsoft.Json;
using System.Collections.Generic;

namespace MainProject.Framework.Models
{
    /// <summary>
    /// Google captcha response result after checking token
    /// </summary>
    public class CaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}