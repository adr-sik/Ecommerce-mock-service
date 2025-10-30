namespace Shared.Models.DTOs
{
    public class UserClaimsDTO
    {
        public string Username { get; set; }
        public List<ClaimDTO> Claims { get; set; }
    }

    public class ClaimDTO
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
