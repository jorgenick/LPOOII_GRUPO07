using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

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
                /*lblMensaje.Foreground = Brushes.Green;
                lblMensaje.Text = "Acceso concedido como Administrador.";*/
                mostrarMenuPrincipal("Administrador");
                this.Close();
            }
            else if (usuarioIngresado == "vendedor" && claveIngresada == "1234")
            {
                /*lblMensaje.Foreground = Brushes.Blue;
                lblMensaje.Text = "Acceso concedido como Vendedor.";*/
                mostrarMenuPrincipal("Vendedor");

                this.Close();
            }
            else
            {
                lblMensaje.Foreground = Brushes.Red;
                lblMensaje.Text = "Usuario o contraseña incorrectos.";
                var fade = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromMilliseconds(300)
                };
                lblMensaje.BeginAnimation(UIElement.OpacityProperty, fade);
                txtPassword.Clear();
                txtPassword.Focus();
            }

        }

        void mostrarMenuPrincipal(String rol)
        {
            MenuPrincipal ventana = new MenuPrincipal(rol);
            ventana.Show();
        }
    }
}