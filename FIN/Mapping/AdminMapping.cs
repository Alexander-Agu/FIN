using System.Text;
using FIN.Dtos.AdminDtos;
using FIN.Entities;

namespace FIN.Mapping
{
    public static class AdminMapping
    {
        public static string GenerateOtp()
        {
            Random rnd = new Random();
            string otp = "";

            for (int i = 0; i < 4; i++)
            {
                otp += rnd.Next(1, 9);
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
    }
}
