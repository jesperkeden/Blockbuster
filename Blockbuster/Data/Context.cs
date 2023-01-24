using Blockbuster.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Blockbuster.Data
{
    internal class Context : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }
        public DbSet<CustomerMovie> CustomerMovies { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) //lägg upp lokalt
        //{
        //    optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=Blockbuster;Trusted_Connection=True;"); 
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) //lägg upp på azure
        {
            optionsBuilder.UseSqlServer("Server = tcp:jesperbookingappdb.database.windows.net, 1433; Initial Catalog = jesperbookingapp; Persist Security Info = False; User ID = jesper; Password = Hejmicke123; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;");
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // configure many-to-many relationship between movies and genres
            modelBuilder.Entity<MovieGenre>()
                .HasKey(mg => new { mg.MovieId, mg.GenreId });
            modelBuilder.Entity<MovieGenre>()
                .HasOne(mg => mg.Movie)
                .WithMany(m => m.Genres)
                .HasForeignKey(mg => mg.MovieId);
            modelBuilder.Entity<MovieGenre>()
                .HasOne(mg => mg.Genre)
                .WithMany(g => g.Movies)
                .HasForeignKey(mg => mg.GenreId);

            // configure many-to-many relationship between movies and customers
            modelBuilder.Entity<CustomerMovie>()
                .HasKey(cm => new { cm.CustomerId, cm.MovieId });
            modelBuilder.Entity<CustomerMovie>()
                .HasOne(cm => cm.Customer)
                .WithMany(c => c.Movies)
                .HasForeignKey(cm => cm.CustomerId);
            modelBuilder.Entity<CustomerMovie>()
                .HasOne(cm => cm.Movie)
                .WithMany(m => m.Customers)
                .HasForeignKey(cm => cm.MovieId);

            modelBuilder.Entity<Movie>()
                .Property(m => m.UnitPrice)
                .HasColumnType("decimal(18, 2)");
        }

    }
}
