using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentClubSystem.Models
{
    public class EventRegistration
    {
        [Key]
        public int Id { get; set; }

        public int EtkinlikId { get; set; }

        
        [ForeignKey("EtkinlikId")]
        public virtual Event Event { get; set; }

        
        public int KullaniciId { get; set; }

        
        [ForeignKey("KullaniciId")]
        public virtual User User { get; set; }

        
        public DateTime KayitTarihi { get; set; } = DateTime.Now;

        public string OnayDurumu { get; set; } = "Beklemede"; 

        public string KatilimDurumu { get; set; } = "-"; 
    }
}