namespace Backend_Api.dtos
{
    public class PagoEmpleadoDTO
    {
        public int EmpleadoID { get; set; }
        public decimal SueldoBase { get; set; }
        public decimal ComisionVenta { get; set; }
        public decimal TotalVentas { get; set; } // Total de ventas realizadas por el empleado
        public decimal TotalComisiones { get; set; } // Total de comisiones por productos o servicios vendidos
        public decimal TotalPago { get; set; }
        public DateTime FechaPago { get; set; }
        public string Estado { get; set; } // Ejemplo: "PENDIENTE", "PAGADO"
    }
}
