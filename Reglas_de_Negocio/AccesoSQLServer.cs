using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using System.IO;
using System.Net.Configuration;
using System.Data.Common;
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using FirebirdSql.Data.FirebirdClient;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System.Diagnostics.Contracts;
using FirebirdSql.Data.Services;
using BaseDeDatosSQL;
using Microsoft.VisualBasic.ApplicationServices;
using Org.BouncyCastle.Tls;

namespace Reglas_de_Negocio
{
    public class AccesoSQLServer
    {
        public String sLastError = string.Empty;
        public SqlException lastsqlException = null;
        public static string sRutaBD;
        
        //El cerebro de las conexiones a los sistemas Gestores de Datos :O!
        public DbConnection GetDBConnection(string gestor, string servidor, string usuario, string contraseña, string Database ="", string Ruta = "")
        {
            DbConnection conexion;

            switch (gestor.ToLower())
            {
                case "sqlserver":
                    conexion = new SqlConnection($"Data Source={servidor};Initial Catalog=master;User ID={usuario};Password={contraseña};MultipleActiveResultSets=true;");
                    break;
                case "mysql":
                    conexion = new MySqlConnection($"Server={servidor};User ID={usuario};Password={contraseña};");
                    break;
                case "postgresql":
                    conexion = new NpgsqlConnection($"Host={servidor};Username={usuario};Password={contraseña};Database={Database}");
                    break;
                case "oracle":
                    conexion = new OracleConnection($"User Id={usuario};Password={contraseña};Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={servidor})(PORT=1521))(CONNECT_DATA=(SID=XE)))");
                    //conexion = new OracleConnection($"Data Source={servidor};User Id={usuario};Password={contraseña};");
                    break;
                case "firebird":
                    conexion = new FbConnection($"DataSource={servidor};Database={Ruta};User={usuario};Password={contraseña};Charset=UTF8;");
                    break;
                default:
                    throw new ArgumentException("Sistema gestor no soportado");
            }

            return conexion;
        }


        public string GetCustomSQLConnection(string sServidor, string sUsuario, string sContraseña, string database)
        {
            string sSQLConexion = $"Data Source={sServidor};Initial Catalog={database};User ID={sUsuario};Password={sContraseña};MultipleActiveResultSets=true;";
            return sSQLConexion;
        }
        //Comprobar conexion a SQL Server
        public Boolean SiHayConexion(string sServidor, string sUsuario, string sContraseña)
        {
            Boolean bAllOk = false;

            string sSQLConexion = $"Data Source={sServidor};Initial Catalog=master;User ID={sUsuario};Password='{sContraseña}';";

            try
            {
                SqlConnection conexion = new SqlConnection(sSQLConexion);
                conexion.Open();
                conexion.Close();

                bAllOk = true;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 18488) // El número 18488 indica que la contraseña ha caducado
                {
                    sLastError = ex.Message;
                    lastsqlException = ex;
                }
                else
                {
                    sLastError = ex.Message;
                }
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                bAllOk = false;
            }

            return bAllOk;
        }

        //Comprobar conexion a MySQL
        public Boolean SiHayConexionMySQL(string sServidor, string sUsuario, string sContraseña)
        {
            Boolean bAllOk = false;

            string conexion = $"Server={sServidor}; Uid={sUsuario}; Pwd={sContraseña};";

            using (MySqlConnection conn = new MySqlConnection(conexion))
            {
                try
                {
                    conn.Open();
                    conn.Close();

                    bAllOk = true;
                }
                catch (Exception ex)
                {
                    sLastError = ex.Message;
                    bAllOk = false;
                }
            }

            return bAllOk;
        }
        // Comprobar conexión a PostgreSQL
        public Boolean SiHayConexionPostgreSQL(string sServidor, string sUsuario, string sContraseña)
        {
            Boolean bAllOk = false;

            string conexion = $"Host={sServidor};Username={sUsuario};Password={sContraseña};";

            using (NpgsqlConnection conn = new NpgsqlConnection(conexion))
            {
                try
                {
                    conn.Open();
                    conn.Close();

                    bAllOk = true;
                }
                catch (Exception ex)
                {
                    sLastError = ex.Message;
                    bAllOk = false;
                }
            }

            return bAllOk;
        }

