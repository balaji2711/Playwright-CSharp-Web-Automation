using Microsoft.Playwright;

namespace PlaywrightAutomator.Pages
{
    public class LoginPage
    {
        private readonly IPage _page;
        ILocator title => _page.Locator("//span[@class='title']");

        public LoginPage(IPage page)
        {
            _page = page;
        }

        public void SetUserName(string username)
        {
            _page.FillAsync("[id='user-name']", username).Wait();
        }        

        public void SetPassword(string password)
        {
            _page.FillAsync("[id='password']", password).Wait();
        }
            
        public void ClickLoginButtonAsync()
        {            
           _page.ClickAsync("[id='login-button']").Wait();
        }

        public string ValidateLoginAsync()
        {                        
            var text = Task.Run(async () => await title.InnerTextAsync()).Result;
            return text;
        }
    }
}