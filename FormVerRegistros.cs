using Reglas_de_Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseDeDatosSQL
{
    public partial class FormVerRegistros : Form
    {
        AccesoSQLServer accesoSQLServer = new AccesoSQLServer();
        private string sNombre;    // Variable para almacenar el nombre de usuario
        private string sContraseña; // Variable para almacenar la contraseña
        private string sServidor;  // Variable para almacenar el servidor
        private string sGestor;  // Variable para almacenar el Sistema Gestor
        string sSQLConnection;
        string tablename;
        string db;

        private SqlDataAdapter dataAdapter;


        private DataTable tabla = new DataTable();

        public FormVerRegistros(System.Windows.Forms.TreeView treeview, Userdata userdata, string tablename, string db)
        {
            InitializeComponent();

            this.tablename = tablename;
            this.db = db;

            sNombre = userdata.Usuario;
            sContraseña = userdata.Contraseña;
            sServidor = userdata.Servidor;
            sGestor = userdata.SistemaGestor;
            DbConnection conexion = accesoSQLServer.GetDBConnection(sGestor, sServidor, sNombre, sContraseña);
        }

        private void FormVerRegistros_Load(object sender, EventArgs e)
        {
            // Crear el menú contextual
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            ToolStripMenuItem borrarFilaMenuItem = new ToolStripMenuItem("Borrar fila");
            borrarFilaMenuItem.Click += BorrarFilaMenuItem_Click; // Manejador del evento
            contextMenuStrip.Items.Add(borrarFilaMenuItem);

            // Asociar el menú contextual con el DataGridView
            dgvDataSource.ContextMenuStrip = contextMenuStrip;


            // Configurar SqlDataAdapter y SqlCommandBuilder
            SqlConnection conexion = new SqlConnection(sSQLConnection);
            dataAdapter = new SqlDataAdapter($"USE {db}; SELECT * FROM \"{tablename}\"", conexion);
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

            if (accesoSQLServer.cargaDatosDGV(ref tabla, tablename, conexion, db, dataAdapter))
            {
                dgvDataSource.DataSource = tabla;

            }
            else
            {
                MessageBox.Show("Algo no salió bien: " + accesoSQLServer.sLastError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BorrarFilaMenuItem_Click(object sender, EventArgs e)
        {
            accesoSQLServer.Delete(dgvDataSource, tablename, db, sServidor, sNombre, sContraseña);
        }

    }
}
