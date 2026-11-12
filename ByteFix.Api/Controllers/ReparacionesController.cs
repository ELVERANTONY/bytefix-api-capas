using ByteFix.Entidades.DTOs;
using ByteFix.Entidades.Exceptions;
using ByteFix.Negocio.Services;
using Microsoft.AspNetCore.Mvc;

namespace ByteFix.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReparacionesController : ControllerBase
{
    private readonly IReparacionService _service;

    public ReparacionesController(IReparacionService service)
    {
        _service = service;
    }

    /// <summary>Lista reparaciones con filtros por técnico, estado, prioridad y texto.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ReparacionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ReparacionDto>>> GetAll(
        [FromQuery] int? tecnicoId,
        [FromQuery] string? estado,
        [FromQuery] string? prioridad,
        [FromQuery] string? buscar)
    {
        var reparaciones = await _service.GetAllAsync(tecnicoId, estado, prioridad, buscar);
        return Ok(reparaciones);
    }

    /// <summary>Obtiene una reparación por su Id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ReparacionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReparacionDto>> GetById(int id)
    {
        var reparacion = await _service.GetByIdAsync(id);
        if (reparacion is null)
            return NotFound(new { mensaje = $"No se encontró la reparación con Id {id}." });

        return Ok(reparacion);
    }

    /// <summary>Registra una reparación. No permite técnicos inactivos ni series repetidas.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ReparacionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ReparacionDto>> Create([FromBody] ReparacionCrearDto dto)
    {
        try
        {
            var creado = await _service.CrearAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = creado.ReparacionId }, creado);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    /// <summary>Actualiza una reparación. Una reparación entregada ya no se puede modificar.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(int id, [FromBody] ReparacionActualizarDto dto)
    {
        if (id != dto.ReparacionId)
            return BadRequest(new { mensaje = "El Id de la ruta no coincide con el Id del cuerpo." });

        try
        {
            var actualizado = await _service.ActualizarAsync(id, dto);
            if (!actualizado)
                return NotFound(new { mensaje = $"No se encontró la reparación con Id {id}." });

            return NoContent();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    /// <summary>Elimina una reparación que todavía no fue entregada.</summary>
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
                return NotFound(new { mensaje = $"No se encontró la reparación con Id {id}." });

            return NoContent();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }
}
