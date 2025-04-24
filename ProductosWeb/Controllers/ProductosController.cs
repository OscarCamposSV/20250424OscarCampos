using Entidades.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositorios.ProductoRepos;

namespace ProductosWeb.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductosController : ControllerBase
	{
		private readonly IProductoRepository _repository;

		public ProductosController(IProductoRepository productoRepository)
		{
			_repository = productoRepository;
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var producto = await _repository.GetByIdAsync(id); 
			if (producto == null) return NotFound();
			return Ok(producto);
		}

		[HttpGet("all")]
		public async Task<IActionResult> GetAll()
		{
			var productos = await _repository.GetAllAsync(); 
			return Ok(productos);
		}

		[HttpPost]
		public async Task<IActionResult> Crear([FromBody] Productos producto)
		{
			if (producto.Precio <= 0)
			{
				return BadRequest("El precio debe ser mayor que 0.");
			}

			if (producto.PrecioDescuento.HasValue && producto.PrecioDescuento <= 0)
			{
				return BadRequest("El precio debe ser mayor que 0.");
			}

			await _repository.AddAsync(producto); 
			return CreatedAtAction(nameof(GetById), new { id = producto.IdProducto }, producto);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Actualizar(int id, [FromBody] Productos producto)
		{
			var productoActual = await _repository.GetByIdAsync(id);

			if (productoActual == null)
			{
				return NotFound();
			}

			if (id != productoActual.IdProducto)
			{
				return BadRequest("El ID del producto no coincide.");
			}

			var resultado = ActualizarProducto(productoActual, producto);
			if (resultado != null)
			{
				return BadRequest(resultado);
			}

			await _repository.UpdateAsync(productoActual);
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Eliminar(int id)
		{
			var producto = await _repository.GetByIdAsync(id);
			if (producto == null) return NotFound();

			await _repository.DeleteAsync(producto); 
			return NoContent();
		}

		[HttpGet("stored")]
		public async Task<IActionResult> GetViaStoredProc()
		{
			var productos = await _repository.GetAllFromStoredProcedureAsync(); 
			return Ok(productos);
		}


		private string? ActualizarProducto(Productos productoActual, Productos productoNuevo)
		{
			if (!string.IsNullOrEmpty(productoNuevo.Nombre))
			{
				productoActual.Nombre = productoNuevo.Nombre;
			}

			if (!string.IsNullOrEmpty(productoNuevo.Descripcion))
			{
				productoActual.Descripcion = productoNuevo.Descripcion;
			}

			if (productoNuevo.Precio.HasValue)
			{
				if (productoNuevo.Precio <= 0)
				{
					return "El precio debe ser mayor que 0.";
				}
				productoActual.Precio = productoNuevo.Precio.Value;
			}

			if (productoNuevo.PrecioDescuento.HasValue)
			{
				if (productoNuevo.PrecioDescuento <= 0)
				{
					return "El precio con descuento debe ser mayor que 0.";
				}
				productoActual.PrecioDescuento = productoNuevo.PrecioDescuento.Value;
			}

			if (!string.IsNullOrEmpty(productoNuevo.RutaImagen))
			{
				productoActual.RutaImagen = productoNuevo.RutaImagen;
			}

			return null; 
		}
	}
}
