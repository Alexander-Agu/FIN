namespace FIN.Entities
{
    public class User
    {
        // User data
        public int Id { get; set; }
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public bool Enabled { get; set; }

        // Tokens
        public string Token { get; set; } = string.Empty;
        public string ConfirmationToken { get; set; } = string.Empty;
        public DateTime ConfirmationDeadline { get; set; }

        // Dates
        public DateOnly CreatedAt { get; set; }
        public DateOnly UpdatedAt { get; set; }

    }
}
