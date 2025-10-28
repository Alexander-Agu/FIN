using FIN.Dtos.UserDtos;
using FIN.Entities;
using FIN.Enums;
using FIN.Mapping;
using FIN.Repository;
using FIN.Service.EmailServices;
using FIN.Service.ToolService;
using Microsoft.EntityFrameworkCore;


namespace FIN.Service.UserService
{
    public class UserService(FinContext context, IToolService toolService) : IUserService
    {
        /* 
            TODO: Register's user account ( save user data to the database )
            
            Takes in RegisterUserDto and returns Dictionary response
            
            If user was resgistered:
                return {
                    result: Okay,
                    message : User Id or Token
                }
            Else:
                return {
                    result : Error,
                    message : "Invalid password" or "Invalid email type" or "Invalid phone type" or "Invalid information"
                }
         */
        public async Task<Dictionary<string, object>> RegisterUserAsync(RegisterUserDto registerUser)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            User? user = registerUser.ToEnity();

            // If any required field is empty return an error response
            if (!ValidateFields(registerUser))
            {
                return toolService.Response(Result.Error, "Fill in all required fields");
            }

            // validate email
            if (!toolService.ValidateEmail(registerUser.Email))
            {
                return toolService.Response(Result.Error, "Invalid email type");
            }

            // Validate password
            if (!toolService.ValidatePassword(registerUser.Password))
            {
                return toolService.Response(Result.Error, "Invalid password type");
            }

            // Check if email already exists
            if (await context.users.Where(u => u.Email == registerUser.Email).AnyAsync())
            {
                return toolService.Response(Result.Error, "Account already exists");
            }

            // Validate phone number if user tries to input it
            if (!string.IsNullOrEmpty(registerUser.Phone) && !toolService.ValidatePhoneNumber(registerUser.Phone))
            {
                return toolService.Response(Result.Error, "Invalid phone number type");
            }

            user.Password = registerUser.Password;
            user.Phone = registerUser.Phone;

            await context.users.AddAsync(user);
            await context.SaveChangesAsync();

            // Send confirmation email after data has been saved
            SendConfirmationEmail(user.Email, user.ConfirmationToken);

            return toolService.Response(Result.Success, "Confirmation email sent");
        }


        /*
         * TODO: Enables / Activates user account
         * 
         *  Through an email sent when creating an account
         *  user's will get an option to verify their account.
         *  In doing so their account will be activated 
         *  
         *  Time limited:
         *      User's have 30min to activate their account
         *      If account was failed to be activated after 30 days than all the user data will be deleted
         *  
         *  If account was verified:
         *      return {
         *          result : Error,
         *          message : "Failed to verify account"
         *      }
         *  Else:
         *      return {
         *          result : Okay,
         *          message : "Account verified"
         *      }
         */
        public async Task<Dictionary<string, object>> ConfirmEmailAsync(string token)
        {
            var user = await context.users.FirstOrDefaultAsync(u => u.ConfirmationToken == token);

            // Check if user exists
            if (user == null) return toolService.Response(Result.Success, "Failed to verify account");

            // Check if token expired
            if (DateTime.UtcNow > user.ConfirmationDeadline) return toolService.Response(Result.Error, "Token expired");

            user.Enabled = true;
            await context.SaveChangesAsync();

            return toolService.Response(Result.Success, "Account verified");
        }


        /*
         * TODO: Resends user their varification link
         * 
         *  In an event that a user failed to verufy their account on time
         *  they can request to resend the varification email to enable their 
         *  account
         *  
         *  Takes in user email and return a response message
         *  
         *  If mail was sent:
         *      return { result : Success, message : "Varification mail sent" }
         *  
         *  else:
         *      return { result : Error, message "Varification mail failed to send" } 
         */
        public async Task<Dictionary<string, object>> ResendVarificationMailAsync(string email)
        {
            User? user = await context.users.Where(e => e.Email == email).FirstOrDefaultAsync();
            Dictionary<string, object> response = new Dictionary<string, object>();

            // If email was not found
            if (user == null) return toolService.Response(Result.Error, "Email not found, Varification not sent");


            user.ConfirmationToken = Guid.NewGuid().ToString();
            user.ConfirmationDeadline = DateTime.UtcNow;
            await context.SaveChangesAsync();
            SendConfirmationEmail(user.Email, user.ConfirmationToken);

            return toolService.Response(Result.Success, "Varification email sent");
        }


        /*
         * TODO: Login user by validating their info
         *  
         *  Takes in LoginDto and returns a response
         *  
         *  If Logged in:
         *      return { result : Okay, message : User Id or Token }
         *  Else:
         *      return { result : Error, message : "Invalid password or email" }
         */
        public async Task<Dictionary<string, object>> LoginAsync(LoginDto login)
        {
            User? user = await context.users.Where(e => e.Email == login.Email).FirstOrDefaultAsync();
            
            // Checking if the email exists
            if (user == null) return toolService.Response(Result.Error, "Invalid email or password");

            // Checking if account was enabled
            if (!user.Enabled) return toolService.Response(Result.Error, "Account not verified, resend varification email");

            // Validating password
            if (user.Password != login.Password && !toolService.ValidatePassword(login.Password)) return toolService.Response(Result.Error, "Invalid email or password");

            return toolService.Response(Result.Success, "Logged in successfully");
        }


