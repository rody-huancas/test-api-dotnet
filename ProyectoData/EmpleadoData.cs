using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Data;
using ProyectoModelo;

namespace ProyectoData
{
    public class EmpleadoData
    {
        private readonly ConnectionStrings conexiones;

        public EmpleadoData(IOptions<ConnectionStrings> connectionStrings)
        {
            conexiones = connectionStrings.Value;
        }

        public async Task<List<Empleado>> ListarEmpleados()
        {
            List<Empleado> lista = new List<Empleado>();

            using (var conexion = new SqlConnection(conexiones.CadenaSQL))
            {
                await conexion.OpenAsync();

                SqlCommand cmd = new SqlCommand("sp_listaEmpleados", conexion);

                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Empleado
                        {
                            IdEmpleado = Convert.ToInt32(reader["IdEmpleado"]),
                            NombreCompleto = reader["NombreCompleto"].ToString(),
                            Sueldo = Convert.ToDecimal(reader["Sueldo"]),
                            FechaContrato = reader["FechaContrato"].ToString(),
                            Departamento = new Departamento
                            {
                                IdDepartamento = Convert.ToInt32(reader["IdDepartamento"]),
                                Nombre = reader["Nombre"].ToString()
                            }
                        });
                    }
                }
            }

            return lista;
        }


        public async Task<Boolean> RegistrarEmpleado(Empleado objeto)
        {
            bool respuesta = true;

            using (var conexion = new SqlConnection(conexiones.CadenaSQL))
            {
                await conexion.OpenAsync();

                SqlCommand cmd = new SqlCommand("sp_crearEmpleado", conexion);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@NombreCompleto", objeto.NombreCompleto);
                cmd.Parameters.AddWithValue("@IdDepartamento", objeto.Departamento!.IdDepartamento);
                cmd.Parameters.AddWithValue("@Sueldo"        , objeto.Sueldo);
                cmd.Parameters.AddWithValue("@FechaContrato" , objeto.FechaContrato);

                try
                {
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    respuesta = false;

                    Console.WriteLine("Error al registrar el empleado: " + ex.Message);
                }

                return respuesta;
            }
        }


        public async Task<Boolean> EditarEmpleado(Empleado objeto)
        {
            bool respuesta = true;

            using (var conexion = new SqlConnection(conexiones.CadenaSQL))
            {
                await conexion.OpenAsync();

                SqlCommand cmd = new SqlCommand("sp_editarEmpleado", conexion);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdEmpleado", objeto.IdEmpleado);
                cmd.Parameters.AddWithValue("@NombreCompleto", objeto.NombreCompleto);
                cmd.Parameters.AddWithValue("@IdDepartamento", objeto.Departamento!.IdDepartamento);
                cmd.Parameters.AddWithValue("@Sueldo", objeto.Sueldo);
                cmd.Parameters.AddWithValue("@FechaContrato", objeto.FechaContrato);

                try
                {
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    respuesta = false;

                    Console.WriteLine("Error al editar el empleado: " + ex.Message);
                }

                return respuesta;
            }
        }

        public async Task<Boolean> EliminarEmpleado(int idEmpleado)
        {
            bool respuesta = true;
            using (var conexion = new SqlConnection(conexiones.CadenaSQL))
            {
                await conexion.OpenAsync();

                SqlCommand cmd = new SqlCommand("sp_eliminarEmpleado", conexion);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdEmpleado", idEmpleado);

                try
                {
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    respuesta = false;
                    Console.WriteLine("Error al eliminar el empleado: " + ex.Message);
                }

                return respuesta;
            }
        }
    }
}
