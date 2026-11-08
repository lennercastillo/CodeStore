using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Entidades
{
    public class Clientes
    {
        public int IdCliente { get; set; }
        public required string NumeroIdentificacion { get; set; }
        public required string Nombres { get; set; }
        public required string Apellidos { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? Direccion { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
