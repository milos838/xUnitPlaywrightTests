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
        private ILocator minPriceField => _page.GetByRole(AriaRole.Textbox, new() { Name = "Min Price" });
        private ILocator maxPriceField => _page.GetByRole(AriaRole.Textbox, new() { Name = "Max Price" });
        private ILocator priceSearchedProduct => _page.GetByText("iphone 13 pro");
        private ILocator fashionCheckBox => _page.Locator("//*[@id='sidebar']//input[@type='checkbox' and (ancestor::label[normalize-space(.)='fashion'] or ../label[normalize-space(.)='fashion'] or following-sibling::label[1][normalize-space(.)='fashion'] or @id = //label[normalize-space(.)='fashion']/@for)]");
        private ILocator electronicsCheckBox => _page.Locator("//*[@id='sidebar']//input[@type='checkbox' and (ancestor::label[normalize-space(.)='electronics'] or ../label[normalize-space(.)='electronics'] or following-sibling::label[1][normalize-space(.)='electronics'] or @id = //label[normalize-space(.)='electronics']/@for)]");

        private ILocator householdCheckBox => _page.Locator("//*[@id='sidebar']//input[@type='checkbox' and (ancestor::label[normalize-space(.)='household'] or ../label[normalize-space(.)='household'] or following-sibling::label[1][normalize-space(.)='household'] or @id = //label[normalize-space(.)='household']/@for)]");
        
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
        public async Task SetPriceFilterAsync(string minPrice, string maxPrice)
        {
            await minPriceField.FillAsync(minPrice);
            await maxPriceField.FillAsync(maxPrice);
            await maxPriceField.PressAsync("Enter");
        }
        public async Task CheckFashionBoxAsync()
        {
            await fashionCheckBox.CheckAsync();
        }
        public async Task CheckElectronicsBoxAsync()
        {
            await electronicsCheckBox.CheckAsync();
        }
        public async Task CheckHouseholdBoxAsync()
        {
            await householdCheckBox.CheckAsync();
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
        public async Task AssertPriceFilterFieldsVisibleAsync()
        {
            await Assertions.Expect(minPriceField).ToBeVisibleAsync();
            await Assertions.Expect(maxPriceField).ToBeVisibleAsync();
        }
        public async Task AssertPriceSearchedProductVisibleAsync()
        {
            await Assertions.Expect(priceSearchedProduct).ToBeVisibleAsync();
        }
        public async Task AssertCategoryCheckBoxesVisibleAsync()
        {
            await Assertions.Expect(fashionCheckBox).ToBeVisibleAsync();
            await Assertions.Expect(electronicsCheckBox).ToBeVisibleAsync();
            await Assertions.Expect(householdCheckBox).ToBeVisibleAsync();
        }
        public async Task AssertCategoryCheckBoxCheckedAsync(string category)
        {
            ILocator checkBox = category switch
            {
                "fashion" => fashionCheckBox,
                "electronics" => electronicsCheckBox,
                "household" => householdCheckBox,
                _ => throw new ArgumentException("Invalid category")
            };
            await Assertions.Expect(checkBox).ToBeCheckedAsync();
        }
        
    }
}