        // Comprobar conexión a Oracle

        public Boolean SiHayConexionOracle(string sServidor, string sUsuario, string sContraseña)
        {
            Boolean bAllOk = false;

            string conexion = $"Data Source={sServidor};User Id={sUsuario};Password={sContraseña};";

            using (OracleConnection conn = new OracleConnection(conexion))
            {
                try
                {
                    conn.Open();
                    conn.Close();

                    bAllOk = true;
                }
                catch (Exception ex)
                {
                    sLastError = ex.Message;
                    bAllOk = false;
                }
            }

            return bAllOk;
        }


        //comprobar conexion en firebird
        public Boolean SiHayConexionFirebird(string sServidor, string sUsuario, string sContraseña, string sRutadb)
        {
            try
            {
                string conexion = $"DataSource={sServidor};Database={sRutadb};User={sUsuario};Password={sContraseña};Charset=UTF8;";
                
                using (FbConnection conn = new FbConnection(conexion))
                {
                    conn.Open();
                    conn.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                return false;
            }
        }

        public bool ExcecuteQuery(string query, SqlConnection connection)
        {
            Boolean bAllOk = false;
            try
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }
            catch(Exception ex) { MessageBox.Show( ex.Message ); }
            connection.Close();
            return bAllOk;
        }



        public void CargarBasesdeDatos(string sGestor, string sServidor, string sUsuario, string sContraseña, TreeView tv)
        {
            DbConnection conexion = GetDBConnection(sGestor, sServidor, sUsuario, sContraseña);
            DbCommand cm;
            string consulta = "";

            switch (sGestor.ToLower())
            {
                case "sqlserver":
                    consulta = "SELECT name FROM sys.databases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb');";
                    cm = new SqlCommand(consulta, (SqlConnection)conexion);
                    break;
                case "mysql":
                    consulta = "SHOW DATABASES;";
                    cm = new MySqlCommand(consulta, (MySqlConnection)conexion);
                    break;
                case "postgresql":
                    consulta = "SELECT datname FROM pg_database WHERE datistemplate = false;";
                    cm = new NpgsqlCommand(consulta, (NpgsqlConnection)conexion);
                    break;
                case "oracle":
                    consulta = "SELECT username FROM all_users;";
                    cm = new OracleCommand(consulta, (OracleConnection)conexion);
                    break;
                case "firebird":
                    consulta = "SELECT rdb$database_name FROM rdb$databases;";
                    cm = new FbCommand(consulta, (FbConnection)conexion);
                    break;
                default:
                    throw new ArgumentException("Sistema gestor no soportado");
            }

            try
            {
                conexion.Open();
                TreeNode nodoServidor = new TreeNode($"{sServidor}");
                tv.Nodes.Add(nodoServidor);

                DbDataAdapter da;
                DataTable dt = new DataTable();

                if (sGestor.ToLower() == "sqlserver")
                    da = new SqlDataAdapter((SqlCommand)cm);
                else if (sGestor.ToLower() == "mysql")
                    da = new MySqlDataAdapter((MySqlCommand)cm);
                else if (sGestor.ToLower() == "postgresql")
                    da = new NpgsqlDataAdapter((NpgsqlCommand)cm);
                else if (sGestor.ToLower() == "oracle")
                    da = new OracleDataAdapter((OracleCommand)cm);
                else if (sGestor.ToLower() == "firebird")
                    da = new FbDataAdapter((FbCommand)cm);
                else
                    throw new ArgumentException("Sistema gestor no soportado");

                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    TreeNode nodoBD = new TreeNode(dr[0].ToString());
                    nodoServidor.Nodes.Add(nodoBD);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar bases de datos: " + ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }
        //version nueva de la carga de base de datos DEPENDIENDO DEL SISTEMA GESTOR DE BASE DE DATOS
        public void CargarServidores(TreeView treeView, DbConnection conexion, string gestor, bool clearTreeView = true, Userdata userdata = null)
        {
            if (clearTreeView)
                treeView.Nodes.Clear();

            // Crear el nodo raíz del servidor
            string nombreNodo = userdata != null ? $"{userdata.SistemaGestor} - {userdata.Servidor}" : $"{gestor} - {conexion.Database}";
            TreeNode serverNode = new TreeNode(nombreNodo)
            {
                Tag = userdata // Almacenar Userdata en el Tag del nodo raíz
            };
            treeView.Nodes.Add(serverNode);

            try
            {
                conexion.Open();

                // Cargar bases de datos o esquemas según el gestor
                switch (gestor.ToLower())
                {
                    case "sqlserver":
                        CargarBasesDeDatosSQLServer(conexion, serverNode);
                        break;

                    case "mysql":
                        CargarBasesDeDatosMySQL(conexion, serverNode);
                        break;

                    case "postgresql":
                        CargarBasesDeDatosPostgreSQL(conexion, serverNode);
                        break;

                    case "oracle":
                        CargarBasesDeDatosOracle(conexion, serverNode);
                        break;

                    case "firebird":
                        CargarBasesDeDatosFirebird(conexion, serverNode);
                        break;

                    default:
                        throw new ArgumentException("Sistema gestor no soportado");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el TreeView: " + ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }

        // Métodos auxiliares para cada gestor de bases de datos

        private void CargarBasesDeDatosSQLServer(DbConnection conexion, TreeNode serverNode)
        {
            DataTable databases = conexion.GetSchema("Databases");
            foreach (DataRow database in databases.Rows)
            {
                string dbName = database["database_name"].ToString();
                TreeNode dbNode = new TreeNode(dbName) { Tag = "BaseDeDatos" };
                serverNode.Nodes.Add(dbNode);

                // Cargar tablas de la base de datos
                using (var cmd = conexion.CreateCommand())
                {
                    cmd.CommandText = $"USE [{dbName}]; SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE';";
                    using (var adapter = new SqlDataAdapter((SqlCommand)cmd))
                    {
                        DataTable tables = new DataTable();
                        adapter.Fill(tables);

                        foreach (DataRow table in tables.Rows)
                        {
                            string tableName = table["TABLE_NAME"].ToString();
                            TreeNode tableNode = new TreeNode(tableName) { Tag = "Tabla" };
                            dbNode.Nodes.Add(tableNode);
                        }
                    }
                }
            }
        }

        private void CargarBasesDeDatosMySQL(DbConnection conexion, TreeNode serverNode)
        {
            using (var cmd = new MySqlCommand("SHOW DATABASES", (MySqlConnection)conexion))
            using (var adapter = new MySqlDataAdapter(cmd))
            {
                DataTable databases = new DataTable();
                adapter.Fill(databases);

                foreach (DataRow database in databases.Rows)
                {
                    string dbName = database[0].ToString();
                    TreeNode dbNode = new TreeNode(dbName) { Tag = "BaseDeDatos" };
                    serverNode.Nodes.Add(dbNode);

                    // Cargar tablas de la base de datos
                    using (var cmdTablas = new MySqlCommand($"USE `{dbName}`; SHOW TABLES;", (MySqlConnection)conexion))
                    using (var adapterTablas = new MySqlDataAdapter(cmdTablas))
                    {
                        DataTable tables = new DataTable();
                        adapterTablas.Fill(tables);

                        foreach (DataRow table in tables.Rows)
                        {
                            string tableName = table[0].ToString();
                            TreeNode tableNode = new TreeNode(tableName) { Tag = "Tabla" };
                            dbNode.Nodes.Add(tableNode);
                        }
                    }
                }
            }
        }

        private void CargarBasesDeDatosPostgreSQL(DbConnection conexion, TreeNode serverNode)
        {
            using (var cmd = new NpgsqlCommand("SELECT datname FROM pg_database WHERE datistemplate = false;", (NpgsqlConnection)conexion))
            using (var adapter = new NpgsqlDataAdapter(cmd))
            {
                DataTable databases = new DataTable();
                adapter.Fill(databases);

                foreach (DataRow database in databases.Rows)
                {
                    string dbName = database["datname"].ToString();
                    TreeNode dbNode = new TreeNode(dbName) { Tag = "BaseDeDatos" };
                    serverNode.Nodes.Add(dbNode);

                    // Cargar tablas de la base de datos
                    using (var cmdTablas = new NpgsqlCommand(
                        $"SELECT table_name FROM information_schema.tables WHERE table_schema = 'public' AND table_type = 'BASE TABLE';",
                        (NpgsqlConnection)conexion))
                    using (var adapterTablas = new NpgsqlDataAdapter(cmdTablas))
                    {
                        DataTable tables = new DataTable();
                        adapterTablas.Fill(tables);

                        foreach (DataRow table in tables.Rows)
                        {
                            string tableName = table["table_name"].ToString();
                            TreeNode tableNode = new TreeNode(tableName) { Tag = "Tabla" };
                            dbNode.Nodes.Add(tableNode);
                        }
                    }
                }
            }
        }

        private void CargarBasesDeDatosOracle(DbConnection conexion, TreeNode serverNode)
        {
            using (OracleCommand cmd = new OracleCommand(
                "SELECT name, cdb FROM v$database",
                (OracleConnection)conexion))
            {
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        bool esCDB = reader["cdb"].ToString() == "YES";
                        string nombreCDB = reader["name"].ToString();

                        TreeNode cdbNode = new TreeNode($"CDB: {nombreCDB}");
                        serverNode.Nodes.Add(cdbNode);

                        if (esCDB)
                        {
                            // Obtener PDBs
                            using (OracleCommand cmdPDB = new OracleCommand(
                                "SELECT name FROM v$pdbs WHERE open_mode = 'READ WRITE'",
                                (OracleConnection)conexion))
                            using (OracleDataReader pdbReader = cmdPDB.ExecuteReader())
                            {
                                while (pdbReader.Read())
                                {
                                    string pdbName = pdbReader["name"].ToString();
                                    TreeNode pdbNode = new TreeNode($"PDB: {pdbName}");
                                    cdbNode.Nodes.Add(pdbNode);

                                    // Conectar a la PDB para obtener esquemas
                                    string connStringPDB = ((OracleConnection)conexion).ConnectionString
                                        .Replace($"Service Name={nombreCDB}", $"Service Name={pdbName}");

                                    using (OracleConnection pdbConnection = new OracleConnection(connStringPDB))
                                    {
                                        pdbConnection.Open();
                                        CargarEsquemasOracle(pdbConnection, pdbNode);
                                    }
                                }
                            }
                        }
                        else
                        {
                            // No es CDB, cargar esquemas directamente
                            CargarEsquemasOracle((OracleConnection)conexion, cdbNode);
                        }
                    }
                }
            }
        }

        private void CargarEsquemasOracle(OracleConnection conexion, TreeNode parentNode)
        {
            using (OracleCommand cmd = new OracleCommand(
                "SELECT username FROM all_users WHERE username NOT IN ('SYS', 'SYSTEM') ORDER BY username",
                conexion))
            using (OracleDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string schemaName = reader["username"].ToString();
                    TreeNode schemaNode = new TreeNode($"Esquema: {schemaName}") { Tag = "Esquema" };
                    parentNode.Nodes.Add(schemaNode);

                    // Cargar tablas del esquema
                    using (OracleCommand cmdTablas = new OracleCommand(
                        $"SELECT table_name FROM all_tables WHERE owner = '{schemaName}'",
                        conexion))
                    using (OracleDataReader tablasReader = cmdTablas.ExecuteReader())
                    {
                        while (tablasReader.Read())
                        {
                            string tablaNombre = tablasReader["table_name"].ToString();
                            TreeNode tablaNode = new TreeNode(tablaNombre) { Tag = "Tabla" };
                            schemaNode.Nodes.Add(tablaNode);
                        }
                    }
                }
            }
        }

        private void CargarBasesDeDatosFirebird(DbConnection conexion, TreeNode serverNode)
        {
            // En Firebird, cada conexión es una base de datos
            TreeNode firebirdNode = new TreeNode($"Firebird - {conexion.Database}") { Tag = "BaseDeDatos" };
            serverNode.Nodes.Add(firebirdNode);

            try
            {
                using (FbCommand cmd = new FbCommand(
                    "SELECT TRIM(RDB$RELATION_NAME) AS TABLE_NAME FROM RDB$RELATIONS WHERE RDB$SYSTEM_FLAG = 0;",
                    (FbConnection)conexion))
                using (FbDataAdapter adapter = new FbDataAdapter(cmd))
                {
                    DataTable tables = new DataTable();
                    adapter.Fill(tables);

                    foreach (DataRow table in tables.Rows)
                    {
                        string tableName = table["TABLE_NAME"].ToString();
                        TreeNode tableNode = new TreeNode(tableName) { Tag = "Tabla" };
                        firebirdNode.Nodes.Add(tableNode);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las tablas de Firebird: " + ex.Message);
            }
        }
        public void BasedeDatosEnComboBox(System.Windows.Forms.ComboBox comboBox, SqlConnection conexion)
        {
            try
            {
                conexion.Open();

                DataTable databases = conexion.GetSchema("Databases");
                foreach (DataRow database in databases.Rows)
                {
                    string dbName = database["database_name"].ToString();
                    comboBox.Items.Add(dbName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las bases de datos: " + ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }

        //cargar columnas de los nodos
        private void CargarColumnas(TreeNode tableNode, string dbName, string tableName, SqlConnection conexion)
        {
            try
            {
                // Obtener la lista de columnas de la tabla
                SqlCommand columnCmd = new SqlCommand($"USE [{dbName}]; SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'", conexion);
                SqlDataReader columnReader = columnCmd.ExecuteReader();

                while (columnReader.Read())
                {
                    // Nodo para cada columna con su tipo de dato
                    string columnName = columnReader["COLUMN_NAME"].ToString();
                    string dataType = columnReader["DATA_TYPE"].ToString();
                    TreeNode columnNode = new TreeNode($"{columnName} ({dataType})");
                    tableNode.Nodes.Add(columnNode);
                }

                columnReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las columnas: " + ex.Message);
            }
        }
        //carga columnas de datagridview
        public List<string> CargarColumnas(string sGestor, string db, string tablename, string sServidor, string sUsuario, string sContraseña)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            DataGridView dgvDataSource = new DataGridView();
            //MakeQueryInsert(dict, tablename, dgvDataSource, dataAdapter, sGestor, sServidor, sUsuario, sContraseña, db);

            List<string> Columnas = new List<string>();
            DbConnection conexion = GetDBConnection(sGestor, sServidor, sUsuario, sContraseña);
            DbCommand cmd;
            string query = "";

            switch (sGestor.ToLower())
            {
                case "sqlserver":
                    query = $"SELECT COLUMN_NAME FROM {db}.INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tablename}'";
                    cmd = new SqlCommand(query, (SqlConnection)conexion);
                    break;
                case "mysql":
                    query = $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '{db}' AND TABLE_NAME = '{tablename}';";
                    cmd = new MySqlCommand(query, (MySqlConnection)conexion);
                    break;
                case "postgresql":
                    query = $"SELECT column_name FROM information_schema.columns WHERE table_schema = 'public' AND table_name = '{tablename}';";
                    cmd = new NpgsqlCommand(query, (NpgsqlConnection)conexion);
                    break;
                case "oracle":
                    query = $"SELECT COLUMN_NAME FROM ALL_TAB_COLUMNS WHERE TABLE_NAME = UPPER('{tablename}')";
                    cmd = new OracleCommand(query, (OracleConnection)conexion);
                    break;
                case "firebird":
                    query = $"SELECT RDB$FIELD_NAME FROM RDB$RELATION_FIELDS WHERE RDB$RELATION_NAME = UPPER('{tablename}')";
                    cmd = new FbCommand(query, (FbConnection)conexion);
                    break;
                default:
                    throw new ArgumentException("Sistema gestor no soportado");
            }

            try
            {
                conexion.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Columnas.Add(reader[0].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al cargar columnas: " + ex.Message);
            }
            finally
            {
                conexion.Close();
            }

            return Columnas;
        }

        public bool cargaDatosDGV(ref DataTable tabla, string sNombreTabla, SqlConnection conexion, String db, SqlDataAdapter adaptador)
        {
            bool bAllOk = false;

            try
            {
                conexion.Open();
                string sConsultaSQL = $"USE {db}; SELECT * FROM \"{sNombreTabla}\"";
                SqlCommand cmd = new SqlCommand(sConsultaSQL, conexion);
                cmd.ExecuteNonQuery();

                adaptador.Fill(tabla);
                
                conexion.Close();
                bAllOk = true;
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                conexion.Close();
            }
            return bAllOk;
        }
        public bool DoInsert(SqlDataAdapter dataAdapter, ref DataTable tabla) //insert
        {
            bool bAllOk = false;
            try
            {
                // Actualiza los cambios en el DataTable
                dataAdapter.Update(tabla);

                MessageBox.Show("Datos guardados correctamente.");
                bAllOk = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                bAllOk = false;
            }

            return bAllOk;
        }
        public void DoInsertV2(Dictionary<string, object> columnValues, string sConexion, string InsertQuery)
        {
            using (SqlConnection connection = new SqlConnection(sConexion))
            {
                connection.Open();

                // Ejecutar la consulta INSERT dinámica
                using (SqlCommand command = new SqlCommand(InsertQuery, connection))
                {
                    // Agregar los parámetros con sus valores al comando
                    foreach (var kvp in columnValues)
                    {
                        command.Parameters.AddWithValue("@" + kvp.Key, kvp.Value);
                    }

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Se insertaron datos correctamente.");
                    }
                    else
                    {
                        MessageBox.Show("No se insertaron datos.");
                    }
                }
            }
        }
        //public void MakeQueryInsert(Dictionary<string, object> columnValues, string tableName, DataGridView dgvDataSource, DbDataAdapter dataAdapter, string sGestor, string sServidor, string sUsuario, string sContraseña, string db)
        //{
        //    try
        //    {
        //        GetRowsInfo(columnValues, dgvDataSource);
        //        DataGridViewRow currentRow = dgvDataSource.CurrentRow;

        //        if (currentRow == null)
        //        {
        //            MessageBox.Show("No hay una fila seleccionada para insertar.");
        //            return;
        //        }

        //        // Obtener valores de la fila seleccionada
        //        foreach (DataGridViewCell cell in currentRow.Cells)
        //        {
        //            string columnName = dgvDataSource.Columns[cell.ColumnIndex].HeaderText;
        //            object cellValue = cell.Value;

        //            if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue.ToString()))
        //            {
        //                columnValues[columnName] = cellValue;
        //            }
        //        }

        //        // Crear la consulta de inserción dinámica
        //        string insertColumns = string.Join(", ", columnValues.Keys);
        //        string insertValues = string.Join(", ", columnValues.Keys.Select(key => "@" + key));
        //        string insertQuery = $"INSERT INTO {tableName} ({insertColumns}) VALUES ({insertValues})";

        //        using (DbConnection conexion = GetDBConnection(sGestor, sServidor, sUsuario, sContraseña, db))
        //        {
        //            using (DbCommand command = conexion.CreateCommand())
        //            {
        //                command.CommandText = insertQuery;

        //                // Agregar los parámetros
        //                foreach (var kvp in columnValues)
        //                {
        //                    DbParameter parameter = command.CreateParameter();
        //                    parameter.ParameterName = "@" + kvp.Key;
        //                    parameter.Value = kvp.Value ?? DBNull.Value;
        //                    command.Parameters.Add(parameter);
        //                }

        //                conexion.Open();
        //                int rowsAffected = command.ExecuteNonQuery();
        //                conexion.Close();

        //                MessageBox.Show(rowsAffected > 0 ? "Se insertaron datos correctamente." : "No se insertaron datos.");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error: " + ex.Message);
        //    }
        //}
        public void GetRowsInfo(Dictionary<string, object> columnValues, DataGridView dgvDataSource)
        {
            DataGridViewRow currentRow = dgvDataSource.CurrentRow;

            // Recorrer las columnas del DataGridView y obtener los datos de la fila
            foreach (DataGridViewCell cell in currentRow.Cells)
            {
                string columnName = dgvDataSource.Columns[cell.ColumnIndex].HeaderText;

                object cellValue = cell.Value;

                // Verificar si el valor no es nulo y no está en blanco
                if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue.ToString()))
                {
                    // Agregar el nombre de la columna y el valor al diccionario
                    columnValues[columnName] = cellValue;
                }
            }
        }//obtiene la informacion de las celdas parta poder armar el insert
        public bool Update(SqlDataAdapter dataAdapter, ref DataTable tabla)
        {
            bool bAllOk = false;
            try
            {
                // Actualiza los cambios en el DataTable
                dataAdapter.Update(tabla);

                MessageBox.Show("Datos guardados correctamente.");
                bAllOk = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                bAllOk = false;
            }

            return bAllOk;
        }
        public void Select(DataGridView dataGridView, string query, SqlConnection sSQLConection)
        {
            try
            {
                sSQLConection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(query, sSQLConection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView.DataSource = dataTable;
                MessageBox.Show("Comando realizado correctamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ejecutar la consulta: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Delete(DataGridView dgvDataSource, string tablename, string db, string sServidor, string sNombre, string sContraseña)
        {
            try
            {
                if (dgvDataSource.SelectedRows.Count > 0)
                {
                    // Obtener la fila seleccionada
                    DataGridViewRow selectedRow = dgvDataSource.SelectedRows[0];

                    // Obtener el nombre de la columna de la clave primaria (asumiendo que es la primera columna)
                    string primaryKeyColumnName = dgvDataSource.Columns[0].Name; // Cambia [0] si no es la primera columna

                    // Obtener el valor de la clave primaria de la fila seleccionada
                    object primaryKeyValue = selectedRow.Cells[primaryKeyColumnName].Value;

                    if (primaryKeyValue != null)
                    {
                        // Realizar la consulta SQL DELETE
                        string query = $"USE {db}; DELETE FROM {tablename} WHERE {primaryKeyColumnName} = @PrimaryKeyValue"; // Reemplaza con tu variable tablename y columna de clave primaria
                        using (SqlConnection connection = new SqlConnection($"Data Source={sServidor};Initial Catalog={db};User ID={sNombre};Password={sContraseña};"))
                        {
                            connection.Open();
                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@PrimaryKeyValue", primaryKeyValue);
                                int rowsAffected = command.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    // La fila se ha eliminado correctamente de la base de datos
                                    // También puedes eliminar la fila del DataGridView
                                    dgvDataSource.Rows.Remove(selectedRow);
                                }
                                else
                                {
                                    MessageBox.Show("No se pudo eliminar la fila.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se pudo obtener el valor de la clave primaria.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void MustChangeQuery(string sNombre, string contraQuery, SqlConnection conexion)
        {
            string createUserQuery = $"ALTER LOGIN [{sNombre}] WITH PASSWORD = '{contraQuery}', CHECK_POLICY = ON, CHECK_EXPIRATION = ON;";
            if(ExcecuteQuery(createUserQuery, conexion))
            {
                MessageBox.Show("Se cambio la contraseña exitosamente");
                
            }
        }
    }
}