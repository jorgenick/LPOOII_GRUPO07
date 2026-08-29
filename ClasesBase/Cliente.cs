using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClasesBase
{
    class Cliente
    {
        public string DNI { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }

        public Cliente(string dni, string apellido, string nombre, string direccion)
        {
            this.DNI = dni;
            this.Apellido = apellido;
            this.Nombre = nombre;
            this.Direccion = direccion;

        }
    }
}
