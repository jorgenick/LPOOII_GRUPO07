using System;
using System.IO;
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

namespace Vistas
{
    public partial class Presentacion : Window
    {
        private bool yaAbrio = false;

        public Presentacion()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                       "Recursos", "Audio", "Elevator-Music.wav");
            if (!File.Exists(ruta))
            {
                AbrirPrincipal();
                return;
            }
            reproductor.Source = new Uri(ruta, UriKind.Absolute);
            reproductor.Play();
        }

        private void reproductor_MediaEnded(object sender, RoutedEventArgs e)
        {
            AbrirPrincipal();
        }

        private void reproductor_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            AbrirPrincipal();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AbrirPrincipal();
        }

        private void AbrirPrincipal()
        {
            if (yaAbrio) return;
            yaAbrio = true;
            reproductor.Stop();
            Login principal = new Login();
            principal.Show();
            this.Close();
        }
    }
}
