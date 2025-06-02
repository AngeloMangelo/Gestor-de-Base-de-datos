using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Windows.Forms;
using Reglas_de_Negocio;

namespace BaseDeDatosSQL
{
    public partial class FormSeleccionObjetos : Form
    {
        private Userdata origen;
        private Userdata destino;
        private AccesoSQLServer acceso = new AccesoSQLServer();

        public List<string> tablasSeleccionadas = new List<string>();
        public List<string> vistasSeleccionadas = new List<string>();
        public List<string> procedimientosSeleccionados = new List<string>();

        public FormSeleccionObjetos(Userdata origen, Userdata destino)
        {
            InitializeComponent();
            rbTodos.Checked = true;
            this.Load += FormSeleccionObjetos_Load;
            rbTodos.CheckedChanged += rbModoSeleccion_CheckedChanged;
            rbSeleccionManual.CheckedChanged += rbModoSeleccion_CheckedChanged;

            this.origen = origen;
            this.destino = destino;
        }

        private void rbModoSeleccion_CheckedChanged(object sender, EventArgs e)
        {
            bool esManual = rbSeleccionManual.Checked;

            clbTablas.Enabled = esManual;
            clbVistas.Enabled = esManual;
            clbProcedimientos.Enabled = esManual;

            if (!esManual)
            {
                // Seleccionar todos automáticamente
                for (int i = 0; i < clbTablas.Items.Count; i++)
                    clbTablas.SetItemChecked(i, true);

                for (int i = 0; i < clbVistas.Items.Count; i++)
                    clbVistas.SetItemChecked(i, true);

                for (int i = 0; i < clbProcedimientos.Items.Count; i++)
                    clbProcedimientos.SetItemChecked(i, true);
            }
        }


        private void FormSeleccionObjetos_Load(object sender, EventArgs e)
        {
            try
            {
                DbConnection conn = acceso.GetDBConnection(origen.SistemaGestor, origen.Servidor, origen.Usuario, origen.Contraseña, "", origen.Ruta);
                conn.Open();

                // Obtener bases de datos según el gestor
                string consulta = "";
                DbCommand cmd = conn.CreateCommand();

                switch (origen.SistemaGestor.ToLower())
                {
                    case "sqlserver":
                        consulta = "SELECT name FROM sys.databases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb')";
                        cmd = new System.Data.SqlClient.SqlCommand(consulta, (System.Data.SqlClient.SqlConnection)conn);
                        break;
                    case "mysql":
                        consulta = "SHOW DATABASES";
                        cmd = new MySql.Data.MySqlClient.MySqlCommand(consulta, (MySql.Data.MySqlClient.MySqlConnection)conn);
                        break;
                    case "postgresql":
                        consulta = "SELECT datname FROM pg_database WHERE datistemplate = false";
                        cmd = new Npgsql.NpgsqlCommand(consulta, (Npgsql.NpgsqlConnection)conn);
                        break;
                    case "oracle":
                        consulta = "SELECT username FROM all_users";
                        cmd = new Oracle.ManagedDataAccess.Client.OracleCommand(consulta, (Oracle.ManagedDataAccess.Client.OracleConnection)conn);
                        break;
                    case "firebird":
                        // Firebird solo tiene una base por conexión
                        cbBasesDeDatos.Items.Add(conn.Database);
                        cbBasesDeDatos.SelectedIndex = 0;
                        conn.Close();
                        CargarObjetos(conn.Database);
                        return;
                }

                cbBasesDeDatos.Items.Clear(); // evita duplicados

                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cbBasesDeDatos.Items.Add(reader[0].ToString());
                    }
                }

                if (cbBasesDeDatos.Items.Count > 0)
                    cbBasesDeDatos.SelectedIndex = 0;

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar bases: " + ex.Message);
            }

