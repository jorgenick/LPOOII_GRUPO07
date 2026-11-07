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
using System.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Vistas
{
    /// <summary>
    /// Lógica de interacción para ListadoClientes.xaml
    /// </summary>
    public partial class ListadoClientes : Window
    {
        public ListadoClientes()
        {
            InitializeComponent();
        }

        private ICollectionView vista;

        private void dataGridClientes_Loaded(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Cliente> lista = TrabajarClientes.TraerClientes();
            vista = CollectionViewSource.GetDefaultView(lista);

            // 2a: orden ascendente por Apellido
            vista.SortDescriptions.Clear();
            vista.SortDescriptions.Add(new SortDescription("Apellido", ListSortDirection.Ascending));

            dataGridClientes.ItemsSource = vista;
        }

        // filtro por apellido
        private void txtFiltro_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (vista == null) return;

            string texto = txtFiltro.Text.Trim().ToLower();
            vista.Filter = item =>
            {
                Cliente c = item as Cliente;
                return c != null && (c.Apellido ?? "").ToLower().Contains(texto);
            };
        }

        private void btnVistaPrevia_Click(object sender, RoutedEventArgs e)
        {
            if (vista == null) return;

            List<Cliente> clientes = vista.Cast<Cliente>().ToList();
            VistaPreviaClientes ventana = new VistaPreviaClientes(clientes);
            ventana.Owner = this;
            ventana.ShowDialog();
        }
    }
}
