using ByteFix.Entidades.Entities;

namespace ByteFix.Datos.Repositories;

public interface IReparacionRepository
{
    Task<IReadOnlyList<Reparacion>> GetAllAsync(int? tecnicoId, string? estado, string? prioridad, string? buscar);

    Task<Reparacion?> GetByIdAsync(int id);

    Task<IReadOnlyList<Reparacion>> GetByTecnicoIdAsync(int tecnicoId);

    Task<bool> SerieExisteAsync(string serie, int? exceptoId = null);

    Task AgregarAsync(Reparacion reparacion);

    Task ActualizarAsync(Reparacion reparacion);

    Task EliminarAsync(Reparacion reparacion);
}
