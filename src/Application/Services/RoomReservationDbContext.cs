using Microsoft.EntityFrameworkCore;
using RoomRes.Domain.Models;

namespace RoomRes.Application.Services {
    public class RoomReservationDbContext : DbContext {
        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomReservation> RoomReservations { get; set; }

        public RoomReservationDbContext(DbContextOptions options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>();
            modelBuilder.Entity<Room>();
            modelBuilder.Entity<RoomReservation>();
        }
    }
}