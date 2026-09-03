using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClasesBase
{
    public class Producto
    {
        public string CodProducto { get; set; }
        public string Categoria { get; set; }
        public string Color { get; set; }
        public string Descripcion { get; set; }
        public string Precio { get; set; }


        public Producto(string codProducto, string categoria, string color,string descripcion, string precio)
        {
            this.CodProducto = codProducto;
            this.Categoria = categoria;
            this.Color = color;
            this.Descripcion = descripcion;
            this.Precio = precio;
        }
    }
}
