using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseDeDatosSQL
{
    public class Userdata
    {
        //clase en la que se obtienen los datos del inicio de sesion extrayendolos de los textos de los TextBox
        private string Name;
        private string Password;
        private string Server;
        private string Gestor;
        private string Database;
        private string ruta;

        public Userdata(string sName, string sPassword, string sServer, string sGestor, string sDatabase = "", string ruta = null)
        {
            this.Name = sName;
            this.Password = sPassword;
            this.Server = sServer;
            this.Gestor = sGestor;
            this.Database = sDatabase;
            this.ruta = ruta;
        }

        public string Usuario
        {
            get { return Name; }
        }

        public string Contraseña
        {
            get { return Password; }
        }

        public string Servidor
        {
            get { return Server; }
        }
        public string SistemaGestor
        {
            get { return Gestor; }
        }
        public string BaseDeDatos
        {
            get { return Database; }
            set { Database = value; }
        }
        public string Ruta
        {
            get { return ruta; }
            set { ruta = value; }
        }

        // Método para obtener el nombre de usuario
        public string ObtenerNombreUsuario()
        {
            return Usuario;
        }

        // Método para obtener la contraseña
        public string ObtenerContraseña()
        {
            return Contraseña;
        }

        // Método para obtener la contraseña
        public string ObtenerServidor()
        {
            return Servidor;
        }
    }
}
