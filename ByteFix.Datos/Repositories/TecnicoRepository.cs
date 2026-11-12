using ByteFix.Datos.Data;
using ByteFix.Entidades.Entities;
using Microsoft.EntityFrameworkCore;

namespace ByteFix.Datos.Repositories;

public class TecnicoRepository : ITecnicoRepository
{
    private readonly ByteFixContext _context;

    public TecnicoRepository(ByteFixContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Tecnico>> GetAllAsync(string? buscar, bool? activo, string? especialidad)
    {
        var query = _context.Tecnicos.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            var termino = buscar.Trim();
            query = query.Where(t =>
                t.Nombre.Contains(termino) ||
                t.Apellido.Contains(termino) ||
                t.Documento.Contains(termino) ||
                t.Email.Contains(termino));
        }

        if (activo.HasValue)
            query = query.Where(t => t.Activo == activo.Value);

        if (!string.IsNullOrWhiteSpace(especialidad))
        {
            var filtro = especialidad.Trim();
            query = query.Where(t => t.Especialidad.Contains(filtro));
        }

        return await query.ToListAsync();
    }

    public async Task<Tecnico?> GetByIdAsync(int id)
    {
        return await _context.Tecnicos.FirstOrDefaultAsync(t => t.TecnicoId == id);
    }

    public async Task<bool> ExisteAsync(int id)
    {
        return await _context.Tecnicos.AnyAsync(t => t.TecnicoId == id);
    }

    public async Task<bool> DocumentoExisteAsync(string documento, int? exceptoId = null)
    {
        return await _context.Tecnicos.AnyAsync(t =>
            t.Documento == documento && (!exceptoId.HasValue || t.TecnicoId != exceptoId.Value));
    }

    public async Task<bool> EmailExisteAsync(string email, int? exceptoId = null)
    {
        return await _context.Tecnicos.AnyAsync(t =>
            t.Email == email && (!exceptoId.HasValue || t.TecnicoId != exceptoId.Value));
    }

    public async Task<bool> TieneReparacionesAsync(int id)
    {
        return await _context.Reparaciones.AnyAsync(r => r.TecnicoId == id);
    }

    public async Task<bool> TieneReparacionesAbiertasAsync(int id)
    {
        return await _context.Reparaciones.AnyAsync(r =>
            r.TecnicoId == id && r.Estado != "Entregado");
    }

    public async Task AgregarAsync(Tecnico tecnico)
    {
        await _context.Tecnicos.AddAsync(tecnico);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Tecnico tecnico)
    {
        _context.Tecnicos.Update(tecnico);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Tecnico tecnico)
    {
        _context.Tecnicos.Remove(tecnico);
        await _context.SaveChangesAsync();
    }
}
