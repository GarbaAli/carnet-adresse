using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace carnet_adresse.Models
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext (DbContextOptions<AppDbContext> options) :base(options) 
        {
            
        }

        public DbSet<Contact> contacts { get; set; }
        public DbSet<Adresses> adresses { get; set; }
    }
}
