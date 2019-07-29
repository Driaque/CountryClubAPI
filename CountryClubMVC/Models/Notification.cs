using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CountryClubMVC.Models
{
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }
        public int User_ID { get; set; }
        [Required]
        public string Message { get; set; }
        public string Time { get; set; }
        public bool IsSeen { get; set; }
    }
}