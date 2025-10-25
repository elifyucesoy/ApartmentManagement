using System.ComponentModel.DataAnnotations;

namespace ApartmentManagement.Model
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // 1 Kullanıcı -> N Apartment
        public ICollection<Apartment>? Apartments { get; set; }
    }
}
