using System.ComponentModel.DataAnnotations;

namespace StudentClubSystem.Models
{
    public class Club
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Kulüp adı zorunludur.")]
        [StringLength(100)]
        public string Ad { get; set; }

        // DÜZELTME: 'Acıklama' -> 'Aciklama' (Türkçe karakter kaldırıldı)
        public string Aciklama { get; set; }

        public DateTime KurulusTarihi { get; set; }

        public bool AktifMi { get; set; } = true;

        // Navigation Property (İlişki)
        public virtual ICollection<Event>? Events { get; set; }
    }
}