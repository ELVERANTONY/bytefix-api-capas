using ByteFix.Datos.Data;
using ByteFix.Entidades.Entities;
using Microsoft.EntityFrameworkCore;

namespace ByteFix.Datos.Repositories;

public class ReparacionRepository : IReparacionRepository
{
    private readonly ByteFixContext _context;

    public ReparacionRepository(ByteFixContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Reparacion>> GetAllAsync(int? tecnicoId, string? estado, string? prioridad, string? buscar)
    {
        var query = _context.Reparaciones
            .Include(r => r.Tecnico)
            .AsNoTracking()
            .AsQueryable();

        if (tecnicoId.HasValue)
            query = query.Where(r => r.TecnicoId == tecnicoId.Value);

        if (!string.IsNullOrWhiteSpace(estado))
        {
            var filtroEstado = estado.Trim();
            query = query.Where(r => r.Estado == filtroEstado);
        }

        if (!string.IsNullOrWhiteSpace(prioridad))
        {
            var filtroPrioridad = prioridad.Trim();
            query = query.Where(r => r.Prioridad == filtroPrioridad);
        }

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            var termino = buscar.Trim();
            query = query.Where(r =>
                r.Serie.Contains(termino) ||
                r.Equipo.Contains(termino) ||
                r.Marca.Contains(termino) ||
                r.FallaReportada.Contains(termino));
        }

        return await query.ToListAsync();
    }

    public async Task<Reparacion?> GetByIdAsync(int id)
    {
        return await _context.Reparaciones
            .Include(r => r.Tecnico)
            .FirstOrDefaultAsync(r => r.ReparacionId == id);
    }

    public async Task<IReadOnlyList<Reparacion>> GetByTecnicoIdAsync(int tecnicoId)
    {
        return await _context.Reparaciones
            .Include(r => r.Tecnico)
            .AsNoTracking()
            .Where(r => r.TecnicoId == tecnicoId)
            .ToListAsync();
    }

    public async Task<bool> SerieExisteAsync(string serie, int? exceptoId = null)
    {
        return await _context.Reparaciones.AnyAsync(r =>
            r.Serie == serie && (!exceptoId.HasValue || r.ReparacionId != exceptoId.Value));
    }

    public async Task AgregarAsync(Reparacion reparacion)
    {
        await _context.Reparaciones.AddAsync(reparacion);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Reparacion reparacion)
    {
        _context.Reparaciones.Update(reparacion);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Reparacion reparacion)
    {
        _context.Reparaciones.Remove(reparacion);
        await _context.SaveChangesAsync();
    }
}
