using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Text.RegularExpressions;
using FIN.Dtos.UserDtos;
using FIN.Entities;
using FIN.Enums;
using FIN.Mapping;
using FIN.Repository;
using FIN.Service.EmailServices;
using Microsoft.EntityFrameworkCore;


namespace FIN.Service.UserService
{
    public class UserService(FinContext context) : IUserService
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
                return Response(Result.Error, "Fill in all required fields");
            }

            // validate email
            if (!ValidateEmail(registerUser.Email))
            {
                return Response(Result.Error, "Invalid email type");
            }

            // Validate password
            if (!ValidatePassword(registerUser.Password))
            {
                return Response(Result.Error, "Invalid password");
            }

            // Check is email already exists
            if (await context.users.Where(u => u.Email == registerUser.Email).AnyAsync())
            {
                return Response(Result.Error, "Email already exists");
            }

            user.Password = registerUser.Password;
            user.Phone = registerUser.Phone;

            await context.users.AddAsync(user);
            await context.SaveChangesAsync();

            // Send confirmation email after data has been saved
            SendConfirmationEmail(user.Email, user.ConfirmationToken);

            return Response(Result.Success, user.Id);
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
            Dictionary<string, object> response = new Dictionary<string, object>();
            TimeSpan passedTime;
            var user = await context.users.FirstOrDefaultAsync(u => u.ConfirmationToken == token); 


            if (user == null) return Response(Result.Success, "Failed to verify account");

            passedTime = user.ConfirmationDeadline - DateTime.UtcNow;

            if (passedTime.Minutes >= 30) return Response(Result.Error, "Token expired");

            user.Enabled = true;
            await context.SaveChangesAsync();

            return Response(Result.Success, "Account verified");
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
         *      
         */
        public async Task<Dictionary<string, object>> ResendVarificationMailAsync(string email)
        {
            User? user = await context.users.Where(e => e.Email == email).FirstOrDefaultAsync();
            Dictionary<string, object> response = new Dictionary<string, object>();

            // If email was not found
            if (user == null) return Response(Result.Error, "Email not found, Varification not sent");


            user.ConfirmationToken = Guid.NewGuid().ToString();
            user.ConfirmationDeadline = DateTime.UtcNow;
            await context.SaveChangesAsync();
            SendConfirmationEmail(user.Email, user.ConfirmationToken);

            return Response(Result.Success, "Varification email sent");
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
            if (user == null) return Response(Result.Error, "Invalid email or password");

            // Checking if account was enabled
            if (!user.Enabled) return Response(Result.Error, "Account not verified, resend varification email");

            // Validating password
            if (user.Password != login.Password && ValidatePassword(login.Password)) Response(Result.Error, "Invalid email or password");


            return Response(Result.Success, user.Id);
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
            if (user == null) return Response(Result.Error, "User not found");

            return Response(Result.Success, user.ToGetUserDto());
        }


        /*
         * HELPER METHOD -> Creates a response message and returns it
         */
        private Dictionary<string, object> Response(Result result, object message)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();

            response.Add("result", result);
            response.Add("message", message);
            return response;
        }


        /*
            HELPER METHOD -> Validates password
            
            Requirements:
                1. At least 8 letters by length
                2. At least one uppercase letter
                3. At least one lowercase letter
                4. At least one number
                5. At least one special character
         */
        private bool ValidatePassword(string password) {
            {
                if (password.Length < 8) return false; // Checks if password is over 8 characters


                bool number = false, special = false, lower = false, upper = false;

                for (int x = 0; x <= password.Length - 1; x++)
                {
                    if (char.IsNumber(password, x)) number = true; // Checks if password has a number

                    if (char.IsUpper(password, x)) upper = true; // Checks if password has an uppercase letter

                    if (char.IsLower(password, x)) lower = true; // Checks if password has a lowercase letter

                    if ("!@#$%^&*()_{}:''?//><|".Contains(password[x])) special = true; // Checks if password has special characters

                    if (number && special && lower && upper) break; // If all these conditions have been met stop checking
                }

                if (number && special && lower && upper)
                {
                    return true;
                }
                return false;
            }
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
         * HELPER METHOD -> Validates email address
         */
        private bool ValidateEmail(string email)
        {
            var emailValidator = new EmailAddressAttribute();

            return emailValidator.IsValid(email);
        }


        /*
         * HELPER METHOD -> Validates south african number
         */
        private bool ValidatePhoneNumber(int num)
        {
            string pattern = @"^(?:\+27|0)(6|7|8)\d{8}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(num + "");
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
