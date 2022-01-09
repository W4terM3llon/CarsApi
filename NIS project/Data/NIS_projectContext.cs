#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NIS_project.Models;

namespace NIS_project.Data
{
    public class NIS_projectContext : DbContext
    {
        public NIS_projectContext (DbContextOptions<NIS_projectContext> options)
            : base(options)
        {
        }

        public DbSet<NIS_project.Models.Car> Car { get; set; }
        public DbSet<NIS_project.Models.Engine> Engine { get; set; }
        public DbSet<NIS_project.Models.Manufacturer> Manufacturer { get; set; }
        public DbSet<NIS_project.Models.Owner> Owner { get; set; }
    }
}
