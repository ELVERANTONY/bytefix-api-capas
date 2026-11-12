namespace ByteFix.Entidades.DTOs;

/// <summary>DTO de lectura de una reparación. Incluye el nombre del técnico en lugar del objeto.</summary>
public class ReparacionDto
{
    public int ReparacionId { get; set; }

    public int TecnicoId { get; set; }

    public string TecnicoNombre { get; set; } = null!;

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
}
