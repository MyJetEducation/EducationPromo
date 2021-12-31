using Microsoft.EntityFrameworkCore;

namespace EducationPromo.Postgres
{
	public class DatabaseContext : DbContext
	{
		public const string Schema = "education";

		public DbSet<EmailEntity> Entities { get; set; }

		public DatabaseContext(DbContextOptions options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.HasDefaultSchema(Schema);

			modelBuilder.Entity<EmailEntity>().ToTable("promo_emails");
			modelBuilder.Entity<EmailEntity>().HasKey(e => e.Id);
			modelBuilder.Entity<EmailEntity>().Property(e => e.Date).IsRequired();
			modelBuilder.Entity<EmailEntity>().Property(e => e.Id).ValueGeneratedOnAdd();
			modelBuilder.Entity<EmailEntity>().Property(e => e.Email).HasMaxLength(320).IsRequired();

			base.OnModelCreating(modelBuilder);
		}
	}
}