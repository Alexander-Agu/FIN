namespace FIN.Dtos.AdminDtos
{
    public class AdminDto
    {
        // Admin data
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        // Dates
        public DateOnly Created_At { get; set; }
        public DateOnly Updated_At { get; set; }
    }
}
