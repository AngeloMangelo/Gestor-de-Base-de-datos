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
        public Boolean bSesionIniciada = false;
        public Userdata userdata;


        public Login()
        {
            InitializeComponent();

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        async private void btnIngresar_Click(object sender, EventArgs e)
        {
            cbMotorDB.Enabled = false;
            tbServidor.Enabled = false;
            tbUsuario.Enabled = false;
            tbContraseña.Enabled = false;

            pgEspera.Visible = true;
            pgEspera.Style = ProgressBarStyle.Marquee;
            btnIngresar.Enabled = false;

            try
            {
                string sGestor = cbMotorDB.Text;
                string sUsuario = tbUsuario.Text;
                string sContraseña = tbContraseña.Text;
                string sServidor = tbServidor.Text;

                await Task.Run(() => Conectar(sGestor, sUsuario, sContraseña, sServidor));
                if (bLoginIsCorrect)
                { this.Close(); }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                pgEspera.Visible = false;
                btnIngresar.Enabled = true;

                cbMotorDB.Enabled = true;
                tbServidor.Enabled = true;
                tbUsuario.Enabled = true;
                tbContraseña.Enabled = true;
            }


        }

        public bool Conectar(string sGestor, string sUsuario, string sContraseña, string sServidor) 
        {
            AccesoSQLServer acceso = new AccesoSQLServer();
            


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
                        return bLoginIsCorrect;
                    }

                    bLoginIsCorrect = false;
                    return bLoginIsCorrect;
                }
                else
                {
                    bLoginIsCorrect = true;
                    bSesionIniciada = true;
                    return bLoginIsCorrect;                   
                }
            }
            else if (sGestor == "MySQL")
            {
                if (!acceso.SiHayConexionMySQL(userdata.Servidor, userdata.Usuario, userdata.Contraseña))
                {
                    MessageBox.Show("Error: " + acceso.sLastError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    bLoginIsCorrect = false;
                    return bLoginIsCorrect;
                }
                else
                {
                    bLoginIsCorrect = true;
                    bSesionIniciada = true;
                    return bLoginIsCorrect;
                }
            }
            else if (sGestor == "PostgreSQL")
            {
                if (!acceso.SiHayConexionPostgreSQL(userdata.Servidor, userdata.Usuario, userdata.Contraseña))
                {
                    MessageBox.Show("Error: " + acceso.sLastError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    bLoginIsCorrect = false;
                    return bLoginIsCorrect;
                }
                else
                {
                    bLoginIsCorrect = true;
                    bSesionIniciada = true;
                    return bLoginIsCorrect;
                }
            }
            else if (sGestor == "Oracle")
            {
                if (!acceso.SiHayConexionOracle(userdata.Servidor, userdata.Usuario, userdata.Contraseña))
                {
                    MessageBox.Show("Error: " + acceso.sLastError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    bLoginIsCorrect = false;
                    return bLoginIsCorrect;
                }
                else
                {
                    bLoginIsCorrect = true;
                    bSesionIniciada = true;
                    return bLoginIsCorrect;
                }
            }
            else if (sGestor == "Firebird")
            {
                string sBaseDatos = "";
                bool esLocal = (userdata.Servidor == "localhost" ||
                               userdata.Servidor == "127.0.0.1" ||
                               userdata.Servidor == "::1");

                // Obtener ruta de la base de datos desde la UI
                if (esLocal)
                {
                    using (OpenFileDialog ofd = new OpenFileDialog())
                    {
                        ofd.Filter = "Archivos Firebird (*.fdb)|*.fdb";
                        ofd.Title = "Seleccionar base de datos local";

                        this.Invoke((MethodInvoker)delegate
                        {
                            if (ofd.ShowDialog() != DialogResult.OK)
                            {
                                bLoginIsCorrect = false;
                            }
                            sBaseDatos = ofd.FileName;
                        }); 
                    }
                }
                else
                {
                    sBaseDatos = Microsoft.VisualBasic.Interaction.InputBox(
                        "Ingrese ruta remota de la base de datos:",
                        "Firebird Remoto",
                        "/ruta/ejemplo.fdb"
                    );

                    if (string.IsNullOrWhiteSpace(sBaseDatos))
                    {
                        bLoginIsCorrect = false;
                        return bLoginIsCorrect;
                    }
                }

                userdata.Ruta = sBaseDatos; // Guardar ruta en UserData si es necesario
                // Llamar al método de la DLL con los 4 parámetros
                if (!acceso.SiHayConexionFirebird(
                    userdata.Servidor,
                    userdata.Usuario,
                    userdata.Contraseña,
                    userdata.Ruta)) // <-- Nuevo parámetro

                {
                    MessageBox.Show("Error: " + acceso.sLastError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    bLoginIsCorrect = false;
                }
                else
                {
                    bLoginIsCorrect = true;
                    bSesionIniciada = true;
                    userdata.Ruta = sBaseDatos; // Guardar ruta en UserData si es necesario
                }
                return bLoginIsCorrect;
            }
            //else if (sGestor == "Firebird")
            //{


            //    try
            //    {
            //        bool conexionExitosa = ConectarFirebird(sServidor, sUsuario, sContraseña).Result;
            //        if (!conexionExitosa)
            //        {
            //            MessageBox.Show("Error: " + acceso.sLastError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            bLoginIsCorrect = false;
            //            return bLoginIsCorrect;
            //        }
            //        else
            //        {
            //            bLoginIsCorrect = true;
            //            bSesionIniciada = true;
            //            return bLoginIsCorrect;
            //        }
            //    }
            //    finally
            //    {
            //        // Aquí puedes manejar cualquier limpieza o cierre de recursos si es necesario

            //    }
            //}
            else
            {
                return bLoginIsCorrect;
            }
        }

        public async Task<bool> ConectarFirebird(string sServidor, string sUsuario, string sContraseña)
        {
            string sBaseDatos = "";
            bool esLocal = (sServidor == "localhost" || sServidor == "127.0.0.1" || sServidor == "::1");

            // Ejecutar en el hilo de UI
            await Task.Run(() =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    if (esLocal)
                    {
                        using (OpenFileDialog ofd = new OpenFileDialog())
                        {
                            ofd.Filter = "Archivos Firebird (*.fdb)|*.fdb";
                            ofd.Title = "Seleccione la base de datos Firebird";
                            if (ofd.ShowDialog() == DialogResult.OK)
                            {
                                sBaseDatos = ofd.FileName;
                            }
                        }
                    }
                    else
                    {
                        sBaseDatos = Microsoft.VisualBasic.Interaction.InputBox(
                            "Ingrese la ruta remota de la base de datos:",
                            "Ruta de Firebird",
                            "/ruta/ejemplo.fdb"
                        );
                    }
                });
            });

            if (string.IsNullOrEmpty(sBaseDatos)) return false;

            // Llama al método de la DLL
            AccesoSQLServer acceso = new AccesoSQLServer();
            return acceso.SiHayConexionFirebird(sServidor, sUsuario, sContraseña, sBaseDatos);
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
                tbUsuario.Text = "system";
                tbContraseña.Text = "12345";
            }
            //Firebird
            else if (cbMotorDB.SelectedIndex == 4)
            {
                tbServidor.Text = "localhost";
                tbUsuario.Text = "SYSDBA";
                tbContraseña.Text = "12345";
            }

        }
    }
}
