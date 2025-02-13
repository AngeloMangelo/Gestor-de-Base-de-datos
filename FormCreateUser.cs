using Reglas_de_Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace BaseDeDatosSQL
{
    public partial class FormCreateUser : Form
    {
        AccesoSQLServer accesoSQLServer = new AccesoSQLServer();
        string sSQLConnection;
        string db;

        private string sNombre;
        private string sContraseña; 
        private string sServidor;  


        public FormCreateUser(Userdata userdata)
        {
            InitializeComponent();

            sNombre = userdata.Usuario;
            sContraseña = userdata.Contraseña;
            sServidor = userdata.Servidor;
            sSQLConnection = accesoSQLServer.GetSQLConnection(sServidor, sNombre, sContraseña);

        }

        private void tbContrasena_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Control.IsKeyLocked(Keys.CapsLock))
            {
                // Caps Lock está activado, muestra la advertencia
                toolTip1.Show("Caps Lock está activado", tbContrasena, 0, -30, 2000);
            }
            else
            {
                // Caps Lock no está activado, oculta la advertencia
                toolTip1.Hide(tbContrasena);
            }
        }

        private void FormCreateUser_Load(object sender, EventArgs e)
        {
            SqlConnection conexion = new SqlConnection(sSQLConnection);
            accesoSQLServer.BasedeDatosEnComboBox(cbBasesDeDatos, conexion);

            clbRoles.SetItemChecked(8, true);
        }

        private void btnCrearUsuario_Click(object sender, EventArgs e)
        {

            string nombre = tbNombre.Text;
            string contraseña = tbContrasena.Text;
            string baseDeDatos = cbBasesDeDatos.Text;

            SqlConnection conexion = new SqlConnection(accesoSQLServer.GetCustomSQLConnection(sServidor, sNombre, sContraseña, baseDeDatos));

            string createLoginQuery = $"CREATE LOGIN [{nombre}] WITH PASSWORD = '{contraseña}' MUST_CHANGE, CHECK_EXPIRATION = ON;";
           if (!accesoSQLServer.ExcecuteQuery(createLoginQuery, conexion))
            {
                return;
            }

            string createUserQuery = $"USE {baseDeDatos}; CREATE USER [{nombre}] FOR LOGIN [{nombre}]";
            if (!accesoSQLServer.ExcecuteQuery(createUserQuery, conexion))
            {
                return;
            }

            // Asignar roles seleccionados
            foreach (object itemChecked in clbRoles.CheckedItems)
            {
                string roleName = itemChecked.ToString();
                string assignRoleQuery = $"USE {baseDeDatos}; EXEC sp_addrolemember '{roleName}', '{nombre}'";
                if(!accesoSQLServer.ExcecuteQuery(assignRoleQuery, conexion))
                {
                    return;
                }
            }
            MessageBox.Show("El login se a creado!");
            //// Hacer que la contraseña caduque
            //string expirePasswordQuery = $"ALTER LOGIN {nombre} WITH PASSWORD = '{contraseña}', CHECK_POLICY = OFF, CHECK_EXPIRATION = OFF;";
            //if (!accesoSQLServer.ExcecuteQuery(expirePasswordQuery, conexion))
            //{
            //    return;
            //}

            //string mustChangeQuery = $"ALTER LOGIN {nombre} WITH CHECK_POLICY = ON;";
            //if (!accesoSQLServer.ExcecuteQuery(expirePasswordQuery, conexion))
            //{
            //    return;
            //}

        }

        public void ValidarCampos()
        {
            // Verificar si los campos requeridos están llenos
            bool camposLlenos = !string.IsNullOrWhiteSpace(tbNombre.Text) &&
                                !string.IsNullOrWhiteSpace(tbContrasena.Text) &&
                                !string.IsNullOrWhiteSpace(cbBasesDeDatos.Text);

            // Habilitar el botón si todos los campos están llenos
            btnCrearUsuario.Enabled = camposLlenos;
        }

        private void tbNombre_TextChanged(object sender, EventArgs e)
        {
            ValidarCampos();
        }

        private void tbContrasena_TextChanged(object sender, EventArgs e)
        {
            ValidarCampos();
        }

        private void cbBasesDeDatos_SelectedValueChanged(object sender, EventArgs e)
        {
            ValidarCampos();
        }
    }
}
