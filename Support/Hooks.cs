using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using BoDi;
using Microsoft.Playwright;
using NUnit.Framework;
using PlaywrightAutomator.DriverFactory;
using PlaywrightAutomator.Utility;
using BrowserType = PlaywrightAutomator.DriverFactory.BrowserType;

[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: LevelOfParallelism(4)]

namespace PlaywrightAutomator.Support
{
    [Binding]
    public class Hooks
    {
        public static int totalCount = 0;
        public static int passCount = 0;
        public static int failCount = 0;
        [ThreadStatic]
        private static ExtentTest? featureName;
        [ThreadStatic]
        private static ExtentTest? scenario;
        private static ExtentReports? extent;
        public static string CurrentRunFolderName = string.Format("{0}-{1}-{2}-{3}-{4}-{5}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        public static string filePath = @"C:/" + Environment.MachineName + "/" + "AutomationReport_Timestamp_" + CurrentRunFolderName;
        static HashSet<string> failedFeatures = new HashSet<string>();

        [Obsolete]
        private static ExtentHtmlReporter? ExtentHtmlReporter;

        public static IBrowser browser;
        public static IBrowserContext context;
        [ThreadStatic]
        public static IPage page;
        public static IPlaywright playwright;
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;
        public string Url = "https://www.saucedemo.com/";

        public Hooks(IObjectContainer objectContainer, ScenarioContext scenarioContext)
        {
            _objectContainer = objectContainer;
            _scenarioContext = scenarioContext;
        }

        [BeforeTestRun()]
        [Obsolete]
        public static void BeforeTestRun()
        {
            ExtentHtmlReporter = new ExtentHtmlReporter(filePath + "\\index.html");
            ExtentHtmlReporter.Config.Theme = Theme.Dark;
            ExtentHtmlReporter.Config.DocumentTitle = "Automation Test Run Report";
            ExtentHtmlReporter.Config.ReportName = "Regression";
            extent = new ExtentReports();
            extent.AttachReporter(ExtentHtmlReporter);                      
            browser = PlaywrightDriver.CreatePlaywright(BrowserType.Chromium, new BrowserTypeLaunchOptions { Headless = false, SlowMo = 50 });
        }

        [AfterTestRun()]
        public static void AfterTestRun()
        {
            browser.CloseAsync();
            extent.Flush();
        }

        [BeforeScenario()]
        public void BeforeScenario()
        {            
            context = Task.Run(async () => await browser.NewContextAsync()).Result;
            context.Tracing.StartAsync(new TracingStartOptions
            {
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });
            page = Task.Run(async () => await context.NewPageAsync()).Result;
            page.SetViewportSizeAsync(1920, 1080);            
            page.GotoAsync(Url);
            _objectContainer.RegisterInstanceAs(page);
        }

        [AfterScenario()]
        public void AfterScenario()
        {
            if (_scenarioContext.TestError != null)
            {
                Utils.Screenshot(page);
            }
            context.Tracing.StopAsync(new TracingStopOptions
            {
                Path = "trace.zip"
            });
            page.CloseAsync();
        }

        [BeforeFeature]
        [Obsolete]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            featureName = extent.CreateTest<Feature>(featureContext.FeatureInfo.Title);
        }

        [AfterFeature]
        [Obsolete]
        public static void AfterFeature(FeatureContext featureContext)
        {}

        [Before]
        public static void Before(ScenarioContext scenarioContext)
        {
            scenario = featureName.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Title);
            scenario.AssignCategory(scenarioContext.ScenarioInfo.Tags);
        }

        [AfterStep]
        [Obsolete]
        public void AfterStep(ScenarioContext scenarioContext)
        {
            var stepType = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            if (scenarioContext.TestError == null)
            {
                if (stepType == "Given")
                    scenario.CreateNode<Given>(scenarioContext.StepContext.StepInfo.Text.ToString());
                else if (stepType == "When")
                    scenario.CreateNode<When>(scenarioContext.StepContext.StepInfo.Text.ToString());
                else if (stepType == "Then")
                    scenario.CreateNode<Then>(scenarioContext.StepContext.StepInfo.Text.ToString());
                else if (stepType == "And")
                    scenario.CreateNode<And>(scenarioContext.StepContext.StepInfo.Text.ToString());
            }
            else if (scenarioContext.TestError != null)
            {
                var screenshot = CaptureScreenShot(scenarioContext.ScenarioInfo.Title.Trim());
                if (stepType == "Given")
                    scenario.CreateNode<Given>(scenarioContext.StepContext.StepInfo.Text.ToString()).Fail(scenarioContext.TestError.Message, screenshot);
                else if (stepType == "When")
                    scenario.CreateNode<When>(scenarioContext.StepContext.StepInfo.Text.ToString()).Fail(scenarioContext.TestError.Message, screenshot);
                else if (stepType == "And")
                    scenario.CreateNode<When>(scenarioContext.StepContext.StepInfo.Text.ToString()).Fail(scenarioContext.TestError.Message, screenshot);
                else if (stepType == "Then")
                    scenario.CreateNode<Then>(scenarioContext.StepContext.StepInfo.Text.ToString()).Fail(scenarioContext.TestError.Message, screenshot);
            }
        }

        public static MediaEntityModelProvider CaptureScreenShot(string name)
        {            
            byte[] bytes = page.ScreenshotAsync().Result;
            string result = Convert.ToBase64String(bytes);
            return MediaEntityBuilder.CreateScreenCaptureFromBase64String(result, name).Build();
        }
    }
}