using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SuperPanel.App.Helpers;
using SuperPanel.App.Services.Abstract;

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
        public IActionResult GetUsers(int pageSize, int pageNumber)
        {
            var users = _userService.GetUsersBy(pageSize, pageNumber);
            return Json(users);
        }
    }
}
