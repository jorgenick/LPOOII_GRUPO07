using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
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
    public partial class AltaProveedor : Window
    {
        private Proveedor oProveedor;
        private ObservableCollection<Proveedor> listaProveedores;
        private ICollectionView vista;
        private bool modoModificar = false;

        public AltaProveedor()
        {
            InitializeComponent();
            CargarProveedores();
            HabilitarEdicion(false);
        }

        private void CargarProveedores()
        {
            try
            {
                listaProveedores = TrabajarProveedores.TraerColeccionProveedores();
                vista = CollectionViewSource.GetDefaultView(listaProveedores);
                this.DataContext = listaProveedores;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("No se pudieron cargar los proveedores: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PosicionarEn(string cuit)
        {
            Proveedor encontrado = listaProveedores.FirstOrDefault(p => p.CUIT == cuit);
            if (encontrado != null)
                vista.MoveCurrentTo(encontrado);
        }

        private void HabilitarEdicion(bool editando)
        {
            txtCUIT.IsEnabled = editando;
            txtRazonSocial.IsEnabled = editando;
            txtDomicilio.IsEnabled = editando;
            txtTelefono.IsEnabled = editando;

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

        // -------------------- ABM --------------------

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            oProveedor = new Proveedor();
            this.DataContext = oProveedor;
            modoModificar = false;
            HabilitarEdicion(true);
            txtCUIT.Focus();
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (vista == null || vista.CurrentItem == null)
            {
                MessageBox.Show("No hay un proveedor seleccionado.", "Modificar",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            oProveedor = (Proveedor)vista.CurrentItem;
            modoModificar = true;
            HabilitarEdicion(true);
            txtCUIT.IsEnabled = false;
            txtRazonSocial.Focus();
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (vista == null || vista.CurrentItem == null)
            {
                MessageBox.Show("No hay un proveedor seleccionado.", "Eliminar",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Proveedor seleccionado = (Proveedor)vista.CurrentItem;

            MessageBoxResult confirmacion = MessageBox.Show(
                "¿Eliminar el proveedor " + seleccionado.RazonSocial + " (" + seleccionado.CUIT + ")?",
                "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (confirmacion != MessageBoxResult.Yes)
                return;

            try
            {
                int filas = TrabajarProveedores.EliminarProveedor(seleccionado.CUIT);
                if (filas == 1)
                    MessageBox.Show("Proveedor eliminado.", "Baja",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("El proveedor ya no existe en la base.", "Baja",
                        MessageBoxButton.OK, MessageBoxImage.Warning);

                CargarProveedores();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                    MessageBox.Show("No se puede eliminar: el proveedor está asociado a otros registros.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("Error al eliminar: " + ex.Message, "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (Validation.GetHasError(txtCUIT) || Validation.GetHasError(txtRazonSocial) ||
                Validation.GetHasError(txtDomicilio) || Validation.GetHasError(txtTelefono))
            {
                MessageBox.Show("Error al Guardar, revise los campos en rojo.", "Datos Inválidos",
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
                    ? TrabajarProveedores.ModificarProveedor(oProveedor)
                    : TrabajarProveedores.InsertarProveedor(oProveedor);

                if (filas == 1)
                {
                    MessageBox.Show(modoModificar ? "Proveedor modificado." : "Proveedor guardado correctamente.",
                        "Guardar", MessageBoxButton.OK, MessageBoxImage.Information);

                    string cuit = oProveedor.CUIT;
                    modoModificar = false;
                    CargarProveedores();
                    PosicionarEn(cuit);
                    HabilitarEdicion(false);
                }
                else
                {
                    MessageBox.Show("No se encontró el proveedor a modificar.", "Guardar",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                    MessageBox.Show("Ya existe un proveedor con ese CUIT.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("Error al guardar: " + ex.Message, "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            modoModificar = false;
            CargarProveedores();
            HabilitarEdicion(false);
        }

        // -------------------- NAVEGACION BOTONES --------------------

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