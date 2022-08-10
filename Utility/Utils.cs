using Microsoft.Playwright;

namespace PlaywrightAutomator.Utility
{
    public static class Utils
    {
        public static void Screenshot(IPage page)
        {
            var date = DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss");
            var title = page.TitleAsync();
            var path = $"../../../screenshots/{date}_{title}-.png";
            var pageScreenshotOptions = new PageScreenshotOptions()
            {
                Path = path,
                FullPage = true,
            };
            page.ScreenshotAsync(pageScreenshotOptions);
        }
    }
}