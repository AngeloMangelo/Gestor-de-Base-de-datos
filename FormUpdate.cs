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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BaseDeDatosSQL
{
    public partial class FormUpdate : Form
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


        DataTable tabla = new DataTable();
        SqlDataAdapter dataAdapter;
        public FormUpdate(System.Windows.Forms.TreeView treeview, Userdata userdata, string tablename, string db)
        {
            InitializeComponent();
            this.tv = treeview;
            this.userdata = userdata;
            this.tablename = tablename;
            this.db = db;

            sNombre = userdata.Usuario;
            sContraseña = userdata.Contraseña;
            sServidor = userdata.Servidor;
            sSQLConnection = $"Data Source={sServidor};Initial Catalog={db};User ID={sNombre};Password={sContraseña};";
        }
        private void FormUpdate_Load(object sender, EventArgs e)
        {

            // Configurar SqlDataAdapter y SqlCommandBuilder
            SqlConnection conexion = new SqlConnection(sSQLConnection);
            dataAdapter = new SqlDataAdapter($"SELECT * FROM {tablename}", conexion);
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

            if (accesoSQLServer.cargaDatosDGV(ref tabla, tablename, conexion, db, dataAdapter))
            {
                dgvDataSource.DataSource = tabla;
                // ReadOnlyIdentity();
            }
            else
            {
                MessageBox.Show("Algo no salió bien: " + accesoSQLServer.sLastError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            accesoSQLServer.Update(dataAdapter, ref tabla);
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
