using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CountryClubAPI.Models
{
    public class AuthHelper
    {
        public bool IsAuthenticated { get; set; }
        //public User User { get; set; }
        public int UserID { get; set; }
        public string FamilyID { get; set; }
    }
}