using Entidades.Modelos;
using Microsoft.EntityFrameworkCore;
using Repositorios.Contexto;
using Repositorios.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorios.ProductoRepos
{
	public class ProductosRepository : Repository<Productos>, IProductoRepository
	{

		public ProductosRepository(ApplicationDbContext dbContext) : base(dbContext) { }

		public async Task<List<Productos>> GetAllFromStoredProcedureAsync()
		{
			return await _db.Productos.FromSqlRaw("EXEC GetAllProductos").ToListAsync();
		}
	}
}
