using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using RoomRes.Domain.Interfaces;
using RoomRes.Domain.Models;

public class AdminController : Controller {
    private readonly IRoomService _roomService;
    private readonly IRoomReservationService _roomReservationService;
    private readonly IUserService _userService;

    public AdminController(IRoomService roomService, IRoomReservationService roomReservationService, IUserService userService) {
        _roomService = roomService;
        _roomReservationService = roomReservationService;
        _userService = userService;
    }

    public IActionResult Index() {
        return View();
    }

    public override void OnActionExecuting(ActionExecutingContext context) {
        if (HttpContext.Session.GetString("userId") != "1") {
            context.Result = RedirectToAction("Index", "User");
        }

        base.OnActionExecuting(context);
    }

    public async Task<IActionResult> ManageRooms() {
        List<Room> rooms = await _roomService.GetAllAsync();
        return View(rooms);
    }

    public async Task<IActionResult> ManageRoomReservations() {
        List<RoomReservation> roomReservations = await _roomReservationService.GetAllAsync();
        return View(roomReservations);
    }

    public async Task<IActionResult> ManageUsers() {
        List<User> users = await _userService.GetAllAsync();
        return View(users);
    }
}