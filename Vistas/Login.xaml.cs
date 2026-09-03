using System;
using System.Windows;
using System.Windows.Media;

namespace Vistas
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnclick_onClick(object sender, RoutedEventArgs e)
        {
            string usuarioIngresado = txtUsuario.Text.Trim();
            string claveIngresada = txtPassword.Password;

            if (usuarioIngresado == "admin" && claveIngresada == "1234")
            {
                lblMensaje.Foreground = Brushes.Green;
                lblMensaje.Text = "Acceso concedido como Administrador.";
                MenuPrincipal ventana = new MenuPrincipal("Administrador");
                ventana.Show();
                this.Close();
            }
            else if (usuarioIngresado == "vendedor" && claveIngresada == "1234")
            {
                lblMensaje.Foreground = Brushes.Blue;
                lblMensaje.Text = "Acceso concedido como Vendedor.";
                MenuPrincipal ventana = new MenuPrincipal("Vendedor");
                ventana.Show();
                this.Close();
            }
            else
            {
                lblMensaje.Foreground = Brushes.Red;
                lblMensaje.Text = "Usuario o contraseña incorrectos.";
            }
        }
    }
}