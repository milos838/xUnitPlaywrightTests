using System.Threading.Tasks;
using Microsoft.Playwright;
using PlaywrightTests.Utilities;

namespace PlaywrightTests.Pages
{
    public class ManagerPage
    {
        private readonly HomePage _homePage;
        private readonly CartPage _cartPage;

        public ManagerPage(IPage page)
        {
            _homePage = new HomePage(page);
            _cartPage = new CartPage(page);
        }

        public async Task VerifyLoginFunctionAsync(string url)
        {
            await _homePage.NavigateAsync(url);
            await _homePage.AssertLoginFieldsVisibleAsync();
            await _homePage.SubmitLoginAsync();
            await _homePage.AssertDashboardHeaderVisibleAsync();
        }

        public async Task VerifyHomePageURLAsync(string url, string expectedURL)
        {
            await _homePage.NavigateAsync(url);
            await _homePage.SubmitLoginAsync();
            await _homePage.AssertURLAsync(expectedURL);
        }

        public async Task NavigateToAsync(string url)
        {
            await _homePage.NavigateAsync(url);
        }

        public async Task NavigateAndVerifyHomeTitleAsync(string url, string expectedTitle)
        {
            await _homePage.NavigateAsync(url);
            await _homePage.AssertPageTitleAsync(expectedTitle);
        }

        public async Task LoginAsync()
        {
            await _homePage.SubmitLoginAsync();
        }

        public async Task LoginAndVerifyAsync()
        {
            await _homePage.SubmitLoginAsync();
            await _homePage.AssertDashboardHeaderVisibleAsync();
        }

        public async Task VerifyHeaderLinksAsync(string url)
        {
            await _homePage.NavigateAsync(url);
            await _homePage.SubmitLoginAsync();
            await _homePage.AssertHeaderLinksVisibleAsync();
        }

        public async Task VerifySearchFieldAsync(string url, string searchTerm)
        {
            await _homePage.NavigateAsync(url);
            await _homePage.SubmitLoginAsync();
            await _homePage.AssertSearchBoxVisibleAsync();
            await _homePage.SearchProductAsync(searchTerm);
            await _homePage.AssertSearchedProductVisibleAsync();
        }

        public async Task VerifyPriceFilterAsync(string url, string minPrice, string maxPrice)
        {
            await _homePage.NavigateAsync(url);
            await _homePage.SubmitLoginAsync();
            await _homePage.AssertPriceFilterFieldsVisibleAsync();
            await _homePage.SetPriceFilterAsync(minPrice, maxPrice);
            await _homePage.AssertPriceSearchedProductVisibleAsync();
        }

        public async Task VerifyCategoryFiltersAsync(string url, string category1, string category2, string category3)
        {
            await _homePage.NavigateAsync(url);
            await _homePage.SubmitLoginAsync();
            await _homePage.AssertCategoryCheckBoxesVisibleAsync();
            await _homePage.CheckFashionBoxAsync();
            await _homePage.CheckElectronicsBoxAsync();
            await _homePage.CheckHouseholdBoxAsync();
            await _homePage.AssertCategoryCheckBoxCheckedAsync(category1);
            await _homePage.AssertCategoryCheckBoxCheckedAsync(category2);
            await _homePage.AssertCategoryCheckBoxCheckedAsync(category3);
        }

        public async Task VerifySubCategoryFiltersAsync(string url)
        {
            await _homePage.NavigateAsync(url);
            await _homePage.SubmitLoginAsync();
            await _homePage.AssertSubCategoryCheckBoxesVisibleAsync();
            await _homePage.CheckTShirtsBoxAsync();
            await _homePage.AssertSubCategoryTShirtsCheckBoxCheckedAsync();
            await _homePage.CheckShirtsBoxAsync();
            await _homePage.AssertSubCategoryShirtsCheckBoxCheckedAsync();
            await _homePage.CheckShoesBoxAsync();
            await _homePage.AssertSubCategoryShoesCheckBoxCheckedAsync();
            await _homePage.CheckMobilesBoxAsync();
            await _homePage.AssertSubCategoryMobilesCheckBoxCheckedAsync();
            await _homePage.CheckLaptopsBoxAsync();
            await _homePage.AssertSubCategoryLaptopsCheckBoxCheckedAsync();
        }

