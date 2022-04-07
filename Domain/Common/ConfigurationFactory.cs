using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Domain.Common
{
    public static class ConfigurationFactory
    {
        public static EntityTypeBuilder<TEntity> ConfigureBaseEntity<TEntity>(this EntityTypeBuilder<TEntity> builder)
           where TEntity : BaseEntity
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.SoftDeleted).IsRequired().HasDefaultValue(false);

            builder.HasQueryFilter(e => !e.SoftDeleted);

            return builder;
        }

        public static EntityTypeBuilder<TEntity> ConfigureAuditingEntity<TEntity>(
            this EntityTypeBuilder<TEntity> builder)
            where TEntity : AuditingEntity
        {
            builder.ConfigureBaseEntity();

            builder.Property(e => e.CreatedBy).IsRequired();
            builder.Property(e => e.CreatedAt).IsRequired()
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("now() AT TIME ZONE 'UTC'");

            builder.Property(e => e.LastModifiedBy).IsRequired();
            builder.Property(e => e.LastModifiedAt).IsRequired()
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("now() AT TIME ZONE 'UTC'");

            return builder;
        }
    }
}
