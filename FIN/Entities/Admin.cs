namespace FIN.Entities
{
    public class Admin
    {
        // Admin data
        public int Id { get; set; }
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public bool Enable { get; set; }

        // Tokens
        public int OTP { get; set; }
        public string Token { get; set; } = string.Empty;
        public string ConfirmationToken { get; set; } = string.Empty;
        public DateTime ConfirmationDeadline { get; set; }

        // Dates
        public DateOnly Created_At { get; set; }
        public DateOnly Updated_At { get; set; }
    }
}
