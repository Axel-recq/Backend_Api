using System;
using System.Collections.Generic;

namespace Backend_Api.Models;

public partial class DetalleVenta
{
    public int DetalleVentaId { get; set; }

    public int VentaId { get; set; }

    public int? ProductoId { get; set; }

    public int? KitId { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal SubTotal { get; set; }

    public virtual Kit? Kit { get; set; }

    public virtual Producto? Producto { get; set; }

    public virtual Venta Venta { get; set; } = null!;
}
