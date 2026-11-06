
using AgriStream.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<Farm> Farms { get; set; }

    public DbSet<Field> Fields { get; set; }

    public DbSet<DroneImage> DroneImages { get; set; }

    public DbSet<IrrigationCommand> IrrigationCommands
    { get; set; }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Farm>()
            .HasOne(f => f.Farmer)
            .WithMany(u => u.Farms)
            .HasForeignKey(f => f.FarmerId)
            .OnDelete(DeleteBehavior.Restrict);

      
        modelBuilder.Entity<Field>()
            .HasOne(f => f.Farm)
            .WithMany(f => f.Fields)
            .HasForeignKey(f => f.FarmId)
            .OnDelete(DeleteBehavior.Restrict);

      
        modelBuilder.Entity<Field>()
            .Property(f => f.AreaHectares)
            .HasPrecision(10, 2);

        modelBuilder.Entity<DroneImage>()
            .HasOne(d => d.Farm)
            .WithMany(f => f.DroneImages)
            .HasForeignKey(d => d.FarmId)
            .OnDelete(DeleteBehavior.Restrict);

        
        modelBuilder.Entity<DroneImage>()
            .HasOne(d => d.Field)
            .WithMany(f => f.DroneImages)
            .HasForeignKey(d => d.FieldId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<DroneImage>()
            .HasOne(d => d.UploadedByUser)
            .WithMany(u => u.UploadedDroneImages)
            .HasForeignKey(d => d.UploadedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<IrrigationCommand>()
            .HasOne(i => i.Farm)
            .WithMany(f => f.IrrigationCommands)
            .HasForeignKey(i => i.FarmId)
            .OnDelete(DeleteBehavior.Restrict);

        
        modelBuilder.Entity<IrrigationCommand>()
            .HasOne(i => i.Field)
            .WithMany(f => f.IrrigationCommands)
            .HasForeignKey(i => i.FieldId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<IrrigationCommand>()
            .HasOne(i => i.RequestedByUser)
            .WithMany(u => u.IrrigationCommands)
            .HasForeignKey(i => i.RequestedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        
        modelBuilder.Entity<DroneImage>()
            .HasIndex(d => d.BlobName)
            .IsUnique();

        modelBuilder.Entity<IrrigationCommand>()
            .HasIndex(i => new { i.FarmId, i.CreatedAtUtc });
    }
}
