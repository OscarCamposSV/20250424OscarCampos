using Entidades.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorios.Contexto
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		public DbSet<Productos> Productos { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Productos>(entity =>
			{
				entity.ToTable("PRODUCTOS");

				entity.HasKey(p => p.IdProducto);

				entity.Property(p => p.IdProducto).HasColumnName("ID_PRODUCTO").ValueGeneratedOnAdd();

				entity.Property(p => p.Nombre).IsRequired().HasColumnName("NOMBRE").HasMaxLength(100);

				entity.Property(p => p.Descripcion).IsRequired().HasColumnName("DESCRIPCION");

				entity.Property(p => p.Precio).IsRequired().HasColumnType("decimal(18,2)").HasColumnName("PRECIO");

				entity.Property(p => p.PrecioDescuento).HasColumnType("decimal(18,2)").HasColumnName("PRECIO_DESCUENTO");

				entity.Property(p => p.RutaImagen).IsRequired().HasColumnName("RUTA_IMAGEN");
			});
		}
	}
}
