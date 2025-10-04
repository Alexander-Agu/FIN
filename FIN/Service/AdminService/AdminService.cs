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


        // Returns admin data
        public async Task<Dictionary<string, object>> GetAdminAsync(int id)
        {
            Admin? admin = await context.admins.FindAsync(id);

            if (ValidateAdmin(admin)) {
                return toolService.Response(Result.Error,"Failed to get admin");
            }

            return toolService.Response(Result.Success, admin.ToAdminDto());
        }


        // Login admins by validating their info
        public async Task<Dictionary<string, object>> LoginAsync(LoginDto login)
        {
            Admin? admin = await context.admins.Where(e => e.Email == login.Email).FirstOrDefaultAsync();

            // Validate admin
            if (!ValidateAdmin(admin)) {
                return toolService.Response(Result.Error, "Invalid password or email");
            }

            // Checking if password is valid
            if (admin.Password != login.Password)
            {
                return toolService.Response(Result.Error, "Invalid password or email");
            }

            return toolService.Response(Result.Success, "Logged in");
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


        // Resends admin their varification link
        public async Task<Dictionary<string, object>> ResendVarificarionEmailAsync(string email)
        {
            Admin? admin = await context.admins.Where(e => e.Email == email).FirstOrDefaultAsync();

            // Validate admin
            if (!ValidateAdmin(admin))
            {
                return toolService.Response(Result.Error, "Failed to send email");
            }

            admin.OTP = toolService.GenerateOtp();
            admin.ConfirmationDeadline = DateTime.UtcNow.AddMinutes(30);
            await context.SaveChangesAsync();

            SendConfirmationEmail(admin.Email, admin.Token, admin.OTP);

            return toolService.Response(Result.Success, "Email sent");
        }

        public Task<Dictionary<string, object>> SendUpdatePasswordEmailAsync(string email)
        {
            throw new NotImplementedException();
        }


        // Upates admin's profile data
        public async Task<Dictionary<string, object>> UpdateAdminProfile(int id, UpdateAdminProfileDto profile)
        {
            Admin? admin = await context.admins.FindAsync(id);

            // Validate admin
            if (!ValidateAdmin(admin)) {
                return toolService.Response(Result.Error, "failed to updated admin");
            }

            // Checking if admin wants to update Firstname
            if (!string.IsNullOrEmpty(profile.Firstname) && admin.Firstname != profile.Firstname)
            {
                admin.Firstname = profile.Firstname;
            }

            // Checking if admin wants to update Lastname
            if (!string.IsNullOrEmpty(profile.Lastname) && admin.Lastname != profile.Lastname)
            {
                admin.Lastname = profile.Lastname;
            }

            // Checking if admin wants to update Phone Number
            if (!string.IsNullOrEmpty(profile.Phone) && admin.Phone != profile.Phone)
            {
                admin.Phone = profile.Phone;
            }

            await context.SaveChangesAsync();
            return toolService.Response(Result.Success, "Profile updated");
        }


        // Updates email
        public async Task<Dictionary<string, object>> UpdateEmailAsync(int id, UpdateEmailDto email)
        {
            Admin? admin = await context.admins.FindAsync(id);

            // Validate admin
            if (!ValidateAdmin(admin))
            {
                return toolService.Response(Result.Error, "failed to updated email");
            }

            // Validate email
            bool isNewEmailValid = !string.IsNullOrEmpty(email.Email) && admin.Email != email.Email;
            if (isNewEmailValid && toolService.ValidateEmail(email.Email)){

                admin.Email = email.Email;

            } else
            {
                return toolService.Response(Result.Error, "Invalid email type");
            }

            await context.SaveChangesAsync();
            return toolService.Response(Result.Success, "Email updated");
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
         * HELPER METHOD -> Validates admin
         * 
         *  Checks if admin's account is avtivated and if they have been found
         */
        private bool ValidateAdmin(Admin admin)
        {
            // Check if admin was found
            if (admin == null)
            {
                return false;
            }

            // Checking if account has already been activated
            if (admin.Enable == false)
            {
                return false;
            }

            return true;
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
