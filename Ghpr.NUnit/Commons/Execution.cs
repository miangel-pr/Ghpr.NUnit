using Ghpr.Core.Common;
using Ghpr.Core.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ghpr.NUnit.Commons
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Execution : IRun
    {
        [JsonProperty(PropertyName = "testRunFiles")]
        public List<string> TestRunFiles { get; set; }

        [JsonProperty(PropertyName = "runInfo")]
        public ItemInfo RunInfo { get; set; }

        [JsonProperty(PropertyName = "sprint")]
        public string Sprint { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "summary")]
        public IRunSummary RunSummary { get; set; }

        public Execution(Guid runGuid)
        {
            RunInfo = new ItemInfo
            {
                Guid = runGuid
            };
            Name = string.Empty;
            Sprint = string.Empty;
        }

        public Execution(string json)
        {
            JObject jObjectCustomRun = JObject.Parse(json);
            var testRunFiles = jObjectCustomRun["testRunFiles"].ToList();
            Sprint = jObjectCustomRun["sprint"].ToString();
            Name = jObjectCustomRun["name"].ToString();
            JToken summary = jObjectCustomRun["summary"];
            JToken runInfo = jObjectCustomRun["runInfo"];
                        
            TestRunFiles = new List<string>();
            foreach (var testRunFile in testRunFiles)
            {
                TestRunFiles.Add(testRunFile.ToString());
            }

            RunInfo = new CustomItemInfo(runInfo.ToString());
            RunSummary = new CustomRunSummary(summary.ToString());
        }

    }
}
