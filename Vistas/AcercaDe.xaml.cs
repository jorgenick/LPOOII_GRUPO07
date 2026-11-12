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
using System.Windows.Threading; // Para el DispatcherTimer
using System.Windows.Controls.Primitives;

namespace Vistas
{
    /// <summary>
    /// Lógica de interacción para AcercaDe.xaml
    /// </summary>
    public partial class AcercaDe : Window
    {

        // Declaramos el timer para mover el slider
        private DispatcherTimer timer;
        // Bandera para saber si el usuario está arrastrando el slider
        private bool isDragging = false;

        public AcercaDe()
        {
            InitializeComponent();

            // Configuramos el timer para que actualice cada medio segundo
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
        }

        // --- CONFIGURACIÓN INICIAL DEL VIDEO ---
        private void reproductor_MediaOpened(object sender, RoutedEventArgs e)
        {
            // Cuando el video carga, le decimos al slider cuál es el tiempo máximo (en segundos)
            if (reproductor.NaturalDuration.HasTimeSpan)
            {
                sliderVideo.Maximum = reproductor.NaturalDuration.TimeSpan.TotalSeconds;
            }
        }

        // --- ACTUALIZACIÓN DEL SLIDER ---
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Si el usuario no está arrastrando el slider manualmente, lo movemos con el video
            if (!isDragging)
            {
                sliderVideo.Value = reproductor.Position.TotalSeconds;
            }
        }

        // --- EVENTOS DE LOS BOTONES ---
        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            reproductor.Play();
            timer.Start(); // Iniciamos el movimiento del slider
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            reproductor.Pause();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            reproductor.Stop();
            timer.Stop();
            sliderVideo.Value = 0; // Reiniciamos el slider
        }

        // --- EVENTOS DEL SLIDER (PARA ADELANTAR/ATRASAR EL VIDEO) ---
        private void sliderVideo_DragStarted(object sender, DragStartedEventArgs e)
        {
            isDragging = true; // El usuario tomó el control
        }

        private void sliderVideo_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            isDragging = false; // El usuario soltó el slider
            // Cambiamos la posición del video al punto donde el usuario soltó el slider
            reproductor.Position = TimeSpan.FromSeconds(sliderVideo.Value);
        }
    }
}
