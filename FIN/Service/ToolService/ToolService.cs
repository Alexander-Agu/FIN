using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using FIN.Enums;

namespace FIN.Service.ToolService
{
    public class ToolService : IToolService
    {
        /*
         * Creates a response message and returns it
         */
        public Dictionary<string, object> Response(Result result, object message)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();

            response.Add("result", result);
            response.Add("message", message);
            return response;
        }

        /*
         *Validates email address
         */
        public bool ValidateEmail(string email)
        {
            var emailValidator = new EmailAddressAttribute();

            return emailValidator.IsValid(email);
        }

        /*
           Validates password
            
            Requirements:
                1. At least 8 letters by length
                2. At least one uppercase letter
                3. At least one lowercase letter
                4. At least one number
                5. At least one special character
         */
        public bool ValidatePassword(string password)
        {
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
         * Validates south african number
         */
        public bool ValidatePhoneNumber(string num)
        {
            string pattern = @"^(?:\+27|0)(6|7|8)\d{8}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(num);
        }


        // Generates One-Time-Password
        public string GenerateOtp()
        {
            Random rnd = new Random();
            string otp = "";

            for (int i = 0; i < 4; i++)
            {
                otp += rnd.Next(0, 9);
            }

            return otp;
        }
    }
}
