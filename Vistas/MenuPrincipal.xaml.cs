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

namespace Vistas
{
    /// <summary>
    /// Interaction logic for MenuPrincipal.xaml
    /// </summary>
    public partial class MenuPrincipal : Window
    {
        private List<string> codigosRoles;

        public MenuPrincipal(List<string> codigosRoles)
        {
            InitializeComponent();

            this.codigosRoles = codigosRoles;

            string resumenRoles = string.Join(", ", codigosRoles.ToArray());
            lblRol.Content = "Rol: " + resumenRoles;
            System.Console.WriteLine("==============Log info roles ingresados: " + resumenRoles + "==============");

            ConfigurarPermisos();
        }

        private void ConfigurarPermisos()
        {
            this.btnVendedor.Visibility = System.Windows.Visibility.Hidden;
            this.btnAltaUsuarios.Visibility = System.Windows.Visibility.Hidden;

            if (codigosRoles.Contains("ADMI"))
            {
                this.btnVendedor.Visibility = System.Windows.Visibility.Visible;
                this.btnAltaUsuarios.Visibility = System.Windows.Visibility.Visible;
            }

            if (codigosRoles.Contains("AUDI") && !codigosRoles.Contains("ADMI"))
            {
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            AltaCliente ventana = new AltaCliente();
            ventana.ShowDialog();
        }

        private void btnProvedor_Click(object sender, RoutedEventArgs e)
        {
            AltaProveedor ventana = new AltaProveedor();
            ventana.ShowDialog();
        }

        private void btnProducto_Click(object sender, RoutedEventArgs e)
        {
            AltaProducto ventana = new AltaProducto();
            ventana.ShowDialog();
        }

        private void btnVendedor_Click(object sender, RoutedEventArgs e)
        {
            AltaVendedor ventana = new AltaVendedor();
            ventana.ShowDialog();
        }

        private void btnCliente_Click(object sender, RoutedEventArgs e)
        {
            AbmClientes ventanaClientes = new AbmClientes();
            ventanaClientes.ShowDialog();
        }

        private void btn_CerrarSesion(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resultado = MessageBox.Show(
                "¿Está seguro que desea cerrar la sesión?",
                "Confirmar Cierre de Sesión",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (resultado == MessageBoxResult.Yes)
            {
                Login ventanaLogin = new Login();
                ventanaLogin.Show();
                this.Close();
            }
        }

        private void btnListadoClientes_Click(object sender, RoutedEventArgs e)
        {
            ListadoClientes ventanaListadoClientes = new ListadoClientes();
            ventanaListadoClientes.ShowDialog();
        }

        private void btnVentas_Click(object sender, RoutedEventArgs e)
        {
            AltaVenta ventana = new AltaVenta();
            ventana.ShowDialog();
        }

        private void btnAcercaDe_Click(object sender, RoutedEventArgs e)
        {
            AcercaDe ventana = new AcercaDe();
            ventana.ShowDialog();
        }

        private void btnAltaUsuarios_Click(object sender, RoutedEventArgs e)
        {
            AbmUsuarios ventana = new AbmUsuarios();
            ventana.ShowDialog();
        }
    }
}