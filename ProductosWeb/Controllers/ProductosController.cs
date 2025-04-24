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

		[HttpPost]
		public async Task<IActionResult> Crear([FromBody] Productos producto)
		{
			if (string.IsNullOrEmpty(producto.Nombre) || string.IsNullOrEmpty(producto.Descripcion) || !producto.Precio.HasValue || string.IsNullOrEmpty(producto.RutaImagen))
			{
				return BadRequest("Los campos Nombre, Descripcion, Precio e Imagen son obligatorios");
			}

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
			var productoActualizar = await _repository.GetByIdAsync(id);

			if (productoActualizar == null)
			{
				return NotFound();
			}

			if (id != productoActualizar.IdProducto)
			{
				return BadRequest("El ID del producto no coincide.");
			}

			var resultado = ActualizarProducto(productoActualizar, producto);
			if (resultado != null)
			{
				return BadRequest(resultado);
			}

			await _repository.UpdateAsync(productoActualizar);
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

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var producto = await _repository.GetByIdAsync(id); 
			if (producto == null) return NotFound("no se encontro registro que coincida");
			return Ok(producto);
		}

		[HttpGet("all")]
		public async Task<IActionResult> GetAll()
		{
			var productos = await _repository.GetAllAsync();

			if (productos == null || !productos.Any())
			{
				return Ok(new { mensaje = "No hay productos registrados", productos = new List<Productos>() });
			}

			return Ok(productos);
		}

		[HttpGet("stored")]
		public async Task<IActionResult> GetViaStoredProc()
		{
			var productos = await _repository.GetAllFromStoredProcedureAsync();

			if (productos == null || !productos.Any()) {
				return Ok(new { mensaje = "No se encontraron productos.", productos = new List<Productos>() });
			}
				
			return Ok(productos);
		}


		private string? ActualizarProducto(Productos productoActualizar, Productos productoNuevo)
		{
			if (!string.IsNullOrEmpty(productoNuevo.Nombre))
			{
				productoActualizar.Nombre = productoNuevo.Nombre;
			}

			if (!string.IsNullOrEmpty(productoNuevo.Descripcion))
			{
				productoActualizar.Descripcion = productoNuevo.Descripcion;
			}

			if (productoNuevo.Precio.HasValue)
			{
				if (productoNuevo.Precio <= 0)
				{
					return "El precio debe ser mayor que 0.";
				}
				productoActualizar.Precio = productoNuevo.Precio.Value;
			}

			if (productoNuevo.PrecioDescuento.HasValue)
			{
				if (productoNuevo.PrecioDescuento <= 0)
				{
					return "El precio con descuento debe ser mayor que 0.";
				}
				productoActualizar.PrecioDescuento = productoNuevo.PrecioDescuento.Value;
			}

			if (!string.IsNullOrEmpty(productoNuevo.RutaImagen))
			{
				productoActualizar.RutaImagen = productoNuevo.RutaImagen;
			}

			return null; 
		}
	}
}
