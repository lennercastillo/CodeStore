using Microsoft.Data.SqlClient;

namespace Presentacion.Helpers
{
    public static class MensajeErrorHelper
    {
        public static string ObtenerMensajeAmigable(Exception ex, string accion = "procesar la solicitud")
        {
            var sqlEx = ex as SqlException ?? ex.InnerException as SqlException;
            if (sqlEx != null)
            {
                if (sqlEx.Number is -1 or -2 or 2 or 53 or 4060 or 18456 or 233)
                {
                    return "No pudimos conectarnos con el servicio de datos en este momento. Por favor, intenta nuevamente en unos minutos o contacta al administrador.";
                }

                if (sqlEx.Number is 2601 or 2627)
                {
                    return "Ya existe un registro con esos mismos datos. Verifica que el código, documento o correo no esté repetido.";
                }

                if (sqlEx.Number == 547)
                {
                    return "No se puede completar esta acción porque este registro está vinculado a otros movimientos del sistema.";
                }

                if (sqlEx.Number >= 50000)
                {
                    return sqlEx.Message;
                }
            }

            string msg = ex.Message ?? string.Empty;

            if (msg.Contains("network-related", StringComparison.OrdinalIgnoreCase) ||
                msg.Contains("instance-specific", StringComparison.OrdinalIgnoreCase) ||
                msg.Contains("Named Pipes", StringComparison.OrdinalIgnoreCase) ||
                msg.Contains("Could not open a connection", StringComparison.OrdinalIgnoreCase) ||
                msg.Contains("server was not found", StringComparison.OrdinalIgnoreCase) ||
                msg.Contains("Login failed", StringComparison.OrdinalIgnoreCase) ||
                msg.Contains("Cannot open database", StringComparison.OrdinalIgnoreCase) ||
                msg.Contains("ConnectionString", StringComparison.OrdinalIgnoreCase) ||
                msg.Contains("transport-level", StringComparison.OrdinalIgnoreCase) ||
                msg.Contains("Timeout expired", StringComparison.OrdinalIgnoreCase))
            {
                return "No pudimos conectarnos con el servicio de datos en este momento. Por favor, intenta nuevamente en unos minutos o contacta al administrador.";
            }

            if (msg.Contains("REFERENCE constraint", StringComparison.OrdinalIgnoreCase) ||
                msg.Contains("FOREIGN KEY", StringComparison.OrdinalIgnoreCase))
            {
                return "No se puede completar esta acción porque este registro está vinculado a otros movimientos del sistema.";
            }

            if (msg.Contains("UNIQUE KEY", StringComparison.OrdinalIgnoreCase) ||
                msg.Contains("duplicate key", StringComparison.OrdinalIgnoreCase))
            {
                return "Ya existe un registro con esos mismos datos. Verifica que no esté duplicado.";
            }

            if (msg.Contains("CHECK constraint", StringComparison.OrdinalIgnoreCase))
            {
                return "Uno de los valores ingresados no es válido o está fuera del rango permitido.";
            }

            if (msg.Contains("Sql", StringComparison.OrdinalIgnoreCase) ||
                msg.Contains("Object reference", StringComparison.OrdinalIgnoreCase) ||
                msg.Contains("Index was outside", StringComparison.OrdinalIgnoreCase) ||
                msg.Contains("FormatException", StringComparison.OrdinalIgnoreCase) ||
                msg.Contains("InvalidCast", StringComparison.OrdinalIgnoreCase))
            {
                return $"Ocurrió un inconveniente inesperado al {accion}. Por favor, intenta de nuevo.";
            }

            // Si ya es un mensaje amigable en español (ej. de THROW 50002 o validación de servicio)
            if (!string.IsNullOrWhiteSpace(msg) && msg.Length < 180)
            {
                return msg;
            }

            return $"Ocurrió un inconveniente al {accion}. Por favor, intenta nuevamente.";
        }
    }
}

