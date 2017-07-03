using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using Helper.IO;
using Newtonsoft.Json;
using Helper.Extensions;

namespace Helper.Validation
{
    public class SpellCheckResult
    {
        private const string BASE_URI = "https://montanaflynn-spellcheck.p.mashape.com/check/";
        private const string API_KEY = "Ann4bQIkramshtKMQ7lwQU7Ho5ncp16Ue18jsni36cB8fHOiK9";

        public static SpellCheckResult Check(string originalText)
        {
            var request = HttpHelper.GenerateRequest(HttpMethod.Get, "application/json", $"{BASE_URI}?text={HttpUtility.UrlEncode(originalText)}", MashapeHeader, null);
            var response = HttpHelper.GetResponse(request);

            return response.StatusCode == HttpStatusCode.OK ? JsonConvert.DeserializeObject<SpellCheckResult>(response.Content.ToString(Encoding.UTF8)) : default(SpellCheckResult);
        }

        private static WebHeaderCollection MashapeHeader => new WebHeaderCollection
        {
            {"X-Mashape-Key", API_KEY}
        };

        [JsonProperty("original")]
        public string OriginalText { get; set; }

        [JsonProperty("suggestion")]
        public string Suggestion { get; set; }

        [JsonProperty("corrections")]
        public Dictionary<string, string[]> Corrections { get; set; }
    }
}

