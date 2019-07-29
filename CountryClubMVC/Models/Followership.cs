using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CountryClubMVC.Models
{
    public class Followership
    {
        [Key]
        public int FollowershipID { get; set; }
        public int UserID { get; set; }
        public int FollowerID { get; set; }
        public bool isFollowing { get; set; }
    }
}