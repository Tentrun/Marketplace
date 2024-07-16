using AuthorizationService.Services.Interface;
using Marketplace.BaseLibrary;
using Marketplace.BaseLibrary.Interfaces.Base;
using Marketplace.LoggerService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using User = Marketplace.BaseLibrary.Entity.Base.User.User;

namespace AuthorizationService.Repository.Implementation;

public class AuthorizationRepository(ApplicationDbContext context, ILogger<AuthorizationRepository> logger) : BaseRepository<User>(context), IAuthorizationRepository
{
    public async Task<User?> GetAsync(AuthorizeRequest request)
    {
        try
        {
            return await context.Set<User>().SingleOrDefaultAsync(e => e.UserAlias == request.UserAlias);
        }
        catch (Exception e)
        {
            logger.LogCritical(e.ToString());
            throw;
        }
    }
}