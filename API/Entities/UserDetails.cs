namespace API.Entities
{
    public class UserDetails
    {
        public string idToken { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
        public bool IsAdmin { get; set; } = false;
    }
}