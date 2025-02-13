using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseDeDatosSQL
{
    public partial class FormAddBD : Form
    {
        private Userdata userdata; // Variable para almacenar la instancia de UserData
        string Servidor;
        public FormAddBD(Userdata userdata)
        {
            InitializeComponent();
            this.userdata = userdata;
        }

        private void btnExaminar_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    // Mostrar la ruta seleccionada en el TextBox de la ruta
                    tbRuta.Text = folderDialog.SelectedPath;
                }
            }
        }

        private void btnCrearBD_Click(object sender, EventArgs e)
        {
            Servidor = userdata.Servidor;
            try
            {
                string nombreBaseDatos = tbDatabaseName.Text;
                string ruta = tbRuta.Text;

                // Conectar a SQL Server (reemplaza 'tu_servidor' con el nombre de tu servidor)
                string connectionString = $"Data Source={Servidor};Integrated Security=True;";
                SqlConnection sqlConnection = new SqlConnection(connectionString);

                // Crear la cadena SQL para crear la base de datos
                string createDatabaseQuery = $"CREATE DATABASE {nombreBaseDatos} ON PRIMARY " +
                    $"(NAME = {nombreBaseDatos}_Data, " +
                    $"FILENAME = '{ruta}\\{nombreBaseDatos}.mdf', " +
                    "SIZE = 5MB, MAXSIZE = 100MB, FILEGROWTH = 10%) " +
                    "LOG ON (NAME = " + $"{nombreBaseDatos}_Log, " +
                    $"FILENAME = '{ruta}\\{nombreBaseDatos}_log.ldf', " +
                    "SIZE = 1MB, " +
                    "MAXSIZE = 50MB, " +
                    "FILEGROWTH = 10%)";

                // Ejecutar la consulta para crear la base de datos
                SqlCommand sqlCommand = new SqlCommand(createDatabaseQuery, sqlConnection);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();

                MessageBox.Show("Base de datos creada exitosamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear la base de datos: " + ex.Message);
            }
        }
    }
}
