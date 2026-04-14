using System.Threading.Tasks;
using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace PlaywrightTests.Pages
{
    public class CartPage
    {
        private readonly IPage _page;

        // Constructor 
        public CartPage(IPage page)
        {
            _page = page;
        }

        //Functions or methods

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
}
}