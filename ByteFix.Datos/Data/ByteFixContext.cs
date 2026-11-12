using ByteFix.Entidades.Entities;
using Microsoft.EntityFrameworkCore;

namespace ByteFix.Datos.Data;

public partial class ByteFixContext : DbContext
{
    public ByteFixContext(DbContextOptions<ByteFixContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Reparacion> Reparaciones { get; set; }

    public virtual DbSet<Tecnico> Tecnicos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reparacion>(entity =>
        {
            entity.HasKey(e => e.ReparacionId);

            entity.HasIndex(e => e.Estado, "IX_Reparaciones_Estado");

            entity.HasIndex(e => e.TecnicoId, "IX_Reparaciones_TecnicoId");

            entity.HasIndex(e => e.Serie, "UQ_Reparaciones_Serie").IsUnique();

            entity.Property(e => e.Costo).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Equipo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasDefaultValue("Pendiente", "DF_Reparaciones_Estado");
            entity.Property(e => e.FallaReportada)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FechaEntrega).HasPrecision(0);
            entity.Property(e => e.FechaIngreso)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())", "DF_Reparaciones_FechaIngreso");
            entity.Property(e => e.Marca)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Modelo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Prioridad)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("Media", "DF_Reparaciones_Prioridad");
            entity.Property(e => e.Serie)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.Tecnico).WithMany(p => p.Reparaciones)
                .HasForeignKey(d => d.TecnicoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reparaciones_Tecnicos");
        });

        modelBuilder.Entity<Tecnico>(entity =>
        {
            entity.HasIndex(e => e.Documento, "UQ_Tecnicos_Documento").IsUnique();

            entity.HasIndex(e => e.Email, "UQ_Tecnicos_Email").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true, "DF_Tecnicos_Activo");
            entity.Property(e => e.Apellido)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.Documento)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(120)
                .IsUnicode(false);
            entity.Property(e => e.Especialidad)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.FechaIngreso)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())", "DF_Tecnicos_FechaIngreso");
            entity.Property(e => e.Nombre)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
