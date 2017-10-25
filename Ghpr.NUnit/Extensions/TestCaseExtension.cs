using Ghpr.Core.Interfaces;
using Ghpr.Core.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghpr.NUnit.Extensions
{
    public static class TestCaseExtension
    {
        public static void Update(this ITestRun target, ITestRun run)
        {
            //if (target.TestInfo.Guid.Equals(Guid.Empty))
            //{
            //    target.TestInfo.Guid = GuidConverter.ToMd5HashGuid(target.FullName);
            //}
            //target.Screenshots.AddRange(run.Screenshots.Where(s => !target.Screenshots.Any(ts => ts.Name.Equals(s.Name))));
            //target.Events.AddRange(run.Events.Where(e => !target.Events.Any(te => te.Name.Equals(e.Name))));
            //return target;
        }

        public static string GetFileName(this ITestRun testRun)
        {
            return $"testcase_{testRun.RunGuid}.json";
        }

        public static void Save(this ITestRun testRun, string path, string name = "")
        {
            Paths.Create(path);
            var fullPath = Path.Combine(path, name.Equals("") ? testRun.GetFileName() : name);
            using (var file = File.CreateText(fullPath))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(file, testRun);
            }
        }

        public static ITestRun GetTest(this List<ITestRun> testRuns, ITestRun testRun)
        {
            //var tr = testRuns.FirstOrDefault(t => t.TestInfo.Guid.Equals(testRun.TestInfo.Guid) && !t.TestInfo.Guid.Equals(Guid.Empty))
            //              ?? testRuns.FirstOrDefault(t => t.FullName.Equals(testRun.FullName))
            //              ?? new TestRun();
            //return tr;
            return null;
        }

        public static void SaveScreenshot(this ITestRun testRun, byte[] screenBytes, string reportOutputPath)
        {
            //var screenPath = Path.Combine(reportOutputPath, Paths.Folders.Tests, testRun.TestInfo.Guid.ToString(), Paths.Folders.Img);
            //var screenshotName = ScreenshotHelper.SaveScreenshot(screenPath, screenBytes, DateTime.Now);
            //var screenshot = new TestScreenshot(screenshotName);
            //testRun.Screenshots.Add(screenshot);
            //return testRun;
        }
    }
}
