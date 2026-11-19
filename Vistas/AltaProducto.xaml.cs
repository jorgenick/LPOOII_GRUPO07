using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Data.SqlClient;
using ClasesBase.Dao;
using ClasesBase.Entidades;
using Microsoft.Win32;
using System.IO;

namespace Vistas
{
    public partial class AltaProducto : Window
    {
        private Producto oProducto;
        private ObservableCollection<Producto> listaProductos;
        private ICollectionView vista;
        private bool modoModificar = false;
        private string nombreArchivoDestino = "";

        public AltaProducto()
        {
            InitializeComponent();
            CargarProductos();
            HabilitarEdicion(false);
        }

        private string ObtenerCarpetaImagenes()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory; // Ej: ...\Vistas\bin\Debug\
            DirectoryInfo dirBin = Directory.GetParent(baseDir.TrimEnd('\\')); // Ej: ...\Vistas\bin

            if (dirBin != null && dirBin.Parent != null)
            {
                string rutaProyecto = dirBin.Parent.FullName; // Carpeta raíz del proyecto Vistas
                if (Directory.Exists(System.IO.Path.Combine(rutaProyecto, "images")))
                {
                    return System.IO.Path.Combine(rutaProyecto, "images", "imagenes-productos");
                }
            }

            return System.IO.Path.Combine(baseDir, "imagenes-productos");
        }

        private void CargarProductos()
        {
            try
            {
                listaProductos = TrabajarProductos.TraerColeccionProductos();
                vista = CollectionViewSource.GetDefaultView(listaProductos);
                this.DataContext = listaProductos;

                if (vista != null)
                {
                    vista.CurrentChanged += (s, e) => MostrarImagenActual();
                }

                MostrarImagenActual();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("No se pudieron cargar los productos: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MostrarImagenActual()
        {
            if (vista == null || vista.CurrentItem == null)
            {
                imgProducto.Source = null;
                return;
            }

            Producto actual = (Producto)vista.CurrentItem;

            if (string.IsNullOrWhiteSpace(actual.NombreArchivoImagen))
            {
                imgProducto.Source = null;
                return;
            }

            string soloNombre = System.IO.Path.GetFileName(actual.NombreArchivoImagen.Trim());
            string ruta = System.IO.Path.Combine(ObtenerCarpetaImagenes(), soloNombre);

            if (File.Exists(ruta))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(ruta, UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                imgProducto.Source = bitmap;
            }
            else
            {
                imgProducto.Source = null;
            }
        }

        private void PosicionarEn(string codProducto)
        {
            Producto encontrado = listaProductos.FirstOrDefault(p => p.CodProducto == codProducto);
            if (encontrado != null)
            {
                vista.MoveCurrentTo(encontrado);
                MostrarImagenActual();
            }
        }

        private void HabilitarEdicion(bool editando)
        {
            txtCodProducto.IsEnabled = false;
            txtCategoria.IsEnabled = editando;
            txtColor.IsEnabled = editando;
            txtDescripcion.IsEnabled = editando;
            txtPrecio.IsEnabled = editando;
            btnSeleccionarImagen.IsEnabled = editando;

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


        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            oProducto = new Producto();
            oProducto.CodProducto = TrabajarProductos.GenerarSiguienteCodigo();
            nombreArchivoDestino = "";
            imgProducto.Source = null;

            this.DataContext = oProducto;
            modoModificar = false;
            HabilitarEdicion(true);
            txtCategoria.Focus();
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
            nombreArchivoDestino = string.IsNullOrWhiteSpace(oProducto.NombreArchivoImagen)
                ? ""
                : System.IO.Path.GetFileName(oProducto.NombreArchivoImagen.Trim());

            modoModificar = true;
            HabilitarEdicion(true);
            txtCodProducto.IsEnabled = false;
            MostrarImagenActual();
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
            if (Validation.GetHasError(txtCategoria) ||
                Validation.GetHasError(txtColor) || Validation.GetHasError(txtDescripcion) ||
                Validation.GetHasError(txtPrecio))
            {
                MessageBox.Show("Error al Guardar revise los campos en rojo", "Datos Invalidos",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            oProducto.NombreArchivoImagen = nombreArchivoDestino;

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
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            modoModificar = false;
            nombreArchivoDestino = "";
            CargarProductos();
            HabilitarEdicion(false);
        }


        private void btnPrimero_Click(object sender, RoutedEventArgs e)
        {
            if (vista == null) return;
            vista.MoveCurrentToFirst();
            MostrarImagenActual();
        }

        private void btnAnterior_Click(object sender, RoutedEventArgs e)
        {
            if (vista == null) return;
            vista.MoveCurrentToPrevious();
            if (vista.IsCurrentBeforeFirst)
                vista.MoveCurrentToLast();
            MostrarImagenActual();
        }

        private void btnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            if (vista == null) return;
            vista.MoveCurrentToNext();
            if (vista.IsCurrentAfterLast)
                vista.MoveCurrentToFirst();
            MostrarImagenActual();
        }

        private void btnUltimo_Click(object sender, RoutedEventArgs e)
        {
            if (vista == null) return;
            vista.MoveCurrentToLast();
            MostrarImagenActual();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            ListaDeEstados test = new ListaDeEstados();
            test.Owner = this;
            test.ShowDialog();
        }

        private void btnVerProductos_Click(object sender, RoutedEventArgs e)
        {
            ListadoProductos listado = new ListadoProductos();
            listado.ShowDialog();
        }

        private void btnSeleccionarImagen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de imagen (*.jpg, *.png)|*.jpg;*.png";

            if (openFileDialog.ShowDialog() == true)
            {
                string rutaOrigen = openFileDialog.FileName;
                string nombreArchivo = System.IO.Path.GetFileName(rutaOrigen);

                string carpetaDestino = ObtenerCarpetaImagenes();

                if (!Directory.Exists(carpetaDestino))
                {
                    Directory.CreateDirectory(carpetaDestino);
                }

                string rutaDestino = System.IO.Path.Combine(carpetaDestino, nombreArchivo);
                File.Copy(rutaOrigen, rutaDestino, true);

                nombreArchivoDestino = nombreArchivo;

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(rutaDestino, UriKind.Absolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                imgProducto.Source = bitmap;
            }
        }
    }
}