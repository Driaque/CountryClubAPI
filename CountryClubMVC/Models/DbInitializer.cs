using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CountryClubMVC.Models
{
    public class DbInitializer : System.Data.Entity
 .DropCreateDatabaseIfModelChanges<AppDbContext>
    {
        // Seed Method
        // ...
    }
}