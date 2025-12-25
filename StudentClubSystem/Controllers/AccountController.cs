using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies; // Gerekli
using Microsoft.AspNetCore.Mvc;
using StudentClubSystem.Models;
using StudentClubSystem.Repositories;
using System.Security.Claims;

namespace StudentClubSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IGenericRepository<User> _userRepository;

        public AccountController(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            // Rol ve İlişkili veriler formdan gelmez, validasyondan çıkaralım
            ModelState.Remove("Rol");
            ModelState.Remove("EventRegistrations");

            if (ModelState.IsValid)
            {
                // Email kontrolü
                var existingUser = _userRepository.Get(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    ViewBag.Error = "Bu E-posta zaten kayıtlı.";
                    return View(user);
                }

                user.Rol = "Ogrenci"; // Varsayılan rol
                _userRepository.Add(user);
                return RedirectToAction("Login");
            }
            return View(user);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string sifre)
        {
            var user = _userRepository.Get(u => u.Email == email && u.Sifre == sifre);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.AdSoyad),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Rol),
                    new Claim("UserId", user.Id.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return RedirectToAction("Index", "Home"); // Home yoksa Club/Index'e yönlendiririz
            }

            ViewBag.Error = "E-posta veya şifre hatalı!";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // Profil Sayfası
        [Microsoft.AspNetCore.Authorization.Authorize] //Sadece giriş yapanlar görebilir
        public IActionResult Profile()
        {
            // Giriş yapan kullanıcısı Id sini bul
            var userIdClaim = User.FindFirst("UserId");
            if(userIdClaim==null)
            {
                return RedirectToAction("Login");
            }
            int userId = int.Parse(userIdClaim.Value);

            //2. kullanıcıyı getir + katıldığı etkinlikleri (EventRegistrations) + 0 etkinliklerini detaylarını  (event)
            // repostorymizdeki "includeProperties" parametresi sayesinde iç içe tabloları çekebiliriz.
            // "EventRegistrations.Event" demek: Kayıtlar tablosuna git, oradan da Etkinlik tablosuna git.
            var user = _userRepository.Get(u => u.Id == userId, includeProperties: "EventRegistrations.Event");
            return View(user);
        }

    }
}