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
    public partial class FormSelect : Form
    {
        private System.Windows.Forms.TreeView tv; // Variable para almacenar la instancia de Sistema que contiene el treeview para obtener el nombre de la base de datos
        private Userdata userdata; // Variable para almacenar la instancia de UserData
        AccesoSQLServer accesoSQLServer = new AccesoSQLServer();
        private string sNombre;    // Variable para almacenar el nombre de usuario
        private string sContraseña; // Variable para almacenar la contraseña
        private string sServidor;  // Variable para almacenar el servidor
        private string sGestor;    // Variable para almacenar el gestor
        string sSQLConnection;
        string tablename;
        string db;

        private DataTable tabla = new DataTable();
        private SqlDataAdapter dataAdapter;
        public FormSelect(System.Windows.Forms.TreeView treeview, Userdata userdata, string db)
        {
            InitializeComponent();
            this.tv = treeview;
            this.userdata = userdata;
            this.db = db;

            sNombre = userdata.Usuario;
            sContraseña = userdata.Contraseña;
            sServidor = userdata.Servidor;
            sGestor = userdata.SistemaGestor;
            DbConnection conexion = accesoSQLServer.GetDBConnection(sGestor, sServidor, sNombre, sContraseña);           
        }
        private void btnEjecutarQuery_Click(object sender, EventArgs e)
        {
            string sQuery = rtbQuery.Text;
            SqlConnection conexion = new SqlConnection(sSQLConnection);

            accesoSQLServer.Select(dgvDataSource, sQuery, conexion);
        }

        private void FormSelect_Load(object sender, EventArgs e)
        {
            rtbQuery.Text = $"USE {db};";
        }
    }
}
