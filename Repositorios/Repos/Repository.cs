using Microsoft.EntityFrameworkCore;
using Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorios.Repos
{
	public class Repository<T> : IRepository<T> where T : class
	{

		protected readonly ApplicationDbContext _db;
		protected readonly DbSet<T> _dbSet;

		public Repository(ApplicationDbContext dbContext)
		{
			_db = dbContext;
			_dbSet = dbContext.Set<T>();
		}

		public async Task AddAsync(T entity)
		{
			await _dbSet.AddAsync(entity);
			await _db.SaveChangesAsync();
		}

		public async Task DeleteAsync(T entity)
		{
			_dbSet.Remove(entity);
			await _db.SaveChangesAsync();
		}

		public async Task<IEnumerable<T>> GetAllAsync()
		{
			return await _dbSet.ToListAsync();
		}

		public async Task<T?> GetByIdAsync(int id)
		{
			return await _dbSet.FindAsync(id);
		}

		public async Task UpdateAsync(T entity)
		{
			_dbSet.Update(entity);
			await _db.SaveChangesAsync();
		}
	}
}
