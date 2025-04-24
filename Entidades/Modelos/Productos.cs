using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Modelos
{
	public class Productos
	{
		public int? IdProducto { get; set; }

		public string? Nombre { get; set; }

		public string? Descripcion { get; set; }

		public decimal? Precio { get; set; }

		public decimal? PrecioDescuento { get; set; }

		public string? RutaImagen { get; set; }
	}
}
