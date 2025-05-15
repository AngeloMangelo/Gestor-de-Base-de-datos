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
        private Userdata userdata;
        public formMigracion(Userdata userdata)
        {
            InitializeComponent();
            this.userdata = userdata;
        }

        private void btnMigrar_Click(object sender, EventArgs e)
        {

        }
    }
}
