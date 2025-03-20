using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using RoomRes.Domain.Models;
using RoomRes.Presentation.ViewModels;
using RoomRes.Domain.Interfaces;

namespace RoomRes.Presentation.Controllers {
    public class RoomReservationController : Controller {
        private readonly IRoomReservationService _roomReservationService;
        private readonly IRoomService _roomService;
        private readonly IUserService _userService;

        public RoomReservationController(IRoomReservationService roomReservationService, IRoomService roomService, IUserService userService) {
            _roomReservationService = roomReservationService;
            _roomService = roomService;
            _userService = userService;
        }

        public async Task<IActionResult> Index() {
            if(HttpContext.Session.GetString("userId") != "1") {
                return RedirectToAction("Index", "User");
            }

            RoomReservationListViewModel viewModel = new RoomReservationListViewModel() {
                RoomReservations = await _roomReservationService.GetAllAsync(),
            };

            return View(viewModel);
        }

        public IActionResult Add() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(RoomReservationAddViewModel viewModel) {
            ModelState.Remove("RoomReservation.Id");
            ModelState.Remove("RoomReservation.UserId");
            ModelState.Remove("RoomReservation.ValidationTime");
            if(!ModelState.IsValid || viewModel.RoomReservation is null || viewModel.RoomReservation.RoomId is null) {
                return BadRequest();
            }

            string roomId = viewModel.RoomReservation.RoomId;
            Room room = await _roomService.GetByIdAsync(roomId);
            if(room is null) {
                ViewData["ErrorMessage"] = $"There is no room of id: {roomId}";
                return View(viewModel);
            }

            if(!await _roomReservationService.IsRoomFree(roomId, viewModel.RoomReservation.StartingTime, viewModel.RoomReservation.Duration)) {
                ViewData["ErrorMessage"] = $"Room has reservation during this time.";
                return View(viewModel);
            }

            string? userId = HttpContext.Session.GetString("userId");
            if (userId is null) {
                return RedirectToAction("Login", "Account");
            }

            try {
                string nextId = await _roomReservationService.GetNextIdAsync();
                RoomReservation newReservation = new RoomReservation(nextId, roomId, userId, DateTime.Now, viewModel.RoomReservation.StartingTime, viewModel.RoomReservation.Duration);

                await _roomReservationService.AddAsync(newReservation);
            } catch (Exception ex) {
                ViewData["ErrorMessage"] = $"Failed to add room reservatioin. Error: {ex.Message}";
            }
            
            
            if(userId == "1") {
                return RedirectToAction("Index");
            }
            
            return RedirectToAction("Index", "User");
        }

        public async Task<IActionResult> Update(string id) {
            if(HttpContext.Session.GetString("userId") != "1") {
                return RedirectToAction("Index", "User");
            }

            RoomReservation reservation = await _roomReservationService.GetByIdAsync(id);

            return View(reservation);
        }

        [HttpPost]
        public async Task<IActionResult> Update(RoomReservation reservation) {
            if(HttpContext.Session.GetString("userId") != "1") {
                return RedirectToAction("Index", "User");
            }
            
            if(!ModelState.IsValid || reservation.Id is null) {
                return View(reservation);
            }

            RoomReservation existingReservation = await _roomReservationService.GetByIdAsync(reservation.Id);

            if(existingReservation is not null) {
                await _roomReservationService.UpdateAsync(reservation);
                return Redirect("Index");
            }
            
            ViewData["ErrorMessage"] = $"Wybrana rezerwacja nie istnieje.";
            return View(reservation);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string roomReservationId) {
            try {
                RoomReservation roomReservation = await _roomReservationService.GetByIdAsync(roomReservationId);
                await _roomReservationService.DeleteAsync(roomReservation);
            } catch (Exception ex) {
                TempData["ErrorMessage"] = $"Deleting the room reservation failed. Error: {ex.Message}";
            }

            if(HttpContext.Session.GetString("userId") == "1") {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index", "User");
        }
    }
}