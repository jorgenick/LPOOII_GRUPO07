using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ClasesBase.Entidades
{
    public class Proveedor : IDataErrorInfo
    {
        public string CUIT { get; set; }
        public string RazonSocial { get; set; }
        public string Domicilio { get; set; }
        public string Telefono { get; set; }

        public Proveedor()
        {
        }

        public Proveedor(string cuit, string razonSocial, string domicilio, string telefono)
        {
            this.CUIT = cuit;
            this.RazonSocial = razonSocial;
            this.Domicilio = domicilio;
            this.Telefono = telefono;
        }


        public string Error
        {
            get { return null; }
        }

        public string this[string columnName]
        {
            get
            {
                string resultado = null;

                switch (columnName)
                {
                    case "CUIT":
                        if (string.IsNullOrWhiteSpace(CUIT))
                            resultado = "El CUIT es obligatorio";
                        break;
                    case "RazonSocial":
                        if (string.IsNullOrWhiteSpace(RazonSocial))
                            resultado = "La Razón Social es obligatoria";
                        break;
                    case "Domicilio":
                        if (string.IsNullOrWhiteSpace(Domicilio))
                            resultado = "El domicilio es obligatorio";
                        break;
                    case "Telefono":
                        if (string.IsNullOrWhiteSpace(Telefono))
                            resultado = "El teléfono es obligatorio";
                        break;
                }

                return resultado;
            }
        }

    }
}
