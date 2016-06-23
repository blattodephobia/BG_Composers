using BGC.Core;
using Microsoft.AspNet.Identity.EntityFramework;
using MySql.Data.Entity;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;

namespace BGC.Data
{
	[DbConfigurationType(typeof(MySqlEFConfiguration))]
	internal class ComposersDbContext : IdentityDbContext<BgcUser, BgcRole, long, BgcUserLogin, BgcUserRole, BgcUserClaim>, IUnitOfWork
	{
        public DbSet<Setting> Settings { get; set; }

		public DbSet<Composer> Composers { get; set; }

		public DbSet<ComposerArticle> ComposerArticles { get; set; }

		public DbSet<ComposerName> LocalizedComposerNames { get; set; }

		public ComposersDbContext() : this("MySqlConnection")
		{
		}

		public ComposersDbContext(string connectionStringName) :
			base(connectionStringName)
		{
			Database.SetInitializer<ComposersDbContext>(new CreateDatabaseIfNotExists<ComposersDbContext>());
            this.RequireUniqueEmail = true;
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
            
			modelBuilder.Entity<BgcUser>().HasKey(user => user.Id);
			modelBuilder.Entity<BgcRole>().HasKey(role => role.Id);
			modelBuilder.Entity<BgcUserLogin>().HasKey(userLogin => new { userLogin.UserId, userLogin.ProviderKey });
			modelBuilder.Entity<BgcUserRole>().HasKey(userRole => new { userRole.UserId, userRole.RoleId });
			modelBuilder.Entity<BgcUserClaim>().HasKey(userClaim => userClaim.Id);

			// The Role's Name and the User's UserName lengths are reduced, because otherwise MySQL wouldn't allow indexing them;
			// Max key size is 767 bytes and a string with length 256 in UTF-8 is at most 1024 bytes.
			modelBuilder.Entity<BgcRole>().Property(anr => anr.Name).HasMaxLength(64);
			modelBuilder.Entity<BgcUser>().Property(anu => anu.UserName).HasMaxLength(32);

			modelBuilder.Entity<ComposerName>().Property(name => name.FirstName)
				.HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));
			modelBuilder.Entity<ComposerName>().Property(name => name.FullName)
				.HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));
			modelBuilder.Entity<ComposerName>().Property(name => name.LastName)
				.HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));
            modelBuilder.Entity<ComposerName>().Property(name => name.LanguageInternal)
                .HasColumnName(nameof(ComposerName.Language))
                .IsRequired();

            modelBuilder.Entity<ComposerArticle>().Property(entry => entry.LanguageInternal)
                .HasColumnName(nameof(ComposerArticle.Language))
                .IsRequired();

            modelBuilder.Entity<BgcRole>().HasMany(role => role.Permissions).WithMany();
            modelBuilder.Entity<BgcUser>().HasMany(user => user.UserSettings).WithMany();
		}

		public IRepository<T> GetRepository<T>()
			where T : class
		{
			return new MySqlRepository<T>(this);
		}
	}
}

