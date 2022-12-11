using DemoApplication.Areas.Client.ViewComponents;
using DemoApplication.Areas.Client.ViewModels.Basket;
using DemoApplication.Database;
using DemoApplication.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Xml;

namespace DemoApplication.Areas.Client.Controllers
{
    [Area("client")]
    [Route("basket")]
    public class BasketController : Controller
    {
        private readonly DataContext _dataContext;

        public BasketController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }


        [HttpGet("add/{id}", Name = "client-basket-add")]
        public async Task<IActionResult> AddProductAsync([FromRoute] int id)
        {
            var product = await _dataContext.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (product is null)
            {
                return NotFound();
            }

            var productCookieValue = HttpContext.Request.Cookies["products"];
            var productsCookieViewModel = productCookieValue is not null 
                ?  JsonSerializer.Deserialize<List<ProductCookieViewModel>>(productCookieValue)
                : new List<ProductCookieViewModel> { };

            var productCookieViewModel = productsCookieViewModel!.FirstOrDefault(pcvm => pcvm.Id == id);
            if (productCookieViewModel is null)
            {
                productsCookieViewModel
                    !.Add(new ProductCookieViewModel(product.Id, product.Title, string.Empty, 1, product.Price, product.Price));
            }
            else
            {
                productCookieViewModel.Quantity += 1;
                productCookieViewModel.Total = productCookieViewModel.Quantity * productCookieViewModel.Price;
            }

            HttpContext.Response.Cookies.Append("products", JsonSerializer.Serialize(productsCookieViewModel));

            return ViewComponent(nameof(ShopCart), productsCookieViewModel);
        }

        [HttpGet("delete/{id}", Name = "client-basket-delete")]
        public async Task<IActionResult> DeleteProductAsync([FromRoute] int id)
        {
            var product = await _dataContext.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (product is null)
            {
                return NotFound();
            }

            var productCookieValue = HttpContext.Request.Cookies["products"];
            if (productCookieValue is null)
            {
                return NotFound();
            }

            var productsCookieViewModel = JsonSerializer.Deserialize<List<ProductCookieViewModel>>(productCookieValue);
            productsCookieViewModel!.RemoveAll(pcvm => pcvm.Id == id);

            HttpContext.Response.Cookies.Append("products", JsonSerializer.Serialize(productsCookieViewModel));

            return ViewComponent(nameof(ShopCart));
        }



        [HttpGet("addcolor/{id}", Name = "client-basket-color-add")]
        public async Task<IActionResult> AddColorAsync([FromRoute] int id)
        {
            var color = await _dataContext.Colors.FirstOrDefaultAsync(c => c.Id == id);

            if (color is null)
            {
                return NotFound();
            }

            var colorCookieValue = HttpContext.Request.Cookies["colors"];

            var colorsCookieViewModel = colorCookieValue is not null
                ? JsonSerializer.Deserialize<List<ColorCookieViewModel>>(colorCookieValue)
                : new List<ColorCookieViewModel> {} ;


            var colorCookieViewModel = colorsCookieViewModel!.FirstOrDefault(cvm => cvm.Id == id);
            if (colorCookieViewModel is null)
            {
                colorsCookieViewModel
                    
                    !.Add(new ColorCookieViewModel(color.Id, color.Name));

            }
            if (colorsCookieViewModel.Count>1)
            {
                colorsCookieViewModel.Remove(colorsCookieViewModel[0]);
            }

            //tamamda onda list mentiqi qalibdi yenede

            //he dolayi yolnan getmisiz mence commit edin yeniokz yazin. He ele e ok..Cunki sonra nese ok istesez bu list problem yarada biler neceki bayaq yaradirdi.

            //bu neye lazimdi?
            
            HttpContext.Response.Cookies.Append("colors", JsonSerializer.Serialize(colorsCookieViewModel));

            return ViewComponent(nameof(ColorShopCart), colorsCookieViewModel);
        }
        //[HttpGet("delete/{id}", Name = "client-basket-color-delete")] 
        //public async Task<IActionResult> DeleteColorAsync([FromRoute] int id)
        //{
        //    var product = await _dataContext.Books.FirstOrDefaultAsync(b => b.Id == id);
        //    if (product is null)
        //    {
        //        return NotFound();
        //    }

        //    var productCookieValue = HttpContext.Request.Cookies["products"];
        //    if (productCookieValue is null)
        //    {
        //        return NotFound();
        //    }

        //    var productsCookieViewModel = JsonSerializer.Deserialize<List<ProductCookieViewModel>>(productCookieValue);
        //    productsCookieViewModel!.RemoveAll(pcvm => pcvm.Id == id);

        //    HttpContext.Response.Cookies.Append("products", JsonSerializer.Serialize(productsCookieViewModel));

        //    return ViewComponent(nameof(ShopCart));
        //}
    }
}
