using Google.Protobuf.WellKnownTypes;

namespace Marketplace.BaseLibrary.Interfaces.Base.Service;

public interface IBaseService
{
    public Task<Struct> CallService(string method, Struct requestBody);
}