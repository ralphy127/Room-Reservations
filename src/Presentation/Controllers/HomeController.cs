using Microsoft.AspNetCore.Mvc;
using RoomRes.Domain.Interfaces;

public class HomeController : Controller {
    private readonly IUserService _userService;

    public HomeController(IUserService userService) {
        _userService = userService;
    }
    public IActionResult Index() {
        return View();
    }
}