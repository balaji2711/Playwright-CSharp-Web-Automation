using Microsoft.Playwright;

namespace PlaywrightAutomator.DriverFactory
{
    public class PlaywrightDriver
    {
        public static IBrowser CreatePlaywright(BrowserType inBrowser, BrowserTypeLaunchOptions inLaunchOptions)
        {
            var playwright  = Task.Run(async () => await Playwright.CreateAsync()).Result;
            IBrowser? browser = null;
            if (inBrowser == BrowserType.Chromium)
            {
                browser = Task.Run(async () => await playwright.Chromium.LaunchAsync(inLaunchOptions)).Result;
            }

            if (inBrowser == BrowserType.Firefox)
            {
                browser = Task.Run(async () => await playwright.Firefox.LaunchAsync(inLaunchOptions)).Result;
            }

            if (inBrowser == BrowserType.WebKit)
            {
                browser = Task.Run(async () => await playwright.Webkit.LaunchAsync(inLaunchOptions)).Result;
            }
            return browser;
        }
    }
}