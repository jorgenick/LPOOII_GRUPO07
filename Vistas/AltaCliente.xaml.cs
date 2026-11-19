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
using ClasesBase.Dao;
using ClasesBase.Entidades;

namespace Vistas
{
    public partial class AltaCliente : Window
    {
        private Cliente oCliente;
        private List<Cliente> listaClientes;
        private int indiceActual;
        private bool modoModificar = false;

        public AltaCliente()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listaClientes = TrabajarClientes.TraerClientes().ToList();

            if (listaClientes.Count > 0)
            {
                indiceActual = 0;
                MostrarClienteActual();
            }
        }

        private void MostrarClienteActual()
        {
            Cliente c = listaClientes[indiceActual];

            txtDNI.Text = c.Dni;
            txtApellido.Text = c.Apellido;
            txtNombre.Text = c.Nombre;
            txtDireccion.Text = c.Direccion;
        }

        private void btnPrimero_Click(object sender, RoutedEventArgs e)
        {
            if (listaClientes.Count == 0) return;
            indiceActual = 0;
            MostrarClienteActual();
        }

        private void btnAnterior_Click(object sender, RoutedEventArgs e)
        {
            if (indiceActual > 0)
            {
                indiceActual--;
                MostrarClienteActual();
            }
        }

        private void btnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            if (indiceActual < listaClientes.Count - 1)
            {
                indiceActual++;
                MostrarClienteActual();
            }
        }

        private void btnUltimo_Click(object sender, RoutedEventArgs e)
        {
            if (listaClientes.Count == 0) return;
            indiceActual = listaClientes.Count - 1;
            MostrarClienteActual();
        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            modoModificar = false;

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

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (listaClientes.Count == 0) return;

            modoModificar = true;

            txtDNI.IsEnabled = false; // el DNI identifica la fila: no se edita
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

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (listaClientes.Count == 0) return;

            Cliente seleccionado = listaClientes[indiceActual];

            MessageBoxResult confirmacion = MessageBox.Show(
                "¿Eliminar al cliente " + seleccionado.Apellido + "?",
                "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (confirmacion != MessageBoxResult.Yes)
                return;

            try
            {
                TrabajarClientes.EliminarCliente(seleccionado.Dni);
                MessageBox.Show("Cliente eliminado.", "Baja", MessageBoxButton.OK, MessageBoxImage.Information);

                listaClientes = TrabajarClientes.TraerClientes().ToList();
                if (listaClientes.Count > 0)
                {
                    indiceActual = 0;
                    MostrarClienteActual();
                }
                else
                {
                    txtDNI.Text = "";
                    txtApellido.Text = "";
                    txtNombre.Text = "";
                    txtDireccion.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

                    if (modoModificar)
                        TrabajarClientes.ModificarCliente(oCliente);
                    else
                        TrabajarClientes.AgregarCliente(oCliente);

                    MessageBox.Show(
                        "DNI: " + oCliente.Dni +
                        "\nApellido: " + oCliente.Apellido +
                        "\nNombre: " + oCliente.Nombre +
                        "\nDirección: " + oCliente.Direccion,
                        modoModificar ? "Cliente modificado con éxito" : "Cliente guardado en BD con éxito",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    modoModificar = false;

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
            modoModificar = false;

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

            if (listaClientes.Count > 0)
                MostrarClienteActual();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}