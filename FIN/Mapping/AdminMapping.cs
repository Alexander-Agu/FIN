using System.Text;
using FIN.Dtos.AdminDtos;
using FIN.Entities;

namespace FIN.Mapping
{
    public static class AdminMapping
    {
        private static string GenerateOtp()
        {
            Random rnd = new Random();
            string otp = "";

            for (int i = 0; i < 4; i++)
            {
                otp += rnd.Next(0, 9);
            }

            return otp;
        }


        public static Admin ToCreateAdminEntity(this CreateAdminDto admin)
        {
            return new Admin()
            {
                Firstname = admin.Firstname,
                Lastname = admin.Lastname,
                Email = admin.Email,
                Password = admin.Password,
                Phone = admin.Phone,
                Enable = false,
                ConfirmationToken = Guid.NewGuid().ToString(),
                ConfirmationDeadline = DateTime.Now,
                Created_At = DateOnly.FromDateTime(DateTime.Now),
                OTP = GenerateOtp(),
            };
        }

        public static AdminDto ToAdminDto(this Admin admin)
        {
            return new AdminDto() {
                Firstname = admin.Firstname,
                Lastname = admin.Lastname,
                Email = admin.Email,
                Phone = admin.Phone,
                Created_At = admin.Created_At,
                Updated_At = admin.Updated_At
            };
        }
    }
}
