using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using RoomRes.Domain.Interfaces;
using RoomRes.Domain.Models;
using RoomRes.Presentation.ViewModels;

namespace RoomRes.Presentation.Controllers {
    public class UserController : Controller {
        private readonly IUserService _userService;
        private readonly IRoomReservationService _roomReservationService;

        public UserController(IUserService userService, IRoomReservationService roomReservationService) {
            _userService = userService;
            _roomReservationService = roomReservationService;
        }

        public async Task<IActionResult> Index() {
            string? userId = HttpContext.Session.GetString("userId");

            if(userId is null) {
                return RedirectToAction("Login");
            }

            List<RoomReservation> roomReservations = await _roomReservationService.GetByUserIdAsync(userId);

            RoomReservationListViewModel viewModel = new RoomReservationListViewModel() {
                RoomReservations = roomReservations,
            };

            return View(viewModel);
        }


        public IActionResult Add() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(UserAddViewModel viewModel) {
            if(!ModelState.IsValid || viewModel.User?.Id is null || viewModel.User.Username is null || viewModel.User.Password is null) {
                return View(viewModel); 
            }

            User newUser = new User(viewModel.User.Id, viewModel.User.Username, viewModel.User.Password);

            await _userService.AddAsync(newUser);
    
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string userId) {
            try {
                User user = await _userService.GetByIdAsync(userId);
                await _userService.DeleteAsync(user);
            } catch (Exception ex) {
                TempData["ErrorMessage"] = $"Deleting the user failed. Error: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        public IActionResult Login() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password) {
            User? user = await _userService.AuthenticateAsync(username, password);
            if(user is null || user.Id is null) {
                ViewData["ErrorMessage"] = "Incorrect username or password.";
                return View();
            }

            HttpContext.Session.SetString("userId", user.Id);
                
            if(user.Id == "1") {
                return RedirectToAction("Index", "Admin");
            }
            return RedirectToAction("Index", "User");
        }


        public IActionResult Register() {
            UserAddViewModel viewModel = new UserAddViewModel {
                User = new User(),
            };

            return View(viewModel);
        }

        [HttpPost] 
        public async Task<IActionResult> Register(UserAddViewModel viewModel) {
            if(viewModel?.User?.Username is null || viewModel.User.Password is null) {
                ViewData["ErrorMessage"] = "Incorrect user data";
                return View();
            }

            if(await _userService.IsUsernameTaken(viewModel.User.Username)) {
                ModelState.AddModelError("User.Username", "Username is taken.");
                return View(viewModel);
            }

            viewModel.User.Id = await _userService.GetNextIdAsync();
            ModelState.Remove("User.Id");

            if(!ModelState.IsValid) {
                ModelState.AddModelError("", "Registering new user failed.");
                return View(viewModel);
            }

            User user = new User(viewModel.User.Id, viewModel.User.Username, viewModel.User.Password);
            await _userService.AddAsync(user);
            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout() {
            await HttpContext.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> List() {
            UserListViewModel viewModel = new() {
                Users = await _userService.GetAllAsync(),
            };

            return View(viewModel);
        }
    }
}