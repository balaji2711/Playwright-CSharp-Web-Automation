using Microsoft.Playwright;
using PlaywrightAutomator.Pages;

namespace PlaywrightAutomator.StepDefinitions.Dummy
{
    [Binding]
    public class LoginSteps
    {
        private LoginPage _loginPage;
        private IPage _page;

        public LoginSteps(IPage page, LoginPage loginPage)
        {
            _page = page;
            _loginPage = loginPage;
        }

        [When(@"I enter the username ""([^""]*)"" and password ""([^""]*)""")]
        public void WhenIEnterTheUsernameAndPassword(string userName, string password)
        {
            _loginPage.SetUserName(userName);
            _loginPage.SetPassword(password);
        }

        [When(@"I click on login")]
        public void WhenIClickOnLogin()
        {
            _loginPage.ClickLoginButtonAsync();  
        }

        [Then(@"I user logged in to the application successfully")]
        public void ThenIUserLoggedInToTheApplicationSuccessfully()
        {
            string text = _loginPage.ValidateLoginAsync();
            text.Should().BeEquivalentTo("PRODUCTS");
        }
    }
}