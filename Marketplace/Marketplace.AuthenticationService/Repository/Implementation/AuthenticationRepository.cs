using Marketplace.AuthenticationService.Repository.Interface;
using Marketplace.BaseLibrary;
using Marketplace.BaseLibrary.Interfaces.Base;
using Marketplace.LoggerService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using User = Marketplace.BaseLibrary.Entity.Base.User.User;

namespace Marketplace.AuthenticationService.Repository.Implementation;

public class AuthenticationRepository(ApplicationDbContext context, ILogger<AuthenticationRepository> logger) : BaseRepository<User>(context), IAuthenticationRepository
{
    public async Task<User?> GetAsync(AuthenticationRequest request)
    {
        try
        {
            return await context.Set<User>()
                .SingleOrDefaultAsync(u =>
                    u.Password == request.Password && u.UserAlias == request.UserAlias && u.Email == request.Email);
        }
        catch (Exception e)
        {
            logger.LogCritical(e.ToString());
            throw;
        }
    }
}