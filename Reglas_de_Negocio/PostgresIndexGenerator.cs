using System;
using System.Collections.Generic;
using System.Text;

namespace Reglas_de_Negocio.Helpers
{
    public static class PostgresIndexGenerator
    {
        public static string GenerarIndex(string nombreTabla, string nombreIndice, List<string> columnas)
        {
            if (columnas == null || columnas.Count == 0)
                throw new ArgumentException("Debe especificar al menos una columna para el índice.");

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("CREATE INDEX \"{0}\" ON \"{1}\" (", nombreIndice, nombreTabla);
            sb.Append(string.Join(", ", columnas.ConvertAll(c => $"\"{c}\"")));
            sb.Append(");");

            return sb.ToString();
        }

        public static List<string> GenerarIndices(string nombreTabla, Dictionary<string, List<string>> indicesDefinidos)
        {
            List<string> scripts = new List<string>();

            foreach (var kvp in indicesDefinidos)
            {
                string indexScript = GenerarIndex(nombreTabla, kvp.Key, kvp.Value);
                scripts.Add(indexScript);
            }

            return scripts;
        }
    }
}
