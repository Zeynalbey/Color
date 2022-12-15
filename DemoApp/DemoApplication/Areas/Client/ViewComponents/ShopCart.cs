using DemoApplication.Areas.Client.ViewModels.Basket;
using DemoApplication.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DemoApplication.Areas.Client.ViewComponents
{
    public class ShopCart : ViewComponent
    {
        public IViewComponentResult Invoke(List<BasketViewModel>? viewModels = null)
        {
            var productcookie = HttpContext.Request.Cookies["products"];
            var products = new List<BasketViewModel>();

            if (productcookie is not null)
            {
                products = JsonSerializer.Deserialize<List<BasketViewModel>>(productcookie);

            }
            if (viewModels is not null)
            {
                products = viewModels;
            }
            return View(products);
        }
       

    }
}
