using System.ComponentModel.DataAnnotations;

namespace ByteFix.Entidades.DTOs;

/// <summary>DTO de entrada para actualizar una reparación. Incluye el Id para validar la ruta.</summary>
public class ReparacionActualizarDto
{
    [Range(1, int.MaxValue, ErrorMessage = "El Id de la reparación debe ser mayor que cero.")]
    public int ReparacionId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "El Id del técnico debe ser mayor que cero.")]
    public int TecnicoId { get; set; }

    [Required(ErrorMessage = "La serie es obligatoria.")]
    [StringLength(30, MinimumLength = 5, ErrorMessage = "La serie debe tener entre 5 y 30 caracteres.")]
    [RegularExpression(@"^BF-\d{4}-\d{3}$", ErrorMessage = "La serie debe tener el formato BF-2026-001.")]
    public string Serie { get; set; } = null!;

    [Required(ErrorMessage = "El equipo es obligatorio.")]
    [StringLength(100, ErrorMessage = "El equipo no puede superar los 100 caracteres.")]
    public string Equipo { get; set; } = null!;

    [Required(ErrorMessage = "La marca es obligatoria.")]
    [StringLength(50, ErrorMessage = "La marca no puede superar los 50 caracteres.")]
    public string Marca { get; set; } = null!;

    [StringLength(50, ErrorMessage = "El modelo no puede superar los 50 caracteres.")]
    public string? Modelo { get; set; }

    [Required(ErrorMessage = "La falla reportada es obligatoria.")]
    [StringLength(200, ErrorMessage = "La falla reportada no puede superar los 200 caracteres.")]
    public string FallaReportada { get; set; } = null!;

    [Required(ErrorMessage = "La prioridad es obligatoria.")]
    [RegularExpression(@"^(Baja|Media|Alta)$", ErrorMessage = "La prioridad debe ser Baja, Media o Alta.")]
    public string Prioridad { get; set; } = "Media";

    [Required(ErrorMessage = "El estado es obligatorio.")]
    [RegularExpression(@"^(Pendiente|EnProceso|Entregado)$", ErrorMessage = "El estado debe ser Pendiente, EnProceso o Entregado.")]
    public string Estado { get; set; } = "Pendiente";

    [Range(0, 999999.99, ErrorMessage = "El costo debe estar entre 0 y 999999.99.")]
    public decimal Costo { get; set; }
}
