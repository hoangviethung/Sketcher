using System.Collections.Generic;
using Newtonsoft.Json;

namespace MainProject.Service.Model.Contact
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