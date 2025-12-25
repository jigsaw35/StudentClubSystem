using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentClubSystem.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        public int KulupId { get; set; }

        [ForeignKey("KulupId")]
        public virtual Club Club { get; set; }

        [Required]
        [StringLength(200)]
        // DÜZELTME: 'Baslık' -> 'Baslik'
        public string Baslik { get; set; }

        // DÜZELTME: 'Acıklama' -> 'Aciklama'
        public string Aciklama { get; set; }

        // DÜZELTME: 'BaslangıcTarihi' -> 'BaslangicTarihi'
        public DateTime BaslangicTarihi { get; set; }

        public DateTime BitisTarihi { get; set; }

        public int Kontenjan { get; set; }
        public string Konum { get; set; }

        public string Durum { get; set; } = "Planlandı";

        public virtual ICollection<EventRegistration>? EventRegistrations { get; set; }
    }
}