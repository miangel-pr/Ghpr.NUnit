using Ghpr.Core.Interfaces;
using Ghpr.Core.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Ghpr.Core.Common;
using Ghpr.NUnit.Commons;
using Newtonsoft.Json.Linq;

namespace Ghpr.NUnit.Extensions
{
    public static class RunExtensions
    {
        public static void CustomSave(this IRun run, string path, string fileName = "")
        {
            if (fileName.Equals(string.Empty))
            {
                fileName = $"run_{run.RunInfo.Guid.ToString().ToLower()}.json";
            }
            run.RunInfo.FileName = fileName;
            Paths.Create(path);
            var fullRunPath = Path.Combine(path, fileName);
            
            if (File.Exists(fullRunPath))
            {
                var existingFile = File.ReadAllText(fullRunPath);
                
                IRun previousRun = new CustomRun(existingFile);
                Merge(run, previousRun);
            }

            using (var file = File.CreateText(fullRunPath))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(file, run);
            }
        }

        public static void Merge(IRun a, IRun b)
        {
            if (a.RunInfo.FileName.Equals(b.RunInfo.FileName))
            {

                a.TestRunFiles.AddRange(b.TestRunFiles);


                a.RunSummary.Total += b.RunSummary.Total;
                a.RunSummary.Success += b.RunSummary.Success;
                a.RunSummary.Errors += b.RunSummary.Errors;
                a.RunSummary.Failures += b.RunSummary.Failures;
                a.RunSummary.Inconclusive += b.RunSummary.Inconclusive;
                a.RunSummary.Ignored += b.RunSummary.Ignored;
                a.RunSummary.Unknown += b.RunSummary.Unknown;
            }
        }
    }
}
