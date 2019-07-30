using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CountryClubMVC.ViewModels
{
    public class SettingsViewModel
    {
        [Required]
        public string Password { get; set; }
    }
}