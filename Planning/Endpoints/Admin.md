# 🏢 Admin Onboarding & Management API

Admins can register, verify their accounts, log in, and manage users within their assigned property.  
An admin account is **disabled by default** until verified via email.  

---

## 👥 Admin Roles
1. **Super-Admin**  
   - Manages admins, properties, and users.  
   - Has full control over the platform.  

2. **Admin**  
   - Lower-level admin.  
   - Manages users for their assigned property.  
   - Cannot manage other admins.  

---

## 📌 Base Entry Point => `/admins`
---

## 🚀 Endpoints

### 1. Create Super-Admin/Admin Account
**POST** `/register`  
Creates a new super-admin or admin account. The account remains **disabled** until email verification.  

**Body**  
```json
{
  "name": "John Doe",
  "email": "admin@example.com",
  "password": "password123",
  "phone": "+1234567890",
  "property_id": 1,
  "role": "super-admin" // or "admin"
}
```

**Response**  
```json
{
  "success": true,
  "message": "Verification email sent. Please verify to activate your account."
}
```

---

### 2. Verify Email
**GET** `/verify-email/{token}`  
Verifies the email and enables the account.  

**Response**  
```json
{
  "success": true,
  "message": "Account verified successfully. You can now log in."
}
```

---

### 3. Resend Verification Email
**POST** `/resend-verification`  
Resends the email verification link.  

**Body**  
```json
{
  "email": "admin@example.com"
}
```

**Response**  
```json
{
  "success": true,
  "message": "Verification email resent successfully."
}
```

---

### 4. Login
**POST** `/login`  
Authenticates admin and returns a JWT token.  

**Body**  
```json
{
  "email": "admin@example.com",
  "password": "password123"
}
```

**Response**  
```json
{
  "success": true,
  "token": "jwt-auth-token",
  "expires_in": 3600
}
```

---

### 5. Get Admin Profile
**GET** `/profile`  
Fetches the authenticated admin’s profile.  

**Response**  
```json
{
  "id": 1,
  "name": "John Doe",
  "email": "admin@example.com",
  "phone": "+1234567890",
  "property_id": 1,
  "role": "super-admin",
  "created_at": "2025-09-09T10:00:00Z",
  "updated_at": "2025-09-09T10:00:00Z"
}
```

---

### 6. Update Admin Profile
**PUT** `/profile`  
Updates profile details of the authenticated admin.  

**Body**  
```json
{
  "name": "John Updated",
  "phone": "+1987654321"
}
```

**Response**  
```json
{
  "success": true,
  "message": "Profile updated successfully."
}
```

---

### 7. Change Password
**PUT** `/change-password`  
Allows an authenticated admin to update their password.  

**Body**  
```json
{
  "old_password": "password123",
  "new_password": "newPass456"
}
```

**Response**  
```json
{
  "success": true,
  "message": "Password updated successfully."
}
```

---

### 8. Forgot Password
**POST** `/forgot-password`  
Sends reset link to email.  

**Body**  
```json
{
  "email": "admin@example.com"
}
```

**Response**  
```json
{
  "success": true,
  "message": "Password reset email sent."
}
```

---

### 9. Reset Password
**POST** `/reset-password/{token}`  

**Body**  
```json
{
  "new_password": "newPass456"
}
```

**Response**  
```json
{
  "success": true,
  "message": "Password has been reset successfully."
}
```

---

## ✅ Summary
This API covers:  
- Admin registration & email verification  
- Authentication & profile management  
- Security flows (password change & reset)  
