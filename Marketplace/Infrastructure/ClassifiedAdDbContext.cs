using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MarketPlace.Domain.Entities;


namespace Marketplace.Infrastructure
{
    public class ClassifiedAdDbContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;
        public ClassifiedAdDbContext(ILoggerFactory loggerFactory, DbContextOptions<ClassifiedAdDbContext> options) : base(options)
            => _loggerFactory = loggerFactory;

        public DbSet<ClassfiedAd> ClassfiedAds { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);
            optionsBuilder.EnableSensitiveDataLogging();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClassifiedAdEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PictureEntityTypeConfiguration());
        }
        public class ClassifiedAdEntityTypeConfiguration : IEntityTypeConfiguration<ClassfiedAd>
        {
            public void Configure(EntityTypeBuilder<ClassfiedAd> builder)
            {
                builder.HasKey(x => x.ClassifiedAdId);
                builder.OwnsOne(x => x.Id);
                builder.OwnsOne(x => x.Price, p => p.OwnsOne(c => c.Currency));
                builder.OwnsOne(x => x.Text);
                builder.OwnsOne(x => x.Title);
                builder.OwnsOne(x => x.Title);
                builder.OwnsOne(x => x.ApprovedBy);
                builder.OwnsOne(x => x.OwnerId);
            }
        }
        public class PictureEntityTypeConfiguration : IEntityTypeConfiguration<Picture>
        {
            public void Configure(EntityTypeBuilder<Picture> builder)
            {
                builder.HasKey(x => x.PictureId);
                builder.OwnsOne(x => x.Id);
                builder.OwnsOne(x => x.ParentId);
                builder.OwnsOne(x => x.Size);
            }
        }
    }
}
