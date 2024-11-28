using System;
using System.Collections.Generic;

namespace Backend_Api.Models;

public partial class PagosEmpleado
{
    public int PagoEmpleadoId { get; set; }

    public int EmpleadoId { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFin { get; set; }

    public decimal SueldoBase { get; set; }

    public decimal ComisionServicios { get; set; }

    public decimal ComisionProductos { get; set; }

    public decimal TotalPago { get; set; }

    public string Estado { get; set; } = null!;

    public DateTime FechaPago { get; set; }

    public string Observaciones { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }

    public virtual Empleado Empleado { get; set; } = null!;
}
