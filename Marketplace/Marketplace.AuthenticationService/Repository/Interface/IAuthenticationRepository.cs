using Marketplace.BaseLibrary;
using Marketplace.BaseLibrary.Interfaces.Base.Repository;
using User = Marketplace.BaseLibrary.Entity.Base.User.User;

namespace Marketplace.AuthenticationService.Repository.Interface;

public interface IAuthenticationRepository : IBaseRepository<User>
{
    public Task<User?> GetAsync(AuthenticationRequest request);
}