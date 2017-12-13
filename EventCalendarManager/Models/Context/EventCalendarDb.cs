namespace EventCalendarManager.Models.Context
{
    using Microsoft.EntityFrameworkCore;

    public class EventCalendarDb : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Event> Events { get; set; }

        public EventCalendarDb() { }

        public EventCalendarDb(DbContextOptions<EventCalendarDb> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder
                .UseSqlServer("Server=.;Database=EventCalendarDb;Integrated Security=True");
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<User>()
                .HasMany(u => u.Events)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId);
        }
    }
}
