using Microsoft.EntityFrameworkCore;
using MotorControl.Api.Entity;

namespace MotorControl.Api.Repository.Context
{
    public class MotorControlDbContext:DbContext
    {
        public MotorControlDbContext(DbContextOptions<MotorControlDbContext> options) : base(options) { }

        public DbSet<Motor> motors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Motor>()
                .Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()");
        }

    }
}
