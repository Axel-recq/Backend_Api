using System;
using System.Collections.Generic;

namespace Backend_Api.Models;

public partial class Empleado
{
    public int EmpleadoId { get; set; }

    public string Dni { get; set; } = null!;

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public DateOnly FechaContratacion { get; set; }

    public decimal SueldoBase { get; set; }

    public bool Estado { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual ICollection<DetalleVentasServicio> DetalleVentasServicios { get; set; } = new List<DetalleVentasServicio>();

    public virtual ICollection<PagosEmpleado> PagosEmpleados { get; set; } = new List<PagosEmpleado>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();

    public static implicit operator Empleado(Empleado v)
    {
        throw new NotImplementedException();
    }
}
