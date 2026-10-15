using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ClasesBase
{
    public class Producto : IDataErrorInfo
    {
        public string CodProducto { get; set; }
        public string Categoria { get; set; }
        public string Color { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }

        public Producto()
        {
        }


        public Producto(string codProducto, string categoria, string color,string descripcion, decimal precio)
        {
            this.CodProducto = codProducto;
            this.Categoria = categoria;
            this.Color = color;
            this.Descripcion = descripcion;
            this.Precio = precio;
        }

        // IDataErrorInfo: simplemente responde mensajes de errores segun unas reglas que le configure (devuelve el mensaje de error de cada propiedad según las reglas de validación) 
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
                    case "CodProducto":
                        if (string.IsNullOrWhiteSpace(CodProducto))
                        {
                            resultado = "El codigo de producto es obligatorio ";
                        }
                        break;

                    case "Categoria":
                        if (string.IsNullOrWhiteSpace(Categoria))
                        {
                            resultado = "La categoria es obligatoria ";
                        }
                        break;
                    case "Color":
                        if (string.IsNullOrWhiteSpace(Color))
                        {
                            resultado = "El color es obligatorio ";
                        }
                        break;
                    case "Descripcion":
                        if (string.IsNullOrWhiteSpace(Descripcion))
                        {
                            resultado = "La descripcion es obligatoria ";
                        }
                        break;
                    case "Precio":
                        if (Precio<=0)
                        {
                            resultado = "El precio debe ser mayor a 0 ";
                        }
                        break;
                }

                return resultado;
            }
        }
    }
}
