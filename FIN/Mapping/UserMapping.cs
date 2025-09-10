using FIN.Dtos.UserDtos;
using FIN.Entities;

namespace FIN.Mapping
{
    public static class UserMapping
    {
        public static User ToEnity(this RegisterUserDto user)
        {
            return new User()
            {
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                Phone = user.Phone,
                Token = Guid.NewGuid().ToString(),
                CreatedAt = DateOnly.FromDateTime(DateTime.Now),
            };
        }


        public static GetUserDto ToGetUserDto(this User user) {
            return new()
            {
                Id = user.Id,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                Phone = user.Phone,
            };
        } 
    }
}
