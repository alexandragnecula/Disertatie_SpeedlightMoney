using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DataLayer.Entities
{
    public class ApplicationUser : IdentityUser<long>
    {
        public ApplicationUser()
        {
            Wallets = new HashSet<Wallet>();
            Borrows = new HashSet<Loan>();
            Loans = new HashSet<Loan>();
            Users = new HashSet<Friend>();
            UserFriends = new HashSet<Friend>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        [MinLength(13), MaxLength(13)]
        public string CNP { get; set; }
        public string Country { get; set; }
        public string County { get; set; }
        public string City { get; set; }
        public string StreetName { get; set; }
        public string StreetNumber { get; set; }
        public string CurrentStatus { get; set; }
        [MinLength(16), MaxLength(16)]
        public string CardNumber { get; set; }
        [MinLength(3), MaxLength(3)]
        public string Cvv { get; set; }
        public DateTime ExpireDate { get; set; }
        public double Salary { get; set; }
        public bool? IsActive { get; set; }

        public ICollection<Wallet> Wallets { get; private set; }
        public ICollection<Loan> Borrows { get; private set; }
        public ICollection<Loan> Loans { get; private set; }
        public ICollection<Friend> Users { get; private set; }
        public ICollection<Friend> UserFriends { get; private set; }

        public string GetFullName()
        {
            return FirstName + " " + LastName;
        }
    }
}
