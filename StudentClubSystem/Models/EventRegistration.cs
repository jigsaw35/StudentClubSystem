using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentClubSystem.Models
{
    public class EventRegistration
    {
        [Key]
        public int Id { get; set; }

        public int EtkinlikId { get; set; }

        // DÜZELTME: Attribute doğru yere taşındı
        [ForeignKey("EtkinlikId")]
        public virtual Event Event { get; set; }

        // DÜZELTME: 'KullaniciAdi' -> 'KullaniciId' (int tipine uygun isim)
        public int KullaniciId { get; set; }

        // DÜZELTME: Attribute doğru yere taşındı ve isim düzeltildi
        [ForeignKey("KullaniciId")]
        public virtual User User { get; set; }

        // DÜZELTME: 'KayıtTarihi' -> 'KayitTarihi'
        public DateTime KayitTarihi { get; set; } = DateTime.Now;

        public string OnayDurumu { get; set; } = "Beklemede"; // Beklemede / Onaylı / Reddedildi

        public string KatilimDurumu { get; set; } = "-"; // Geldi / Gelmedi
    }
}