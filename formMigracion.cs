using System;
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

        private void btnMigrar_Click(object sender, EventArgs e)
        {
            if (cbOrigen.SelectedItem == null || cbDestino.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar el tipo de sistema gestor de ORIGEN y DESTINO.");
                return;
            }

            string gestorOrigen = cbOrigen.SelectedItem.ToString();
            string gestorDestino = cbDestino.SelectedItem.ToString();

            // Buscar la conexión activa para el gestor ORIGEN
            var conexionesOrigen = conexionesActivas.Where(c => c.SistemaGestor == gestorOrigen).ToList();
            if (conexionesOrigen.Count == 0)
            {
                MessageBox.Show("No se encontró conexión activa para el gestor origen.");
                return;
            }

            // Si hay más de una conexión, podrías permitir elegir, pero de momento tomamos la primera:
            origenData = conexionesOrigen.First();

            // Buscar la conexión activa para el gestor DESTINO
            var conexionesDestino = conexionesActivas.Where(c => c.SistemaGestor == gestorDestino).ToList();
            if (conexionesDestino.Count == 0)
            {
                MessageBox.Show("No se encontró conexión activa para el gestor destino.");
                return;
            }

            destinoData = conexionesDestino.First();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        private void formMigracion_Load(object sender, EventArgs e)
        {
            var gestoresConectados = conexionesActivas
                .Select(c => c.SistemaGestor)
                .Distinct()
                .ToList();

            cbOrigen.Items.AddRange(gestoresConectados.ToArray());
            cbDestino.Items.AddRange(gestoresConectados.ToArray());

            if (cbOrigen.Items.Count > 0) cbOrigen.SelectedIndex = 0;
            if (cbDestino.Items.Count > 0) cbDestino.SelectedIndex = 0;
        }
    }
}
