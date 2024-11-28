using System;
using System.Collections.Generic;
using Backend_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend_Api.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<CategoriasProducto> CategoriasProductos { get; set; }

    public virtual DbSet<CategoriasServicio> CategoriasServicios { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Compra> Compras { get; set; }

    public virtual DbSet<DetalleCompra> DetalleCompras { get; set; }

    public virtual DbSet<DetalleKit> DetalleKits { get; set; }

    public virtual DbSet<DetalleVenta> DetalleVentas { get; set; }

    public virtual DbSet<DetalleVentasProducto> DetalleVentasProductos { get; set; }

    public virtual DbSet<DetalleVentasServicio> DetalleVentasServicios { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<Kit> Kits { get; set; }

    public virtual DbSet<PagosEmpleado> PagosEmpleados { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Proveedore> Proveedores { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    public virtual DbSet<Venta> Ventas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
      

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<CategoriasProducto>(entity =>
        {
            entity.HasKey(e => e.CategoriaProductoId).HasName("PK__Categori__7C808EDCAA4EDAF1");

            entity.ToTable("CategoriasProducto");

            entity.Property(e => e.CategoriaProductoId).HasColumnName("CategoriaProductoID");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CategoriasServicio>(entity =>
        {
            entity.HasKey(e => e.CategoriaServicioId).HasName("PK__Categori__5D7B5623F65924B4");

            entity.ToTable("CategoriasServicio");

            entity.Property(e => e.CategoriaServicioId).HasColumnName("CategoriaServicioID");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.ClienteId).HasName("PK__Clientes__71ABD0A751C83D6A");

            entity.HasIndex(e => e.Dni, "UQ__Clientes__C035B8DDD4A41377").IsUnique();

            entity.Property(e => e.ClienteId).HasColumnName("ClienteID");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Dni)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DNI");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.NivelFidelizacion)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("BÁSICO");
            entity.Property(e => e.Nombres)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Observaciones)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.CompraId).HasName("PK__Compras__067DA72548D18232");

            entity.Property(e => e.CompraId).HasColumnName("CompraID");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("COMPLETADA");
            entity.Property(e => e.FechaCompra)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Igv)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("IGV");
            entity.Property(e => e.NumeroFactura)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Observaciones)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ProveedorId).HasColumnName("ProveedorID");
            entity.Property(e => e.SubTotal).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Proveedor).WithMany(p => p.Compras)
                .HasForeignKey(d => d.ProveedorId)
                .HasConstraintName("FK__Compras__Proveed__2180FB33");
        });

        modelBuilder.Entity<DetalleCompra>(entity =>
        {
            entity.HasKey(e => e.DetalleCompraId).HasName("PK__DetalleC__F7FC189AD421B69B");

            entity.HasIndex(e => new { e.CompraId, e.ProductoId }, "UK_CompraProducto").IsUnique();

            entity.HasIndex(e => new { e.ProductoId, e.CompraId }, "idx_detallecompra_producto");

            entity.Property(e => e.DetalleCompraId).HasColumnName("DetalleCompraID");
            entity.Property(e => e.CompraId).HasColumnName("CompraID");
            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductoId).HasColumnName("ProductoID");
            entity.Property(e => e.SubTotal).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Compra).WithMany(p => p.DetalleCompras)
                .HasForeignKey(d => d.CompraId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleCompras_Compra");

            entity.HasOne(d => d.Producto).WithMany(p => p.DetalleCompras)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleCompras_Producto");
        });

        modelBuilder.Entity<DetalleKit>(entity =>
        {
            entity.HasKey(e => e.DetalleKitId).HasName("PK__DetalleK__F1F1B6F394724616");

            entity.HasIndex(e => new { e.KitId, e.ProductoId }, "UK_KitProducto").IsUnique();

            entity.Property(e => e.DetalleKitId).HasColumnName("DetalleKitID");
            entity.Property(e => e.KitId).HasColumnName("KitID");
            entity.Property(e => e.ProductoId).HasColumnName("ProductoID");

            entity.HasOne(d => d.Kit).WithMany(p => p.DetalleKits)
                .HasForeignKey(d => d.KitId)
                .HasConstraintName("FK__DetalleKi__KitID__05D8E0BE");

            entity.HasOne(d => d.Producto).WithMany(p => p.DetalleKits)
                .HasForeignKey(d => d.ProductoId)
                .HasConstraintName("FK__DetalleKi__Produ__06CD04F7");
        });

        modelBuilder.Entity<DetalleVenta>(entity =>
        {
            entity.HasKey(e => e.DetalleVentaId).HasName("PK__DetalleV__340EED4493182A7A");

            entity.Property(e => e.DetalleVentaId).HasColumnName("DetalleVentaID");
            entity.Property(e => e.KitId).HasColumnName("KitID");
            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductoId).HasColumnName("ProductoID");
            entity.Property(e => e.SubTotal).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.VentaId).HasColumnName("VentaID");

            entity.HasOne(d => d.Kit).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.KitId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__DetalleVe__KitID__123EB7A3");

            entity.HasOne(d => d.Producto).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__DetalleVe__Produ__114A936A");

            entity.HasOne(d => d.Venta).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.VentaId)
                .HasConstraintName("FK__DetalleVe__Venta__10566F31");
        });

        modelBuilder.Entity<DetalleVentasProducto>(entity =>
        {
            entity.HasKey(e => e.DetalleVentaProductoId).HasName("PK__DetalleV__A3E1D895BC371D4C");

            entity.ToTable("DetalleVentasProducto");

            entity.HasIndex(e => new { e.VentaId, e.ProductoId, e.KitId }, "UK_VentaProducto").IsUnique();

            entity.Property(e => e.DetalleVentaProductoId).HasColumnName("DetalleVentaProductoID");
            entity.Property(e => e.KitId).HasColumnName("KitID");
            entity.Property(e => e.MontoComision).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PorcentajeComision).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductoId).HasColumnName("ProductoID");
            entity.Property(e => e.VentaId).HasColumnName("VentaID");

            entity.HasOne(d => d.Kit).WithMany(p => p.DetalleVentasProductos)
                .HasForeignKey(d => d.KitId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__DetalleVe__KitID__1EA48E88");

            entity.HasOne(d => d.Producto).WithMany(p => p.DetalleVentasProductos)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__DetalleVe__Produ__1DB06A4F");

            entity.HasOne(d => d.Venta).WithMany(p => p.DetalleVentasProductos)
                .HasForeignKey(d => d.VentaId)
                .HasConstraintName("FK__DetalleVe__Venta__1CBC4616");
        });

        modelBuilder.Entity<DetalleVentasServicio>(entity =>
        {
            entity.HasKey(e => e.DetalleVentaServicioId).HasName("PK__DetalleV__4C073DE41664F1DC");

            entity.ToTable("DetalleVentasServicio");

            entity.HasIndex(e => new { e.VentaId, e.ServicioId, e.EmpleadoId }, "UK_VentaServicio").IsUnique();

            entity.HasIndex(e => new { e.VentaId, e.EmpleadoId }, "idx_detalleventa_servicio");

            entity.Property(e => e.DetalleVentaServicioId).HasColumnName("DetalleVentaServicioID");
            entity.Property(e => e.EmpleadoId).HasColumnName("EmpleadoID");
            entity.Property(e => e.MontoComision).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PorcentajeComision).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ServicioId).HasColumnName("ServicioID");
            entity.Property(e => e.VentaId).HasColumnName("VentaID");

            entity.HasOne(d => d.Empleado).WithMany(p => p.DetalleVentasServicios)
                .HasForeignKey(d => d.EmpleadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleVentaServicio_Empleado");

            entity.HasOne(d => d.Servicio).WithMany(p => p.DetalleVentasServicios)
                .HasForeignKey(d => d.ServicioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleVentaServicio_Servicio");

            entity.HasOne(d => d.Venta).WithMany(p => p.DetalleVentasServicios)
                .HasForeignKey(d => d.VentaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleVentaServicio_Venta");
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.EmpleadoId).HasName("PK__Empleado__958BE6F0F1198379");

            entity.HasIndex(e => e.Email, "UQ__Empleado__A9D10534CD4AF2C8").IsUnique();

            entity.HasIndex(e => e.Dni, "UQ__Empleado__C035B8DDB99BC150").IsUnique();

            entity.HasIndex(e => e.Dni, "idx_empleado_dni");

            entity.HasIndex(e => e.Email, "idx_empleado_email");

            entity.Property(e => e.EmpleadoId).HasColumnName("EmpleadoID");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Dni)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DNI");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.Nombres)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SueldoBase).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Telefono)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Kit>(entity =>
        {
            entity.HasKey(e => e.KitId).HasName("PK__Kits__C96EE747D156E95B");

            entity.Property(e => e.KitId).HasColumnName("KitID");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PrecioVenta).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<PagosEmpleado>(entity =>
        {
            entity.HasKey(e => e.PagoEmpleadoId).HasName("PK__PagosEmp__8B0A4BD5331F5DBB");

            entity.Property(e => e.PagoEmpleadoId).HasColumnName("PagoEmpleadoID");
            entity.Property(e => e.ComisionProductos).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ComisionServicios).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.EmpleadoId).HasColumnName("EmpleadoID");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("PENDIENTE");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaPago).HasColumnType("datetime");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.SueldoBase).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TotalPago).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Empleado).WithMany(p => p.PagosEmpleados)
                .HasForeignKey(d => d.EmpleadoId)
                .HasConstraintName("FK__PagosEmpl__Emple__2BFE89A6");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.ProductoId).HasName("PK__Producto__A430AE83F91ADA7A");

            entity.HasIndex(e => e.Codigo, "UQ__Producto__06370DACF883073B").IsUnique();

            entity.Property(e => e.ProductoId).HasColumnName("ProductoID");
            entity.Property(e => e.CategoriaProductoId).HasColumnName("CategoriaProductoID");
            entity.Property(e => e.Codigo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PorcentajeComision)
                .HasDefaultValue(1000m)
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.PrecioCompra).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PrecioVenta).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProveedorId).HasColumnName("ProveedorID");
            entity.Property(e => e.StockMinimo).HasDefaultValue(5);

            entity.HasOne(d => d.CategoriaProducto).WithMany(p => p.Productos)
                .HasForeignKey(d => d.CategoriaProductoId)
                .HasConstraintName("FK__Productos__Categ__787EE5A0");

            entity.HasOne(d => d.Proveedor).WithMany(p => p.Productos)
                .HasForeignKey(d => d.ProveedorId)
                .HasConstraintName("FK__Productos__Prove__797309D9");
        });

        modelBuilder.Entity<Proveedore>(entity =>
        {
            entity.HasKey(e => e.ProveedorId).HasName("PK__Proveedo__61266BB99DC3BE33");

            entity.HasIndex(e => e.Ruc, "UQ__Proveedo__CAF3326B8371CDD6").IsUnique();

            entity.Property(e => e.ProveedorId).HasColumnName("ProveedorID");
            entity.Property(e => e.Direccion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PersonaContacto)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RazonSocial)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Ruc)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("RUC");
            entity.Property(e => e.Telefono)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.ServicioId).HasName("PK__Servicio__D5AEEC22B593214C");

            entity.Property(e => e.ServicioId).HasColumnName("ServicioID");
            entity.Property(e => e.CategoriaServicioId).HasColumnName("CategoriaServicioID");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PorcentajeComision)
                .HasDefaultValue(5000m)
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.CategoriaServicio).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.CategoriaServicioId)
                .HasConstraintName("FK__Servicios__Categ__48CFD27E");
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.VentaId).HasName("PK__Ventas__5B41514C54F68E82");

            entity.Property(e => e.VentaId).HasColumnName("VentaID");
            entity.Property(e => e.ClienteId).HasColumnName("ClienteID");
            entity.Property(e => e.EmpleadoId).HasColumnName("EmpleadoID");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("COMPLETADA");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaVenta)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Igv)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("IGV");
            entity.Property(e => e.MetodoPago)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.NumeroOperacion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Observaciones)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.SubTotal).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Venta)
                .HasForeignKey(d => d.ClienteId)
                .HasConstraintName("FK__Ventas__ClienteI__09A971A2");

            entity.HasOne(d => d.Empleado).WithMany(p => p.Venta)
                .HasForeignKey(d => d.EmpleadoId)
                .HasConstraintName("FK__Ventas__Empleado__0A9D95DB");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
