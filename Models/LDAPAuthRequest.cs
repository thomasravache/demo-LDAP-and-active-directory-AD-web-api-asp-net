namespace DemoLDAPApi.Models
{
    public class LDAPAuthRequest
    {
        public string DomainName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}