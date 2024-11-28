using System;
using System.Collections.Generic;

namespace Backend_Api.Models;

public partial class Cliente
{
    public int ClienteId { get; set; }

    public string Dni { get; set; } = null!;

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    public string? Observaciones { get; set; }

    public string NivelFidelizacion { get; set; } = null!;

    public int PuntosAcumulados { get; set; }

    public bool Estado { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
