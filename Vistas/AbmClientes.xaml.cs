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
using ClasesBase;

namespace Vistas
{
    /// <summary>
    /// Lógica de interacción para AbmClientes.xaml
    /// </summary>
    public partial class AbmClientes : Window
    {
        public AbmClientes()
        {
            InitializeComponent();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {   
            vista.MoveCurrentToNext();
            if (vista.IsCurrentAfterLast)
            {
                vista.MoveCurrentToFirst();
            }
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            vista.MoveCurrentToPrevious();
            if (vista.IsCurrentBeforeFirst)
            {
                vista.MoveCurrentToLast();
            }
        }

        private void btnLast_Click(object sender, RoutedEventArgs e)
        {
            vista.MoveCurrentToLast();
        }

        private void btnFirst_Click(object sender, RoutedEventArgs e)
        {
            vista.MoveCurrentToFirst();
        }

        CollectionView vista;
        ObservableCollection<Cliente> listaCliente;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ObjectDataProvider dataProvider = (ObjectDataProvider) this.Resources["LIST_CLIENTE"];
            listaCliente = dataProvider.Data as ObservableCollection<Cliente>;

            vista = (CollectionView)CollectionViewSource.GetDefaultView(canvas_content.DataContext);
        }

        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
