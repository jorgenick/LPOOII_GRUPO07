using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using ClasesBase;

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

            List<string> codigosRoles = TrabajarUsuarios.ValidarLogin(usuarioIngresado, claveIngresada);

            if (codigosRoles.Count > 0)
            {
                mostrarMenuPrincipal(codigosRoles);
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

        private void mostrarMenuPrincipal(List<string> codigosRoles)
        {
            MenuPrincipal ventana = new MenuPrincipal(codigosRoles);
            ventana.Show();
        }
    }
}