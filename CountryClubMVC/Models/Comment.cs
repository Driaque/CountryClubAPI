using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CountryClubMVC.Models
{
    public class Comment
    {
        [Key]
        public int Comment_ID { get; set; }
        public string Message { get; set; }
        public string Time { get; set; }
        public int Likes { get; set; }
        public int User_ID { get; set; }
        public int Post_ID { get; set; }
    }
}