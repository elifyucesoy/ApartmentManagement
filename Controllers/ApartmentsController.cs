using ApartmentManagement.Model;
using ApartmentManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace ApartmentManagement.Controllers
{
    public class ApartmentsController(ApartmentService apartmentService, ILogger<ApartmentsController> logger) : Controller
    {
        private readonly ApartmentService _apartmentService = apartmentService;

        public async Task<IActionResult> Index()
        {
            var list = await apartmentService.GetAllAsync();
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> GetApartment(int id)
        {
            try
            {
                var apartment = await _apartmentService.GetByIdAsync(id);
                if (apartment == null)
                    return Json(new { success = false, message = "Apartment not found" });

                // Create a new anonymous object with only the needed properties
                var apartmentData = new
                {
                    id = apartment.Id,
                    block = apartment.Block,
                    number = apartment.Number,
                    floor = apartment.Floor,
                    isOccupied = apartment.IsOccupied,
                    createdAt = apartment.CreatedAt
                };

                return Json(new { success = true, data = apartmentData });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]Apartment apartment)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new { success = false, message = "Please check the form data and try again." });

                apartment.CreatedAt = DateTime.Now;
                apartment.UserId = 1;
                var result = await _apartmentService.CreateAsync(apartment);

                return Json(new { success = true, message = "Apartment created successfully.", data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Apartment apartment)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return Json(new { success = false, message = "Validation error", errors });
            }

            try
            {
                var existing = await _apartmentService.GetByIdAsync(apartment.Id);
                if (existing == null)
                    return Json(new { success = false, message = "Apartment not found" });

                var result = await _apartmentService.UpdateAsync(apartment);
                return Json(new { success = true, message = "Apartment updated successfully", data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var apartment = await _apartmentService.GetByIdAsync(id);
                if (apartment == null)
                    return Json(new { success = false, message = "Apartment not found" });

                await _apartmentService.DeleteAsync(id);
                return Json(new { success = true, message = "Apartment deleted successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }
    }
}










