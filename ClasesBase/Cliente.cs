using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ClasesBase
{
    public class Cliente : INotifyPropertyChanged
    {
        private string DNI;
        public string Dni {
            get { return DNI; }
            set {
                DNI = value;
                NotificarCambio(DNI);
            }
        }


        private string apellido;
        public string Apellido
        {
            get { return apellido; }
            set
            {
                apellido = value;
                NotificarCambio(apellido);
            }
        }


        private string nombre;

        public string Nombre
        {
            get { return nombre; }
            set
            {
                nombre = value;
                NotificarCambio(nombre);
            }
        }

        private string direccion;

        public string Direccion
        {
            get { return direccion; }
            set
            {
                direccion = value;
                NotificarCambio(direccion);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Cliente(string dni, string apellido, string nombre, string direccion)
        {
            this.DNI = dni;
            this.Apellido = apellido;
            this.Nombre = nombre;
            this.Direccion = direccion;

        }

        public Cliente() { }

        public void NotificarCambio(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
