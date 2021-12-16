using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SuperPanel.App.Helpers;
using SuperPanel.App.Services.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperPanel.App.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;

        public UsersController(ILogger<UsersController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("/users/get")]
        public async Task<IActionResult> GetUsers(int pageSize, int pageNumber)
        {
            var users = await _userService.GetUsersBy(pageSize, pageNumber);
            return Json(users);
        }

        [HttpPut]
        [Route("api/users/gdpr")]
        public async Task<IActionResult> RequestUserGDPR(string userEmailsJson)
        {
            var userEmails = JsonConvert.DeserializeObject<List<string>>(userEmailsJson);
            
            var gdprResult = await _userService.RequestGDPR(userEmails);

            return Json(gdprResult.NotFoundUserEmails);
        }
    }
}
