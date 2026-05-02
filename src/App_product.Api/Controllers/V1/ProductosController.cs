using App_product.Application.DTOs;
using App_product.Application.Services;
using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace App_product.Api.Controllers.V1;

/// <summary>
/// Controlador REST para la gestión de productos.
/// Versión 1 de la API — ruta base: /api/v1/productos
///
/// Responsabilidad única (SRP): solo orquesta llamadas HTTP, delega la lógica al servicio.
/// Depende de la interfaz <see cref="IProductoService"/>, no de la implementación (DIP).
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public sealed class ProductosController : ControllerBase
{
    private readonly IProductoService _servicio;
    private readonly IValidator<CrearProductoDto> _validadorCrear;
    private readonly IValidator<ActualizarProductoDto> _validadorActualizar;

    public ProductosController(
        IProductoService servicio,
        IValidator<CrearProductoDto> validadorCrear,
        IValidator<ActualizarProductoDto> validadorActualizar)
    {
        _servicio = servicio;
        _validadorCrear = validadorCrear;
        _validadorActualizar = validadorActualizar;
    }

    /// <summary>Obtiene la lista completa de productos.</summary>
    /// <returns>Lista de productos registrados en el sistema.</returns>
    /// <response code="200">Lista retornada exitosamente.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerTodos()
    {
        var productos = await _servicio.ObtenerTodosAsync();
        return Ok(productos);
    }

    /// <summary>Obtiene un producto específico por su identificador.</summary>
    /// <param name="id">Identificador único del producto.</param>
    /// <returns>Datos del producto solicitado.</returns>
    /// <response code="200">Producto encontrado y retornado.</response>
    /// <response code="404">No existe un producto con el Id indicado.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProductoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var producto = await _servicio.ObtenerPorIdAsync(id);
        return Ok(producto);
    }

    /// <summary>Crea un nuevo producto en el sistema.</summary>
    /// <param name="dto">Datos del producto a crear.</param>
    /// <returns>El producto creado con su identificador asignado.</returns>
    /// <response code="201">Producto creado exitosamente. Se retorna en el cuerpo y en el header Location.</response>
    /// <response code="400">Los datos del producto no son válidos (violación de reglas de validación).</response>
    /// <remarks>
    /// Ejemplo de cuerpo de solicitud:
    /// <code>
    /// {
    ///   "nombre": "Cuaderno universitario",
    ///   "descripcion": "Cuaderno de 200 hojas tamaño carta",
    ///   "precio": 8500.00
    /// }
    /// </code>
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(typeof(ProductoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Crear([FromBody] CrearProductoDto dto)
    {
        var resultado = await _validadorCrear.ValidateAsync(dto);
        if (!resultado.IsValid)
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Error de validación",
                Detail = string.Join(" | ", resultado.Errors.Select(e => e.ErrorMessage))
            });

        var creado = await _servicio.CrearAsync(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, creado);
    }

    /// <summary>Actualiza completamente un producto existente.</summary>
    /// <param name="id">Identificador del producto a actualizar.</param>
    /// <param name="dto">Nuevos datos del producto.</param>
    /// <response code="204">Producto actualizado exitosamente. Sin contenido en la respuesta.</response>
    /// <response code="400">Los datos del producto no son válidos.</response>
    /// <response code="404">No existe un producto con el Id indicado.</response>
    /// <remarks>
    /// Ejemplo de cuerpo de solicitud:
    /// <code>
    /// {
    ///   "nombre": "Cuaderno universitario actualizado",
    ///   "descripcion": "Nueva descripción",
    ///   "precio": 9000.00
    /// }
    /// </code>
    /// </remarks>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarProductoDto dto)
    {
        var resultado = await _validadorActualizar.ValidateAsync(dto);
        if (!resultado.IsValid)
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Error de validación",
                Detail = string.Join(" | ", resultado.Errors.Select(e => e.ErrorMessage))
            });

        await _servicio.ActualizarAsync(id, dto);
        return NoContent();
    }

    /// <summary>Elimina un producto del sistema.</summary>
    /// <param name="id">Identificador del producto a eliminar.</param>
    /// <response code="204">Producto eliminado exitosamente. Sin contenido en la respuesta.</response>
    /// <response code="404">No existe un producto con el Id indicado.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(int id)
    {
        await _servicio.EliminarAsync(id);
        return NoContent();
    }
}
