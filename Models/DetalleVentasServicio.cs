using System;
using System.Collections.Generic;

namespace Backend_Api.Models;

public partial class DetalleVentasServicio
{
    public int DetalleVentaServicioId { get; set; }

    public int VentaId { get; set; }

    public int ServicioId { get; set; }

    public int EmpleadoId { get; set; }

    public decimal Precio { get; set; }

    public decimal? PorcentajeComision { get; set; }

    public decimal? MontoComision { get; set; }

    public virtual Empleado Empleado { get; set; } = null!;

    public virtual Servicio Servicio { get; set; } = null!;

    public virtual Venta Venta { get; set; } = null!;
}
