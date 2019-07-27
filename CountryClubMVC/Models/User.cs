using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CountryClubMVC.Models
{
    public class User
    {
        [Key]
        public int User_ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Town { get; set; }
        public string Street { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Family_ID { get; set; }
        public string Bio { get; set; }
        public string Title { get; set; }
        public string DateOfBirth { get; set; }
        public string DisplayPicture { get; set; }
        public string CoverPicture { get; set; }
        public string DateJoined { get; set; }
        public bool IsPasswordReset { get; set; }
        public Family family { get; set; }
        public ICollection<User> Followers { get; set; }
        public ICollection<User> Following { get; set; }
    }
}