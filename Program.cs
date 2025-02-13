using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseDeDatosSQL
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //abrir el Dialogo login como primer paso
            Login dlgLogin = new Login();
            Application.Run(dlgLogin);

            if(dlgLogin.bLoginIsCorrect)
            {
                Application.Run(new Sistema(dlgLogin.userdata));
            }

        }
    }
}
