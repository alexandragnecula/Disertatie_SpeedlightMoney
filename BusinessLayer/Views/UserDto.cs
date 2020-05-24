using System;
namespace BusinessLayer.Views
{
    public class UserDto
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public string CNP { get; set; }
        public string Country { get; set; }
        public string County { get; set; }
        public string City { get; set; }
        public string StreetName { get; set; }
        public string StreetNumber { get; set; }
        public string CurrentStatus { get; set; }
        public string CardNumber { get; set; }
        public string Cvv { get; set; }
        public DateTime ExpireDate { get; set; }
        public double Salary { get; set; }
        public string PhoneNumber { get; set; }

        public long RoleId { get; set; }
    }
}
