using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CountryClubMVC.Models
{
    public class Post
    {
        [Key]
        public int Post_ID { get; set; }
        [Required]
        public string Content { get; set; }
        public int Likes { get; set; }
        public string Image { get; set; }
        public string TimePosted { get; set; }
        public string TimeEdited { get; set; }
        public int User_ID { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}