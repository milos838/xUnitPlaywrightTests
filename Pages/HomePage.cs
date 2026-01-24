using System.Threading.Tasks;
using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace PlaywrightTests.Pages
{
    public class HomePage
    {
        private readonly IPage _page;
        private ILocator pageTitle => _page.Locator("//*[@class='title' and contains(text(), 'Practice')]");
        private ILocator username => _page.Locator("#userEmail");
        private ILocator password => _page.Locator("#userPassword");
        private ILocator loginButton => _page.Locator("#login");
        private ILocator dashboardHeader => _page.Locator("//*[contains(text(), 'Automation Practice')]");
        
        //Constructor
        public HomePage(IPage page)
        {
            _page = page;
        }

        //Functions or methods
        public async Task NavigateAsync(string url)
        {
            await _page.GotoAsync(url);
        }
        public async Task<string> GetCurrentURLAsync()
        {
            return _page.Url;
        }
        public async Task SubmitLoginAsync(string Username, string Password)
        {
            await username.FillAsync(Username);
            await password.FillAsync(Password);
            await loginButton.ClickAsync();
        }
       
        
        //Assertions
        public async Task AssertURLAsync(string expectedURL)
        {
            await Assertions.Expect(_page).ToHaveURLAsync(expectedURL);
        }
        public async Task AssertPageTitleAsync(string expectedTitle)
        {
            await Assertions.Expect(_page).ToHaveTitleAsync(expectedTitle);
        }
        public async Task AssertLoginFieldsVisibleAsync()
        {
            await Assertions.Expect(username).ToBeVisibleAsync();
            await Assertions.Expect(password).ToBeVisibleAsync();
            await Assertions.Expect(loginButton).ToBeVisibleAsync();
        }
        public async Task AssertDashboardHeaderVisibleAsync()
        {
            await Assertions.Expect(dashboardHeader).ToBeVisibleAsync();
        }
    }
}