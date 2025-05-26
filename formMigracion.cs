using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace BaseDeDatosSQL
{
    public partial class formMigracion : Form
    {
        private List<Userdata> conexionesActivas;
        public Userdata origenData { get; private set; }
        public Userdata destinoData { get; private set; }

        public formMigracion(List<Userdata> conexiones)
        {
            InitializeComponent();
            this.conexionesActivas = conexiones;
        }

        private void formMigracion_Load(object sender, EventArgs e)
        {
            // Solo mostrar SQL Server en origen
            cbOrigen.Items.Clear();
            cbOrigen.Items.Add("SQLServer");
            cbOrigen.SelectedIndex = 0;
            cbOrigen.Enabled = false;

            // Mostrar como destino todos los gestores distintos a SQL Server
            var gestoresDestino = conexionesActivas
                .Where(c => c.SistemaGestor != "SQLServer")
                .Select(c => c.SistemaGestor)
                .Distinct()
                .ToList();

            cbDestino.Items.Clear();
            cbDestino.Items.AddRange(gestoresDestino.ToArray());

            if (cbDestino.Items.Count > 0)
                cbDestino.SelectedIndex = 0;
        }

        private void btnMigrar_Click(object sender, EventArgs e)
        {
            if (cbDestino.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un gestor de base de datos de DESTINO.");
                return;
            }

            string gestorDestino = cbDestino.SelectedItem.ToString();

            // Buscar conexión activa de SQL Server (origen)
            var conexionesOrigen = conexionesActivas
                .Where(c => c.SistemaGestor == "SQLServer")
                .ToList();

            if (conexionesOrigen.Count == 0)
            {
                MessageBox.Show("No se encontró conexión activa a SQL Server como origen.");
                return;
            }

            origenData = conexionesOrigen.First();

            // Buscar conexión activa del destino
            var conexionesDestino = conexionesActivas
                .Where(c => c.SistemaGestor == gestorDestino)
                .ToList();

            if (conexionesDestino.Count == 0)
            {
                MessageBox.Show("No se encontró conexión activa para el gestor destino.");
                return;
            }

            destinoData = conexionesDestino.First();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
