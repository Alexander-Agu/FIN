using FIN.Dtos.UserDtos;

namespace FIN.Service.UserService
{
    public interface IUserService
    {
        // Register user -> returns a dictionary response message
        public Task<Dictionary<string, object>> RegisterUserAsync(RegisterUserDto registerUser);

        // Confirms and enable user account -> returns a dictionary response message
        public Task<Dictionary<string, object>> ConfirmEmailAsync(string token);

        // Re-sends varification email
        public Task<Dictionary<string, object>> ResendVarificationMailAsync(string email);
    }
}
