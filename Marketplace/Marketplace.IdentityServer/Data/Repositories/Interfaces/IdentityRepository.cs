using Marketplace.BaseLibrary.Entity.Identity;
using Marketplace.Identity.Data.Repositories.Implementations;
using Marketplace.JwtExtension.Models;
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

    public async Task<bool> AddRefreshToken(RefreshToken refreshToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateRefreshToken(RefreshToken refreshToken)
    {
        var currentToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Id == refreshToken.Id);
        if (currentToken == null)
        {
            return false;
        }

        currentToken = refreshToken;
        _context.RefreshTokens.Entry(currentToken).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RevokeRefreshToken(Guid tokenId)
    {
        var currentToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Id == tokenId);
        if (currentToken == null)
        {
            return false;
        }

        currentToken.IsRevoked = true;
        _context.RefreshTokens.Entry(currentToken).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return true;
    }
}