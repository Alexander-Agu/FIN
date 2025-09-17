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

        // Login
        public Task<Dictionary<string, object>> LoginAsync(LoginDto login);

        // Get user data
        public Task<Dictionary<string, object>> GetUserAsync(int Id);

        // Update user information
        public Task<Dictionary<string, object>> UpdateUserAsync(int id, UpdateUserDto updateUser);

        // Update Email
        public Task<Dictionary<string, object>> UpdateEmailAsync(int id, UpdateEmailDto updateEmail);
    }
}
