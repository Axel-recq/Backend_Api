using System;
using System.Collections.Generic;

namespace Backend_Api.Models;

public partial class Kit
{
    public int KitId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal PrecioVenta { get; set; }

    public bool Estado { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual ICollection<DetalleKit> DetalleKits { get; set; } = new List<DetalleKit>();

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();

    public virtual ICollection<DetalleVentasProducto> DetalleVentasProductos { get; set; } = new List<DetalleVentasProducto>();
}
