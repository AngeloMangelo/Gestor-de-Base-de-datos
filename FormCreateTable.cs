using Reglas_de_Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BaseDeDatosSQL
{
    public partial class FormCreateTable : Form
    {
        private System.Windows.Forms.TreeView tv; // Variable para almacenar la instancia de Sistema que contiene el treeview para obtener el nombre de la base de datos
        AccesoSQLServer accesoSQLServer = new AccesoSQLServer();
        private Userdata userdata; // Variable para almacenar la instancia de UserData
        private string sNombre;    // Variable para almacenar el nombre de usuario
        private string sContraseña; // Variable para almacenar la contraseña
        private string sServidor;  // Variable para almacenar el servidor
        string sSQLConnection;

        private DataTable tablaCampos; // DataTable para almacenar la información de los campos

        public FormCreateTable(System.Windows.Forms.TreeView treeview, Userdata userdata)
        {
            InitializeComponent();
            this.tv = treeview;
            this.userdata = userdata;

            sNombre = userdata.Usuario;
            sContraseña = userdata.Contraseña;
            sServidor = userdata.Servidor;
            sSQLConnection = accesoSQLServer.GetDBConnection(sServidor, sNombre, sContraseña);

            // Inicializar el DataTable para campos
            tablaCampos = new DataTable();
            tablaCampos.Columns.Add("Nombre del Campo");
            tablaCampos.Columns.Add("Tipo de Dato");
            tablaCampos.Columns.Add("Longitud");
            tablaCampos.Columns.Add("NOT NULL", typeof(bool));
            tablaCampos.Columns.Add("PRIMARY KEY", typeof(bool));
            tablaCampos.Columns.Add("IDENTITY", typeof(bool));

            // Enlazar el DataGridView al DataTable
            dataGridViewCampos.DataSource = tablaCampos;

            RellenarTiposdeDato();
        }
        private void btnAgregarCampo_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarTabla())
                {
                    // No continuar si la tabla no es válida
                    return;
                }
                // Obtener los valores ingresados en los controles del formulario
                string nombreCampo = tbCampo.Text;
                string tipoDato = cbTipodedato.SelectedItem.ToString();
                string longitud = tbLongitud.Text; // Obtenemos la longitud directamente del TextBox
                bool notNull = cbNotNull.Checked;
                bool primaryKey = cbPk.Checked;
                bool identity = cbIdentity.Checked;

                // Agregar el campo al DataTable
                tablaCampos.Rows.Add(nombreCampo, tipoDato, longitud, notNull, primaryKey, identity);

                // Limpiar los controles después de agregar un campo
                tbCampo.Clear();
                tbLongitud.Clear();
                cbNotNull.Checked = false;
                cbPk.Checked = false;
                cbIdentity.Checked = false;
            }
            catch(NullReferenceException ex)
            {
                MessageBox.Show($"Error al crear el campo: {ex.Message} No puedes dejar este campo vacio");
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error al crear la tabla: {ex.Message}");
            }
        }

        private void btnCrearTabla_Click(object sender, EventArgs e)
        {
            // Obtener la base de datos del nodo seleccionado en el formulario principal
            TreeNode selectedNode = tv.SelectedNode;
            string nombreBaseDeDatos = selectedNode.Text; // Obtener el nombre de la base de datos

            // Obtener el nombre de la tabla ingresado en el TextBox
            string nombreTabla = tbNombreTabla.Text;

            // Construir la consulta SQL para crear la tabla
            StringBuilder sCmd = new StringBuilder();
            sCmd.AppendLine($"USE {nombreBaseDeDatos};");
            sCmd.AppendLine($"CREATE TABLE {nombreTabla} (");

            foreach (DataRow row in tablaCampos.Rows)
            {
                string nombreCampo = row["Nombre del Campo"].ToString();
                string tipoDato = row["Tipo de Dato"].ToString();
                string longitud = Convert.ToString(row["Longitud"]);
                bool notNull = Convert.ToBoolean(row["NOT NULL"]);
                bool primaryKey = Convert.ToBoolean(row["PRIMARY KEY"]);
                bool identity = Convert.ToBoolean(row["IDENTITY"]);

                string campo;

                if (!string.IsNullOrEmpty(longitud))
                {
                    campo = $"{nombreCampo} {tipoDato}({longitud})";
                }
                else
                {
                    campo = $"{nombreCampo} {tipoDato}";
                }

                if (notNull)
                {
                    campo += " NOT NULL";
                }

                if (primaryKey)
                {
                    campo += " PRIMARY KEY";
                }

                if (identity)
                {
                    campo += " IDENTITY";
                }

                // Agregar el campo a la consulta SQL
                sCmd.AppendLine(campo + ",");
            }

            // Eliminar la coma adicional de la última línea
            sCmd.Length -= 3;

            // Finalizar la consulta SQL
            sCmd.AppendLine("\r\n);");

            string cadena = sCmd.ToString();

            try
            {
                // Crear una conexión a SQL Server
                using (SqlConnection conexion = new SqlConnection(sSQLConnection))
                {
                    // Abrir la conexión
                    conexion.Open();

                    // Crear un comando SQL
                    using (SqlCommand comando = new SqlCommand(cadena, conexion))
                    {
                        // Ejecutar la consulta SQL para crear la tabla
                        comando.ExecuteNonQuery();

                        // Si la creación de la tabla es exitosa, muestra un mensaje de éxito
                        MessageBox.Show("Tabla creada exitosamente.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir al crear la tabla
                MessageBox.Show($"Error al crear la tabla: {ex.Message}");
            }
        }
        private void RellenarTiposdeDato()
        {
            // Llenar el ComboBox de Tipos de Datos
            cbTipodedato.Items.Add("int");
            cbTipodedato.Items.Add("bigint");
            cbTipodedato.Items.Add("smallint");
            cbTipodedato.Items.Add("tinyint");
            cbTipodedato.Items.Add("decimal");
            cbTipodedato.Items.Add("numeric");
            cbTipodedato.Items.Add("money");
            cbTipodedato.Items.Add("smallmoney");
            cbTipodedato.Items.Add("float");
            cbTipodedato.Items.Add("real");
            cbTipodedato.Items.Add("date");
            cbTipodedato.Items.Add("datetime");
            cbTipodedato.Items.Add("datetime2");
            cbTipodedato.Items.Add("datetimeoffset");
            cbTipodedato.Items.Add("time");
            cbTipodedato.Items.Add("char");
            cbTipodedato.Items.Add("varchar");
            cbTipodedato.Items.Add("text");
            cbTipodedato.Items.Add("nchar");
            cbTipodedato.Items.Add("nvarchar");
            cbTipodedato.Items.Add("ntext");
            cbTipodedato.Items.Add("binary");
            cbTipodedato.Items.Add("varbinary");
            cbTipodedato.Items.Add("image");
            cbTipodedato.Items.Add("bit");

            cbTipodedato.SelectedIndex = -1;
        }
        private bool ValidarTabla()
        {
            bool esValida = true;

            // Verificar si ya existe una columna con la restricción IDENTITY
            bool existeIdentity = tablaCampos.AsEnumerable().Any(row => Convert.ToBoolean(row["IDENTITY"]));

            if (existeIdentity && cbIdentity.Checked)
            {
                // Si ya existe una columna con IDENTITY, desactivar el checkbox y mostrar un mensaje de error
                cbIdentity.Checked = false;
                MessageBox.Show("Ya existe una columna con la restricción IDENTITY en la tabla.");
                esValida = false;
            }

            // Verificar otros criterios de validación aquí, como la longitud de los tipos de datos, etc.
            // Si se encuentran problemas, establecer esValida en false y mostrar mensajes de error.

            return esValida;
        }
        private string RemoverLlaves(string input)
        {
            // Reemplazar todas las llaves por una cadena vacía
            string result = input.Replace("{", "").Replace("}", "");

            return result;
        }

        private void cbTipodedato_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Obtener el tipo de dato seleccionado
            string tipoDato = cbTipodedato.SelectedItem.ToString();

            // Habilitar o deshabilitar el TextBox de longitud según el tipo de dato
            if (tipoDato == "varchar" || tipoDato == "nvarchar" || tipoDato == "char" || tipoDato == "decimal" || tipoDato == "numeric" || tipoDato == "text")
            {
                tbLongitud.Enabled = true;
            }
            else
            {
                tbLongitud.Enabled = false;
                tbLongitud.Clear(); // Limpia el valor si el tipo de dato no lo requiere
            }
        }

        private void tbLongitud_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verificar si la tecla presionada no es un número o la tecla de retroceso (Backspace)
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                // Cancelar la entrada de caracteres que no son números
                e.Handled = true;
            }
        }
    }
}
