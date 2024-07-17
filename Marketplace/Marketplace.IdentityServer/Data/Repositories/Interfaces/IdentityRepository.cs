using Marketplace.BaseLibrary.Entity.Identity;
using Marketplace.Identity.Data.Repositories.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Identity.Data.Repositories.Interfaces;

public class IdentityRepository : IIdentityRepository
{
    private readonly ApplicationIdentityDbContext _context;

    public IdentityRepository(IDbContextFactory<ApplicationIdentityDbContext> dbContextFactory)
    {
        _context = dbContextFactory.CreateDbContext();
    }

    public async Task<IdentityUserModel?> GetUserByPhone(string phone)
    {
        return await _context.Users.AsNoTrackingWithIdentityResolution().FirstOrDefaultAsync(x => x.PhoneNumber == phone);
    }

    public async Task<IdentityUserModel?> GetUserByEmail(string email)
    {
        return await _context.Users.AsNoTrackingWithIdentityResolution().FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<bool> AddUserToRole(IdentityUserModel user, string role)
    {
        var existedRole = await _context.Roles.FirstOrDefaultAsync(x => x.Name == role);

        if (existedRole == null)
        {
            return false;
        }
        
        await _context.UserRoles.AddAsync(new IdentityUserRole<long>
        {
            UserId = user.Id,
            RoleId = existedRole.Id
        });
        await _context.SaveChangesAsync();

        return true;
    }
}