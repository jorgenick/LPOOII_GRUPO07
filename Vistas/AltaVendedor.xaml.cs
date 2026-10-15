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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;

using ClasesBase;

namespace Vistas
{
    public partial class AltaVendedor : Window
    {
        private Vendedor oVendedor;
        private ObservableCollection<Vendedor> listaVendedores;
        private ICollectionView vista;
        private bool modoModificar = false;

        public AltaVendedor()
        {
            InitializeComponent();
            CargarVendedores();
            HabilitarEdicion(false);
        }


        //Cargar Vendedores,

        private void CargarVendedores()
        {
            try
            {
                listaVendedores = TrabajarVendedores.TraerColeccionVendedores();
                vista = CollectionViewSource.GetDefaultView(listaVendedores);
                this.DataContext = listaVendedores;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("No se pudieron cargar los vendedores: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PosicionarEn(string legajo)
        {
            Vendedor encontrado = listaVendedores.FirstOrDefault(v => v.Legajo == legajo);
            if (encontrado != null)
                vista.MoveCurrentTo(encontrado);
        }

        private bool DatosCompletos()
        {
            return !string.IsNullOrWhiteSpace(oVendedor.Legajo)
                && !string.IsNullOrWhiteSpace(oVendedor.Apellido)
                && !string.IsNullOrWhiteSpace(oVendedor.Nombre);
        }

        //me ahorra el trabajo de habilitar a cada rato

        private void HabilitarEdicion(bool editando)
        {
            txtLegajo.IsEnabled = editando;
            txtApellido.IsEnabled = editando;
            txtNombre.IsEnabled = editando;

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

        //ABM

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            oVendedor = new Vendedor();
            this.DataContext = oVendedor;
            modoModificar = false;
            HabilitarEdicion(true);
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (vista == null || vista.CurrentItem == null)
            {
                MessageBox.Show("No hay un vendedor seleccionado.", "Modificar",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            oVendedor = (Vendedor)vista.CurrentItem;
            modoModificar = true;
            HabilitarEdicion(true);
            txtLegajo.IsEnabled = false; // clave primaria: no se edita
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (vista == null || vista.CurrentItem == null)
            {
                MessageBox.Show("No hay un vendedor seleccionado.", "Eliminar",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Vendedor seleccionado = (Vendedor)vista.CurrentItem;

            MessageBoxResult confirmacion = MessageBox.Show(
                "¿Eliminar al vendedor " + seleccionado.Apellido + ", " + seleccionado.Nombre + "?",
                "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (confirmacion != MessageBoxResult.Yes)
                return;

            try
            {
                int filas = TrabajarVendedores.EliminarVendedor(seleccionado.Legajo);
                if (filas == 1)
                    MessageBox.Show("Vendedor eliminado.", "Baja",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("El vendedor ya no existe en la base.", "Baja",
                        MessageBoxButton.OK, MessageBoxImage.Warning);

                CargarVendedores();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                    MessageBox.Show("No se puede eliminar: el vendedor tiene ventas registradas.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("Error al eliminar: " + ex.Message, "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (!DatosCompletos())
            {
                MessageBox.Show("Todos los campos son obligatorios.", "Datos Invalidos",
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
                    ? TrabajarVendedores.ModificarVendedor(oVendedor)
                    : TrabajarVendedores.InsertarVendedor(oVendedor);

                if (filas == 1)
                {
                    MessageBox.Show(modoModificar ? "Vendedor modificado." : "Vendedor guardado correctamente.",
                        "Guardar", MessageBoxButton.OK, MessageBoxImage.Information);

                    string legajo = oVendedor.Legajo;
                    modoModificar = false;
                    CargarVendedores();
                    PosicionarEn(legajo);
                    HabilitarEdicion(false);
                }
                else
                {
                    MessageBox.Show("No se encontró el vendedor a modificar.", "Guardar",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                    MessageBox.Show("Ya existe un vendedor con ese legajo.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("Error al guardar: " + ex.Message, "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            modoModificar = false;
            CargarVendedores();
            HabilitarEdicion(false);

        }

        //--------Navegacion

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
    }
}