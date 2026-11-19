using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using ClasesBase.Dao;
using ClasesBase.Entidades;

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

            Usuario usuarioLoggeado = TrabajarUsuarios.ValidarLogin(usuarioIngresado, claveIngresada);
            List<string> codigosRoles = usuarioLoggeado.ObtenerCodigosRoles();

            if (codigosRoles.Count > 0)
            {
                mostrarMenuPrincipal(usuarioLoggeado);
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

        private void mostrarMenuPrincipal(Usuario usuarioLoggeado)
        {
            MenuPrincipal ventana = new MenuPrincipal(usuarioLoggeado);
            ventana.Show();
        }
    }
}