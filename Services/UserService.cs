namespace ApartmentManagement.Services;

using ApartmentManagement.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class UserService(AppDbContext db)
{
    private readonly AppDbContext _db = db;

    // Tüm kullanıcıları getir
    public async Task<List<User>> GetAllAsync()
    {
        return await _db.Users
                        .AsNoTracking()
                        .ToListAsync();
    }

    // Id'ye göre getir
    public async Task<User?> GetByIdAsync(int id )
    {
        return await _db.Users
                        .AsNoTracking()
                        .FirstOrDefaultAsync(u => u.Id == id );
    }

    // Yeni kullanıcı oluştur
    public async Task<User> CreateAsync(User user )
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        await _db.Users.AddAsync(user );
        await _db.SaveChangesAsync();

        return user;
    }

    // Güncelleme (varsa true döner)
    public async Task<bool> UpdateAsync(User user )
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        var exists = await _db.Users.AnyAsync(u => u.Id == user.Id );
        if (!exists) return false;

        _db.Users.Update(user);
        await _db.SaveChangesAsync();
        return true;
    }

    // Silme (varsa true döner)
    public async Task<bool> DeleteAsync(int id )
    {
        var entity = await _db.Users.FindAsync(new object[] { id } );
        if (entity == null) return false;

        _db.Users.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}

