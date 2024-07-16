using Google.Protobuf.WellKnownTypes;
using Marketplace.BaseLibrary.Entity.Base.User;

namespace Marketplace.BaseLibrary.Utils;

public static class GrpcHelper
{
    public static UserGrpc ConvertToGrpc(User? user) =>
        user != null
            ? new UserGrpc
            {
                UserRole = (UserRole)user.UserRole,
                CreatedTime = user.CreatedTime.ToTimestamp(),
                UpdatedTime = user.UpdatedTime?.ToTimestamp(),
                Name = user.Name,
                Patronimic = user.Patronimic,
                Surname = user.Surname,
                Phone = user.Phone,
                PhoneConfirmed = user.PhoneConfirmed,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                UserAlias = user.UserAlias,
                Password = user.Password,
                PasswordHash = user.PasswordHash
            }
            : new UserGrpc();
}