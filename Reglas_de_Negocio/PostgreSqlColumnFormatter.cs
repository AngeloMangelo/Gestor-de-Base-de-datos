using System;
using Reglas_de_Negocio;

namespace Reglas_de_Negocio.PostgreSQL
{
    public static class PostgreSQLColumnFormatter
    {
        public static string FormatearColumna(ColumnaTabla col)
        {
            string tipo = ConvertirTipoPostgres(col);
            string nulo = col.EsNula ? "" : " NOT NULL";
            string extra = "";

            if (col.EsAutoIncrement)
                extra += " GENERATED ALWAYS AS IDENTITY";

            if (!string.IsNullOrWhiteSpace(col.ValorPorDefecto) &&
                !tipo.StartsWith("BYTEA") && !tipo.StartsWith("TEXT"))
            {
                string def = col.ValorPorDefecto.Trim('(', ')').Replace("N'", "").Replace("'", "").Trim();

                if (def.Equals("getdate", StringComparison.OrdinalIgnoreCase) ||
                    def.Equals("sysdatetime", StringComparison.OrdinalIgnoreCase) ||
                    def.Equals("current_timestamp", StringComparison.OrdinalIgnoreCase))
                {
                    def = "CURRENT_TIMESTAMP";
                }
                else if (def.Equals("newid", StringComparison.OrdinalIgnoreCase))
                {
                    def = "uuid_generate_v4()";
                }
                else if ((def == "0" || def == "1") && tipo == "BOOLEAN")
                {
                    def = def == "1" ? "TRUE" : "FALSE";
                }
                else if (!decimal.TryParse(def, out _))
                {
                    def = $"'{def}'";
                }

                extra += $" DEFAULT {def}";
            }

            return $"\"{col.Nombre}\" {tipo}{nulo}{extra}";
        }

        private static string ConvertirTipoPostgres(ColumnaTabla col)
        {
            string tipoSqlServer = col.Tipo.ToLower();

            if (tipoSqlServer.Contains("int")) return "INTEGER";
            if (tipoSqlServer == "bit") return "BOOLEAN";
            if (tipoSqlServer == "uniqueidentifier") return "UUID";
            if (tipoSqlServer == "nvarchar" || tipoSqlServer == "varchar" || tipoSqlServer == "nchar" || tipoSqlServer == "char")
                return (col.Tamaño == -1 || col.Tamaño > 1000) ? "TEXT" : $"VARCHAR({Math.Max(1, col.Tamaño / 2)})";
            if (tipoSqlServer == "text" || tipoSqlServer == "ntext") return "TEXT";
            if (tipoSqlServer == "datetime" || tipoSqlServer == "smalldatetime") return "TIMESTAMP";
            if (tipoSqlServer == "decimal" || tipoSqlServer == "numeric" || tipoSqlServer == "money")
                return $"NUMERIC({(col.Precision == 0 ? 19 : col.Precision)},{col.Escala})";
            if (tipoSqlServer == "float" || tipoSqlServer == "real") return "FLOAT8";
            if (tipoSqlServer == "varbinary" || tipoSqlServer == "image" || tipoSqlServer == "binary") return "BYTEA";

            return "TEXT";
        }
    }
}
