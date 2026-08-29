using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClasesBase
{
    class Venta
    {
        public int NroFactura { get; set; }
        public string FechaFactura { get; set; }
        public string Legajo { get; set; }
        public string DNI { get; set; }
        public string CodigoProducto { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public decimal Importe { get; set; }

        public Venta(int nroFactura, string fechaFactura, string legajo, string dni, string codigoProducto, decimal precio, int cantidad, decimal importe)
        {
            this.NroFactura = nroFactura;
            this.FechaFactura = fechaFactura;
            this.Legajo = legajo;
            this.DNI = dni;
            this.CodigoProducto = codigoProducto;
            this.Precio = precio;
            this.Cantidad = cantidad;
            this.Importe = importe;
        }
    
    }
}
