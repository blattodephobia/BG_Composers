using BGC.Core;
using BGC.Core.Models;
using BGC.Data.Conventions;
using BGC.Data.Relational;
using BGC.Data.Relational.Mappings;
using Microsoft.AspNet.Identity.EntityFramework;
using MySql.Data.Entity;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;

namespace BGC.Data.Relational
{
	[DbConfigurationType(typeof(MySqlEFConfiguration))]
	internal class ComposersDbContext :
        IdentityDbContext<BgcUser, BgcRole, long, BgcUserLogin, BgcUserRole, BgcUserClaim>,
        IUnitOfWork,
        IDtoFactory
	{
        public DbSet<Setting> Settings { get; set; }

		public DbSet<ComposerRelationalDto> Composers { get; set; }

		public DbSet<ArticleRelationalDto> ComposerArticles { get; set; }

        public DbSet<ArticleMediaRelationalDto> ArticleMedia { get; set; }

		public DbSet<NameRelationalDto> LocalizedComposerNames { get; set; }

        public DbSet<MediaTypeInfoRelationalDto> ContentMedia { get; set; }

        public DbSet<Invitation> Invitations { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<GlossaryEntry> GlossaryEntries { get; set; }

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

            modelBuilder.Conventions.Add<UnicodeSupportConvention>();

			modelBuilder.Entity<BgcUser>().HasKey(user => user.Id);
			modelBuilder.Entity<BgcUser>()
                .Property(anu => anu.UserName)
                .HasMaxLength(32);
            modelBuilder.Entity<BgcUser>().HasMany(user => user.UserSettings).WithMany();

			modelBuilder.Entity<BgcRole>().HasKey(role => role.Id);
			/* The Role's Name and the User's UserName lengths are reduced,
             * because otherwise MySQL wouldn't allow indexing them.
			 * Max key (and hence - index) size is 767 bytes and a string with
             * length 256 in UTF-8 is at most 1024 bytes.
             */
			modelBuilder.Entity<BgcRole>()
                .Property(anr => anr.Name)
                .HasMaxLength(64);
            modelBuilder.Entity<BgcRole>()
                .HasMany(role => role.Permissions)
                .WithMany(permission => permission.Roles)
                .Map(c => c
                    .MapLeftKey($"{nameof(BgcRole)}_{nameof(BgcRole.Id)}")
                    .MapRightKey($"{nameof(Permission)}_{nameof(Permission.Id)}")
                    .ToTable($"{nameof(BgcRole)}{nameof(Permission)}"));
            
			modelBuilder.Entity<BgcUserLogin>().HasKey(userLogin => new { userLogin.UserId, userLogin.ProviderKey });

			modelBuilder.Entity<BgcUserRole>().HasKey(userRole => new { userRole.UserId, userRole.RoleId });

			modelBuilder.Entity<BgcUserClaim>().HasKey(userClaim => userClaim.Id);

            modelBuilder.Entity<ComposerRelationalDto>().HasOptional(c => c.Profile).WithRequired();

            modelBuilder.Entity<GlossaryDefinition>()
                .Property(definition => definition.LanguageInternal)
                .HasColumnName(nameof(GlossaryDefinition.Language))
                .IsRequired();

            modelBuilder.Entity<GlossaryEntry>()
                .HasMany(d => d.Definitions)
                .WithRequired()
                .WillCascadeOnDelete(true);
                        
            modelBuilder.Entity<Setting>().Property(s => s.Description).IsUnicode();
            modelBuilder.Entity<Setting>().Property(s => s.StringValue).IsUnicode();

            modelBuilder.Entity<Invitation>().HasMany(invitation => invitation.AvailableRoles).WithMany();
		}

		public IRepository<T> GetRepository<T>()
			where T : class
		{
			return new MySqlRepository<T>(this);
		}

        public void SetState<T>(T entity, EntityState state)
            where T : class
        {
            Entry(entity).State = state;
        }

        public TRelationalDto GetDtoFor<TRelationalDto, TEntity>(TEntity entity, RelationalPropertyMapper<TEntity, TRelationalDto> mapper) where TRelationalDto : RelationdalDtoBase
        {
            DbSet<TRelationalDto> set = Set<TRelationalDto>();
            return set.FirstOrDefault(mapper.GetPredicateFor(entity)) ?? set.Create<TRelationalDto>();
        }
    }
}

