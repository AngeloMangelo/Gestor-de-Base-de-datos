using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Drawing;
using System.Collections;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Net.Configuration;
using System.Data.Common;
using System.Collections.ObjectModel;

namespace Reglas_de_Negocio
{
    public class AccesoSQLServer
    {
        public String sLastError = string.Empty;
        public SqlException lastsqlException = null;
        public string GetSQLConnection(string sServidor, string sUsuario, string sContraseña)
        {
            string sSQLConexion = $"Data Source={sServidor};Initial Catalog=master;User ID={sUsuario};Password={sContraseña};MultipleActiveResultSets=true;";
            return sSQLConexion;
        }
        public string GetCustomSQLConnection(string sServidor, string sUsuario, string sContraseña, string database)
        {
            string sSQLConexion = $"Data Source={sServidor};Initial Catalog={database};User ID={sUsuario};Password={sContraseña};MultipleActiveResultSets=true;";
            return sSQLConexion;
        }
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

        public void CargarBasesdeDatos(string sServidor, string sUsuario, string sContraseña, System.Windows.Forms.TreeView tv)
        {
            string sConexion = GetSQLConnection(sServidor, sUsuario, sContraseña);
            SqlConnection conexion = new SqlConnection(sConexion);

            SqlCommand cm = new SqlCommand("SELECT NAME FROM DATABASES databases WHERE NAME NOT IN('master', 'tempdb', 'model', 'msdb');", conexion);
            try
            {
                TreeNode nodo = new TreeNode($"{sServidor}");
                tv.Nodes.Add(nodo);
                SqlDataAdapter da = new SqlDataAdapter(cm);
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    TreeNode node = new TreeNode(dr["NAME"].ToString());
                    tv.Nodes.Add(node);
                }
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
            }

        }
        //version nueva de la carga de base de datos
        public void CargarServidores(System.Windows.Forms.TreeView treeView, SqlConnection conexion)
        {
            treeView.Nodes.Clear();

            TreeNode serverNode = new TreeNode(conexion.DataSource);
            treeView.Nodes.Add(serverNode);

            try
            {
                conexion.Open();

                DataTable databases = conexion.GetSchema("Databases");
                foreach (DataRow database in databases.Rows)
                {
                    string dbName = database["database_name"].ToString();
                    TreeNode dbNode = new TreeNode(dbName);
                    dbNode.Tag = "BaseDeDatos"; // Asigna el tipo de nodo como "BaseDeDatos"
                    serverNode.Nodes.Add(dbNode);

                    SqlCommand cmd = new SqlCommand($"USE [{dbName}]; SELECT table_name FROM information_schema.tables WHERE table_type = 'BASE TABLE'", conexion);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string tableName = reader["table_name"].ToString();

                        TreeNode tableNode = new TreeNode(tableName);
                        tableNode.Tag = "Tabla"; // Asigna el tipo de nodo como "Tabla"
                        dbNode.Nodes.Add(tableNode);

                        CargarColumnas(tableNode, dbName, tableName, conexion);
                    }

                    reader.Close();
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
        public List<string> CargarColumnas(string db, string tablename, string sServidor, string sUsuario, string sContraseña)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            DataGridView dgvDataSource = new DataGridView();
            MakeQueryInsert(dict, tablename, dgvDataSource, dataAdapter, sServidor, sUsuario, sContraseña, db);
            List<string> Columnas = new List<string>();
            string sConexion = GetSQLConnection(sServidor, sUsuario, sContraseña);

            try
            {
                using (SqlConnection conexion = new SqlConnection(sConexion))
                {
                    conexion.Open();
                    string query = $"SELECT COLUMN_NAME FROM {db}.INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = \"{tablename}\"";

                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    {
                       // cmd.Parameters.AddWithValue("@Tabla", $"{tablename}");
                        SqlDataReader lectura = cmd.ExecuteReader();

                        while (lectura.Read())
                        {
                            Columnas.Add(lectura[0].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones: aquí puedes registrar errores o mostrar mensajes de error.
                Console.WriteLine("Error: " + ex.Message);
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
        public void MakeQueryInsert(Dictionary<string, object> columnValues, string tableName, DataGridView dgvDataSource, SqlDataAdapter dataAdapter, string sServidor, string sUsuario, string sContraseña, string db)
        {
            try
            {
                GetRowsInfo(columnValues, dgvDataSource);

                DataGridViewRow currentRow = dgvDataSource.CurrentRow;

                // Recorrer las columnas del DataGridView y obtener los datos de la fila
                foreach (DataGridViewCell cell in currentRow.Cells)
                {
                    // Obtener el nombre de la columna desde el encabezado de la columna
                    string columnName = dgvDataSource.Columns[cell.ColumnIndex].HeaderText;

                    object cellValue = cell.Value;

                    if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue.ToString()))
                    {
                        // Agregar el nombre de la columna y el valor al diccionario
                        columnValues[columnName] = cellValue;
                    }
                }

                // Crear una consulta INSERT dinámica basada en los datos de la fila
                string insertColumns = string.Join(", ", columnValues.Keys);
                string insertValues = string.Join(", ", columnValues.Keys.Select(key => "@" + key));
                string insertQuery = $"USE {db}; INSERT INTO {tableName} ({insertColumns}) VALUES ({insertValues})";

                string sSQLConnection = GetSQLConnection(sServidor, sUsuario, sContraseña);
                using (SqlConnection conexion = new SqlConnection(sSQLConnection))
                {
                    conexion.Open();
                    using (SqlCommand command = new SqlCommand(insertQuery, conexion))
                    {
                        // Agregar los parámetros
                        foreach (var kvp in columnValues)
                        {
                            command.Parameters.AddWithValue("@" + kvp.Key, kvp.Value);

                        }

                        int rowsAffected = command.ExecuteNonQuery();
                        conexion.Close();

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
            }catch(Exception ex)
            {
                MessageBox.Show( ex.Message );
            }
        }
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