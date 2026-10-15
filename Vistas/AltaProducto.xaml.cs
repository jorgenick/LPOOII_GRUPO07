using System;
using System.Collections.Generic;
using System.Collections.ObjectModel; //
using System.ComponentModel; //
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
using System.Data.SqlClient; //Importa el espacio de nombres donde viven las clases para hablar con SQL Server
using ClasesBase;

namespace Vistas
{
    public partial class AltaProducto : Window
    {
        private Producto oProducto;
        private ObservableCollection<Producto> listaProductos;
        private ICollectionView vista;
        private bool modoModificar = false;

        public AltaProducto()
        {
            InitializeComponent();
            CargarProductos();
            HabilitarEdicion(false);

        }

        // Carga y estado de la ventan

        private void CargarProductos()
        {
            try
            {
                listaProductos = TrabajarProductos.TraerColeccionProductos();
                vista = CollectionViewSource.GetDefaultView(listaProductos);
                this.DataContext = listaProductos;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("No se pudieron cargar los productos: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PosicionarEn(string codProducto)
        {
            Producto encontrado = listaProductos.FirstOrDefault(p => p.CodProducto == codProducto);
            if (encontrado != null)
                vista.MoveCurrentTo(encontrado);
        }

        private void HabilitarEdicion(bool editando) //me ahorra el trabajo de habilitar a cada rato 
        {
            txtCodProducto.IsEnabled = editando;
            txtCategoria.IsEnabled = editando;
            txtColor.IsEnabled = editando;
            txtDescripcion.IsEnabled = editando;
            txtPrecio.IsEnabled = editando;

            btnGuardar.IsEnabled = editando;
            btnCancelar.IsEnabled = editando;

            btnNuevo.IsEnabled = !editando;
            btnModificar.IsEnabled = !editando;
            btnEliminar.IsEnabled = !editando;
            btnPrimero.IsEnabled = !editando;
            btnAnterior.IsEnabled = !editando;
            btnSiguiente.IsEnabled = !editando;
            btnUltimo.IsEnabled = !editando;
        }



        // --------------------ABM

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            oProducto = new Producto();
            this.DataContext = oProducto;
            modoModificar = false;
            HabilitarEdicion(true);
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (vista == null || vista.CurrentItem == null)
            {
                MessageBox.Show("No hay un producto seleccionado.", "Modificar",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            oProducto = (Producto)vista.CurrentItem;
            modoModificar = true;
            HabilitarEdicion(true);
            txtCodProducto.IsEnabled = false; // el código identifica la fila: no se edita
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (vista == null || vista.CurrentItem == null)
            {
                MessageBox.Show("No hay un producto seleccionado.", "Eliminar",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Producto seleccionado = (Producto)vista.CurrentItem;

            MessageBoxResult confirmacion = MessageBox.Show(
                "¿Eliminar el producto " + seleccionado.CodProducto + "?",
                "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (confirmacion != MessageBoxResult.Yes)
                return;

            try
            {
                int filas = TrabajarProductos.EliminarProducto(seleccionado.CodProducto);
                if (filas == 1)
                    MessageBox.Show("Producto eliminado.", "Baja",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("El producto ya no existe en la base.", "Baja",
                        MessageBoxButton.OK, MessageBoxImage.Warning);

                CargarProductos();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                    MessageBox.Show("No se puede eliminar: el producto está usado en otros registros.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("Error al eliminar: " + ex.Message, "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (Validation.GetHasError(txtCodProducto) || Validation.GetHasError(txtCategoria) ||
                Validation.GetHasError(txtColor) || Validation.GetHasError(txtDescripcion) ||
                Validation.GetHasError(txtPrecio))
            {
                MessageBox.Show("Error al Guardar revise los campos en rojo", "Datos Invalidos",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult confirmacion = MessageBox.Show(
                "¿Confirma guardar los datos ingresados?",
                "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirmacion != MessageBoxResult.Yes)
                return;

            try
            {
                int filas = modoModificar
                    ? TrabajarProductos.ModificarProducto(oProducto)
                    : TrabajarProductos.InsertarProducto(oProducto);

                if (filas == 1)
                {
                    MessageBox.Show(modoModificar ? "Producto modificado." : "Producto guardado correctamente.",
                        "Guardar", MessageBoxButton.OK, MessageBoxImage.Information);

                    string cod = oProducto.CodProducto;
                    modoModificar = false;
                    CargarProductos();
                    PosicionarEn(cod);
                    HabilitarEdicion(false);
                }
                else
                {
                    MessageBox.Show("No se encontró el producto a modificar.", "Guardar",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                    MessageBox.Show("Ya existe un producto con ese código.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("Error al guardar: " + ex.Message, "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
            }
        } // btnGuardar



        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            // Recarga desde la base: descarta cambios a medio editar
            modoModificar = false;
            CargarProductos();
            HabilitarEdicion(false);
        }

        // NAVEGAR


        private void btnPrimero_Click(object sender, RoutedEventArgs e)
        {
            vista.MoveCurrentToFirst();
        }

        private void btnAnterior_Click(object sender, RoutedEventArgs e)
        {
            vista.MoveCurrentToPrevious();
            if (vista.IsCurrentBeforeFirst)
                vista.MoveCurrentToLast();
        }

        private void btnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            vista.MoveCurrentToNext();
            if (vista.IsCurrentAfterLast)
                vista.MoveCurrentToFirst();
        }

        private void btnUltimo_Click(object sender, RoutedEventArgs e)
        {
            vista.MoveCurrentToLast();
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