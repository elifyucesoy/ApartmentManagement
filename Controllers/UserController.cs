using ApartmentManagement.Model;
using ApartmentManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagement.Controllers
{
    public class UserController(UserService userService) : Controller
    {
        private readonly UserService _userService = userService;

        // Ana sayfa - Tüm kullanıcıları listeler
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllAsync();
            return View(users);
        }

        // Kullanıcı detaylarını getir (Ajax)
        [HttpGet]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound(new { success = false, message = "Kullanıcı bulunamadı" });

            return Json(new { success = true, data = user });
        }

        // Yeni kullanıcı oluştur (Ajax)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return Json(new { success = false, message = "Doğrulama hatası", errors });
            }

            try
            {
                user.CreatedAt = DateTime.Now;
                var result = await _userService.CreateAsync(user);
                return Json(new { success = true, message = "Kullanıcı başarıyla oluşturuldu", data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Bir hata oluştu: " + ex.Message });
            }
        }

        // Kullanıcı güncelle (Ajax)
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return Json(new { success = false, message = "Doğrulama hatası", errors });
            }

            try
            {
                var existingUser = await _userService.GetByIdAsync(user.Id);
                if (existingUser == null)
                    return Json(new { success = false, message = "Kullanıcı bulunamadı" });

                var result = await _userService.UpdateAsync(user);
                return Json(new { success = true, message = "Kullanıcı başarıyla güncellendi", data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Bir hata oluştu: " + ex.Message });
            }
        }

        // Kullanıcı sil (Ajax)
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                if (user == null)
                    return Json(new { success = false, message = "Kullanıcı bulunamadı" });

                await _userService.DeleteAsync(id);
                return Json(new { success = true, message = "Kullanıcı başarıyla silindi" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Bir hata oluştu: " + ex.Message });
            }
        }

        // Kullanıcı durumunu değiştir (Aktif/Pasif)
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                if (user == null)
                    return Json(new { success = false, message = "Kullanıcı bulunamadı" });

                user.IsActive = !user.IsActive;
                await _userService.UpdateAsync(user);

                return Json(new { 
                    success = true, 
                    message = $"Kullanıcı {(user.IsActive ? "aktif" : "pasif")} edildi",
                    isActive = user.IsActive 
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Bir hata oluştu: " + ex.Message });
            }
        }
    }
}