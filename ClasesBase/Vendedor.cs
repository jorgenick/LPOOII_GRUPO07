using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClasesBase
{
    public class Vendedor
    {
        public string Legajo { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }


        public Vendedor(string legajo, string apellido, string nombre)
        {
            this.Legajo = legajo;
            this.Apellido = apellido;
            this.Nombre = nombre;
        }

    }
}
