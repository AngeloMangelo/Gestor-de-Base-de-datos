using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using FirebirdSql.Data.FirebirdClient;
using BaseDeDatosSQL;

namespace Reglas_de_Negocio
{
    public class MigradorDatos
    {
        private string servidor;
        private string usuario;
        private string contraseña;
        private string baseDatos;

        public MigradorDatos(string servidor, string usuario, string contraseña, string baseDatos)
        {
            this.servidor = servidor;
            this.usuario = usuario;
            this.contraseña = contraseña;
            this.baseDatos = baseDatos;
        }

        public void MigrarDatos(string tabla, Userdata destino)
        {
            try
            {
                // 1. Leer datos desde SQL Server
                string connSql = $"Data Source={servidor};Initial Catalog={baseDatos};User ID={usuario};Password={contraseña};";
                DataTable datos = new DataTable();

                using (SqlConnection conn = new SqlConnection(connSql))
                {
                    conn.Open();
                    string[] partes = tabla.Split('.');
                    string tablaFormateada = partes.Length == 2 ? $"[{partes[0]}].[{partes[1]}]" : $"[{tabla}]";
                    string query = $"SELECT * FROM {tablaFormateada}";
                    using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                    {
                        da.Fill(datos);
                    }
                }

                if (datos.Rows.Count == 0)
                {
                    MessageBox.Show($"La tabla {tabla} no tiene registros.", "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 2 Conectar al destino
                using (DbConnection connDestino = CrearConexionDestino(destino))
                {
                    connDestino.Open();

                    foreach (DataRow fila in datos.Rows)
                    {
                        using (DbCommand cmd = connDestino.CreateCommand())
                        {
                            var insert = GenerarInsertConParametros(tabla, fila, destino.SistemaGestor, cmd);
                            cmd.CommandText = insert;
                            cmd.ExecuteNonQuery();
                        }
                    }

                    connDestino.Close();
                }

                MessageBox.Show($"Datos migrados correctamente a la tabla {tabla}.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al migrar datos de {tabla}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GenerarInsertConParametros(string tabla, DataRow fila, string gestorDestino, DbCommand cmd)
        {
            StringBuilder columnas = new StringBuilder();
            StringBuilder parametros = new StringBuilder();
            int index = 0;

            foreach (DataColumn col in fila.Table.Columns)
            {
                if (columnas.Length > 0)
                {
                    columnas.Append(", ");
                    parametros.Append(", ");
                }

                string nombreCol = FormatearIdentificador(col.ColumnName, gestorDestino);
                string paramName = $"@p{index++}";

                columnas.Append(nombreCol);
                parametros.Append(paramName);

                var valor = fila[col];
                var parameter = cmd.CreateParameter();
                parameter.ParameterName = paramName;

                if (valor == DBNull.Value)
                {
                    parameter.Value = DBNull.Value;
                }
                else if (valor is byte[])
                {
                    parameter.DbType = DbType.Binary;
                    parameter.Value = (byte[])valor;
                }
                else
                {
                    parameter.Value = valor;
                }

                cmd.Parameters.Add(parameter);
            }

            return $"INSERT INTO {FormatearIdentificador(tabla, gestorDestino)} ({columnas}) VALUES ({parametros});";
        }


        private DbConnection CrearConexionDestino(Userdata destino)
        {
            switch (destino.SistemaGestor.ToLower())
            {
                case "mysql":
                    return new MySqlConnection($"Server={destino.Servidor};Database={destino.BaseDeDatos};User ID={destino.Usuario};Password={destino.Contraseña};");
                case "postgresql":
                    return new NpgsqlConnection($"Host={destino.Servidor};Database={destino.BaseDeDatos};Username={destino.Usuario};Password={destino.Contraseña}");
                case "oracle":
                    return new OracleConnection($"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={destino.Servidor})(PORT=1521))(CONNECT_DATA=(SID=XE)));User Id={destino.Usuario};Password={destino.Contraseña};");
                case "firebird":
                    return new FbConnection($"DataSource={destino.Servidor};Database={destino.Ruta};User={destino.Usuario};Password={destino.Contraseña};Charset=UTF8;");
                default:
                    throw new NotSupportedException("Gestor destino no soportado.");
            }
        }

        private string GenerarInsert(string tabla, DataRow fila, string gestorDestino)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder columnas = new StringBuilder();
            StringBuilder valores = new StringBuilder();

            foreach (DataColumn col in fila.Table.Columns)
            {
                if (columnas.Length > 0)
                {
                    columnas.Append(", ");
                    valores.Append(", ");
                }

                string nombreCol = FormatearIdentificador(col.ColumnName, gestorDestino);
                columnas.Append(nombreCol);
                valores.Append(FormatearValor(fila[col], gestorDestino));
            }

            sb.Append($"INSERT INTO {FormatearIdentificador(tabla, gestorDestino)} ({columnas}) VALUES ({valores});");
            return sb.ToString();
        }

        private string FormatearIdentificador(string nombre, string gestor)
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

        private string FormatearValor(object valor, string gestor)
        {
            if (valor == DBNull.Value)
                return "NULL";

            if (valor is string || valor is DateTime)
                return $"'{valor.ToString().Replace("'", "''")}'";

            if (valor is bool)
                return ((bool)valor) ? "1" : "0";

            return valor.ToString();
        }
    }
}
