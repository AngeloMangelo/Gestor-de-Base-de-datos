﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;
using MySql.Data.MySqlClient;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using Reglas_de_Negocio;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BaseDeDatosSQL
{
    public partial class Sistema : Form
    {

        private Userdata userdata; // Variable para almacenar la instancia de UserData
        AccesoSQLServer accesoSQLServer = new AccesoSQLServer();
        private string sNombre;    // Variable para almacenar el nombre de usuario
        private string sContraseña; // Variable para almacenar la contraseña
        private string sServidor;  // Variable para almacenar el servidor
        private string sGestor;    // Variable para almacenar el gestor de base de datos

        private string selectedTablename;
        private string SelectedDb;


        public Sistema(Userdata userdata)
        {
            InitializeComponent();
            this.userdata = userdata;
            treeViewAsistente.BeforeExpand += treeViewAsistente_BeforeExpand;
        }

        private void Sistema_Load(object sender, EventArgs e)
        {
            treeViewAsistente.AfterSelect += treeViewAsistente_AfterSelect;

            sNombre = userdata.Usuario;
            sContraseña = userdata.Contraseña;
            sServidor = userdata.Servidor;
            sGestor = userdata.SistemaGestor;

            //SqlConnection conexion = new SqlConnection(accesoSQLServer.GetDBConnection(sGestor, sServidor, sNombre, sContraseña));
            DbConnection conexion = accesoSQLServer.GetDBConnection(sGestor, sServidor, sNombre, sContraseña);


            treeViewAsistente.ShowPlusMinus = true;  // Muestra los botones de expansión
            treeViewAsistente.ShowRootLines = true;  // Muestra las líneas de expansión en el nodo raíz (opcional)
            treeViewAsistente.Font = new Font(treeViewAsistente.Font, FontStyle.Bold);

            accesoSQLServer.CargarServidores(treeViewAsistente, conexion, sGestor, true, userdata);
        }
        private void treeViewAsistente_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
        }

        private void treeViewAsistente_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

        }

        private void treeViewTablas_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {

        }

        private void btnAddBD_Click(object sender, EventArgs e)
        {
            FormAddBD formAddBD = new FormAddBD(userdata);
            formAddBD.ShowDialog();
        }

        private void btnSearchDB_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Archivos de base de datos (*.mdf)|*.mdf|Todos los archivos (*.*)|*.*";
                openFileDialog.Title = "Seleccionar Base de Datos";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string connectionString = $"Data Source={sServidor};Integrated Security=True;";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        try
                        {
                            connection.Open();

                            // Crear un nodo raíz para el TreeView
                            treeViewAsistente.Nodes.Clear();
                            TreeNode rootNode = new TreeNode("Base de Datos");
                            treeViewAsistente.Nodes.Add(rootNode);

                            // Obtener las tablas de la base de datos
                            DataTable tables = connection.GetSchema("Tables");
                            foreach (DataRow table in tables.Rows)
                            {
                                string tableName = table["TABLE_NAME"].ToString();
                                TreeNode tableNode = new TreeNode(tableName);
                                tableNode.Tag = "Tabla"; // Puedes usar el Tag para identificar el tipo de nodo
                                rootNode.Nodes.Add(tableNode);

                                // Obtener las columnas de la tabla
                                DataTable columns = connection.GetSchema("Columns", new[] { null, null, tableName });
                                foreach (DataRow column in columns.Rows)
                                {
                                    string columnName = column["COLUMN_NAME"].ToString();
                                    TreeNode columnNode = new TreeNode(columnName);
                                    columnNode.Tag = "Columna"; // Puedes usar el Tag para identificar el tipo de nodo
                                    tableNode.Nodes.Add(columnNode);
                                }
                            }

                            // Expandir el nodo raíz para mostrar la estructura
                            rootNode.Expand();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al cargar la estructura de la base de datos: " + ex.Message);
                        }
                    }
                }
            }
        }
        private void MenuItemAgregarTabla_Click(object sender, EventArgs e) //Abrir Crear Tabla
        {
            FormCreateTable formCreateTable = new FormCreateTable(treeViewAsistente, userdata);
            formCreateTable.ShowDialog();
        }
        private void MenuItemAgregarRegistro_Click(object sender, EventArgs e) //Abrir Insertar Registro
        {
            string tablename = selectedTablename;
            string db = SelectedDb;
            FormInsert formInsert = new FormInsert(treeViewAsistente, userdata, tablename, db);
            formInsert.ShowDialog();
        }
        private void MenuItemActualizarRegistro_Click(object sender, EventArgs e) //Abrir Insertar Registro
        {
            string tablename = selectedTablename;
            string db = SelectedDb;
            FormUpdate formUpdate = new FormUpdate(treeViewAsistente, userdata, tablename, db);
            formUpdate.ShowDialog();
        }
        private void MenuItemVerRegistros_Click(object sender, EventArgs e) //Abrir Ver Registro (para el DELETE)
        {
            string tablename = selectedTablename;
            string db = SelectedDb;
            FormVerRegistros formVerRegistros = new FormVerRegistros(treeViewAsistente, userdata, tablename, db);
            formVerRegistros.ShowDialog();
        }
        private void MenuItemSelect_Click(object sender, EventArgs e) //Abrir Ver select
        {
            string tablename = selectedTablename;
            string db = SelectedDb;
            FormSelect formSelect = new FormSelect(treeViewAsistente, userdata, db);
            formSelect.ShowDialog();
        }

        private void btnRefreshDB_Click(object sender, EventArgs e)
        {
            sNombre = userdata.Usuario;
            sContraseña = userdata.Contraseña;
            sServidor = userdata.Servidor;

            DbConnection conexion = accesoSQLServer.GetDBConnection(sGestor, sServidor, sNombre, sContraseña);

            accesoSQLServer.CargarServidores(treeViewAsistente, conexion, sGestor);
        }

        private void treeViewAsistente_MouseClick(object sender, MouseEventArgs e)
        {
            ContextMenuStrip contextMenuTreeView = new ContextMenuStrip();

            ToolStripMenuItem menuItemAgregarTabla = new ToolStripMenuItem("Agregar Tabla");
            ToolStripMenuItem menuItemAgregarRegistro = new ToolStripMenuItem("Agregar Registro...");
            ToolStripMenuItem menuItemActualizarRegistro = new ToolStripMenuItem("Actualizar registro...");
            ToolStripMenuItem menuItemVerRegistros = new ToolStripMenuItem("Ver Registros...");
            ToolStripMenuItem menuItemSelect = new ToolStripMenuItem("Select...");

            menuItemAgregarTabla.Click += MenuItemAgregarTabla_Click;
            menuItemAgregarRegistro.Click += MenuItemAgregarRegistro_Click;
            menuItemActualizarRegistro.Click += MenuItemActualizarRegistro_Click;
            menuItemVerRegistros.Click += MenuItemVerRegistros_Click;
            menuItemSelect.Click += MenuItemSelect_Click;
            
            treeViewAsistente.ContextMenuStrip = contextMenuTreeView;

            if (e.Button == MouseButtons.Right)
            {
                TreeNode selectedNode = treeViewAsistente.GetNodeAt(e.Location);

                if (selectedNode != null)
                {
                    if (selectedNode.Tag != null && selectedNode.Tag.ToString() == "BaseDeDatos")
                    {
                        contextMenuTreeView.Items.Add(menuItemAgregarTabla);
                        // El nodo representa una base de datos, muestra el menú contextual para bases de datos
                        contextMenuTreeView.Show(treeViewAsistente, e.Location);
                    }
                    else if (selectedNode.Tag != null && selectedNode.Tag.ToString() == "Tabla")
                    {
                        selectedTablename = selectedNode.Text;
                        SelectedDb = selectedNode.Parent.ToString();
                        SelectedDb = SelectedDb.Remove(0,10); //eliminar "Treenode: "

                        contextMenuTreeView.Items.Add(menuItemAgregarRegistro);
                        contextMenuTreeView.Items.Add(menuItemActualizarRegistro);
                        contextMenuTreeView.Items.Add(menuItemVerRegistros);
                        contextMenuTreeView.Items.Add(menuItemSelect);
                        // El nodo representa una tabla, muestra el menú contextual para tablas
                        contextMenuTreeView.Show(treeViewAsistente, e.Location);
                    }
                }
            }
        }

        private void llCreateLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormCreateUser formCreateUser = new FormCreateUser(userdata);
            formCreateUser.ShowDialog();
        }

        private void Sistema_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit(); 
        }

        private void btnEjecutarQuery_Click(object sender, EventArgs e)
        {
            // Obtener el query escrito en el RichTextBox
            string query = rtbQuery.Text;
            if (string.IsNullOrWhiteSpace(query))
            {
                MessageBox.Show("Por favor, escribe un query a ejecutar.");
                return;
            }

            // Verificar que se haya seleccionado una base de datos en el ComboBox
            if (cbBaseDeDatos.SelectedItem == null)
            {
                MessageBox.Show("No hay ninguna base de datos seleccionada.");
                return;
            }
            string selectedDatabase = cbBaseDeDatos.SelectedItem.ToString();
            userdata.BaseDeDatos = selectedDatabase;

            // Obtener el nodo seleccionado en el TreeView.
            TreeNode selectedNode = treeViewAsistente.SelectedNode;
            if (selectedNode == null)
            {
                MessageBox.Show("Por favor, selecciona un servidor en el TreeView.");
                return;
            }

            // Si el nodo seleccionado es de tipo "BaseDeDatos", usamos su nodo padre (el servidor)
            if (selectedNode.Tag != null && selectedNode.Tag.ToString() == "BaseDeDatos")
            {
                selectedNode = selectedNode.Parent;
            }

            // Verificamos si el nodo tiene el objeto Userdata
            if (selectedNode.Tag == null || !(selectedNode.Tag is Userdata))
            {
                MessageBox.Show("No se encontró la información de conexión en el nodo seleccionado.");
                return;
            }
            Userdata conexionData = (Userdata)selectedNode.Tag;

            // Crear la conexión a partir de la información del servidor y el gestor.
            DbConnection connection = accesoSQLServer.GetDBConnection(
                conexionData.SistemaGestor,
                conexionData.Servidor,
                conexionData.Usuario,
                conexionData.Contraseña
            );

            try
            {
                connection.Open();
                DbCommand cmd = connection.CreateCommand();

                // Para algunos gestores es necesario cambiar a la base de datos seleccionada
                switch (conexionData.SistemaGestor.ToLower())
                {
                    case "sqlserver":
                        cmd.CommandText = $"USE [{selectedDatabase}]; {query}";
                        break;
                    case "mysql":
                        cmd.CommandText = $"USE `{selectedDatabase}`; {query}";
                        break;
                    case "postgresql":
                        // En PostgreSQL se suele especificar la base de datos en el connection string.
                        // Aquí se cierra la conexión actual y se abre una nueva conexión con la base de datos seleccionada.
                        connection.Close();
                        connection = accesoSQLServer.GetDBConnection(
                            conexionData.SistemaGestor,
                            conexionData.Servidor,
                            conexionData.Usuario,
                            conexionData.Contraseña,
                            conexionData.BaseDeDatos
                        );
                        connection.Open();
                        cmd = connection.CreateCommand();
                        cmd.CommandText = query;
                        break;
                    case "oracle":
                        // Si el usuario no especificó el esquema en la consulta, se lo agregamos
                        if (!query.ToLower().Contains("from") && !query.ToLower().Contains("into"))
                        {
                            query = $"ALTER SESSION SET CURRENT_SCHEMA = {selectedDatabase}; {query}";
                        }
                        cmd.CommandText = query;
                        break;

                    case "firebird":
                        cmd.CommandText = query;
                        break;
                    default:
                        MessageBox.Show("Gestor de base de datos no soportado para ejecutar queries.");
                        return;
                }

                // Si el query es un SELECT, se puede intentar llenar un DataTable y contar las filas
                if (query.TrimStart().StartsWith("select", StringComparison.OrdinalIgnoreCase))
                {
                    DataTable dt = new DataTable();
                    DbDataAdapter adapter = null;
                    if (conexionData.SistemaGestor.ToLower() == "sqlserver")
                    {
                        adapter = new SqlDataAdapter((SqlCommand)cmd);
                    }
                    else if (conexionData.SistemaGestor.ToLower() == "mysql")
                    {
                        adapter = new MySqlDataAdapter((MySqlCommand)cmd);
                    }
                    else if (conexionData.SistemaGestor.ToLower() == "postgresql")
                    {
                        adapter = new NpgsqlDataAdapter((NpgsqlCommand)cmd);
                    }
                    else if (conexionData.SistemaGestor.ToLower() == "oracle")
                    {
                        adapter = new OracleDataAdapter((OracleCommand)cmd);
                    }
                    else if (conexionData.SistemaGestor.ToLower() == "firebird")
                    {
                        adapter = new FbDataAdapter((FbCommand)cmd);
                    }

                    if (adapter != null)
                    {
                        adapter.Fill(dt);
                        MessageBox.Show($"Query ejecutado correctamente. Filas retornadas: {dt.Rows.Count}");
                    }
                    else
                    {
                        MessageBox.Show("No se pudo ejecutar el query en el gestor seleccionado.");
                    }
                }
                else
                {
                    // Para queries que no retornan resultados (INSERT, UPDATE, DELETE, etc.)
                    int rowsAffected = cmd.ExecuteNonQuery();
                    MessageBox.Show($"Query ejecutado correctamente. Filas afectadas: {rowsAffected}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ejecutar el query: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }


        private void btnNuevaConexion_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.ShowDialog();

            if (login.bSesionIniciada)
            {
                Userdata nuevaConexionUserdata = login.userdata;

                DbConnection nuevaConexion = accesoSQLServer.GetDBConnection(
                    nuevaConexionUserdata.SistemaGestor,
                    nuevaConexionUserdata.Servidor,
                    nuevaConexionUserdata.Usuario,
                    nuevaConexionUserdata.Contraseña
                );

                // Pasar "nuevaConexionUserdata" como último parámetro
                accesoSQLServer.CargarServidores(
                    treeViewAsistente,
                    nuevaConexion,
                    nuevaConexionUserdata.SistemaGestor,
                    clearTreeView: false,
                    userdata: nuevaConexionUserdata
                );
            }
        }

        private void treeViewAsistente_AfterSelect(object sender, TreeViewEventArgs e)
        {
            cbBaseDeDatos.Items.Clear();

            TreeNode selectedNode = e.Node;
            if (selectedNode == null)
                return;

            // Si el nodo seleccionado es una base de datos, usamos su nodo padre (el servidor)
            if (selectedNode.Tag != null && selectedNode.Tag.ToString() == "BaseDeDatos")
            {
                selectedNode = selectedNode.Parent;
            }

            // Asumimos que el nodo servidor no tiene Tag o tiene un Tag diferente a "BaseDeDatos"
            if (selectedNode != null)
            {
                // Recorremos los nodos hijos (que deben ser las bases de datos)
                foreach (TreeNode child in selectedNode.Nodes)
                {
                    if (child.Tag != null && child.Tag.ToString() == "BaseDeDatos")
                    {
                        cbBaseDeDatos.Items.Add(child.Text);
                    }
                }

                if (cbBaseDeDatos.Items.Count > 0)
                    cbBaseDeDatos.SelectedIndex = 0;
            }
        }
    }
}
