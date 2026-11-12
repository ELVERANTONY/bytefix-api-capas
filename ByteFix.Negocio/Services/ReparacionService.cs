using AutoMapper;
using ByteFix.Datos.Repositories;
using ByteFix.Entidades.DTOs;
using ByteFix.Entidades.Entities;
using ByteFix.Entidades.Exceptions;

namespace ByteFix.Negocio.Services;

public class ReparacionService : IReparacionService
{
    private readonly IReparacionRepository _reparaciones;
    private readonly ITecnicoRepository _tecnicos;
    private readonly IMapper _mapper;

    public ReparacionService(
        IReparacionRepository reparaciones,
        ITecnicoRepository tecnicos,
        IMapper mapper)
    {
        _reparaciones = reparaciones;
        _tecnicos = tecnicos;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<ReparacionDto>> GetAllAsync(int? tecnicoId, string? estado, string? prioridad, string? buscar)
    {
        var lista = await _reparaciones.GetAllAsync(tecnicoId, estado, prioridad, buscar);
        return _mapper.Map<List<ReparacionDto>>(lista)
            .OrderByDescending(r => r.FechaIngreso)
            .ThenBy(r => r.ReparacionId)
            .ToList();
    }

    public async Task<ReparacionDto?> GetByIdAsync(int id)
    {
        var entidad = await _reparaciones.GetByIdAsync(id);
        return entidad is null ? null : _mapper.Map<ReparacionDto>(entidad);
    }

    public async Task<ReparacionDto> CrearAsync(ReparacionCrearDto dto)
    {
        var tecnico = await _tecnicos.GetByIdAsync(dto.TecnicoId);
        if (tecnico is null)
            throw new ReglaNegocioException($"El técnico con Id {dto.TecnicoId} no existe.");

        if (!tecnico.Activo)
            throw new ReglaNegocioException(
                "No se puede asignar una reparación a un técnico inactivo.");

        if (await _reparaciones.SerieExisteAsync(dto.Serie))
            throw new ReglaNegocioException($"Ya existe una reparación con la serie {dto.Serie}.");

        var entidad = _mapper.Map<Reparacion>(dto);
        entidad.Estado = "Pendiente";
        entidad.FechaIngreso = DateTime.Now;
        entidad.FechaEntrega = null;
        entidad.Tecnico = tecnico;

        await _reparaciones.AgregarAsync(entidad);
        return _mapper.Map<ReparacionDto>(entidad);
    }

    public async Task<bool> ActualizarAsync(int id, ReparacionActualizarDto dto)
    {
        var entidad = await _reparaciones.GetByIdAsync(id);
        if (entidad is null)
            return false;

        if (entidad.Estado == "Entregado")
            throw new ReglaNegocioException(
                "La reparación ya fue entregada y no se puede modificar.");

        var tecnico = await _tecnicos.GetByIdAsync(dto.TecnicoId);
        if (tecnico is null)
            throw new ReglaNegocioException($"El técnico con Id {dto.TecnicoId} no existe.");

        if (!tecnico.Activo)
            throw new ReglaNegocioException(
                "No se puede asignar una reparación a un técnico inactivo.");

        if (await _reparaciones.SerieExisteAsync(dto.Serie, id))
            throw new ReglaNegocioException($"Ya existe otra reparación con la serie {dto.Serie}.");

        _mapper.Map(dto, entidad);
        entidad.Tecnico = tecnico;
        entidad.FechaEntrega = dto.Estado == "Entregado" ? DateTime.Now : null;

        await _reparaciones.ActualizarAsync(entidad);
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var entidad = await _reparaciones.GetByIdAsync(id);
        if (entidad is null)
            return false;

        if (entidad.Estado == "Entregado")
            throw new ReglaNegocioException(
                "No se puede eliminar una reparación que ya fue entregada.");

        await _reparaciones.EliminarAsync(entidad);
        return true;
    }
}
