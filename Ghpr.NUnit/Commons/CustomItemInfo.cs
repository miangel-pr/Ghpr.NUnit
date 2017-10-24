using Ghpr.Core.Common;
using Newtonsoft.Json.Linq;
using System;

namespace Ghpr.NUnit.Commons
{
    public class CustomItemInfo : ItemInfo
    {
        public CustomItemInfo(string json)
        {
            JObject itemInfo = JObject.Parse(json);

            Guid = Guid.Parse(itemInfo["guid"].ToString());
            Start = DateTime.Parse(itemInfo["start"].ToString());
            Finish = DateTime.Parse(itemInfo["finish"].ToString());
            FileName = itemInfo["fileName"].ToString();
        }
    }
}
