namespace ByteFix.Entidades.Entities;

/// <summary>
/// Técnico del taller ByteFix. Es la entidad padre de la relación 1:N.
/// </summary>
public partial class Tecnico
{
    public int TecnicoId { get; set; }

    public string Documento { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Telefono { get; set; }

    public string Especialidad { get; set; } = null!;

    public bool Activo { get; set; }

    public DateTime FechaIngreso { get; set; }

    public virtual ICollection<Reparacion> Reparaciones { get; set; } = new List<Reparacion>();
}
