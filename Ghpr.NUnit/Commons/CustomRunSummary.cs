using Ghpr.Core.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ghpr.NUnit.Commons
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CustomRunSummary : IRunSummary
    {
        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }

        [JsonProperty(PropertyName = "success")]
        public int Success { get; set; }

        [JsonProperty(PropertyName = "errors")]
        public int Errors { get; set; }

        [JsonProperty(PropertyName = "failures")]
        public int Failures { get; set; }

        [JsonProperty(PropertyName = "inconclusive")]
        public int Inconclusive { get; set; }

        [JsonProperty(PropertyName = "ignored")]
        public int Ignored { get; set; }

        [JsonProperty(PropertyName = "unknown")]
        public int Unknown { get; set; }

        public CustomRunSummary(int total = 0, int success = 0, int errors = 0, int failures = 0, int inconclusive = 0, int ignored = 0, int unknown = 0)
        {
            Total = total;
            Success = success;
            Errors = errors;
            Failures = failures;
            Inconclusive = inconclusive;
            Ignored = ignored;
            Unknown = unknown;
        }

        public CustomRunSummary(string json)
        {
            JObject summary = JObject.Parse(json);

            Total = int.Parse(summary["total"].ToString());
            Success = int.Parse(summary["success"].ToString());
            Errors = int.Parse(summary["errors"].ToString());
            Failures = int.Parse(summary["failures"].ToString());
            Inconclusive = int.Parse(summary["inconclusive"].ToString());
            Ignored = int.Parse(summary["ignored"].ToString());
            Unknown = int.Parse(summary["unknown"].ToString());
        }
    }
}
