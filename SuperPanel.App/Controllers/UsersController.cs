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

        //[HttpPut]
        //[Route("/users/gdpr/{userId:int}")]
        //public IActionResult RequestUserGDPR(int userId)
        //{
        //    var user = _userService.RequestGDPR(userId);
        //    return Json(user);
        //}

        //[HttpPut]
        //[Route("/users/gdpr/{userId:int}/delete")]
        //public IActionResult RequestUserGDPRDelete(int userId)
        //{
        //    var user = _userService.RequestGDPRDelete(userId);
        //    return Json(user);
        //}
    }
}
