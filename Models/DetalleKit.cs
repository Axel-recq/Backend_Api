using System;
using System.Collections.Generic;

namespace Backend_Api.Models;

public partial class DetalleKit
{
    public int DetalleKitId { get; set; }

    public int KitId { get; set; }

    public int ProductoId { get; set; }

    public int Cantidad { get; set; }

    public virtual Kit Kit { get; set; } = null!;

    public virtual Producto Producto { get; set; } = null!;
}
