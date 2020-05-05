using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MvcRecaptcha.Models.Services
{
    public class RecaptchaVerifyResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        [JsonPropertyName("challenge_ts")]
        public DateTime ChallengeDate { get; set; }
        [JsonPropertyName("hostname")]
        public string Hostname { get; set; }
        [JsonPropertyName("error-codes")]
        public IEnumerable<string> ErrorCodes { get; set; }
    }
}