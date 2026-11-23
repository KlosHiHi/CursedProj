using OxideClassLibrary.DTOs;
using OxideClassLibrary.Models;

namespace OxideClassLibrary.Extensions
{
    public static class UserExtension
    {
        extension(User user)
        {
            public UserData? ToDto()
                => user is null ? null : new()
                {
                    UserId = user.UserId,
                    Login = user.Login,

                    Email = user.Email,
                    LastActiveTime = user.LastActiveTime,
                    CreateTime = user.CreateTime,

                    IsVerify = user.IsVerify,
                };
        }

        extension(IEnumerable<User> users)
        {
            public IEnumerable<UserData?> ToDtos()
                => users.Select(ToDto);
        }
    }
}
