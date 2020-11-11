using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SecretSantaMVC.Models;
/*
 * Used for database connectivity & dependency injection.
 */
namespace SecretSantaMVC.Data
{
    // DBContext represents a session with the database
    public class SecretSantaMVCContext : DbContext
    {
        public SecretSantaMVCContext (DbContextOptions<SecretSantaMVCContext> options) : base(options)
        {
        }
        // Create a DBSet<Movie> property for the entity set. The set corresponds to a table,
        // with each entity corresponding to a row.
        public DbSet<CommentModel> CommentModel { get; set; }
    }
}