        public async Task VerifySearchForCheckboxesAsync(string url)
        {
            await _homePage.NavigateAsync(url);
            await _homePage.SubmitLoginAsync();
            await _homePage.AssertSearchForMenBoxVisibleAsync();
            await _homePage.AssertSearchForWomenBoxVisibleAsync();
            await _homePage.CheckSearchForMenBoxAsync();
            await _homePage.AssertSearchForMenBoxCheckedAsync();
            await _homePage.CheckSearchForWomenBoxAsync();
            await _homePage.AssertSearchForWomenBoxCheckedAsync();
        }

        public async Task AddProductToCartAndVerifyAsync(string url, string productName)
        {
            await PrepareCartWorkflowAsync(url);
            await _homePage.AddToCartAsync(productName);
            await _homePage.GoToCartAsync();
            await _cartPage.AssertProductInCartAsync(productName);
            await _cartPage.AssertProductAddedToCartAsync(productName);
        }

        public async Task DeleteProductFromCartAndVerifyAsync(string url, string productName)
        {
            await PrepareCartWorkflowAsync(url);
            await _homePage.AddToCartAsync(productName);
            await _homePage.GoToCartAsync();
            await _cartPage.AssertBuyButtonVisibleAsync();
            await _cartPage.DeleteProductFromCartAsync(productName);
            await _cartPage.AssertNoCartItemsMessageVisibleAsync();
        }

        public async Task BuyNowAndVerifyAsync(string url, string productName)
        {
            await PrepareCartWorkflowAsync(url);
            await _homePage.AddToCartAsync(productName);
            await _homePage.GoToCartAsync();
            await _cartPage.AssertBuyButtonVisibleAsync();
            await _cartPage.ClickBuyNowAsync();
            await _cartPage.AssertPlaceOrderButtonVisibleAsync();
        }

        public async Task CheckoutAndVerifyAsync(string url, string productName)
        {
            await PrepareCartWorkflowAsync(url);
            await _homePage.AddToCartAsync(productName);
            await _homePage.GoToCartAsync();
            await _cartPage.AssertCheckoutButtonVisibleAsync();
            await _cartPage.ClickCheckoutAsync();
            await _cartPage.AssertPlaceOrderButtonVisibleAsync();
        }

        public async Task ContinueShoppingAndVerifyAsync(string url, string productName)
        {
            await PrepareCartWorkflowAsync(url);
            await _homePage.AddToCartAsync(productName);
            await _homePage.GoToCartAsync();
            await _cartPage.AssertContinueShoppingButtonVisibleAsync();
            await _cartPage.ClickContinueShoppingAsync();
            await _homePage.AssertDashboardHeaderVisibleAsync();
        }

        public async Task PlaceOrderAndVerifyAsync(string url, string productName, string country)
        {
            await PrepareCartWorkflowAsync(url);
            await _homePage.AddToCartAsync(productName);
            await _homePage.GoToCartAsync();
            await _cartPage.ClickCheckoutAsync();
            await _cartPage.FillSequentlyCountryAsync(country);
            await _cartPage.ClickPlaceOrderAsync();
            await _cartPage.OrderConfirmationMessageVisibleAsync();
        }

        private async Task PrepareCartWorkflowAsync(string url)
        {
            await _homePage.NavigateAsync(url);
            await _homePage.SubmitLoginAsync();
            await _homePage.GoToCartAsync();
            await _cartPage.ClearCartAsync();
            await _homePage.NavigateToDashboardAsync(url);
        }
    }
}
