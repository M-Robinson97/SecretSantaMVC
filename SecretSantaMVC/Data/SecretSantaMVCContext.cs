using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SecretSantaMVC.Models;

namespace SecretSantaMVC.Data
{
    // DBContext represents a session with the database
    public class SecretSantaMVCContext : DbContext
    {
        public SecretSantaMVCContext (DbContextOptions<SecretSantaMVCContext> options) : base(options)
        {
        }
        // Create a DBSet<Movie> property for the entity set.
        // An entity set corresponds to a database table. An entity is a row.
        public DbSet<CommentModel> CommentModel { get; set; }
    }
}