            rbTodos.Checked = true;
            rbModoSeleccion_CheckedChanged(null, null); // <-- ejecuta lógica inicial

        }

        private void cbBasesDeDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string baseSeleccionada = cbBasesDeDatos.SelectedItem.ToString();
            CargarObjetos(baseSeleccionada);
        }

        private void CargarObjetos(string nombreBD)
        {
            clbTablas.Items.Clear();
            clbVistas.Items.Clear();
            clbProcedimientos.Items.Clear();

            try
            {
                DbConnection conn = acceso.GetDBConnection(origen.SistemaGestor, origen.Servidor, origen.Usuario, origen.Contraseña, nombreBD, origen.Ruta);
                conn.Open();

                DbCommand cmd = conn.CreateCommand();

                // === TABLAS ===
                switch (origen.SistemaGestor.ToLower())
                {
                    case "sqlserver":
                        cmd.CommandText = $"USE [{nombreBD}]; SELECT name FROM sys.tables ORDER BY name";
                        break;
                    case "mysql":
                        cmd.CommandText = $"SHOW FULL TABLES WHERE Table_type = 'BASE TABLE'";
                        break;
                    case "postgresql":
                        cmd.CommandText = "SELECT table_name FROM information_schema.tables WHERE table_schema = 'public' AND table_type = 'BASE TABLE'";
                        break;
                    case "oracle":
                        cmd.CommandText = "SELECT table_name FROM user_tables";
                        break;
                    case "firebird":
                        cmd.CommandText = "SELECT RDB$RELATION_NAME FROM RDB$RELATIONS WHERE RDB$SYSTEM_FLAG = 0";
                        break;
                }

                using (DbDataReader reader = cmd.ExecuteReader())
                    while (reader.Read())
                        clbTablas.Items.Add(reader[0].ToString().Trim());

                // === VISTAS ===
                cmd = conn.CreateCommand();
                switch (origen.SistemaGestor.ToLower())
                {
                    case "sqlserver":
                        cmd.CommandText = $"USE [{nombreBD}]; SELECT name FROM sys.views ORDER BY name";
                        break;
                    case "mysql":
                        cmd.CommandText = $"SHOW FULL TABLES WHERE Table_type = 'VIEW'";
                        break;
                    case "postgresql":
                        cmd.CommandText = "SELECT table_name FROM information_schema.views WHERE table_schema = 'public'";
                        break;
                    case "oracle":
                        cmd.CommandText = "SELECT view_name FROM user_views";
                        break;
                    case "firebird":
                        cmd.CommandText = "SELECT RDB$VIEW_NAME FROM RDB$RELATIONS WHERE RDB$VIEW_BLR IS NOT NULL";
                        break;
                }

                using (DbDataReader reader = cmd.ExecuteReader())
                    while (reader.Read())
                        clbVistas.Items.Add(reader[0].ToString().Trim());

                // === PROCEDIMIENTOS ===
                cmd = conn.CreateCommand();
                switch (origen.SistemaGestor.ToLower())
                {
                    case "sqlserver":
                        cmd.CommandText = $"USE [{nombreBD}]; SELECT name FROM sys.procedures ORDER BY name";
                        break;
                    case "mysql":
                        cmd.CommandText = "SELECT routine_name FROM information_schema.routines WHERE routine_type='PROCEDURE' AND routine_schema=DATABASE()";
                        break;
                    case "postgresql":
                        cmd.CommandText = "SELECT proname FROM pg_proc INNER JOIN pg_namespace ON pg_proc.pronamespace = pg_namespace.oid WHERE pg_namespace.nspname = 'public'";
                        break;
                    case "oracle":
                        cmd.CommandText = "SELECT object_name FROM user_procedures WHERE object_type = 'PROCEDURE'";
                        break;
                    case "firebird":
                        cmd.CommandText = "SELECT RDB$PROCEDURE_NAME FROM RDB$PROCEDURES";
                        break;
                }

                using (DbDataReader reader = cmd.ExecuteReader())
                    while (reader.Read())
                        clbProcedimientos.Items.Add(reader[0].ToString().Trim());

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar objetos: " + ex.Message);
            }

            if (rbTodos.Checked)
            {
                for (int i = 0; i < clbTablas.Items.Count; i++)
                    clbTablas.SetItemChecked(i, true);

                for (int i = 0; i < clbVistas.Items.Count; i++)
                    clbVistas.SetItemChecked(i, true);

                for (int i = 0; i < clbProcedimientos.Items.Count; i++)
                    clbProcedimientos.SetItemChecked(i, true);
            }

        }

        private void btnMigrar_Click(object sender, EventArgs e)
        {
            tablasSeleccionadas.Clear();
            vistasSeleccionadas.Clear();
            procedimientosSeleccionados.Clear();

            foreach (var item in clbTablas.CheckedItems)
                tablasSeleccionadas.Add(item.ToString());

            foreach (var item in clbVistas.CheckedItems)
                vistasSeleccionadas.Add(item.ToString());

            foreach (var item in clbProcedimientos.CheckedItems)
                procedimientosSeleccionados.Add(item.ToString());

            if (tablasSeleccionadas.Count == 0 && vistasSeleccionadas.Count == 0 && procedimientosSeleccionados.Count == 0)
            {
                MessageBox.Show("Debe seleccionar al menos un objeto para migrar.");
                return;
            }

            try
            {
                destino.BaseDeDatos = cbBasesDeDatos.SelectedItem.ToString();

                bool creada = EjecutorSQLDestino.CrearBaseDeDatos(destino);
                if (!creada)
                {
                    MessageBox.Show("No se pudo crear la base de datos destino.");
                    return;
                }

                MigradorEstructura migrador = new MigradorEstructura(
                    origen.Servidor,
                    origen.Usuario,
                    origen.Contraseña,
                    cbBasesDeDatos.SelectedItem.ToString()
                );

                MigradorDatos migradorDatos = new MigradorDatos(
                    origen.Servidor,
                    origen.Usuario,
                    origen.Contraseña,
                    cbBasesDeDatos.SelectedItem.ToString()
                );

                StringBuilder resultados = new StringBuilder();

                // ⚙️ Desactivar claves foráneas (solo si es MySQL)
                if (destino.SistemaGestor.ToLower() == "mysql")
                {
                    using (var conn = new MySql.Data.MySqlClient.MySqlConnection(
                        $"Server={destino.Servidor};Database={destino.BaseDeDatos};User ID={destino.Usuario};Password={destino.Contraseña};"))
                    {
                        conn.Open();
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "SET FOREIGN_KEY_CHECKS = 0;";
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                rtbResultados.Clear();

                foreach (var tabla in tablasSeleccionadas)
                {
                    // 🔧 Generar script CREATE TABLE con PK, FK, índices
                    string script = migrador.GenerarCreateTable(tabla, destino.SistemaGestor);

                    // 🧱 Crear la tabla
                    bool ok = EjecutorSQLDestino.EjecutarScript(destino, script);
                    rtbResultados.AppendText(script + "\n\n");

                    if (ok)
                    {
                        // 📥 Migrar registros
                        migradorDatos.MigrarDatos(tabla, destino);

                        // ✅ Validar registros migrados
                        //string validacion = ValidadorMigracion.ValidarConteoRegistros(tabla, origen, destino);
                        //rtbResultados.AppendText(validacion + "\n\n");
                    }
                    else
                    {
                        rtbResultados.AppendText($"-- ERROR al crear la tabla {tabla}, datos no migrados\n\n");
                    }
                }

                // ⚙️ Reactivar claves foráneas (MySQL)
                if (destino.SistemaGestor.ToLower() == "mysql")
                {
                    using (var conn = new MySql.Data.MySqlClient.MySqlConnection(
                        $"Server={destino.Servidor};Database={destino.BaseDeDatos};User ID={destino.Usuario};Password={destino.Contraseña};"))
                    {
                        conn.Open();
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "SET FOREIGN_KEY_CHECKS = 1;";
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                MessageBox.Show("Migración completada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error durante la migración: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



    }
}