        /*
         * TODO: Returns user data
         * 
         *  Takes in user id and returns a response
         *  
         *  If user is found:
         *      return { result: Success, message: GetUserDto() }
         *  Else:
         *      return { result : Error, message: "User not found" }
         */
        public async Task<Dictionary<string, object>> GetUserAsync(int Id)
        {
            User? user = await context.users.FindAsync(Id);

            // Checking if the user exists
            if (user == null) return toolService.Response(Result.Error, "User not found");

            return toolService.Response(Result.Success, user.ToGetUserDto());
        }


        /*
         * TODO: Upates user's basic information
         * 
         *  Takes in UpdateUserDto and returns a response
         *  
         *  If user is updated:
         *      return { result: Success, message : "User updated" }
         *  Else:
         *      return { result: Error, message : "Failed to upadate user" }
         */
        public async Task<Dictionary<string, object>> UpdateUserAsync(int id, UpdateUserDto updateUser)
        {
            User? user = await context.users.FindAsync(id);

            // Checking if the user exists
            if (user == null) return toolService.Response(Result.Error, "User not found");
            
            user.Firstname = string.IsNullOrEmpty(user.Firstname) ? updateUser.Firstname : user.Firstname;
            user.Lastname = string.IsNullOrEmpty(user.Lastname) ? updateUser.Lastname : user.Lastname;

            if (user.Phone != updateUser.Phone && toolService.ValidatePhoneNumber(updateUser.Phone))
            {
                user.Phone = updateUser.Phone;
            }

            await context.SaveChangesAsync();
            return toolService.Response(Result.Success, "User updated");
        }


        /*
         * TODO: Updates email
         * 
         *  Takes in new email, updates it and return back a response
         *  
         *  If Email updated:
         *      retern { result : Success, message : "Email updated" }
         *  Else:
         *      return { result : Error, message : "Failed to update email" }
         */
        public async Task<Dictionary<string, object>> UpdateEmailAsync(int id, UpdateEmailDto updateEmail)
        {
            User? user = await context.users.FindAsync(id);

            // Checking if the user exists
            if (user == null) return toolService.Response(Result.Error, "User not found");

            if (user.Email == updateEmail.Email || !toolService.ValidateEmail(updateEmail.Email)){
                return toolService.Response(Result.Error, "Failed to update email");
            }

            user.Email = updateEmail.Email;
            await context.SaveChangesAsync();
            return toolService.Response(Result.Success, "Email updated");
        }


        /*
         * TODO: Sends an email to change password
         * 
         *  Sends a comfirmation email to change password , and sends back a response
         *  
         *  If email was sent:
         *      return { result : Success, message : "Comfirmation email sent" }
         *  Else:
         *      return { result : Error, message : "Failed to send comfirmation email"
         */
        public async Task<Dictionary<string, object>> SendUpdatePasswordEmailAsync(string email)
        {
            User? user = await context.users.Where(x => x.Email == email).FirstOrDefaultAsync();

            if (user == null) return toolService.Response(Result.Error, "Failed to send mail");
            
            SendConfirmationEmail(email, user.ConfirmationToken);
            return toolService.Response(Result.Success, "Comfirmation email sent");
        }


        /*
         * TODO: Updates password
         * 
         *  Takes in user token and password to change the password and sends back
         *  a response
         *  
         *  If password was changed:
         *      return { result : Success, message : "Password changed" }
         *  Else:
         *      return { result : Error, message : "Failed to change password" }
         */
        public async Task<Dictionary<string, object>> ResetPasswordAsync(string token, string newPassword)
        {
            TimeSpan passedTime;
            var user = await context.users.FirstOrDefaultAsync(u => u.ConfirmationToken == token);

            // Check if user exists
            if (user == null) return toolService.Response(Result.Success, "Failed to verify account");

            // Check if token expired
            passedTime = user.ConfirmationDeadline - DateTime.UtcNow;
            if (passedTime.Minutes >= 30) return toolService.Response(Result.Error, "Token expired");

            // Validating the new password
            if (user.Password == newPassword && !toolService.ValidatePassword(newPassword)) return toolService.Response(Result.Error, "Invalid password");

            user.Password = newPassword;
            await context.SaveChangesAsync();
            return toolService.Response(Result.Success, "Password changed");
        }


        /*
            HELPER METHOD -> Validates required fields
            
            Requirements fields:
                1. Firstname
                2. Email
                3. Password
         */
        private bool ValidateFields(RegisterUserDto user)
        {
            // Checking if firstname is empty
            if (string.IsNullOrEmpty(user.Firstname)) return false;

            // Checking if email is empty
            if (string.IsNullOrEmpty(user.Email)) return false;

            // Checking if passworod is empty
            if (string.IsNullOrEmpty (user.Password)) return false;

            return true;
        }


        /*
         * HELPER METHOD -> sends an account confirmation email
         */
        private async void SendConfirmationEmail(string email, string token)
        {
            var confirmationLink = $"https://localhost:7289/user/confirm-email?token={token}";
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
                            Please confirm your account by clicking the link below:
                          </p>

                          <a href='{confirmationLink}' 
                             style=""display:inline-block; background-color:#ffc93c; color:#ff6f3c; padding:12px 24px; text-decoration:none; border-radius:4px; font-size:16px; font-weight:bold;"">
                             Confirm Account
                          </a>

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
