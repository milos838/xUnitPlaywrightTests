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
        private ILocator homeLink => _page.Locator("//*[contains(text(), ' HOME ')]");
        private ILocator ordersLink => _page.Locator("//*[contains(text(), ' ORDERS')]");
        private ILocator cartLink => _page.Locator("//*[contains(text(), ' Cart ')]");
        private ILocator logoutLink => _page.Locator("//*[contains(text(), ' Sign Out ')]");
        private ILocator searchBox => _page.GetByRole(AriaRole.Textbox, new() { Name = "search" });
        private ILocator searchedProduct => _page.Locator("//*[contains(text(), 'ADIDAS ')]");
        
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
        public async Task SearchProductAsync(string searchTerm)
        {
            await searchBox.FillAsync(searchTerm);
            await searchBox.PressAsync("Enter");
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
        public async Task AssertHeaderLinksVisibleAsync()
        {
            await Assertions.Expect(homeLink).ToBeVisibleAsync();
            await Assertions.Expect(ordersLink).ToBeVisibleAsync();
            await Assertions.Expect(cartLink).ToBeVisibleAsync();
            await Assertions.Expect(logoutLink).ToBeVisibleAsync();
        }
        public async Task AssertSearchBoxVisibleAsync()
        {
            await Assertions.Expect(searchBox).ToBeVisibleAsync();
        }
        public async Task AssertSearchedProductVisibleAsync()
        {
            await Assertions.Expect(searchedProduct).ToBeVisibleAsync();
        }
    }
}