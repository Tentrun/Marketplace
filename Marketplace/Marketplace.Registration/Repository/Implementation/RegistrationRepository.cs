using Marketplace.BaseLibrary.Entity.Base.User;
using Marketplace.BaseLibrary.Interfaces.Base;
using Marketplace.Registration.Data;
using Marketplace.Registration.Repository.Interface;

namespace Marketplace.Registration.Repository.Implementation;

public class RegistrationRepository(ApplicationDbContext context) : BaseRepository<User>(context), IRegistrationRepository;