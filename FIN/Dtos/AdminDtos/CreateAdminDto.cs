namespace FIN.Dtos.AdminDtos
{
    public class CreateAdminDto
    {
        public required string Firstname { get; set; } = string.Empty;
        public required string Lastname { get; set; } = string.Empty;
        public required string Email { get; set; } = string.Empty;
        public required string Password { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
