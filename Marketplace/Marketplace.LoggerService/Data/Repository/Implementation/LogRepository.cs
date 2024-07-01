using Marketplace.BaseLibrary.Entity.Base.Logger;
using Marketplace.BaseLibrary.Interfaces.Base;
using Marketplace.LoggerService.Data.Repository.Interface;

namespace Marketplace.LoggerService.Data.Repository.Implementation;

public class LogRepository(ApplicationDbContext context) : BaseRepository<Log>(context), ILogRepository;