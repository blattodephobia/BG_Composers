using BGC.Core;
using Microsoft.AspNet.Identity.EntityFramework;
using MySql.Data.Entity;
using System.Data.Entity;

namespace BGC.Data
{
	[DbConfigurationType(typeof(MySqlEFConfiguration))]
	internal class ComposersDbContext : IdentityDbContext<AspNetUser, AspNetRole, long, AspNetUserLogin, AspNetUserRole, AspNetUserClaim>, IUnitOfWork
	{
		public ComposersDbContext() : this("MySqlConnection")
		{
			Database.SetInitializer<ComposersDbContext>(new CreateDatabaseIfNotExists<ComposersDbContext>());
		}

		public ComposersDbContext(string connectionStringName) :
			base(connectionStringName)
		{
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<AspNetUser>().HasKey(anu => anu.Id);
			modelBuilder.Entity<AspNetRole>().HasKey(anr => anr.Id);
			modelBuilder.Entity<AspNetUserLogin>().HasKey(anul => new { anul.UserId, anul.ProviderKey });
			modelBuilder.Entity<AspNetUserRole>().HasKey(anur => new { anur.UserId, anur.RoleId });
			modelBuilder.Entity<AspNetUserClaim>().HasKey(anuc => anuc.Id);

			// The Role's Name and the User's UserName lengths are reduced, because otherwise MySQL wouldn't allow indexing them;
			// Max key size is 767 bytes and a string with length 256 in UTF-8 is at most 1024 bytes.
			modelBuilder.Entity<AspNetRole>().Property(anr => anr.Name).HasMaxLength(64);
			modelBuilder.Entity<AspNetUser>().Property(anu => anu.UserName).HasMaxLength(32);
		}

		public void MarkUpdated<T>(T entity)
			where T : class
		{
			this.Entry(entity).State = EntityState.Modified;
		}
	}
}
