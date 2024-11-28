using System;
using System.Collections.Generic;

namespace Backend_Api.Models;

public partial class Producto
{
    public int ProductoId { get; set; }

    public int CategoriaProductoId { get; set; }

    public int ProveedorId { get; set; }

    public string Codigo { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal PrecioCompra { get; set; }

    public decimal PrecioVenta { get; set; }

    public int Stock { get; set; }

    public int StockMinimo { get; set; }

    public decimal PorcentajeComision { get; set; }

    public bool Estado { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual CategoriasProducto CategoriaProducto { get; set; } = null!;

    public virtual ICollection<DetalleCompra> DetalleCompras { get; set; } = new List<DetalleCompra>();

    public virtual ICollection<DetalleKit> DetalleKits { get; set; } = new List<DetalleKit>();

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();

    public virtual ICollection<DetalleVentasProducto> DetalleVentasProductos { get; set; } = new List<DetalleVentasProducto>();

    public virtual Proveedore Proveedor { get; set; } = null!;
}
