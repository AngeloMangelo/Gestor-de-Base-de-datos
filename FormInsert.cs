using Reglas_de_Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseDeDatosSQL
{
    public partial class FormInsert : Form
    {
        private System.Windows.Forms.TreeView tv; // Variable para almacenar la instancia de Sistema que contiene el treeview para obtener el nombre de la base de datos
        private Userdata userdata; // Variable para almacenar la instancia de UserData
        AccesoSQLServer accesoSQLServer = new AccesoSQLServer();
        private string sNombre;    // Variable para almacenar el nombre de usuario
        private string sContraseña; // Variable para almacenar la contraseña
        private string sServidor;  // Variable para almacenar el servidor
        string sSQLConnection;
        string tablename;
        string db;

        private ContextMenuStrip imageContextMenuStrip;

        DataTable tabla = new DataTable();
        SqlDataAdapter dataAdapter;
        private SqlCommandBuilder commandBuilder;

        public FormInsert(System.Windows.Forms.TreeView treeview, Userdata userdata, string tablename, string db)
        {
            InitializeComponent();
            this.tv = treeview;
            this.userdata = userdata;
            this.tablename = tablename;
            this.db = db;

            sNombre = userdata.Usuario;
            sContraseña = userdata.Contraseña;
            sServidor = userdata.Servidor;
            sSQLConnection = sSQLConnection = $"Data Source={sServidor};Initial Catalog={db};User ID={sNombre};Password={sContraseña};";
        }


        private void FormInsert_Load(object sender, EventArgs e)
        {
            imageContextMenuStrip = new ContextMenuStrip();
            ToolStripMenuItem abrirImagenMenuItem = new ToolStripMenuItem("Abrir Imagen");
            abrirImagenMenuItem.Click += AbrirImagenMenuItem_Click;
            imageContextMenuStrip.Items.Add(abrirImagenMenuItem);

            dgvDataSource.ContextMenuStrip = imageContextMenuStrip;

            // Configurar SqlDataAdapter y SqlCommandBuilder
            SqlConnection conexion = new SqlConnection(sSQLConnection);
            dataAdapter = new SqlDataAdapter($"SELECT * FROM \"{tablename}\"", conexion);
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

            if (accesoSQLServer.cargaDatosDGV(ref tabla, tablename, conexion, db, dataAdapter))
            {
                dgvDataSource.DataSource = tabla;
                ReadOnlyIdentity();
            }
            else
            {
                MessageBox.Show("Algo no salió bien: " + accesoSQLServer.sLastError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void AbrirImagenMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewCell selectedCell = dgvDataSource.CurrentCell;

            if (selectedCell != null)
            {
                byte[] existingImageData = selectedCell.Value as byte[];

                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Archivos de imagen|.jpg;.jpeg;.png;.gif;.bmp|Todos los archivos|.";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    byte[] selectedImageData = File.ReadAllBytes(openFileDialog.FileName);

                    selectedCell.Value = selectedImageData;
                }
            }
        }
        private void btnInsert_Click(object sender, EventArgs e)
        {
            tabla.TableName = tablename;

            // Crear un diccionario para almacenar los datos de la fila actual
            Dictionary<string, object> columnValues = new Dictionary<string, object>();

            //Aplicar Query
            accesoSQLServer.DoInsert(dataAdapter, ref tabla);
        }
        private void ReadOnlyIdentity()
        {
            if (dgvDataSource.Columns.Count > 0)
            {
                // Supongamos que deseas configurar la primera columna como solo lectura
                dgvDataSource.Columns[0].ReadOnly = true;
            }
        }
    }
}
