using System.ComponentModel.DataAnnotations;

namespace ByteFix.Entidades.DTOs;

/// <summary>DTO de entrada para actualizar un técnico. Incluye el Id para validar la ruta.</summary>
public class TecnicoActualizarDto
{
    [Range(1, int.MaxValue, ErrorMessage = "El Id del técnico debe ser mayor que cero.")]
    public int TecnicoId { get; set; }

    [Required(ErrorMessage = "El documento es obligatorio.")]
    [StringLength(15, MinimumLength = 8, ErrorMessage = "El documento debe tener entre 8 y 15 caracteres.")]
    [RegularExpression(@"^\d{8,15}$", ErrorMessage = "El documento solo admite entre 8 y 15 dígitos.")]
    public string Documento { get; set; } = null!;

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(80, ErrorMessage = "El nombre no puede superar los 80 caracteres.")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(80, ErrorMessage = "El apellido no puede superar los 80 caracteres.")]
    public string Apellido { get; set; } = null!;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    [StringLength(120, ErrorMessage = "El correo no puede superar los 120 caracteres.")]
    public string Email { get; set; } = null!;

    [StringLength(20, ErrorMessage = "El teléfono no puede superar los 20 caracteres.")]
    [RegularExpression(@"^\d{6,20}$", ErrorMessage = "El teléfono solo admite entre 6 y 20 dígitos.")]
    public string? Telefono { get; set; }

    [Required(ErrorMessage = "La especialidad es obligatoria.")]
    [StringLength(60, ErrorMessage = "La especialidad no puede superar los 60 caracteres.")]
    public string Especialidad { get; set; } = null!;

    public bool Activo { get; set; }
}
