using DemoApplication.Areas.Client.ViewModels.Basket;
using DemoApplication.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DemoApplication.Areas.Client.ViewComponents
{
    public class ColorShopCart : ViewComponent
    {
        public IViewComponentResult Invoke(List<ColorCookieViewModel>? viewModels = null)
        {
            var colorsCookieValue = HttpContext.Request.Cookies["colors"];

            var colorsCookieViewModel = new List<ColorCookieViewModel>();

            if (colorsCookieValue is not null)
            {
                colorsCookieViewModel = JsonSerializer.Deserialize<List<ColorCookieViewModel>>(colorsCookieValue);
            }


            return View(colorsCookieViewModel);
        }
    }
}
