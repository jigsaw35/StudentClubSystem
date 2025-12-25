using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentClubSystem.Models;
using StudentClubSystem.Repositories;
using System.Security.Claims;

namespace StudentClubSystem.Controllers
{
    public class EventController : Controller
    {
        private readonly IGenericRepository<Event> _eventRepository;
        private readonly IGenericRepository<Club> _clubRepository;
        private readonly IGenericRepository<EventRegistration> _registrationRepository;

        public EventController(
            IGenericRepository<Event> eventRepository,
            IGenericRepository<Club> clubRepository,
            IGenericRepository<EventRegistration> registrationRepository)
        {
            _eventRepository = eventRepository;
            _clubRepository = clubRepository;
            _registrationRepository = registrationRepository;
        }

        public IActionResult Index()
        {
            // Filtreleme dropdown'ı için kulüp listesi
            ViewBag.Clubs = new SelectList(_clubRepository.GetAll(), "Id", "Ad");

            // Listeleme
            var events = _eventRepository.GetAll(includeProperties: "Club");
            return View(events);
        }

        public IActionResult Create()
        {
            ViewBag.Clubs = new SelectList(_clubRepository.GetAll(), "Id", "Ad");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Event @event)
        {
            ModelState.Remove("Club");
            ModelState.Remove("EventRegistrations");

            if (ModelState.IsValid)
            {
                _eventRepository.Add(@event);
                return RedirectToAction("Index");
            }

            ViewBag.Clubs = new SelectList(_clubRepository.GetAll(), "Id", "Ad");
            return View(@event);
        }

        public IActionResult Edit(int id)
        {
            var @event = _eventRepository.GetById(id);
            if (@event == null) return NotFound();

            ViewBag.Clubs = new SelectList(_clubRepository.GetAll(), "Id", "Ad", @event.KulupId);
            return View(@event);
        }

        [HttpPost]
        public IActionResult Edit(Event @event)
        {
            ModelState.Remove("Club");
            ModelState.Remove("EventRegistrations");

            if (ModelState.IsValid)
            {
                // DÜZELTME: Add yerine Update kullanıldı
                _eventRepository.Update(@event);
                return RedirectToAction("Index");
            }

            ViewBag.Clubs = new SelectList(_clubRepository.GetAll(), "Id", "Ad", @event.KulupId);
            return View(@event);
        }

        // AJAX JOIN
        [HttpPost]
        [Authorize]
        public IActionResult Join(int eventId)
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null) return Json(new { success = false, message = "Kullanıcı bulunamadı." });

            int userId = int.Parse(userIdClaim.Value);

            var eventEntity = _eventRepository.GetById(eventId);
            if (eventEntity == null) return Json(new { success = false, message = "Etkinlik yok." });

            // Kontrol: Kullanıcı ID ve Etkinlik ID
            var existingReg = _registrationRepository.Get(r => r.EtkinlikId == eventId && r.KullaniciId == userId);
            if (existingReg != null) return Json(new { success = false, message = "Zaten kayıtlısınız." });

            var count = _registrationRepository.GetAll(r => r.EtkinlikId == eventId).Count;
            if (count >= eventEntity.Kontenjan) return Json(new { success = false, message = "Kontenjan dolu." });

            var newReg = new EventRegistration
            {
                EtkinlikId = eventId,
                KullaniciId = userId,
                KayitTarihi = DateTime.Now,
                OnayDurumu = "Beklemede"
            };

            _registrationRepository.Add(newReg);
            return Json(new { success = true, message = "Kayıt başarılı!" });
        }

        // Silme Metotları...
        public IActionResult Delete(int id)
        {
            var @event = _eventRepository.GetById(id);
            if (@event == null) return NotFound();
            return View(@event);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _eventRepository.Delete(id);
            return RedirectToAction("Index");
        }
        // AJAX filtreleme 
        public IActionResult Filter (int? kulupId , string durum , string search)
        {
            // repositoryden verileri çekiyoruz filtreleri uyguluyoruz
            //expression (lambda) ile dinamik sorgu oluşturuyoruz
            var events = _eventRepository.GetAll(
                filter: x =>
                //1 kulup Id filtresi (seçildiyse)
                (!kulupId.HasValue || x.KulupId == kulupId) &&

                //2. durum filtresi (seçildiyse)
                (string.IsNullOrEmpty(durum) || x.Durum == durum) &&

                //3  arama kutusu (doluysa başlık ve açıklamada ara)
                (string.IsNullOrEmpty(search) || x.Baslik.Contains(search) || x.Aciklama.Contains(search)),

                // ilişkili tabloyu çekmeyi unutmuyoruz

                includeProperties: "Club"
                );
            return PartialView("_EventListPartial", events);
        }
        // Modal için katılımcı listesi getirme 
        public IActionResult GetParticipants(int eventId)
        {
            // ilgili etkinliğe ait kayıtları  kullanıcı bilgileriyle beraber çekiyoruz
            var registrations = _registrationRepository.GetAll(
                filter: r => r.EtkinlikId == eventId,
                includeProperties: "User"
                );
            return PartialView("_ParticipantListPartial",registrations);
        }
        // Ajax ile durum güncelleme (onayla reddet)
        [HttpPost]
        public IActionResult UpdateStatus(int id, string status)
        {
            var registration = _registrationRepository.GetById(id);
            if (registration != null)
            {
                return Json(new { succes = false, message = "kayıt bulunamadı" });

                registration.OnayDurumu = status;
                _registrationRepository.Update(registration);
            }
            return Json(new { succes = true, message = "Durum güncellendi" });
        }
    }
}