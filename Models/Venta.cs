using System;
using System.Collections.Generic;

namespace Backend_Api.Models;

public partial class Venta
{
    public int VentaId { get; set; }

    public int ClienteId { get; set; }

    public int EmpleadoId { get; set; }

    public DateTime FechaVenta { get; set; }

    public decimal SubTotal { get; set; }

    public decimal Igv { get; set; }

    public decimal Total { get; set; }

    public string? MetodoPago { get; set; }

    public string? NumeroOperacion { get; set; }

    public string Estado { get; set; } = null!;

    public string? Observaciones { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();

    public virtual ICollection<DetalleVentasProducto> DetalleVentasProductos { get; set; } = new List<DetalleVentasProducto>();

    public virtual ICollection<DetalleVentasServicio> DetalleVentasServicios { get; set; } = new List<DetalleVentasServicio>();

    public virtual Empleado Empleado { get; set; } = null!;
}
