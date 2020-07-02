using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Chambers.Models
{
    public class DocumentContext : DbContext
    {
        public DocumentContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Document> Documents { get; set; }
    }
}
