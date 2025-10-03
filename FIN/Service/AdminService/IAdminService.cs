using FIN.Dtos.AdminDtos;

namespace FIN.Service.AdminService
{
    public interface IAdminService
    {
        /*
         *  TODO: Register admin account ( saves admin data to the databse )
         *  
         *      Takes in CreateAdminDto and returns a response
         *      
         *      IF admin was registered:
         *          return {
         *              result: Success,
         *              message: User id or token
         *          }
         *      ELSE:
         *          return {
         *              result: error,
         *              message: "Failed to create account"
         *          }
         */
        Task<Dictionary<string, object>> RegisterAsync(CreateAdminDto admin);


        /*
         * TODO: Enables / Activates user account
         * 
         *  Through an email sent when creating an account
         *  admin's will get an option to verify their account.
         *  In doing so their account will be activated 
         *  
         *  Time limited:
         *      Admin's have 30min to activate their account
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
        Task<Dictionary<string, object>> ConfirmEmailAsync(string token);


        /*
         * TODO: Resends admin their varification link
         * 
         *  In an event that an admin failed to verufy their account on time
         *  they can request to resend the varification email to enable their 
         *  account
         *  
         *  Takes in admin email and return a response message
         *  
         *  If mail was sent:
         *      return { result : Success, message : "Varification mail sent" }
         *  
         *  else:
         *      return { result : Error, message "Varification mail failed to send" }   
         */
        Task<Dictionary<string, object>> ResendVarificarionEmailAsync();


        /*
         * TODO: Login admins by validating their info
         *  
         *  Takes in LoginDto and returns a response
         *  
         *  If Logged in:
         *      return { result : Okay, message : User Id or Token }
         *  Else:
         *      return { result : Error, message : "Invalid password or email" }
         */
        Task<Dictionary<string, object>> LoginAsync(LoginDto login);


        /*
         * TODO: Returns admin data
         * 
         *  Takes in admin id and returns a response
         *  
         *  If admin is found:
         *      return { result: Success, message: GetUserDto() }
         *  Else:
         *      return { result : Error, message: "User not found" }
         */
        Task<Dictionary<string, object>> GetAdminAsync(int id);


        /*
         * TODO: Upates admin's basic information
         * 
         *  Takes in UpdateAdminProfileDto and returns a response
         *  
         *  If admin is updated:
         *      return { result: Success, message : "User updated" }
         *  Else:
         *      return { result: Error, message : "Failed to upadate user" }
         */
        Task<Dictionary<string, object>> UpdateAdminProfile(int id, UpdateAdminProfileDto profile);


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
        Task<Dictionary<string, object>> UpdateEmailAsync(int id, UpdateEmailDto email);


        /*
         * TODO: Updates password while logged in
         * 
         *  Takes in n ew password, updates it and return a response
         *  
         *  If Password updated:
         *      retern { result : Success, message : "Password updated" }
         *  Else:
         *      return { result : Error, message : "Failed to update password" }
         */
        Task<Dictionary<string, object>> UpdatePasswordAsync(int id, UpdatePasswordDto passoword);


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
        Task<Dictionary<string, object>> SendUpdatePasswordEmailAsync(string email);


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
        Task<Dictionary<string, object>> UpdateForgotenPassword(string token, UpdatePasswordDto password);
    }
}
