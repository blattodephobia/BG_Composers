using BGC.Core;
using Microsoft.AspNet.Identity.EntityFramework;
using MySql.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;

namespace BGC.Data
{
	[DbConfigurationType(typeof(MySqlEFConfiguration))]
	internal class ComposersDbContext : IdentityDbContext<AspNetUser, AspNetRole, long, AspNetUserLogin, AspNetUserRole, AspNetUserClaim>, IUnitOfWork
	{
		public DbSet<Composer> Composers { get; set; }

		public DbSet<ComposerEntry> ComposerArticles { get; set; }

		public DbSet<ComposerName> LocalizedComposerNames { get; set; }

		public ComposersDbContext() : this("MySqlConnection")
		{
		}

		public ComposersDbContext(string connectionStringName) :
			base(connectionStringName)
		{
			Database.SetInitializer<ComposersDbContext>(new CreateDatabaseIfNotExists<ComposersDbContext>());
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<AspNetUser>().HasKey(user => user.Id);
			modelBuilder.Entity<AspNetRole>().HasKey(role => role.Id);
			modelBuilder.Entity<AspNetUserLogin>().HasKey(userLogin => new { userLogin.UserId, userLogin.ProviderKey });
			modelBuilder.Entity<AspNetUserRole>().HasKey(userRole => new { userRole.UserId, userRole.RoleId });
			modelBuilder.Entity<AspNetUserClaim>().HasKey(userClaim => userClaim.Id);

			// The Role's Name and the User's UserName lengths are reduced, because otherwise MySQL wouldn't allow indexing them;
			// Max key size is 767 bytes and a string with length 256 in UTF-8 is at most 1024 bytes.
			modelBuilder.Entity<AspNetRole>().Property(anr => anr.Name).HasMaxLength(64);
			modelBuilder.Entity<AspNetUser>().Property(anu => anu.UserName).HasMaxLength(32);

			modelBuilder.Entity<ComposerName>().Property(name => name.FirstName)
				.HasMaxLength(32)
				.HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));
			modelBuilder.Entity<ComposerName>().Property(name => name.FullName)
				.HasMaxLength(128)
				.HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));
			modelBuilder.Entity<ComposerName>().Property(name => name.LastName)
				.HasMaxLength(32)
				.HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));
		}

		public IRepository<T> GetRepository<T>()
			where T : class
		{
			return new MySqlRepository<T>(this);
		}
	}
}

