using Aplicacion.Interface;
using Dominio.Entidades;
using Estructura.database;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estructura.Repositorio
{
    public class ClientesRepositorio : IClientes
    {
        private readonly BD _cone;

        public ClientesRepositorio (BD name)
        {
            _cone = name;
        }

        public async Task EditarClienteAsync(Clientes cliente)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("ActualizarCliente", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdCliente", cliente.IdCliente));
                cmd.Parameters.Add(new SqlParameter("@NumeroIdentificacion", cliente.NumeroIdentificacion));
                cmd.Parameters.Add(new SqlParameter("@Nombres", cliente.Nombres));
                cmd.Parameters.Add(new SqlParameter("@Apellidos", cliente.Apellidos));
                cmd.Parameters.Add(new SqlParameter("@Telefono", cliente.Telefono == null ? DBNull.Value : cliente.Telefono));
                cmd.Parameters.Add(new SqlParameter("@Correo", cliente.Correo == null ? DBNull.Value : cliente.Correo));
                cmd.Parameters.Add(new SqlParameter("@Direccion", cliente.Direccion == null ? DBNull.Value : cliente.Direccion));
                cmd.Parameters.Add(new SqlParameter("@Activo", cliente.Activo));

                await cmd.ExecuteReaderAsync();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task EliminarClienteAsync(int Id)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("EliminarCliente", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdCliente", Id));
                await cmd.ExecuteReaderAsync();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Clientes>> ListarClienteAsync()
        {
            var lis = new List<Clientes>();
            using var com = _cone.GetConnection();
            await com.OpenAsync();

            try
            {
                using var cmd = new SqlCommand("ListarClientes", com);
                cmd.CommandType = CommandType.StoredProcedure;
                using var Dr = await cmd.ExecuteReaderAsync();

                while (await Dr.ReadAsync())
                {
                    var cl = new Clientes
                    {
                        IdCliente = Convert.ToInt32(Dr["IdCliente"]),
                        NumeroIdentificacion = Dr["NumeroIdentificacion"].ToString()!,
                        Nombres = Dr["Nombres"].ToString()!,
                        Apellidos = Dr["Apellidos"].ToString()!,
                        Telefono = Dr["Telefono"].ToString(),
                        Correo = Dr["Correo"].ToString(),
                        Direccion = Dr["Direccion"].ToString(),
                        Activo = Convert.ToBoolean(Dr["Activo"]),
                        FechaRegistro = Convert.ToDateTime(Dr["FechaRegistro"])
                    };
                    lis.Add(cl);
                }
                return lis;
            }
            catch
            {
                throw;
            }
        }

        public async Task NuevoClienteAsync(Clientes cliente)
        {
            try
            {
                using var con = _cone.GetConnection();
                await con.OpenAsync();
                using var cmd = new SqlCommand("InsertarCliente", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@NumeroIdentificacion", cliente.NumeroIdentificacion));
                cmd.Parameters.Add(new SqlParameter("@Nombres", cliente.Nombres));
                cmd.Parameters.Add(new SqlParameter("@Apellidos", cliente.Apellidos));
                cmd.Parameters.Add(new SqlParameter("@Telefono", cliente.Telefono == null ? DBNull.Value : cliente.Telefono));
                cmd.Parameters.Add(new SqlParameter("@Correo", cliente.Correo == null ? DBNull.Value : cliente.Correo));
                cmd.Parameters.Add(new SqlParameter("@Direccion", cliente.Direccion == null ? DBNull.Value : cliente.Direccion));
                cmd.Parameters.Add(new SqlParameter("@Activo", cliente.Activo));

                await cmd.ExecuteReaderAsync();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
