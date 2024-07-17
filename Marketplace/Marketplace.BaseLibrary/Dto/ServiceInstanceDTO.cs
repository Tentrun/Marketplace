using Marketplace.BaseLibrary.Enum.Base;

namespace Marketplace.BaseLibrary.Dto;

public record ServiceInstanceDTO(string Host, int Port, ServiceStatusEnum ServiceStatus, string Name, string Description);