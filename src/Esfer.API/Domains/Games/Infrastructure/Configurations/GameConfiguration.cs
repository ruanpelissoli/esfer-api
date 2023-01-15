using Esfer.API.Domains.Games.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Esfer.API.Domains.Games.Infrastructure.Configurations;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.HasKey(g => g.Id);
        builder.Property(t => t.Id)
           .IsRequired()
           .HasColumnType("uniqueidentifier");

        builder.Property(g => g.Title)
            .HasColumnType("varchar(50)")
            .IsRequired();

        builder.Property(g => g.Description)
            .HasColumnType("varchar(1000)")
            .IsRequired();

        builder.OwnsOne(g => g.Price)
            .Property(t => t.Value)
            .HasColumnName("Price")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(g => g.ReleaseDate)
            .IsRequired();

        builder.HasMany(g => g.Medias)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(g => g.GameFiles)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(g => g.Comments)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(t => t.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.Property(t => t.UpdatedAt)
            .IsRequired()
            .HasColumnType("datetime2");
    }
}
