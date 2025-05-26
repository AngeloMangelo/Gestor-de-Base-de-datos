using System;
using System.Data.Common;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using FirebirdSql.Data.FirebirdClient;
using System.Data.SqlClient;

namespace BaseDeDatosSQL
{
    public static class EjecutorSQLDestino
    {
        public static bool EjecutarScript(Userdata destino, string script)
        {
            try
            {
                DbConnection conn = CrearConexion(destino);
                conn.Open();

                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = script;
                    cmd.ExecuteNonQuery();
                }

                conn.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ejecutar script:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private static DbConnection CrearConexion(Userdata destino)
        {
            string gestor = destino.SistemaGestor.ToLower();
            string cadena = "";

            switch (gestor)
            {
                case "mysql":
                    cadena = $"Server={destino.Servidor};Database={destino.BaseDeDatos};User ID={destino.Usuario};Password={destino.Contraseña};";
                    return new MySqlConnection(cadena);

                case "postgresql":
                    cadena = $"Host={destino.Servidor};Database={destino.BaseDeDatos};Username={destino.Usuario};Password={destino.Contraseña}";
                    return new NpgsqlConnection(cadena);

                case "oracle":
                    cadena = $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={destino.Servidor})(PORT=1521))(CONNECT_DATA=(SID=XE)));User Id={destino.Usuario};Password={destino.Contraseña};";
                    return new OracleConnection(cadena);

                case "firebird":
                    cadena = $"DataSource={destino.Servidor};Database={destino.Ruta};User={destino.Usuario};Password={destino.Contraseña};Charset=UTF8;";
                    return new FbConnection(cadena);

                default:
                    throw new NotSupportedException("Gestor destino no soportado.");
            }
        }

        public static bool CrearBaseDeDatos(Userdata destino)
        {
            try
            {
                string gestor = destino.SistemaGestor.ToLower();
                string baseDeDatos = destino.BaseDeDatos;
                string cadenaAdmin = "";

                switch (gestor)
                {
                    case "mysql":
                        cadenaAdmin = $"Server={destino.Servidor};User ID={destino.Usuario};Password={destino.Contraseña};";
                        using (var conn = new MySqlConnection(cadenaAdmin))
                        {
                            conn.Open();
                            using (var cmd = conn.CreateCommand())
                            {
                                cmd.CommandText = $"CREATE DATABASE IF NOT EXISTS `{baseDeDatos}`";
                                cmd.ExecuteNonQuery();
                            }
                            conn.Close();
                        }
                        break;

                    case "postgresql":
                        cadenaAdmin = $"Host={destino.Servidor};Username={destino.Usuario};Password={destino.Contraseña};Database=postgres";
                        using (var conn = new NpgsqlConnection(cadenaAdmin))
                        {
                            conn.Open();
                            using (var cmd = conn.CreateCommand())
                            {
                                cmd.CommandText = $"CREATE DATABASE \"{baseDeDatos}\"";
                                cmd.ExecuteNonQuery();
                            }
                            conn.Close();
                        }
                        break;

                    case "oracle":
                        MessageBox.Show("En Oracle se requiere creación manual del esquema (usuario).", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case "firebird":
                        // Ya se usa un archivo .fdb, no se puede crear base en ejecución desde este flujo
                        MessageBox.Show("En Firebird, la base debe existir como archivo .fdb.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    default:
                        throw new NotSupportedException("Gestor no soportado.");
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear base de datos destino: " + ex.Message);
                return false;
            }
        }

    }
}
