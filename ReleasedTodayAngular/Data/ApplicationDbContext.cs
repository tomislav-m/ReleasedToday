using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReleasedTodayAngular.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Identity;

namespace ReleasedTodayAngular.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationRole> ApplicationRole { get; set; }
        public DbSet<ApplicationUser> User { get; set; }
        public DbSet<Album> Albums { get; set; }
    }
}
