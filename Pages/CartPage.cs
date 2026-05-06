using System.Threading.Tasks;
using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace PlaywrightTests.Pages
{
    public class CartPage
    {
        private readonly IPage _page;
        private ILocator deleteButton => _page.Locator(".btn-danger"); 
        private ILocator buyButton => _page.Locator("//*[contains(text(), 'Buy Now')]");
        private ILocator checkoutButton => _page.Locator("//*[contains(text(), 'Checkout')]");
        private ILocator noCartitemsMessage => _page.Locator("//*[contains(text(), 'No Products in Your Cart !')]");
        private ILocator placeOrderButton => _page.Locator("//*[contains(text(), 'Place Order')]");
        private ILocator continueShoppingButton => _page.Locator("//*[contains(text(), 'Continue Shopping')]");

        private ILocator selectCountry => _page.Locator("//input[@placeholder='Select Country']");
        private ILocator orderConfirmationMessage => _page.Locator(".hero-primary");
       

        // Constructor 
        public CartPage(IPage page)
        {
            _page = page;
        }

        //Functions or methods
        public async Task DeleteProductFromCartAsync(string product)
        {
            var productLocator = _page.Locator($"text={product}");
            if (await productLocator.IsVisibleAsync())
            {
                await deleteButton.ClickAsync();
            }
            else
            {
                throw new Exception($"Product '{product}' not found in the cart.");
            }
        }
        public async Task ClickBuyNowAsync()
        {
            await buyButton.ClickAsync();
        }
        public async Task ClickCheckoutAsync()
        {
            await checkoutButton.ClickAsync();
        }
        public async Task ClickPlaceOrderAsync()
        {
            await placeOrderButton.ClickAsync();
        }

        public async Task FillSequentlyCountryAsync(string country)
        {
            await selectCountry.ClickAsync();
            await selectCountry.TypeAsync(country, new LocatorTypeOptions { Delay = 100 });
            
            // Wait for suggestions container to appear
            var suggestionsContainer = _page.Locator(".ta-results");
            await suggestionsContainer.WaitForAsync();
            
            // Add additional wait time for suggestions to populate
            await _page.WaitForTimeoutAsync(500);
            
            // Get all suggestion buttons and iterate to find matching country
            var suggestionButtons = _page.Locator(".ta-results .ng-star-inserted");
            int count = await suggestionButtons.CountAsync();
            
            for (int i = 0; i < count; i++)
            {
                var buttonText = await suggestionButtons.Nth(i).TextContentAsync();
                if (buttonText != null && buttonText.Contains(country, StringComparison.OrdinalIgnoreCase))
                {
                    await suggestionButtons.Nth(i).ClickAsync();
                    break;
                }
            }
        }
        
        public async Task OrderConfirmationMessageVisibleAsync()
        {
            await Expect(orderConfirmationMessage).ToBeVisibleAsync();
        }

        //Assertions
        public async Task AssertProductInCartAsync(string product)
        {
            var productInCart = _page.Locator($"text={product}");
            await Expect(productInCart).ToBeVisibleAsync();
        }
        public async Task AssertProductAddedToCartAsync(string product)
        {
            var cartItems = await _page.Locator(".cart").AllAsync();
            bool productFoundInCart = false;
            foreach (var item in cartItems)
            {
                var text = await item.TextContentAsync();
                if (text != null && text.Contains(product, StringComparison.OrdinalIgnoreCase))
                {
                    productFoundInCart = true;
                    break;
                }
            }
            if (!productFoundInCart)
            {
                throw new Exception($"Product '{product}' was not found in the cart.");
            }   
        }
        public async Task AssertDeleteButtonVisibleAsync()
        {
            await Expect(deleteButton).ToBeVisibleAsync();
        }
        public async Task AssertBuyButtonVisibleAsync()
        {
            await Expect(buyButton).ToBeVisibleAsync();
        }
        public async Task AssertCheckoutButtonVisibleAsync()
        {
            await Expect(checkoutButton).ToBeVisibleAsync();
        }
        public async Task AssertNoCartItemsMessageVisibleAsync()
        {
            await Expect(noCartitemsMessage).ToBeVisibleAsync();
        }
        public async Task AssertPlaceOrderButtonVisibleAsync()
        {
            await Expect(placeOrderButton).ToBeVisibleAsync();
        }
        public async Task AssertContinueShoppingButtonVisibleAsync()
        {
            await Expect(continueShoppingButton).ToBeVisibleAsync();
        }
        public async Task ClickContinueShoppingAsync()
        {
            await continueShoppingButton.ClickAsync();
        }
}
}