using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ClasesBase.Dao;
using ClasesBase.Entidades;

namespace Vistas
{
    public partial class AbmUsuarios : Window
    {
        private Usuario oUsuario;
        private List<Usuario> listaUsuarios;
        private int indiceActual;
        private bool modoModificar = false;

        public AbmUsuarios()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Cargamos el catálogo de roles disponibles en el ListBox
            lstRoles.ItemsSource = TrabajarRoles.TraerRoles().ToList();

            // Cargamos los usuarios
            listaUsuarios = TrabajarUsuarios.TraerUsuarios().ToList();

            if (listaUsuarios.Count > 0)
            {
                indiceActual = 0;
                MostrarUsuarioActual();
            }
        }

        private void MostrarUsuarioActual()
        {
            Usuario u = listaUsuarios[indiceActual];

            txtId.Text = u.Usu_ID.ToString();
            txtNombre.Text = u.Usu_Nombre;
            txtPassword.Text = u.Usu_Password;

            // Marcamos en el ListBox los roles que tiene asignados este usuario
            lstRoles.SelectedItems.Clear();
            if (u.Roles != null)
            {
                foreach (Rol itemLista in lstRoles.Items)
                {
                    if (u.Roles.Any(r => r.Rol_ID == itemLista.Rol_ID))
                    {
                        lstRoles.SelectedItems.Add(itemLista);
                    }
                }
            }
        }

        private void CambiarEstadoControles(bool editando)
        {
            // txtId siempre permanece deshabilitado porque es IDENTITY en SQL Server
            txtNombre.IsEnabled = editando;
            txtPassword.IsEnabled = editando;
            lstRoles.IsEnabled = editando;

            btnGuardar.IsEnabled = editando;
            btnCancelar.IsEnabled = editando;

            btnNuevo.IsEnabled = !editando;
            btnModificar.IsEnabled = !editando;
            btnEliminar.IsEnabled = !editando;
            btnPrimero.IsEnabled = !editando;
            btnAnterior.IsEnabled = !editando;
            btnSiguiente.IsEnabled = !editando;
            btnUltimo.IsEnabled = !editando;
        }

        private void LimpiarCampos()
        {
            txtId.Text = "";
            txtNombre.Text = "";
            txtPassword.Text = "";
            lstRoles.SelectedItems.Clear();
        }

        // --- NAVEGACIÓN ---
        private void btnPrimero_Click(object sender, RoutedEventArgs e)
        {
            if (listaUsuarios.Count == 0) return;
            indiceActual = 0;
            MostrarUsuarioActual();
        }

        private void btnAnterior_Click(object sender, RoutedEventArgs e)
        {
            if (indiceActual > 0)
            {
                indiceActual--;
                MostrarUsuarioActual();
            }
        }

        private void btnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            if (indiceActual < listaUsuarios.Count - 1)
            {
                indiceActual++;
                MostrarUsuarioActual();
            }
        }

        private void btnUltimo_Click(object sender, RoutedEventArgs e)
        {
            if (listaUsuarios.Count == 0) return;
            indiceActual = listaUsuarios.Count - 1;
            MostrarUsuarioActual();
        }

        // --- ACCIONES ABM ---
        private void btnNuevo_Click(object sender, RoutedEventArgs e)
        {
            modoModificar = false;
            LimpiarCampos();
            CambiarEstadoControles(true);
            txtNombre.Focus();
        }

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if (listaUsuarios.Count == 0) return;

            modoModificar = true;
            CambiarEstadoControles(true);
            txtNombre.Focus();
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (listaUsuarios.Count == 0) return;

            Usuario seleccionado = listaUsuarios[indiceActual];

            MessageBoxResult confirmacion = MessageBox.Show(
                "¿Eliminar al usuario " + seleccionado.Usu_Nombre + "?",
                "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (confirmacion != MessageBoxResult.Yes)
                return;

            try
            {
                TrabajarUsuarios.EliminarUsuario(seleccionado.Usu_ID);
                MessageBox.Show("Usuario eliminado.", "Baja", MessageBoxButton.OK, MessageBoxImage.Information);

                listaUsuarios = TrabajarUsuarios.TraerUsuarios().ToList();
                if (listaUsuarios.Count > 0)
                {
                    indiceActual = 0;
                    MostrarUsuarioActual();
                }
                else
                {
                    LimpiarCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            // Validar campos obligatorios
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Por favor complete usuario y contraseña.", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Recolectar los roles marcados
            List<Rol> rolesSeleccionados = new List<Rol>();
            foreach (Rol r in lstRoles.SelectedItems)
            {
                rolesSeleccionados.Add(r);
            }

            if (rolesSeleccionados.Count == 0)
            {
                MessageBox.Show("Debe asignar al menos un rol al usuario.", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBoxResult confirmacion = MessageBox.Show(
                "¿Confirma guardar los datos ingresados?",
                "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirmacion == MessageBoxResult.Yes)
            {
                try
                {
                    oUsuario = new Usuario();
                    oUsuario.Usu_Nombre = txtNombre.Text.Trim();
                    oUsuario.Usu_Password = txtPassword.Text;
                    oUsuario.Roles = rolesSeleccionados;

                    if (modoModificar)
                    {
                        oUsuario.Usu_ID = int.Parse(txtId.Text);
                        TrabajarUsuarios.ModificarUsuario(oUsuario);
                    }
                    else
                    {
                        TrabajarUsuarios.AgregarUsuario(oUsuario);
                    }

                    MessageBox.Show(
                        "Usuario: " + oUsuario.Usu_Nombre +
                        "\nRoles: " + oUsuario.RolesDescripcion,
                        modoModificar ? "Usuario modificado con éxito" : "Usuario guardado en BD con éxito",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    modoModificar = false;
                    CambiarEstadoControles(false);

                    // Recargamos la lista para reflejar los cambios sin cerrar la ventana
                    listaUsuarios = TrabajarUsuarios.TraerUsuarios().ToList();
                    if (listaUsuarios.Count > 0)
                    {
                        indiceActual = listaUsuarios.Count - 1;
                        MostrarUsuarioActual();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar en la Base de Datos: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            modoModificar = false;
            LimpiarCampos();
            CambiarEstadoControles(false);

            if (listaUsuarios.Count > 0)
                MostrarUsuarioActual();
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}