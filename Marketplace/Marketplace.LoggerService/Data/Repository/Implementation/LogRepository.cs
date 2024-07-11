using Marketplace.BaseLibrary.Entity.Base.Logger;
using Marketplace.BaseLibrary.Interfaces.Base;
using Marketplace.LoggerService.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.LoggerService.Data.Repository.Implementation;

public class LogRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory) : BaseRepository<Log, ApplicationDbContext>(dbContextFactory), ILogRepository;