using DemoApplication.Areas.Client.ViewModels.Email;
using DemoApplication.Database;
using DemoApplication.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DemoApplication.Areas.Client.Controllers
{
    [Area("client")]
    [Route("email")]
    public class EmailController : Controller
    {


        protected readonly DataContext _dataContext;

        public EmailController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        #region Add



        [HttpPost("add", Name = "client-email-add")]
        public async Task <IActionResult> AddSync(EmailAddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _dataContext.Emails.Add(new Email
            {
                Name = model.Email,

            });

            await _dataContext.SaveChangesAsync();
            return Ok();

        }

        #endregion
    }
}
