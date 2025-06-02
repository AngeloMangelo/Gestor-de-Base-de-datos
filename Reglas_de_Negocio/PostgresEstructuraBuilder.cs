using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseDeDatosSQL;

namespace Reglas_de_Negocio.PostgreSQL
{
    public class PostgresEstructuraBuilder
    {
        private readonly List<ColumnaTabla> columnas;
        private readonly string nombreTabla;

        public PostgresEstructuraBuilder(List<ColumnaTabla> columnas, string nombreTabla)
        {
            this.columnas = columnas;
            this.nombreTabla = nombreTabla;
        }

        public string GenerarCreateTable()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"CREATE TABLE \"{nombreTabla}\" (");

            for (int i = 0; i < columnas.Count; i++)
            {
                var col = columnas[i];
                string linea = FormatearColumna(col);
                sb.Append("    " + linea);

                if (i < columnas.Count - 1 || columnas.Any(c => c.EsPrimaryKey))
                    sb.Append(",");

                sb.AppendLine();
            }

            var claves = columnas.Where(c => c.EsPrimaryKey).ToList();
            if (claves.Count > 0)
            {
                string pk = string.Join(", ", claves.Select(c => $"\"{c.Nombre}\""));
                sb.AppendLine($"    PRIMARY KEY ({pk})");
            }

            sb.AppendLine(");");
            return sb.ToString();
        }

        private string FormatearColumna(ColumnaTabla col)
        {
            string tipo = ConvertirTipoPostgres(col);
            string nulo = col.EsNula ? "" : " NOT NULL";
            string def = string.Empty;

            if (!string.IsNullOrWhiteSpace(col.ValorPorDefecto))
            {
                string val = col.ValorPorDefecto.Trim('(', ')').Replace("N'", "").Replace("'", "").Trim();

                if (val.Equals("getdate", StringComparison.OrdinalIgnoreCase) ||
                    val.Equals("sysdatetime", StringComparison.OrdinalIgnoreCase))
                {
                    val = "CURRENT_TIMESTAMP";
                }
                else if (!decimal.TryParse(val, out _))
                {
                    val = $"'{val}'";
                }

                def = $" DEFAULT {val}";
            }

            return $"\"{col.Nombre}\" {tipo}{nulo}{def}";
        }

        private string ConvertirTipoPostgres(ColumnaTabla col)
        {
            string tipoSqlServer = col.Tipo.ToLower();

            if (tipoSqlServer.Contains("int")) return "INTEGER";
            if (tipoSqlServer == "bit") return "BOOLEAN";
            if (tipoSqlServer == "nvarchar" || tipoSqlServer == "varchar")
                return col.Tamaño == -1 ? "TEXT" : $"VARCHAR({col.Tamaño / 2})";
            if (tipoSqlServer == "text" || tipoSqlServer == "ntext") return "TEXT";
            if (tipoSqlServer == "datetime" || tipoSqlServer == "smalldatetime") return "TIMESTAMP";
            if (tipoSqlServer == "decimal" || tipoSqlServer == "numeric")
                return $"NUMERIC({(col.Precision == 0 ? 19 : col.Precision)},{col.Escala})";
            if (tipoSqlServer == "float" || tipoSqlServer == "real") return "FLOAT8";
            if (tipoSqlServer == "varbinary" || tipoSqlServer == "binary" || tipoSqlServer == "image") return "BYTEA";

            return "TEXT";
        }
    }
}
