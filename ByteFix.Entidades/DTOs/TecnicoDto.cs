namespace ByteFix.Entidades.DTOs;

/// <summary>DTO de lectura de un técnico. Es lo único que la API devuelve.</summary>
public class TecnicoDto
{
    public int TecnicoId { get; set; }

    public string Documento { get; set; } = null!;

    public string NombreCompleto { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Telefono { get; set; }

    public string Especialidad { get; set; } = null!;

    public bool Activo { get; set; }

    public DateTime FechaIngreso { get; set; }
}
