using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Controls;
using ClasesBase;

namespace Vistas
{
    public partial class ComprobanteVenta : Window
    {
        private FixedDocument documento;

        public ComprobanteVenta(Venta v, DateTime fecha, string clienteApellido, string clienteNombre,
                                   string productoDescripcion, string productoColor, string vendedorNombre)
        {
            InitializeComponent();
            documento = CrearComprobante(v, fecha, clienteApellido, clienteNombre, productoDescripcion, productoColor, vendedorNombre);
            docViewer.Document = documento;
        }

        private FixedDocument CrearComprobante(Venta v, DateTime fecha, string clienteApellido, string clienteNombre,
                                                string productoDescripcion, string productoColor, string vendedorNombre)
        {
            FixedDocument doc = new FixedDocument();
            FixedPage pagina = new FixedPage { Width = 793, Height = 1122 };

            Grid grid = new Grid { Margin = new Thickness(40) };
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            StackPanel panelNegocio = new StackPanel();
            panelNegocio.Children.Add(new TextBlock { Text = "El Roble", FontSize = 22, FontWeight = FontWeights.Bold });
            panelNegocio.Children.Add(new TextBlock { Text = "-Mueblería-" });
            Grid.SetRow(panelNegocio, 0);
            Grid.SetColumn(panelNegocio, 0);
            grid.Children.Add(panelNegocio);

            StackPanel panelVenta = new StackPanel();
            panelVenta.Children.Add(new TextBlock { Text = "Venta", FontSize = 22, FontWeight = FontWeights.Bold, HorizontalAlignment = HorizontalAlignment.Right });
            panelVenta.Children.Add(new TextBlock { Text = "Número: " + v.NroFactura, HorizontalAlignment = HorizontalAlignment.Right });
            panelVenta.Children.Add(new TextBlock { Text = "Fecha y Hora: " + fecha.ToString("dd/MM/yyyy HH:mm"), HorizontalAlignment = HorizontalAlignment.Right });
            Grid.SetRow(panelVenta, 0);
            Grid.SetColumn(panelVenta, 1);
            grid.Children.Add(panelVenta);

            StackPanel panelCliente = new StackPanel { Margin = new Thickness(0, 30, 0, 0) };
            panelCliente.Children.Add(new TextBlock { Text = "Cliente", FontWeight = FontWeights.Bold });
            panelCliente.Children.Add(new TextBlock { Text = "Apellido: " + clienteApellido });
            panelCliente.Children.Add(new TextBlock { Text = "Nombre: " + clienteNombre });
            panelCliente.Children.Add(new TextBlock { Text = "DNI: " + v.DNI });
            Grid.SetRow(panelCliente, 1);
            Grid.SetColumn(panelCliente, 0);
            grid.Children.Add(panelCliente);

            StackPanel panelProducto = new StackPanel { Margin = new Thickness(0, 30, 0, 0) };
            panelProducto.Children.Add(new TextBlock { Text = "Producto", FontWeight = FontWeights.Bold });
            panelProducto.Children.Add(new TextBlock { Text = "Descripción: " + productoDescripcion });
            panelProducto.Children.Add(new TextBlock { Text = "Color: " + productoColor });
            panelProducto.Children.Add(new TextBlock { Text = "Precio: $" + v.Precio.ToString("N2") });
            panelProducto.Children.Add(new TextBlock { Text = "Cantidad: " + v.Cantidad });
            Grid.SetRow(panelProducto, 1);
            Grid.SetColumn(panelProducto, 1);
            grid.Children.Add(panelProducto);

            TextBlock txtVendedor = new TextBlock { Text = "Vendedor: " + vendedorNombre, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 40, 0, 0) };
            Grid.SetRow(txtVendedor, 2);
            Grid.SetColumn(txtVendedor, 0);
            grid.Children.Add(txtVendedor);

            TextBlock txtTotal = new TextBlock { Text = "Total: $" + v.Importe.ToString("N2"), FontSize = 18, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 40, 0, 0) };
            Grid.SetRow(txtTotal, 2);
            Grid.SetColumn(txtTotal, 1);
            grid.Children.Add(txtTotal);

            pagina.Children.Add(grid);
            FixedPage.SetLeft(grid, 0);
            FixedPage.SetTop(grid, 0);

            PageContent pageContent = new PageContent();
            ((System.Windows.Markup.IAddChild)pageContent).AddChild(pagina);
            doc.Pages.Add(pageContent);

            return doc;
        }

        private void btnImprimir_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() == true)
            {
                pd.PrintDocument(documento.DocumentPaginator, "Comprobante de Venta");
            }
        }
    }
}