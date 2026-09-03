using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ClasesBase;

namespace Vistas
{
    public partial class AltaVendedor : Window
    {
        private Vendedor oVendedor;

        public AltaVendedor()
        {
            InitializeComponent();
        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            txtLegajo.Text = "";
            txtApellido.Text = "";
            txtNombre.Text = "";

            txtLegajo.IsEnabled = true;
            txtApellido.IsEnabled = true;
            txtNombre.IsEnabled = true;

            btnGuardar.IsEnabled = true;
            btnCancelar.IsEnabled = true;

            btnNuevo.IsEnabled = false;
            btnModificar.IsEnabled = false;
            btnEliminar.IsEnabled = false;
            btnPrimero.IsEnabled = false;
            btnAnterior.IsEnabled = false;
            btnSiguiente.IsEnabled = false;
            btnUltimo.IsEnabled = false;
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult confirmacion = MessageBox.Show(
                "¿Confirma guardar los datos ingresados?",
                "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirmacion == MessageBoxResult.Yes)
            {
                oVendedor = new Vendedor(
                    txtLegajo.Text,
                    txtApellido.Text,
                    txtNombre.Text);

                MessageBox.Show(
                    "Legajo: " + oVendedor.Legajo +
                    "\nApellido: " + oVendedor.Apellido +
                    "\nNombre: " + oVendedor.Nombre,
                    "Datos almacenados en el objeto");

                txtLegajo.IsEnabled = false;
                txtApellido.IsEnabled = false;
                txtNombre.IsEnabled = false;

                btnGuardar.IsEnabled = false;
                btnCancelar.IsEnabled = false;

                btnNuevo.IsEnabled = true;
                btnModificar.IsEnabled = true;
                btnEliminar.IsEnabled = true;
                btnPrimero.IsEnabled = true;
                btnAnterior.IsEnabled = true;
                btnSiguiente.IsEnabled = true;
                btnUltimo.IsEnabled = true;
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            txtLegajo.Text = "";
            txtApellido.Text = "";
            txtNombre.Text = "";

            txtLegajo.IsEnabled = false;
            txtApellido.IsEnabled = false;
            txtNombre.IsEnabled = false;

            btnGuardar.IsEnabled = false;
            btnCancelar.IsEnabled = false;

            btnNuevo.IsEnabled = true;
            btnModificar.IsEnabled = true;
            btnEliminar.IsEnabled = true;
            btnPrimero.IsEnabled = true;
            btnAnterior.IsEnabled = true;
            btnSiguiente.IsEnabled = true;
            btnUltimo.IsEnabled = true;
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}