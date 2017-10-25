using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ghpr.Core.Interfaces;
using Ghpr.Core.Enums;
using Ghpr.Core.EmbeddedResources;
using Ghpr.Core.Helpers;
using System.IO;
using System.Drawing;
using Ghpr.Core.Utils;
using Ghpr.Core.Common;
using Ghpr.NUnit.Helpers;
using Ghpr.NUnit.Extensions;

using Ghpr.NUnit.Commons;

namespace Ghpr.NUnit
{
    public class CustomReporter : IReporter
    {
        private void InitializeReporter(IReporterSettings settings)
        {
            if (settings.OutputPath == null)
            {
                throw new ArgumentNullException(nameof(settings.OutputPath),
                    "Reporter Output must be specified! Fix your settings.");
            }
            Settings = settings;
            ReportSettings = new ReportSettings(settings.RunsToDisplay, settings.TestsToDisplay);
            _action = new ActionHelper(settings.OutputPath);
            _extractor = new ResourceExtractor(_action, settings.OutputPath);
            TestRunStarted = false;
            //_log = new Log(settings.OutputPath);
        }

        public CustomReporter(IReporterSettings settings)
        {
            InitializeReporter(settings);
        }

        public CustomReporter()
        {
            var settings = ReporterHelper.GetSettingsFromFile();
            InitializeReporter(settings);
        }

        public CustomReporter(TestingFramework framework)
        {
            var fileName = ReporterHelper.GetSettingsFileName(framework);
            var settings = ReporterHelper.GetSettingsFromFile(fileName);
            InitializeReporter(settings);
        }

        private IRun _currentExcution;
        private ITestRun _currentTestRun;
        private List<ITestRun> _currentTestRuns;
        private Guid _currentRunGuid;
        private ResourceExtractor _extractor;
        //private Log _log;
        private static ActionHelper _action;

        public bool TestRunStarted { get; private set; }
        public IReportSettings ReportSettings { get; private set; }
        public IReporterSettings Settings { get; private set; }
        public string TestsPath => Path.Combine(Settings.OutputPath, Paths.Folders.Tests);
        public string ExecutionPath => Path.Combine(Settings.OutputPath, Paths.Folders.Runs);

        private void InitializeRun(DateTime startDateTime, string runGuid = "")
        {
            _action.Safe(() =>
            {
                _currentRunGuid = runGuid.Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(runGuid);

                _currentExcution = new Execution(_currentRunGuid)
                {
                    TestRunFiles = new List<string>(),
                    RunSummary = new CustomRunSummary()
                };
                
                _currentExcution.Name = Settings.RunName;
                _currentExcution.Sprint = Settings.Sprint;
                _currentExcution.RunInfo.Start = startDateTime;

                _currentTestRuns = new List<ITestRun>();

                // Extract report files
                _extractor.ExtractReportBase();                

                // Save ReportSettings file
                ReportSettings.Save(Settings.OutputPath);
            });
        }

        private void GenerateReport(DateTime finishDateTime)
        {
            _action.Safe(() =>
            {
                _currentExcution.RunInfo.Finish = finishDateTime;
                _currentExcution.Save(ExecutionPath);
                var runInfo = new CustomItemInfo(_currentExcution.RunInfo);
                RunsHelper.SaveCurrentRunInfo(ExecutionPath, runInfo);
            });
        }

        public void RunStarted()
        {
            if (!TestRunStarted)
            {
                InitializeRun(DateTime.Now, Settings.RunGuid);
                TestRunStarted = true;
            }
        }

        public void RunFinished()
        {
            GenerateReport(DateTime.Now);
        }

        public void TestStarted(ITestRun testRun)
        {
            _action.Safe(() =>
            {
                _currentTestRuns.Add(testRun);
                _currentTestRun = testRun;
            });
        }

        public void SaveScreenshot(Bitmap screen)
        {
            _action.Safe(() =>
            {
                var testGuid = _currentTestRun.TestInfo.Guid.ToString();
                var date = DateTime.Now;
                var s = new TestScreenshot(date);
                ScreenshotHelper.SaveScreenshot(Path.Combine(TestsPath, testGuid, Paths.Folders.Img), screen, date);
                _currentTestRun.Screenshots.Add(s);
                _currentTestRuns.First(
                    tr => tr.TestInfo.Guid.Equals(_currentTestRun.TestInfo.Guid))
                    .Screenshots.Add(s);
            });
        }

