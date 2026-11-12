using AutoMapper;
using ByteFix.Datos.Repositories;
using ByteFix.Entidades.DTOs;
using ByteFix.Entidades.Entities;
using ByteFix.Entidades.Exceptions;

namespace ByteFix.Negocio.Services;

public class TecnicoService : ITecnicoService
{
    private readonly ITecnicoRepository _tecnicos;
    private readonly IReparacionRepository _reparaciones;
    private readonly IMapper _mapper;

    public TecnicoService(
        ITecnicoRepository tecnicos,
        IReparacionRepository reparaciones,
        IMapper mapper)
    {
        _tecnicos = tecnicos;
        _reparaciones = reparaciones;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TecnicoDto>> GetAllAsync(string? buscar, bool? activo, string? especialidad)
    {
        var lista = await _tecnicos.GetAllAsync(buscar, activo, especialidad);
        return _mapper.Map<List<TecnicoDto>>(lista)
            .OrderBy(t => t.NombreCompleto)
            .ToList();
    }

    public async Task<TecnicoDto?> GetByIdAsync(int id)
    {
        var entidad = await _tecnicos.GetByIdAsync(id);
        return entidad is null ? null : _mapper.Map<TecnicoDto>(entidad);
    }

    public async Task<IReadOnlyList<ReparacionDto>?> GetReparacionesAsync(int id)
    {
        if (!await _tecnicos.ExisteAsync(id))
            return null;

        var lista = await _reparaciones.GetByTecnicoIdAsync(id);
        return _mapper.Map<List<ReparacionDto>>(lista)
            .OrderByDescending(r => r.FechaIngreso)
            .ToList();
    }

    public async Task<TecnicoDto> CrearAsync(TecnicoCrearDto dto)
    {
        if (await _tecnicos.DocumentoExisteAsync(dto.Documento))
            throw new ReglaNegocioException($"Ya existe un técnico con el documento {dto.Documento}.");

        if (await _tecnicos.EmailExisteAsync(dto.Email))
            throw new ReglaNegocioException($"Ya existe un técnico con el correo {dto.Email}.");

        var entidad = _mapper.Map<Tecnico>(dto);
        entidad.FechaIngreso = DateTime.Now;

        await _tecnicos.AgregarAsync(entidad);
        return _mapper.Map<TecnicoDto>(entidad);
    }

    public async Task<bool> ActualizarAsync(int id, TecnicoActualizarDto dto)
    {
        var entidad = await _tecnicos.GetByIdAsync(id);
        if (entidad is null)
            return false;

        if (await _tecnicos.DocumentoExisteAsync(dto.Documento, id))
            throw new ReglaNegocioException($"Ya existe otro técnico con el documento {dto.Documento}.");

        if (await _tecnicos.EmailExisteAsync(dto.Email, id))
            throw new ReglaNegocioException($"Ya existe otro técnico con el correo {dto.Email}.");

        if (entidad.Activo && !dto.Activo && await _tecnicos.TieneReparacionesAbiertasAsync(id))
            throw new ReglaNegocioException(
                "No se puede desactivar un técnico que tiene reparaciones pendientes o en proceso.");

        _mapper.Map(dto, entidad);
        await _tecnicos.ActualizarAsync(entidad);
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var entidad = await _tecnicos.GetByIdAsync(id);
        if (entidad is null)
            return false;

        if (await _tecnicos.TieneReparacionesAsync(id))
            throw new ReglaNegocioException(
                "No se puede eliminar un técnico que tiene reparaciones registradas.");

        await _tecnicos.EliminarAsync(entidad);
        return true;
    }
}
