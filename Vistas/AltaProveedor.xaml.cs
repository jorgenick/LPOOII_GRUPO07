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
    public partial class AltaProveedor : Window
    {
        private Proveedor oProveedor;

        public AltaProveedor()
        {
            InitializeComponent();
        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            // Limpiar y habilitar textboxes
            txtCUIT.Text = "";
            txtRazonSocial.Text = "";
            txtDomicilio.Text = "";
            txtTelefono.Text = "";

            txtCUIT.IsEnabled = true;
            txtRazonSocial.IsEnabled = true;
            txtDomicilio.IsEnabled = true;
            txtTelefono.IsEnabled = true;

            // Habilitar guardar y cancelar
            btnGuardar.IsEnabled = true;
            btnCancelar.IsEnabled = true;

            // Deshabilitar el resto
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
                oProveedor = new Proveedor(
                    txtCUIT.Text,
                    txtRazonSocial.Text,
                    txtDomicilio.Text,
                    txtTelefono.Text);

                MessageBox.Show(
                    "CUIT: " + oProveedor.CUIT +
                    "\nRazón Social: " + oProveedor.RazonSocial +
                    "\nDomicilio: " + oProveedor.Domicilio +
                    "\nTeléfono: " + oProveedor.Telefono,
                    "Datos almacenados en el objeto");

                // Deshabilitar textboxes
                txtCUIT.IsEnabled = false;
                txtRazonSocial.IsEnabled = false;
                txtDomicilio.IsEnabled = false;
                txtTelefono.IsEnabled = false;

                // Deshabilitar guardar y cancelar
                btnGuardar.IsEnabled = false;
                btnCancelar.IsEnabled = false;

                // Habilitar el resto
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
            // Limpiar y deshabilitar textboxes
            txtCUIT.Text = "";
            txtRazonSocial.Text = "";
            txtDomicilio.Text = "";
            txtTelefono.Text = "";

            txtCUIT.IsEnabled = false;
            txtRazonSocial.IsEnabled = false;
            txtDomicilio.IsEnabled = false;
            txtTelefono.IsEnabled = false;

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