using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CountryClubMVC.Models
{
    public class Family
    {
        [Key]
        public string Family_ID { get; set; }
        public string FamilyName { get; set; }
        public int MemberCount { get; set; }
        public ICollection<User> Users { get; set; }
    }
}