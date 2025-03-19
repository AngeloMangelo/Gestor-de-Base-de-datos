namespace CommonEntities
{
    namespace CommonEntities
    {
        public class Userdata
        {
            private string Name;
            private string Password;
            private string Server;
            private string Gestor;

            public Userdata(string sName, string sPassword, string sServer, string sGestor)
            {
                this.Name = sName;
                this.Password = sPassword;
                this.Server = sServer;
                this.Gestor = sGestor;
            }

            public string Usuario => Name;
            public string Contraseña => Password;
            public string Servidor => Server;
            public string SistemaGestor => Gestor;
        }
    }

}
