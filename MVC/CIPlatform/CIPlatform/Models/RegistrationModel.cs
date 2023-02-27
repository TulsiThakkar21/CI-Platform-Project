namespace CIPlatform.Models
{
    public class RegistrationModel
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public int PhoneNumber { get; set; }

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public long CityId { get; set; }

        public long CountryId { get; set; }

    }
}
