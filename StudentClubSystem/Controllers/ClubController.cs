using Microsoft.AspNetCore.Authorization; // Admin yetkisi için gerekebilir
using Microsoft.AspNetCore.Mvc;
using StudentClubSystem.Models;
using StudentClubSystem.Repositories;

namespace StudentClubSystem.Controllers
{
    public class ClubController : Controller
    {
        private readonly IGenericRepository<Club> _clubRepository;

        public ClubController(IGenericRepository<Club> clubRepository)
        {
            _clubRepository = clubRepository;
        }

        public IActionResult Index()
        {
            // Tüm kulüpleri getir
            var clubs = _clubRepository.GetAll();
            return View(clubs);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Club club)
        {
            // Events validasyonunu kaldır (yeni kulübün etkinliği olmaz)
            ModelState.Remove("Events");

            if (ModelState.IsValid)
            {
                club.KurulusTarihi = DateTime.Now;
                _clubRepository.Add(club);
                return RedirectToAction("Index");
            }
            return View(club);
        }

        public IActionResult Edit(int id)
        {
            var club = _clubRepository.GetById(id);
            if (club == null) return NotFound();
            return View(club);
        }

        [HttpPost]
        public IActionResult Edit(Club club)
        {
            ModelState.Remove("Events");

            if (ModelState.IsValid)
            {
                // Tarih formdan gelmediyse, eski tarihi korumak için repodan çekip atayabiliriz
                // Ama view tarafında hidden input koyduysak sorun yok.
                _clubRepository.Update(club);
                return RedirectToAction("Index");
            }
            return View(club);
        }

        public IActionResult Delete(int id)
        {
            var club = _clubRepository.GetById(id);
            if (club == null) return NotFound();
            return View(club);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _clubRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}