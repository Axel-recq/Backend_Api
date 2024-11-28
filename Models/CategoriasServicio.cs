using System;
using System.Collections.Generic;

namespace Backend_Api.Models;

public partial class CategoriasServicio
{
    public int CategoriaServicioId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool Estado { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
}
