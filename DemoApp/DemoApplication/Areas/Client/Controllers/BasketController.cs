
using DemoApplication.Areas.Client.ViewComponents;
using DemoApplication.Areas.Client.ViewModels.Basket;
using DemoApplication.Database;
using DemoApplication.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
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

        [HttpGet("add/{id}",Name = "client-basket-color-add")]   
        public IActionResult Add([FromRoute]int id)
        {
            var productscookie = HttpContext.Request.Cookies["products"];
            var product = _dataContext.Books.FirstOrDefault(b=> b.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var productViewModel = new List<BasketViewModel>();

            if (productscookie == null)
            {
                 productViewModel = new List<BasketViewModel>
                {
                    new BasketViewModel(product.Id,product.Title,String.Empty,1, product.Price,product.Price )
                };
                HttpContext.Response.Cookies.Append("products", JsonSerializer.Serialize(productViewModel));

            }
            else
            {
                 productViewModel = JsonSerializer.Deserialize<List<BasketViewModel>>(productscookie);
                var objectModel = productViewModel.FirstOrDefault(b => b.Id == id);

                if (objectModel is null)
                {
                    productViewModel.Add(new BasketViewModel(product.Id, product.Title, String.Empty, 1, product.Price, product.Price));
                    HttpContext.Response.Cookies.Append("products", JsonSerializer.Serialize(productViewModel));
                }
                else
                {
                    objectModel.Quantity += 1;
                    objectModel.Total = objectModel.Price * objectModel.Quantity;
                    HttpContext.Response.Cookies.Append("products", JsonSerializer.Serialize(productViewModel));
                }

            }
            return ViewComponent(nameof(ShopCart), productViewModel);
        }


        [HttpGet("delete/{id}", Name = "client-basket-delete")]
        public async Task<IActionResult> DeleteProductAsync([FromRoute] int id)
        {
            var product = await _dataContext.Books.FirstOrDefaultAsync(b => b.Id == id);
            var cookiModel = HttpContext.Request.Cookies["products"];
            if (product is null)
            {
                return NotFound();
            }

            var productviewmodel = new List<BasketViewModel> { };
            

            if (cookiModel is null)
            {
                return NotFound();

            }
            else
            {
               productviewmodel = JsonSerializer.Deserialize<List<BasketViewModel>>(cookiModel);
                //productviewmodel!.RemoveAll(pcvm => pcvm.Id == id);

                foreach (var item in productviewmodel)
                {
                    if(item.Id == id)
                    {
                        if (item.Quantity == 1)
                        {
                            productviewmodel.Remove(item);
                            break;
                        }
                        else
                        {
                            item.Quantity -= 1;
                            item.Total = item.Price * item.Quantity;
                            HttpContext.Response.Cookies.Append("products", JsonSerializer.Serialize(productviewmodel));

                        }
                    }
                }

                HttpContext.Response.Cookies.Append("products", JsonSerializer.Serialize(productviewmodel));

                return ViewComponent(nameof(ShopCart), productviewmodel);
            }

        }

    }
}

























