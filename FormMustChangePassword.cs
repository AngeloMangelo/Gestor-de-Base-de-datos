using Reglas_de_Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseDeDatosSQL
{
    public partial class FormMustChangePassword : Form
    {
        AccesoSQLServer accesoSQLServer = new AccesoSQLServer();
        string sServidor;
        string sNombre;
        string sContraseña;
        string sGestor;
        string db;

        public FormMustChangePassword(string sGestor, string sServidor, string sNombre, string sContraseña)
        {
            InitializeComponent();
            this.sServidor = sServidor;
            this.sNombre = sNombre;
            this.sContraseña = sContraseña;
            this.sGestor = sGestor;
            this.db = db;
        }

        private void FormMustChangePassword_Load(object sender, EventArgs e)
        {
            tbNombre.Text = sNombre;
        }

        public void ValidarCampos()
        {
            // Verificar si los campos requeridos están llenos
            bool camposLlenos = !string.IsNullOrWhiteSpace(tbNombre.Text) &&
                                !string.IsNullOrWhiteSpace(tbContrasena.Text);

            if (camposLlenos && tbContrasena.Text == tbConfirmar.Text)
            {
                btnGuardar.Enabled = camposLlenos;
            }
            else
            {
                btnGuardar.Enabled = false;
            }
            
        }

        private void tbNombre_TextChanged(object sender, EventArgs e)
        {
            ValidarCampos();
        }

        private void tbContrasena_TextChanged(object sender, EventArgs e)
        {
            ValidarCampos();
        }

        private void tbConfirmar_TextChanged(object sender, EventArgs e)
        {
            ValidarCampos();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            //SqlConnection conexion = new SqlConnection(accesoSQLServer.GetDBConnection("DESKTOP-VSN2COF", "sa", "12345"));
            //SqlConnection conexion = new SqlConnection($"Data Source={sServidor};Initial Catalog=Tecnologico;User ID={sNombre};Password={sContraseña};MultipleActiveResultSets=true;");
            //accesoSQLServer.MustChangeQuery(sNombre, tbContrasena.Text, conexion);
        }
    }
}
