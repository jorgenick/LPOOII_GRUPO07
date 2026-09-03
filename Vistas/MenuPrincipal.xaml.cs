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
        private string rol;

        public MenuPrincipal(string rol)
        {
            InitializeComponent();


            this.rol = rol;
            lblRol.Content = "Rol: " + rol;
            System.Console.WriteLine("==============Log info rol ingresado:" + rol + "==============");
            if (rol.ToLower().Equals("vendedor"))
            {
                this.btnVendedor.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            AltaCliente ventana = new AltaCliente();
            ventana.Show();
            //this.Close();
        }

        private void btnProvedor_Click(object sender, RoutedEventArgs e)
        {
            AltaProveedor ventana = new AltaProveedor();
            ventana.Show();
            //this.Close();
        }

        private void btnProducto_Click(object sender, RoutedEventArgs e)
        {
            AltaProducto ventana = new AltaProducto();
            ventana.Show();
            //this.Close();
        }

        private void btnVendedor_Click(object sender, RoutedEventArgs e)
        {
            AltaVendedor ventana = new AltaVendedor();
            ventana.Show();
            //this.Close();
        }

        private void btn_CerrarSesion(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resultado = MessageBox.Show(
                "¿Está seguro que desea cerrar la sesión?", "Confirmar Cierre de Sesión",
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

    }
}
