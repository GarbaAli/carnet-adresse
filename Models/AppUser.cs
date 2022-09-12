using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carnet_adresse.Models
{
    public class AppUser: IdentityUser
    {
        public int Age { get; set; }
        public string FirstName { get; set; }
        public string LasttName { get; set; }
    }
}
