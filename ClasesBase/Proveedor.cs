using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClasesBase
{
    class Proveedor
    {
        public string CUIT { get; set; }
        public string RazonSocial { get; set; }
        public string Domicilio { get; set; }
        public string Telefono { get; set; }

        public Proveedor(string cuit, string razonSocial, string domicilio, string telefono)
        {
            this.CUIT = cuit;
            this.RazonSocial = razonSocial;
            this.Domicilio = domicilio;
            this.Telefono = telefono;
        }

    }
}
