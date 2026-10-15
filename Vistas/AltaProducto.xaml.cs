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
    public partial class AltaProducto : Window
    {
        private Producto oProducto;

        public AltaProducto()
        {
            InitializeComponent();
        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            oProducto = new Producto();
            this.DataContext = oProducto;

            txtCodProducto.Text = "";
            txtCategoria.Text = "";
            txtColor.Text = "";
            txtDescripcion.Text = "";
            txtPrecio.Text = "";

            txtCodProducto.IsEnabled = true;
            txtCategoria.IsEnabled = true;
            txtColor.IsEnabled = true;
            txtDescripcion.IsEnabled = true;
            txtPrecio.IsEnabled = true;

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
            if (Validation.GetHasError(txtCodProducto) || Validation.GetHasError(txtCategoria) || Validation.GetHasError(txtColor) || Validation.GetHasError(txtDescripcion) || Validation.GetHasError(txtPrecio))
                   {
                       MessageBox.Show("Error al Guardar revise los campos en rojo", "Datos Invalidos", MessageBoxButton.OK, MessageBoxImage.Warning);
                       return;
                   }
  
            MessageBoxResult confirmacion = MessageBox.Show(
                "¿Confirma guardar los datos ingresados?",
                "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirmacion == MessageBoxResult.Yes)
            {
                
                MessageBox.Show(
                    "Cód. Producto: " + oProducto.CodProducto +
                    "\nCategoría: " + oProducto.Categoria +
                    "\nColor: " + oProducto.Color +
                    "\nDescripción: " + oProducto.Descripcion +
                    "\nPrecio: " + oProducto.Precio,
                    "Datos almacenados en el objeto");

                txtCodProducto.IsEnabled = false;
                txtCategoria.IsEnabled = false;
                txtColor.IsEnabled = false;
                txtDescripcion.IsEnabled = false;
                txtPrecio.IsEnabled = false;

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
            oProducto = null;
            this.DataContext = oProducto;

            txtCodProducto.IsEnabled = false;
            txtCategoria.IsEnabled = false;
            txtColor.IsEnabled = false;
            txtDescripcion.IsEnabled = false;
            txtPrecio.IsEnabled = false;

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

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            ListaDeEstados test = new ListaDeEstados();
            test.Show();
        }

        private void btnVerProductos_Click(object sender, RoutedEventArgs e)
        {
            ListadoProductos listado = new ListadoProductos();
            listado.ShowDialog();
        }
    }
}