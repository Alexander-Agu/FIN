using FIN.Dtos.AdminDtos;
using FIN.Repository;

namespace FIN.Service.AdminService
{
    public class AdminService(FinContext context) : IAdminService
    {
        public Task<Dictionary<string, object>> ConfirmEmailAsync(string token)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> GetAdminAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> LoginAsync(LoginDto login)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> RegisterAsync(CreateAdminDto admin)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> ResendVarificarionEmailAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> SendUpdatePasswordEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> UpdateAdminProfile(int id, UpdateAdminProfileDto profile)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> UpdateEmailAsync(int id, UpdateEmailDto email)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> UpdateForgotenPassword(string token, UpdatePasswordDto password)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> UpdatePasswordAsync(int id, UpdatePasswordDto passoword)
        {
            throw new NotImplementedException();
        }
    }
}