        public void AddCompleteTestRun(ITestRun testRun)
        {
            _action.Safe(() =>
            {
                
                
                
                

                //_currentRun.RunSummary = _currentRun.RunSummary.Update(testRun);

                //testRun.TestInfo.FileName = fileName;
                //testRun.RunGuid = _currentRunGuid;
                //testRun.TestInfo.Start = testRun.TestInfo.Start.Equals(default(DateTime))
                //    ? finishDateTime
                //    : testRun.TestInfo.Start;
                //testRun.TestInfo.Finish = testRun.TestInfo.Finish.Equals(default(DateTime))
                //    ? finishDateTime
                //    : testRun.TestInfo.Finish;
                //testRun.TestDuration = testRun.TestDuration.Equals(0.0)
                //    ? (testRun.TestInfo.Finish - testRun.TestInfo.Start).TotalSeconds
                //    : testRun.TestDuration;
                
                //_currentRun.TestRunFiles.Add(Paths.GetRelativeTestRunPath(testGuid, fileName));

                //TestRunsHelper.SaveCurrentTestInfo(testPath, testRun.TestInfo);
            });
        }

        public void TestFinished(ITestRun testRun)
        {
            _action.Safe(() =>
            {
                testRun.RunGuid = _currentRunGuid;
                string fileName = testRun.GetFileName();

                testRun.TestInfo.FileName = fileName;

                string testCaseGuid = testRun.TestInfo.Guid.ToString();
                string testCasePath = Path.Combine(TestsPath, testCaseGuid);
                
                testRun.Save(testCasePath, fileName);

                _currentExcution.TestRunFiles.Add(Paths.GetRelativeTestRunPath(testCaseGuid, fileName));

                TestRunsHelper.SaveCurrentTestInfo(testCasePath, testRun.TestInfo);



                //_currentRun.RunSummary.Total++;

                //var finishDateTime = testRun.TestInfo.Finish;

                //_currentRun.RunSummary.Total++;

                //var finishDateTime = DateTime.Now;
                //var currentTest = _currentTestRuns.GetTest(testRun);
                //var finalTest = testRun.Update(currentTest);
                //var testGuid = finalTest.TestInfo.Guid.ToString();
                //var testPath = Path.Combine(TestsPath, testGuid);
                //var fileName = finishDateTime.GetTestName();

                //_currentRun.RunSummary = _currentRun.RunSummary.Update(finalTest);

                //finalTest.TestInfo.FileName = fileName;
                //finalTest.RunGuid = _currentRunGuid;
                //finalTest.TestInfo.Start = finalTest.TestInfo.Start.Equals(default(DateTime))
                //    ? finishDateTime
                //    : finalTest.TestInfo.Start;
                //finalTest.TestInfo.Finish = finalTest.TestInfo.Finish.Equals(default(DateTime))
                //    ? finishDateTime
                //    : finalTest.TestInfo.Finish;
                //finalTest.TestDuration = finalTest.TestDuration.Equals(0.0)
                //    ? (finalTest.TestInfo.Finish - finalTest.TestInfo.Start).TotalSeconds
                //    : finalTest.TestDuration;
                //finalTest.Save(testPath, fileName);
                //_currentTestRuns.Remove(currentTest);
                //_currentRun.TestRunFiles.Add(Paths.GetRelativeTestRunPath(testGuid, fileName));

                //TestRunsHelper.SaveCurrentTestInfo(testPath, finalTest.TestInfo);

                //if (Settings.RealTimeGeneration)
                //{
                //    GenerateReport(DateTime.Now);
                //}
            });
        }

        public void GenerateFullReport(List<ITestRun> testRuns, string runGuid = "")
        {
            if (!testRuns.Any())
            {
                throw new Exception("Emplty test runs list!");
            }
            var runStart = testRuns.OrderBy(t => t.TestInfo.Start).First().TestInfo.Start;
            var runFinish = testRuns.OrderByDescending(t => t.TestInfo.Finish).First().TestInfo.Finish;
            GenerateFullReport(testRuns, runStart, runFinish, runGuid);
        }

        public void GenerateFullReport(List<ITestRun> testRuns, DateTime start, DateTime finish, string runGuid = "")
        {
            if (!testRuns.Any())
            {
                throw new Exception("Emplty test runs list!");
            }

            InitializeRun(start, runGuid);
            foreach (var testRun in testRuns)
            {
                AddCompleteTestRun(testRun);
            }
            GenerateReport(finish);
        }
    }
}
