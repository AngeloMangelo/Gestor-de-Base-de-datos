using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reglas_de_Negocio
{
    public class ColumnaTabla
    {
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public int Tamaño { get; set; }
        public byte Precision { get; set; }
        public byte Escala { get; set; }
        public bool EsNula { get; set; }
        public bool EsPrimaryKey { get; set; }
        public bool EsAutoIncrement { get; set; }
        public string ValorPorDefecto { get; set; }
        public bool EsIndexada { get; set; }
    }
}

