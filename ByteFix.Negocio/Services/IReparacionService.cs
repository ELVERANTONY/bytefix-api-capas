using ByteFix.Entidades.DTOs;

namespace ByteFix.Negocio.Services;

public interface IReparacionService
{
    Task<IReadOnlyList<ReparacionDto>> GetAllAsync(int? tecnicoId, string? estado, string? prioridad, string? buscar);

    Task<ReparacionDto?> GetByIdAsync(int id);

    Task<ReparacionDto> CrearAsync(ReparacionCrearDto dto);

    Task<bool> ActualizarAsync(int id, ReparacionActualizarDto dto);

    Task<bool> EliminarAsync(int id);
}
