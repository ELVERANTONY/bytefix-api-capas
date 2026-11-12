using ByteFix.Entidades.DTOs;
using ByteFix.Entidades.Exceptions;
using ByteFix.Negocio.Services;
using Microsoft.AspNetCore.Mvc;

namespace ByteFix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TecnicosController : ControllerBase
{
    private readonly ITecnicoService _service;

    public TecnicosController(ITecnicoService service)
    {
        _service = service;
    }

    /// <summary>Lista técnicos con filtros opcionales por texto, estado y especialidad.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<TecnicoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<TecnicoDto>>> GetAll(
        [FromQuery] string? buscar,
        [FromQuery] bool? activo,
        [FromQuery] string? especialidad)
    {
        var tecnicos = await _service.GetAllAsync(buscar, activo, especialidad);
        return Ok(tecnicos);
    }

    /// <summary>Obtiene un técnico por su Id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TecnicoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TecnicoDto>> GetById(int id)
    {
        var tecnico = await _service.GetByIdAsync(id);
        if (tecnico is null)
            return NotFound(new { mensaje = $"No se encontró el técnico con Id {id}." });

        return Ok(tecnico);
    }

    /// <summary>Ruta anidada: reparaciones asignadas a un técnico.</summary>
    [HttpGet("{id:int}/reparaciones")]
    [ProducesResponseType(typeof(IReadOnlyList<ReparacionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<ReparacionDto>>> GetReparaciones(int id)
    {
        var reparaciones = await _service.GetReparacionesAsync(id);
        if (reparaciones is null)
            return NotFound(new { mensaje = $"No se encontró el técnico con Id {id}." });

        return Ok(reparaciones);
    }

    /// <summary>Registra un técnico. Valida documento y correo únicos.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(TecnicoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<TecnicoDto>> Create([FromBody] TecnicoCrearDto dto)
    {
        try
        {
            var creado = await _service.CrearAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = creado.TecnicoId }, creado);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    /// <summary>Actualiza un técnico existente.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(int id, [FromBody] TecnicoActualizarDto dto)
    {
        if (id != dto.TecnicoId)
            return BadRequest(new { mensaje = "El Id de la ruta no coincide con el Id del cuerpo." });

        try
        {
            var actualizado = await _service.ActualizarAsync(id, dto);
            if (!actualizado)
                return NotFound(new { mensaje = $"No se encontró el técnico con Id {id}." });

            return NoContent();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    /// <summary>Elimina un técnico solo si no tiene reparaciones registradas.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var eliminado = await _service.EliminarAsync(id);
            if (!eliminado)
                return NotFound(new { mensaje = $"No se encontró el técnico con Id {id}." });

            return NoContent();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }
}
