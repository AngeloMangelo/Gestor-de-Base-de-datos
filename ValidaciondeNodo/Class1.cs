using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ValidaciondeNodo
{
    // Clase para representar nodos de bases de datos
    public class NodoBaseDeDatos : TreeNode
    {
        public NodoBaseDeDatos(string nombre)
        {
            this.Text = nombre;
            this.ImageKey = "BaseDeDatos"; // Utiliza una imagen diferente para bases de datos si lo deseas
            this.SelectedImageKey = "BaseDeDatos"; // Imagen seleccionada para bases de datos
            this.Tag = "BaseDeDatos"; // Etiqueta para identificar nodos de bases de datos
        }
    }

    // Clase para representar nodos de tablas
    public class NodoTabla : TreeNode
    {
        public NodoTabla(string nombre)
        {
            this.Text = nombre;
            this.ImageKey = "Tabla"; // Utiliza una imagen diferente para tablas si lo deseas
            this.SelectedImageKey = "Tabla"; // Imagen seleccionada para tablas
            this.Tag = "Tabla"; // Etiqueta para identificar nodos de tablas
        }
    }
}
