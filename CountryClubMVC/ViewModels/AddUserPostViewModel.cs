using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CountryClubMVC.ViewModels
{
    public class AddUserPostViewModel
    {
        [Required]
        public string Content { get; set; }
        public int Likes { get; set; }
        public byte[] Image { get; set; }
        public string TimePosted { get; set; }
        public string TimeEdited { get; set; }
        [Required]
        public int User_ID { get; set; }
    }
}