using AutoMapper;
using ByteFix.Entidades.DTOs;
using ByteFix.Entidades.Entities;

namespace ByteFix.Negocio.Mapping;

/// <summary>
/// Único perfil de mapeos de la solución.
/// Mapea entidades -> DTO de lectura y DTO de entrada -> entidad.
/// No se usa ReverseMap: los sentidos tienen configuraciones distintas
/// (por ejemplo, NombreCompleto y TecnicoNombre solo existen en los DTO de lectura).
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // 1) Técnico -> TecnicoDto: es lo que la API devuelve al cliente.
        //    NombreCompleto se construye con los datos de la entidad.
        CreateMap<Tecnico, TecnicoDto>()
            .ForMember(d => d.NombreCompleto,
                o => o.MapFrom(s => $"{s.Nombre} {s.Apellido}"));

        // 2) TecnicoCrearDto -> Tecnico: lo que llega del cliente se convierte
        //    en entidad para guardarla. El Id y la fecha los genera el servidor.
        CreateMap<TecnicoCrearDto, Tecnico>()
            .ForMember(d => d.TecnicoId, o => o.Ignore())
            .ForMember(d => d.FechaIngreso, o => o.Ignore())
            .ForMember(d => d.Reparaciones, o => o.Ignore());

        // 3) TecnicoActualizarDto -> Tecnico: actualiza un objeto existente.
        //    La fecha de ingreso y la colección de reparaciones no se tocan.
        CreateMap<TecnicoActualizarDto, Tecnico>()
            .ForMember(d => d.FechaIngreso, o => o.Ignore())
            .ForMember(d => d.Reparaciones, o => o.Ignore());

        // 4) Reparación -> ReparacionDto: la API devuelve el nombre del técnico,
        //    no el objeto completo (también evita el ciclo de navegación).
        CreateMap<Reparacion, ReparacionDto>()
            .ForMember(d => d.TecnicoNombre,
                o => o.MapFrom(s => $"{s.Tecnico.Nombre} {s.Tecnico.Apellido}"));

        // 5) ReparacionCrearDto -> Reparacion: el estado, la fecha y el Id
        //    los controla el servidor.
        CreateMap<ReparacionCrearDto, Reparacion>()
            .ForMember(d => d.ReparacionId, o => o.Ignore())
            .ForMember(d => d.Estado, o => o.Ignore())
            .ForMember(d => d.FechaIngreso, o => o.Ignore())
            .ForMember(d => d.FechaEntrega, o => o.Ignore())
            .ForMember(d => d.Tecnico, o => o.Ignore());

        // 6) ReparacionActualizarDto -> Reparacion: actualiza los datos editables
        //    de una reparación existente; las fechas y la navegación se conservan.
        CreateMap<ReparacionActualizarDto, Reparacion>()
            .ForMember(d => d.FechaIngreso, o => o.Ignore())
            .ForMember(d => d.FechaEntrega, o => o.Ignore())
            .ForMember(d => d.Tecnico, o => o.Ignore());
    }
}
