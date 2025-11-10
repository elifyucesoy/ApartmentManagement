using ApartmentManagement.Model;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagement.Services
{
    public class ApartmentService(AppDbContext db)
    {
        private readonly AppDbContext _db = db;

        // Tüm daireleri listele (User bilgisiyle birlikte)
        public async Task<List<Apartment>> GetAllAsync()
        {
            return await _db.Apartments
                            .AsNoTracking()
                            .Include(a => a.User)
                            .ToListAsync();
        }

        // Id'ye göre tek daire
        public async Task<Apartment?> GetByIdAsync(int id)
        {
            return await _db.Apartments
                            .AsNoTracking()
                            .Include(a => a.User)
                            .FirstOrDefaultAsync(a => a.Id == id);
        }

        // Yeni daire oluştur
        public async Task<Apartment> CreateAsync(Apartment apt)
        {
            await _db.Apartments.AddAsync(apt);
            await _db.SaveChangesAsync();
            return apt;
        }

        // Güncelle (varsa true)
        public async Task<bool> UpdateAsync(Apartment apt)
        {
            var exists = await _db.Apartments.AnyAsync(a => a.Id == apt.Id);
            if (!exists) return false;

            _db.Apartments.Update(apt);
            await _db.SaveChangesAsync();
            return true;
        }

        // Sil (varsa true)
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _db.Apartments.FindAsync(id);
            if (entity == null) return false;

            _db.Apartments.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        internal async Task<IEnumerable<object>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }
    }
}
