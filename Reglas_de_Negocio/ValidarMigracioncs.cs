using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using FirebirdSql.Data.FirebirdClient;
using BaseDeDatosSQL;

namespace Reglas_de_Negocio
{
    public static class ValidadorMigracion
    {
        public static string ValidarConteoRegistros(string tabla, Userdata origen, Userdata destino)
        {
            try
            {
                int origenTotal = ContarRegistrosEnOrigen(tabla, origen);
                int destinoTotal = ContarRegistrosEnDestino(tabla, destino);

                if (origenTotal == destinoTotal)
                {
                    return $"✔ Tabla '{tabla}' migrada correctamente ({origenTotal} registros)";
                }
                else
                {
                    return $"⚠ Diferencia en tabla '{tabla}': origen = {origenTotal}, destino = {destinoTotal}";
                }
            }
            catch (Exception ex)
            {
                return $"❌ Error validando tabla '{tabla}': {ex.Message}";
            }
        }

        private static int ContarRegistrosEnOrigen(string tabla, Userdata origen)
        {
            string connStr = $"Data Source={origen.Servidor};Initial Catalog={origen.BaseDeDatos};User ID={origen.Usuario};Password={origen.Contraseña};";
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand($"SELECT COUNT(*) FROM [{tabla}]", conn))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        private static int ContarRegistrosEnDestino(string tabla, Userdata destino)
        {
            string query = $"SELECT COUNT(*) FROM {FormatearIdentificador(tabla, destino.SistemaGestor)}";

            using (var conn = CrearConexion(destino))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        private static DbConnection CrearConexion(Userdata destino)
        {
            switch (destino.SistemaGestor.ToLower())
            {
                case "mysql":
                    return new MySqlConnection($"Server={destino.Servidor};Database={destino.BaseDeDatos};Uid={destino.Usuario};Pwd={destino.Contraseña};");
                case "postgresql":
                    return new NpgsqlConnection($"Host={destino.Servidor};Database={destino.BaseDeDatos};Username={destino.Usuario};Password={destino.Contraseña};");
                case "oracle":
                    return new OracleConnection($"Data Source={destino.Servidor};User Id={destino.Usuario};Password={destino.Contraseña};");
                case "firebird":
                    return new FbConnection($"Database={destino.BaseDeDatos};DataSource={destino.Servidor};User={destino.Usuario};Password={destino.Contraseña};");
                default:
                    throw new NotSupportedException("Gestor destino no soportado");
            }
        }

        private static string FormatearIdentificador(string nombre, string gestor)
        {
            switch (gestor.ToLower())
            {
                case "mysql": return $"`{nombre}`";
                case "postgresql": return $"\"{nombre}\"";
                case "oracle":
                case "firebird": return nombre;
                default: return nombre;
            }
        }
    }
}
