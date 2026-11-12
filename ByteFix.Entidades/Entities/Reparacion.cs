namespace ByteFix.Entidades.Entities;

/// <summary>
/// Reparación registrada en el taller. Es la entidad hija de la relación 1:N.
/// </summary>
public partial class Reparacion
{
    public int ReparacionId { get; set; }

    public int TecnicoId { get; set; }

    public string Serie { get; set; } = null!;

    public string Equipo { get; set; } = null!;

    public string Marca { get; set; } = null!;

    public string? Modelo { get; set; }

    public string FallaReportada { get; set; } = null!;

    public string Prioridad { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public decimal Costo { get; set; }

    public DateTime FechaIngreso { get; set; }

    public DateTime? FechaEntrega { get; set; }

    public virtual Tecnico Tecnico { get; set; } = null!;
}
