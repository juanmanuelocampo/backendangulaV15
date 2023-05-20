using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BackV15II.Models;

namespace BackV15II.Data
{
    public class BackV15IIContext : DbContext
    {
        public BackV15IIContext (DbContextOptions<BackV15IIContext> options)
            : base(options)
        {
        }

        public DbSet<BackV15II.Models.Cliente> Cliente { get; set; }

        public DbSet<BackV15II.Models.Localidad> Localidad { get; set; }

        public DbSet<BackV15II.Models.Solicitud> Solicitud { get; set; }
    }
}
