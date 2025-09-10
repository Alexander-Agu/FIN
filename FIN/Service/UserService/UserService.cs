using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Text.RegularExpressions;
using FIN.Dtos.UserDtos;
using FIN.Entities;
using FIN.Mapping;
using FIN.Repository;
using FIN.Service.EmailServices;
using Microsoft.EntityFrameworkCore;
//using FIN.Service.EmailService.EmailService;

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
                            message : GetUserDto()
                }
            else:
                return {
                            result : Error,
                            message : "Invalid password" or "Invalid email type" or "Invalid phone type" or "Invalid information"
                }
         */
        public async Task<Dictionary<string, object>> RegisterUserAsync(RegisterUserDto registerUser)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            User? user = registerUser.ToEnity();

            // If any required field is is empty return an error response
            if (!ValidateFields(registerUser))
            {
                response.Add("result", "Error");
                response.Add("message", "Fill in all required fields");
                return response;
            }

            // validate email
            if (!ValidateEmail(registerUser.Email))
            {
                response.Add("result", "Error");
                response.Add("message", "Invalid email type");
                return response;
            }

            // Validate password
            if (!ValidatePassword(registerUser.Password))
            {
                response.Add("result", "Error");
                response.Add("message", "Invalid password");
                return response;
            }
            user.Password = registerUser.Password;
            user.Phone = registerUser.Phone;

            await context.users.AddAsync(user);
            await context.SaveChangesAsync();

            SendConfirmationEmail(user, user.Email, user.Token);

            response.Add("result", "Okay");
            response.Add("message", user.ToGetUserDto());
            return response;
        }


        public async Task<Dictionary<string, object>> ConfirmEmailAsync(string token)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            var user = await context.users.FirstOrDefaultAsync(u => u.Token == token);


            if (user == null)
            {
                response.Add("result", "Error");
                response.Add("message", "Account not confirmed");
            }

            user.Enabled = true;
            await context.SaveChangesAsync();

            response.Add("result", "Okay");
            response.Add("message", "Account enabled");
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
            // Pattern for SA mobile numbers
            string pattern = @"^(?:\+27|0)(6|7|8)\d{8}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(num + "");
        }


        /*
         * HELPER METHOD -> sends an account confirmation email
         */
        private async void SendConfirmationEmail(User user, string email, string token)
        {
            var confirmationLink = $"https://localhost:7289/user/confirm-email?token={token}";
            string htmlMessage = $@"
<h3>Welcome to FIN -> Fix It Now</h3>
<p>Please confirm your account by clicking the link below:</p>
<a href='{confirmationLink}' style='background-color:blue;color:white;padding:10px 20px;text-decoration:none;'>Confirm Account</a>
";
            

            var emailService = new EmailService();
            await emailService.SendEmailAsync(user.Email, "Confirm your account", htmlMessage);
        }
    }
}
