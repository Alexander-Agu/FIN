# User Onboarding Flow API

When a user creates an account their information will be saved but their account will remain disabled.  
An account gets enabled after a user has verified their account through email.  

Users log in using their email and password.  

---

# Endpoints
## Entry point => `/user`

1. ### Create an account => `/register`  
   A new user account is created but disabled until email verification.  
   **Body** => `{ name, email, password, phone, property_id, unit_number }`  
   **Response** => True ? Sends a verification email : Failed to create account  

2. ### Verify email => `/verify-email/{token}`  
   Enables a user account once they click the verification link.  
   **Body** => None (token comes from the email link)  
   **Response** => True ? Account enabled : Invalid or expired token  

3. ### Resend verification email => `/resend-verification`  
   Allows a user to request another verification email if the first one expired or was lost.  
   **Body** => `{ email }`  
   **Response** => True ? Verification email resent : Failed to send verification email  

4. ### Login => `/login`  
   Authenticates a user with email and password.  
   **Body** => `{ email, password }`  
   **Response** => True ? Returns token : Invalid email or password  

5. ### Get user profile => `/profile`  
   Returns the currently authenticated user’s information.  
   **Body** => None (requires authentication token)  
   **Response** => User object with id, name, email, phone, property_id, unit_number, created_at, updated_at  

6. ### Update basic information => `/update-info/{user_id}`  
   Allows updating a user’s basic information like name or phone.  
   **Body** => `{ name = ?, phone = ? }`  
   **Response** => True ? Information updated : Failed to update information  

7. ### Update email while logged in => `/update-email`  
   Updates the email address of the logged-in user.  
   **Body** => `{ email }`  
   **Response** => True ? Email updated : Failed to update email  

8. ### Update password => `/update-password/{user_id}`  
   Updates the password for a user account.  
   **Body** => `{ password, new_password }`  
   **Response** => True ? Password changed : Failed to change password  

9. ### Forgot password (request) => `/reset-password-request`  
   Starts the reset password flow by sending a reset link to the user’s email.  
   **Body** => `{ email }`  
   **Response** => True ? Reset email sent : Failed to send reset email  

10. ### Forgot password (confirm) => `/reset-password-confirm`  
    Confirms a new password using the reset token sent via email.  
    **Body** => `{ token, new_password }`  
    **Response** => True ? Password updated : Invalid or expired token  

11. ### Logout => `/logout`  
    Ends the user’s session by invalidating their token.  
    **Body** => None (requires authentication token)  
    **Response** => True ? Session ended : Failed to logout  

12. ### Deactivate/Delete account => `/delete/{user_id}`  
    Allows a user to deactivate or delete their account.  
    **Body** => `{ reason? }`  
    **Response** => True ? Account deleted/deactivated : Failed to delete account  
