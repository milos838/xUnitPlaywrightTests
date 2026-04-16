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
}
}