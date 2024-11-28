using System;
using System.Collections.Generic;

namespace Backend_Api.Models;

public partial class Compra
{
    public int CompraId { get; set; }

    public int ProveedorId { get; set; }

    public DateTime FechaCompra { get; set; }

    public string? NumeroFactura { get; set; }

    public decimal SubTotal { get; set; }

    public decimal Igv { get; set; }

    public decimal Total { get; set; }

    public string Estado { get; set; } = null!;

    public string? Observaciones { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual ICollection<DetalleCompra> DetalleCompras { get; set; } = new List<DetalleCompra>();

    public virtual Proveedore Proveedor { get; set; } = null!;
}
