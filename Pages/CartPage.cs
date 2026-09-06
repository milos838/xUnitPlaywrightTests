using System.Threading.Tasks;
using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace PlaywrightTests.Pages
{
    public class CartPage
    {
        private readonly IPage _page;
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
            var productRow = _page.Locator(".cart")
                .Filter(new() { HasText = product })
                .First;

            await Expect(productRow).ToBeVisibleAsync();
            await productRow.Locator(".btn-danger").ClickAsync();
            await Expect(productRow).ToBeHiddenAsync();
        }

        public async Task ClearCartAsync()
        {
            var deleteButtons = _page.Locator(".btn-danger");
            var itemCount = await deleteButtons.CountAsync();

            while (itemCount > 0)
            {
                await deleteButtons.First.ClickAsync();
                itemCount--;
                await Expect(deleteButtons).ToHaveCountAsync(itemCount);
            }

            await AssertNoCartItemsMessageVisibleAsync();
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
            await selectCountry.FillAsync(string.Empty);
            await selectCountry.PressSequentiallyAsync(country, new() { Delay = 100 });
            
            var suggestionsContainer = _page.Locator(".ta-results");
            await Expect(suggestionsContainer).ToBeVisibleAsync();

            var matchingSuggestion = suggestionsContainer
                .Locator(".ng-star-inserted")
                .Filter(new() { HasText = country })
                .First;

            await Expect(matchingSuggestion).ToBeVisibleAsync();
            await matchingSuggestion.ClickAsync();
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
            await Expect(_page.Locator(".btn-danger").First).ToBeVisibleAsync();
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