using API_GESTION_PRODUTOS.Models;
using Microsoft.EntityFrameworkCore;

namespace API_GESTION_PRODUTOS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        public DbSet<CategoriaProducto> CategoriaProductos { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Pais> Paises { get; set; }
        public DbSet<Ciudad> Ciudades { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetalleVentas { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<DetalleCompra> DetalleCompras { get; set; }
        public DbSet<Ajuste> Ajustes { get; set; }
        public DbSet<DetalleAjuste> DetalleAjustes { get; set; }
        public DbSet<RegistroAlmacen> RegistroAlmacenes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Entity<CategoriaProducto>(categoriaProducto =>
            {
                categoriaProducto.ToTable("CategoriaProducto");
                categoriaProducto.HasKey(p => p.Id);
                categoriaProducto.Property(p => p.Nombre).IsRequired();
            });

            modelBuilder.Entity<Producto>(producto =>
            {
                producto.ToTable("Producto");
                producto.HasKey(p => p.Id);
                producto.Property(p => p.CodigoProducto).IsRequired();
                producto.Property(p => p.Descripcion).IsRequired();
                producto.Property(p => p.PrecioVenta).IsRequired();
                producto.Property(p => p.PrecioCompra).IsRequired();
                producto.HasOne(p => p.CategoriaProducto).WithMany(c => c.Productos).HasForeignKey(p => p.CategoriaProductoId).IsRequired();
            });

            modelBuilder.Entity<Pais>(pais =>
            {
                pais.ToTable("Pais");
                pais.HasKey(p => p.Id);
                pais.Property(p => p.Nombre).IsRequired();
            });

            modelBuilder.Entity<Ciudad>(ciudad =>
            {
                ciudad.ToTable("Ciudad");
                ciudad.HasKey(p => p.Id);
                ciudad.Property(p => p.Nombre).IsRequired();
                ciudad.HasOne(p => p.Pais).WithMany(p => p.Ciudades).HasForeignKey(p => p.PaisId).IsRequired();
            });

            modelBuilder.Entity<Proveedor>(proveedor =>
            {
                proveedor.ToTable("Proveedor");
                proveedor.HasKey(p => p.Id);
                proveedor.Property(p => p.Nombre).IsRequired();
                proveedor.HasOne(p => p.Ciudad).WithMany(c => c.Proveedores).HasForeignKey(p => p.CiudadId).IsRequired();
                proveedor.Property(p => p.Direccion);
                proveedor.Property(p => p.Correo);
                proveedor.Property(p => p.Telefono);
            });

            modelBuilder.Entity<Cliente>(cliente =>
            {
                cliente.ToTable("Cliente");
                cliente.HasKey(p => p.Id);
                cliente.Property(p => p.Nombres).IsRequired();
                cliente.Property(p => p.ApellidoPaterno).IsRequired();
                cliente.Property(p => p.ApellidoMaterno).IsRequired();
                cliente.Property(p => p.CI).IsRequired();
                cliente.Property(p => p.Correo).IsRequired(false);
                cliente.Property(p => p.Telefono).IsRequired(false);
                cliente.Property(p => p.Direccion).IsRequired(false);
            });

            modelBuilder.Entity<Empleado>(empleado =>
            {
                empleado.ToTable("Empleado");
                empleado.HasKey(p=>p.Id);
                empleado.Property(p => p.CodigoEmpleado).IsRequired();
                empleado.Property(p => p.Nombres).IsRequired();
                empleado.Property(p => p.ApellidoPaterno).IsRequired();
                empleado.Property(p => p.ApellidoMaterno).IsRequired();
                empleado.Property(p => p.FechaInicio).IsRequired(false);
                empleado.Property(p => p.Sucursal).IsRequired(false);
                empleado.Property(p => p.CI).IsRequired();
                empleado.Property(p => p.Correo).IsRequired(false);
                empleado.Property(p => p.Telefono).IsRequired(false);
                empleado.Property(p => p.Direccion).IsRequired(false);
            });

            modelBuilder.Entity<Venta>(venta =>
            {
                venta.ToTable("Venta");
                venta.HasKey(p => p.Id);
                venta.Property(p => p.CodigoFactura).IsRequired();
                venta.HasOne(p => p.Cliente).WithMany(c => c.Ventas).HasForeignKey(p => p.ClienteId).IsRequired();
                venta.HasOne(p => p.Empleado).WithMany(e => e.Ventas).HasForeignKey(p => p.EmpleadoId).IsRequired();
                venta.Property(p => p.Fecha).IsRequired();
                venta.Property(p => p.Total).IsRequired();
            });

            modelBuilder.Entity<DetalleVenta>(detalleVenta =>
            {
                detalleVenta.ToTable("DetalleVenta");
                detalleVenta.HasKey(p => p.Id);
                detalleVenta.HasOne(p => p.Venta).WithMany(v => v.DetalleVentas).HasForeignKey(p => p.VentaId).IsRequired();
                detalleVenta.HasOne(p => p.Producto).WithMany(p => p.DetalleVentas).HasForeignKey(p => p.ProductoId).IsRequired();
                detalleVenta.Property(p => p.Cantidad).IsRequired();
                detalleVenta.Property(p => p.PrecioVenta).IsRequired();
                detalleVenta.Property(p => p.Total).IsRequired();
            });

            modelBuilder.Entity<Compra>(compra =>
            {
                compra.ToTable("Compra");
                compra.HasKey(p => p.Id);
                compra.Property(p => p.CodigoFactura).IsRequired();
                compra.HasOne(p => p.Proveedor).WithMany(p => p.Compras).HasForeignKey(p => p.ProveedorId);
                compra.HasOne(p => p.Empleado).WithMany(p => p.Compras).HasForeignKey(p => p.EmpleadoId);
                compra.Property(p => p.Fecha).IsRequired();
                compra.Property(p => p.Total).IsRequired();
            });

            modelBuilder.Entity<DetalleCompra>(detalleCompra =>
            {
                detalleCompra.ToTable("DetalleCompra");
                detalleCompra.HasKey(p => p.Id);
                detalleCompra.HasOne(p => p.Compra).WithMany(v => v.DetalleCompras).HasForeignKey(p => p.CompraId).IsRequired();
                detalleCompra.HasOne(p => p.Producto).WithMany(p => p.DetalleCompras).HasForeignKey(p => p.ProductoId).IsRequired();
                detalleCompra.Property(p => p.Cantidad).IsRequired();
                detalleCompra.Property(p => p.PrecioCompra).IsRequired();
                detalleCompra.Property(p => p.Total).IsRequired();
            });

            modelBuilder.Entity<Ajuste>(ajuste =>
            {
                ajuste.ToTable("Ajuste");
                ajuste.HasKey(p => p.Id);
                ajuste.Property(p => p.Descripcion).IsRequired();
                ajuste.HasOne(p => p.Empleado).WithMany(e => e.Ajustes).HasForeignKey(p => p.EmpleadoId).IsRequired();
                ajuste.Property(p => p.Fecha).IsRequired();
            });

            modelBuilder.Entity<DetalleAjuste>(detalleAjuste =>
            {
                detalleAjuste.ToTable("DetalleAjuste");
                detalleAjuste.HasKey(p => p.Id);
                detalleAjuste.HasOne(p => p.Ajuste).WithMany(a => a.DetalleAjustes).HasForeignKey(p => p.AjusteId).IsRequired();
                detalleAjuste.HasOne(p => p.Producto).WithMany(p => p.DetalleAjustes).HasForeignKey(p => p.ProductoId).IsRequired();
                detalleAjuste.Property(p => p.Cantidad).IsRequired();
                detalleAjuste.Property(p => p.TipoMovimento).IsRequired();
            });

            modelBuilder.Entity<RegistroAlmacen>(registroAlmacen =>
            {
                registroAlmacen.ToTable("RegistroAlmacen");
                registroAlmacen.HasKey(p => p.ProductoId);
                registroAlmacen.HasOne(p => p.Producto).WithMany(p => p.RegistroAlmacenes).HasForeignKey(p => p.ProductoId).IsRequired();
                registroAlmacen.Property(p => p.Cantidad).IsRequired();
            });
        }
    }
}

