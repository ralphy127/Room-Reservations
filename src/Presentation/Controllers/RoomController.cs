using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using RoomRes.Domain.Interfaces;
using RoomRes.Domain.Models;
using RoomRes.Presentation.ViewModels;

namespace RoomRes.Presentation.Controllers {
    public class RoomController : Controller {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService) {
            _roomService = roomService;
        }

        public async Task<IActionResult> Index() {
            RoomListViewModel viewModel = new() {
                Rooms = await _roomService.GetAllAsync(),
            };

            return View(viewModel);
        }

        public IActionResult Add() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(RoomAddViewModel viewModel) {
            if(!ModelState.IsValid || viewModel.Room is null || viewModel.Room.Id is null) {
                return BadRequest();
            }

            string id = viewModel.Room.Id;
            int capacity = viewModel.Room.Capacity;

            Room room = await _roomService.GetByIdAsync(id);
            if(room is not null) {
                ViewData["ErrorMessage"] = $"ID: {id} is taken.";
                return View(viewModel);
            }

            Room newRoom = new Room(id, capacity);
            await _roomService.AddAsync(newRoom);

            return RedirectToAction("Index");
        }

        public IActionResult Update() {
            if(HttpContext.Session.GetString("userId") != "1") {
                return RedirectToAction("Index", "User");
            }

            return View();
        }

        [HttpGet("Room/Update/{roomId}")]
        public async Task<IActionResult> Update(string roomId) {
            Room room = await _roomService.GetByIdAsync(roomId);
            if (room is null) {
                return NotFound();
            }

            RoomAddViewModel viewModel = new RoomAddViewModel {
                Room = room,
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(RoomAddViewModel viewModel) {
            if (!ModelState.IsValid || viewModel.Room is null  || viewModel.Room.Id is null) {
                return View(viewModel);
            }

            Room existingRoom = await _roomService.GetByIdAsync(viewModel.Room.Id);
            if (existingRoom is null) {
                return NotFound();
            }

            existingRoom.Capacity = viewModel.Room.Capacity;
            await _roomService.UpdateAsync(existingRoom);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string roomId) {
            try {
                Room room = await _roomService.GetByIdAsync(roomId);
                await _roomService.DeleteAsync(room);
            } catch (Exception ex) {
                TempData["ErrorMessage"] = $"Deleting the room failed. Error: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}