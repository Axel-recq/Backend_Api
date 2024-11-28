using System;
using System.Collections.Generic;

namespace Backend_Api.Models;

public partial class Servicio
{
    public int ServicioId { get; set; }

    public int CategoriaServicioId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public int? DuracionMinutos { get; set; }

    public decimal PorcentajeComision { get; set; }

    public bool Estado { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual CategoriasServicio CategoriaServicio { get; set; } = null!;

    public virtual ICollection<DetalleVentasServicio> DetalleVentasServicios { get; set; } = new List<DetalleVentasServicio>();
    public DateTime FechaServicio { get; internal set; }
}
