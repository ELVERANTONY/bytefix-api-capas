using ByteFix.Entidades.Entities;

namespace ByteFix.Datos.Repositories;

public interface ITecnicoRepository
{
    Task<IReadOnlyList<Tecnico>> GetAllAsync(string? buscar, bool? activo, string? especialidad);

    Task<Tecnico?> GetByIdAsync(int id);

    Task<bool> ExisteAsync(int id);

    Task<bool> DocumentoExisteAsync(string documento, int? exceptoId = null);

    Task<bool> EmailExisteAsync(string email, int? exceptoId = null);

    Task<bool> TieneReparacionesAsync(int id);

    Task<bool> TieneReparacionesAbiertasAsync(int id);

    Task AgregarAsync(Tecnico tecnico);

    Task ActualizarAsync(Tecnico tecnico);

    Task EliminarAsync(Tecnico tecnico);
}
