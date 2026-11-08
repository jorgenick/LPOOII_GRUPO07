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
using System.Data;
using ClasesBase;

namespace Vistas
{
    public partial class AltaVenta : Window
    {
        public AltaVenta()
        {
            InitializeComponent();
            dpFecha.SelectedDate = DateTime.Now;

            cbVendedor.ItemsSource = TrabajarVendedores.TraerColeccionVendedores();
            cbVendedor.DisplayMemberPath = "Apellido";
            cbVendedor.SelectedValuePath = "Legajo";

            cbCliente.ItemsSource = TrabajarClientes.TraerClientesDataTable().DefaultView;
            cbCliente.DisplayMemberPath = "Apellido";
            cbCliente.SelectedValuePath = "ClienteDNI";

            cbProducto.ItemsSource = TrabajarProductos.TraerColeccionProductos();
            cbProducto.DisplayMemberPath = "Descripcion";
            cbProducto.SelectedValuePath = "CodProducto";
        }

        private void cbProducto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Producto p = cbProducto.SelectedItem as Producto;
            if (p != null)
            {
                txtPrecio.Text = p.Precio.ToString();
            }
        }

        private void btnConfirmar_Click(object sender, RoutedEventArgs e)
        {
            int nroFactura;
            if (!int.TryParse(txtNroFactura.Text, out nroFactura))
            {
                MessageBox.Show("Número de venta inválido.");
                return;
            }

            if (cbVendedor.SelectedValue == null || cbCliente.SelectedValue == null || cbProducto.SelectedValue == null)
            {
                MessageBox.Show("Completar todos los campos.");
                return;
            }

            int cantidad;
            if (!int.TryParse(txtCantidad.Text, out cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Cantidad inválida.");
                return;
            }

            decimal precio = decimal.Parse(txtPrecio.Text);
            DateTime fecha = dpFecha.SelectedDate ?? DateTime.Now;

            Venta v = new Venta(
                nroFactura,
                fecha.ToString("yyyy-MM-dd HH:mm:ss"),
                (string)cbVendedor.SelectedValue,
                (string)cbCliente.SelectedValue,
                (string)cbProducto.SelectedValue,
                precio,
                cantidad,
                precio * cantidad
            );

            TrabajarVentas.InsertarVenta(v);

            var vendedorSel = (Vendedor)cbVendedor.SelectedItem;
            var productoSel = (Producto)cbProducto.SelectedItem;
            var clienteRow = ((DataRowView)cbCliente.SelectedItem).Row;

            string clienteApellido = clienteRow["Apellido"].ToString();
            string clienteNombre = clienteRow["Nombre"].ToString();

            ComprobanteVenta vc = new ComprobanteVenta(
                v, fecha, clienteApellido, clienteNombre,
                productoSel.Descripcion, productoSel.Color, vendedorSel.Nombre
            );
            vc.ShowDialog();

            this.Close();
        }
    }
}