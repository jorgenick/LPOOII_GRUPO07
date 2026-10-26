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
    public partial class AltaCliente : Window
    {
        private Cliente oCliente;

        public AltaCliente()
        {
            InitializeComponent();
        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            txtDNI.Text = "";
            txtApellido.Text = "";
            txtNombre.Text = "";
            txtDireccion.Text = "";

            txtDNI.IsEnabled = true;
            txtApellido.IsEnabled = true;
            txtNombre.IsEnabled = true;
            txtDireccion.IsEnabled = true;

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
            // Validar que los campos no estén vacíos
            if (string.IsNullOrWhiteSpace(txtDNI.Text) || string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MessageBox.Show("Por favor complete los campos requeridos (DNI y Apellido).", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult confirmacion = MessageBox.Show(
                "¿Confirma guardar los datos ingresados?",
                "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirmacion == MessageBoxResult.Yes)
            {
                try
                {
                    oCliente = new Cliente(
                        txtDNI.Text,
                        txtApellido.Text,
                        txtNombre.Text,
                        txtDireccion.Text);

                    TrabajarClientes.AgregarCliente(oCliente);

                    MessageBox.Show(
                        "DNI: " + oCliente.Dni +
                        "\nApellido: " + oCliente.Apellido +
                        "\nNombre: " + oCliente.Nombre +
                        "\nDirección: " + oCliente.Direccion,
                        "Cliente guardado en BD con éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                    txtDNI.IsEnabled = false;
                    txtApellido.IsEnabled = false;
                    txtNombre.IsEnabled = false;
                    txtDireccion.IsEnabled = false;

                    btnGuardar.IsEnabled = false;
                    btnCancelar.IsEnabled = false;

                    btnNuevo.IsEnabled = true;
                    btnModificar.IsEnabled = true;
                    btnEliminar.IsEnabled = true;
                    btnPrimero.IsEnabled = true;
                    btnAnterior.IsEnabled = true;
                    btnSiguiente.IsEnabled = true;
                    btnUltimo.IsEnabled = true;

                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar en la Base de Datos: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            txtDNI.Text = "";
            txtApellido.Text = "";
            txtNombre.Text = "";
            txtDireccion.Text = "";

            txtDNI.IsEnabled = false;
            txtApellido.IsEnabled = false;
            txtNombre.IsEnabled = false;
            txtDireccion.IsEnabled = false;

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
