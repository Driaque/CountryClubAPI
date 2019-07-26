using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CountryClubMVC.Models
{
    public class Friend
    {
        [Key]
        public int Friendship_ID { get; set; }
        public string Friend_Name { get; set; }
        public int friend_UserID { get; set; }
        public string User_ID { get; set; }
        public string Status { get; set; }
       
        public string TimeEstablished { get; set; }
        public string TimeUpdated { get; set; }

    }
}