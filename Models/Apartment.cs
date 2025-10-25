using ApartmentManagement.Model;
using System.ComponentModel.DataAnnotations;

namespace ApartmentManagement.Model
{
    public class Apartment
    {
        public int Id { get; set; }

        public string Title { get; set; }    // Daire adı / kısa açıklama (örn: "A Blok 3.Kat 12 No")

        [MaxLength(20)]
        public string? Block { get; set; }           // A/B/C blok gibi (opsiyonel)

        public int? Floor { get; set; }              // Kat (opsiyonel)
        public int? Number { get; set; }             // Daire no (opsiyonel)

        // Sahibi/Kiracısı: User FK (opsiyonel artık)
        public int? UserId { get; set; }

        public bool IsOccupied { get; set; } = true; // Dolu mu?
        public decimal? MonthlyFee { get; set; }     // Aidat (opsiyonel)

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public User? User { get; set; }
    }
}

