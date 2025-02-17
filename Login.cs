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
using MySql.Data.MySqlClient;

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
            string sGestor = cbMotorDB.Text;
            string sUsuario = tbUsuario.Text;
            string sContraseña = tbContraseña.Text;
            string sServidor = tbServidor.Text;


            userdata = new Userdata(sUsuario, sContraseña, sServidor, sGestor);
            if (sGestor == "SQLServer")
            {
                if (!acceso.SiHayConexion(userdata.Servidor, userdata.Usuario, userdata.Contraseña))
                {
                    MessageBox.Show("Error: " + acceso.sLastError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    try
                    {
                        if (acceso.lastsqlException.Number == 18488)
                        {
                            FormMustChangePassword formMustChangePassword = new FormMustChangePassword(sGestor, sServidor, sUsuario, sContraseña);
                            formMustChangePassword.ShowDialog();
                        }
                    }
                    catch (NullReferenceException ex)
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
            else if (sGestor == "MySQL")
            {
                if (!acceso.SiHayConexionMySQL(userdata.Servidor, userdata.Usuario, userdata.Contraseña))
                {
                    MessageBox.Show("Error: " + acceso.sLastError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 
                    bLoginIsCorrect = false;
                }
                else
                {
                    bLoginIsCorrect = true;
                    this.Close();
                }
            }
            else if (sGestor == "PostgreSQL")
            {
                if (!acceso.SiHayConexionPostgreSQL(userdata.Servidor, userdata.Usuario, userdata.Contraseña))
                {
                    MessageBox.Show("Error: " + acceso.sLastError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    bLoginIsCorrect = false;
                }
                else
                {
                    bLoginIsCorrect = true;
                    this.Close();
                }
            }
            else if (sGestor == "Oracle")
            {
                MessageBox.Show("Error: No se ha implementado la conexión a Oracle", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (sGestor == "Firebird")
            {
                MessageBox.Show("Error: No se ha implementado la conexión a Firebird", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Error: No se ha seleccionado un motor de base de datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void cbMotorDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            //SQL Server
            if (cbMotorDB.SelectedIndex == 0)
            {
                tbServidor.Text = "DESKTOP-6FTA15K\\SQLANGEL";
                tbUsuario.Text = "sa";
                tbContraseña.Text = "12345";
            }
            //MySQL
            else if (cbMotorDB.SelectedIndex == 1)
            {
                tbServidor.Text = "localhost";
                tbUsuario.Text = "root";
                tbContraseña.Text = "12345";
            }
            //PostgreSQL
            else if (cbMotorDB.SelectedIndex == 2)
            {
                tbServidor.Text = "localhost";
                tbUsuario.Text = "postgres";
                tbContraseña.Text = "12345";
            }
            //Oracle
            else if (cbMotorDB.SelectedIndex == 3)
            {
                tbServidor.Text = "localhost";
            }
            //Firebird
            else if (cbMotorDB.SelectedIndex == 4)
            {
                tbServidor.Text = "localhost";
            }

        }
    }
}
