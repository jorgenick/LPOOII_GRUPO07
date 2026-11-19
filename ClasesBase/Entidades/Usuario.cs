using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClasesBase.Entidades
{
    public class Usuario
    {
        public int Usu_ID { get; set; }
        public string Usu_Nombre { get; set; }
        public string Usu_Password { get; set; }
        public List<Rol> Roles { get; set; }

        public Usuario()
        {
            Roles = new List<Rol>();
        }

        public string RolesDescripcion
        {
            get
            {
                if (Roles == null || Roles.Count == 0) return "Sin roles";
                return string.Join(", ", Roles.Select(r => r.Rol_Descripcion).ToArray());
            }
        }
    }
}
