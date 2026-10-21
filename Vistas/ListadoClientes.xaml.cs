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

        private void dataGridClientes_Loaded(object sender, RoutedEventArgs e)
        {
            DataTable tableClientes = TrabajarClientes.TraerClientesDataTable();
            dataGridClientes.ItemsSource = tableClientes.DefaultView;
        }

       

       
        

        
    }
}
