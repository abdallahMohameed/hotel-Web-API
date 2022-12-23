using hotel_Web_API.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace hotel_Web_API.Data
{
    public class hotel_DB_Context:DbContext
    {
        private readonly IConfiguration configuration;

        public hotel_DB_Context(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public DbSet<user> users { get; set; }
        public DbSet<Room> rooms { get; set; }
        public DbSet<booking> bookings { get; set; }
        public DbSet<roomBooking> roomBookings { get; set; }

        public DbSet<branch> branches { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
       
            //roomBooking
            //modelBuilder.Entity<roomBooking>(a =>
            //{
            //    a.HasKey(c => new { c.bookingId, c.roomId });
            //});


            base.OnModelCreating(modelBuilder);
        }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("con"));
            base.OnConfiguring(optionsBuilder);

        }


    }
}
