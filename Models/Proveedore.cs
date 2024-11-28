using System;
using System.Collections.Generic;

namespace Backend_Api.Models;

public partial class Proveedore
{
    public int ProveedorId { get; set; }

    public string Ruc { get; set; } = null!;

    public string RazonSocial { get; set; } = null!;

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public string? PersonaContacto { get; set; }

    public bool? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
