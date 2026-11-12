using ByteFix.Entidades.DTOs;

namespace ByteFix.Negocio.Services;

public interface ITecnicoService
{
    Task<IReadOnlyList<TecnicoDto>> GetAllAsync(string? buscar, bool? activo, string? especialidad);

    Task<TecnicoDto?> GetByIdAsync(int id);

    Task<IReadOnlyList<ReparacionDto>?> GetReparacionesAsync(int id);

    Task<TecnicoDto> CrearAsync(TecnicoCrearDto dto);

    Task<bool> ActualizarAsync(int id, TecnicoActualizarDto dto);

    Task<bool> EliminarAsync(int id);
}
