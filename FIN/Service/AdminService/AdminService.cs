using FIN.Dtos.AdminDtos;
using FIN.Entities;
using FIN.Enums;
using FIN.Mapping;
using FIN.Repository;
using FIN.Service.EmailServices;
using FIN.Service.ToolService;
using Microsoft.EntityFrameworkCore;

namespace FIN.Service.AdminService
{
    public class AdminService(FinContext context, IToolService toolService) : IAdminService
    {
        // Enables or Activates user account
        public async Task<Dictionary<string, object>> ConfirmEmailAsync(string otp)
        {
            Admin? findAdmin = await context.admins.Where(x => x.OTP == otp).FirstOrDefaultAsync();

            if (findAdmin == null) {
                return toolService.Response(Result.Error, "Admin not found");
            }

            // Check if OTP expired
            if (DateTime.UtcNow > findAdmin.ConfirmationDeadline)
            {
                return toolService.Response(Result.Error, "OTP expired");
            }

            findAdmin.Enable = true;
            await context.SaveChangesAsync();

            return toolService.Response(Result.Success, "Account activated");
        }

        public Task<Dictionary<string, object>> GetAdminAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>> LoginAsync(LoginDto login)
        {
            throw new NotImplementedException();
        }


        // Register admin account ( saves admin data to the databse )
        public async Task<Dictionary<string, object>> RegisterAsync(CreateAdminDto admin)
        {
            Admin? findAdmin = await context.admins.Where(x => x.Email == admin.Email).FirstOrDefaultAsync();

            // If admin already exist do not create a new account
            if (findAdmin != null)
            {
                return toolService.Response(Result.Error, "Admin already exists");
            }

            // Validate Email
            if (!toolService.ValidateEmail(admin.Email))
            {
                return toolService.Response(Result.Error, "Invalid email type");
            }

            // Validate Password
            if (!toolService.ValidatePassword(admin.Password))
            {
                return toolService.Response(Result.Error, "Invalid password type");
            }

            // Validate phone number if inserted
            if (!string.IsNullOrEmpty(admin.Phone) && !toolService.ValidatePhoneNumber(admin.Phone))
            {
                return toolService.Response(Result.Error, "Invalid phone number");
            }

            Admin newAdmin = admin.ToCreateAdminEntity();

            await context.admins.AddAsync(newAdmin);
            await context.SaveChangesAsync();

            SendConfirmationEmail(newAdmin.Email, newAdmin.Token, newAdmin.OTP);
            return toolService.Response(Result.Success, newAdmin);
        }

        public async Task<Dictionary<string, object>> ResendVarificarionEmailAsync(string email)
        {
            Admin? admin = await context.admins.Where(e => e.Email == email).FirstOrDefaultAsync();

            // Check if admin was found
            if (admin == null)
            {
                return toolService.Response(Result.Error, "Failed to send email, Admin not found");
            }

            admin.OTP = toolService.GenerateOtp();
            admin.ConfirmationDeadline = DateTime.UtcNow;
            await context.SaveChangesAsync();

            SendConfirmationEmail(admin.Email, admin.Token, admin.OTP);

            return toolService.Response(Result.Success, "Email sent");
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


        /*
         * HELPER METHOD -> sends an account confirmation email
         */
        private async void SendConfirmationEmail(string email, string token, string OTP)
        {
            //var confirmationLink = $"https://localhost:7289/admin/confirm-email?token={token}";
            string htmlMessage = $@"
            <body style=""margin:0; padding:0; text-align:center;"">
              <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"">
                <tr>
                  <td align=""center"" style=""padding:20px;"">

                    <!-- Content Wrapper -->
                    <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"" style=""max-width:600px; text-align:center;"">
                      <tr>
                        <td style=""padding:20px; font-family:Arial, sans-serif;"">

                          <h2 style=""margin:0; font-size:24px; font-weight:bold; color:#000000;"">
                            Help us protect your <span style=""color:orange;"">account</span>
                          </h2>

                          <p style=""margin:20px 0; font-size:16px; color:#333333;"">
                            Please confirm your account by this One-Time Password (OTP):
                          </p>

                          <div style=""display:inline-block; background-color:#f0f0f0; color:orange; 
                                       padding:12px 24px; font-size:20px; font-weight:bold; 
                                       border-radius:6px; letter-spacing:2px;"">
                              {OTP}
                          </div>

                        </td>
                      </tr>
                    </table>

                  </td>
                </tr>
              </table>
            </body>";

            var emailService = new EmailService();
            await emailService.SendEmailAsync(email, "Confirm your account", htmlMessage);
        }
    }
}
