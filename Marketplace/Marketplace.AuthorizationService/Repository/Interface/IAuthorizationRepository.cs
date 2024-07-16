using Marketplace.BaseLibrary;
using Marketplace.BaseLibrary.Interfaces.Base.Repository;
using User = Marketplace.BaseLibrary.Entity.Base.User.User;

namespace AuthorizationService.Services.Interface;

public interface IAuthorizationRepository : IBaseRepository<User>
{
    public Task<User?> GetAsync(AuthorizeRequest request);
}