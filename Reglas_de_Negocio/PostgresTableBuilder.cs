using System.Collections.Generic;
using System.Text;
using BaseDeDatosSQL;

namespace Reglas_de_Negocio.PostgreSQL
{
    public class PostgresTableBuilder
    {
        public static string GenerarCreateTable(string nombreTabla, List<ColumnaTabla> columnas, List<MigradorRelacional.ForeignKey> fks, List<string> indices)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"CREATE TABLE \"{nombreTabla}\" (");

            for (int i = 0; i < columnas.Count; i++)
            {
                string linea = PostgresColumnFormatter.FormatearColumna(columnas[i]);
                sb.Append("    " + linea);

                if (i < columnas.Count - 1)
                    sb.Append(",");

                sb.AppendLine();
            }

            var claves = columnas.FindAll(c => c.EsPrimaryKey);
            if (claves.Count > 0)
            {
                if (columnas.Count > 0)
                    sb.AppendLine(",");

                string camposPK = string.Join(", ", claves.ConvertAll(c => $"\"{c.Nombre}\""));
                sb.AppendLine($"    PRIMARY KEY ({camposPK})");
            }

            sb.AppendLine(");");

            // FKs
            if (fks != null && fks.Count > 0)
            {
                sb.AppendLine();
                foreach (var fk in fks)
                    sb.AppendLine(PostgresFKGenerator.GenerarScriptFK(fk));
            }

            // Índices
            if (indices != null && indices.Count > 0)
            {
                sb.AppendLine();
                foreach (var idx in indices)
                    sb.AppendLine(idx);
            }

            return sb.ToString();
        }
    }
}
