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
    public partial class Login : Form
    {
        public Boolean bLoginIsCorrect = false;
        public Userdata userdata;

        public Login()
        {
            InitializeComponent();
            
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            AccesoSQLServer acceso = new AccesoSQLServer();
            string sUsuario = tbUsuario.Text;
            string sContraseña = tbContraseña.Text;
            string sServidor = tbServidor.Text;


            userdata = new Userdata(sUsuario, sContraseña, sServidor);

            if (!acceso.SiHayConexion(userdata.Servidor, userdata.Usuario, userdata.Contraseña))
            {
                MessageBox.Show("Error: " + acceso.sLastError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                try
                {
                    if (acceso.lastsqlException.Number == 18488)
                    {
                        FormMustChangePassword formMustChangePassword = new FormMustChangePassword(sServidor, sUsuario, sContraseña);
                        formMustChangePassword.ShowDialog();
                    }
                }
                catch(NullReferenceException ex)
                {
                    bLoginIsCorrect = false;
                }
                
                bLoginIsCorrect = false;
            }
            else
            {
                bLoginIsCorrect = true;
                this.Close();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
