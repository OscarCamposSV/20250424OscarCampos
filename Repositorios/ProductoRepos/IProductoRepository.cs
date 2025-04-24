using Entidades.Modelos;
using Repositorios.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorios.ProductoRepos
{
	public interface IProductoRepository : IRepository<Productos>
	{
		Task<List<Productos>> GetAllFromStoredProcedureAsync();
	}
}
