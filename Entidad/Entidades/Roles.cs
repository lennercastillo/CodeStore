using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class Roles
    {
        public int IdRol { get; set; }
        public required string CodigoRol { get; set; }
        public required string NombreRol { get; set; }
        public required string Descripcion { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
