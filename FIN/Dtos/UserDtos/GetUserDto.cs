namespace FIN.Dtos.UserDtos
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Phone { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
