using System.ComponentModel.DataAnnotations;

namespace StudentClubSystem.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)] 
        public string AdSoyad { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Sifre { get; set; }

        public string Rol { get; set; } 

        public virtual ICollection<EventRegistration>? EventRegistrations { get; set; }
    }
}