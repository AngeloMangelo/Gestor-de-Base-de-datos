using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BaseDeDatosSQL;

namespace Reglas_de_Negocio.PostgreSQL
{
    public static class PostgresInsertBuilder
    {
        public static string GenerarInsert(string tabla, DataRow fila)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder columnas = new StringBuilder();
            StringBuilder valores = new StringBuilder();

            foreach (DataColumn col in fila.Table.Columns)
            {
                if (columnas.Length > 0)
                {
                    columnas.Append(", ");
                    valores.Append(", ");
                }

                columnas.Append('"' + col.ColumnName + '"');
                valores.Append(FormatearValor(fila[col]));
            }

            sb.Append($"INSERT INTO \"{tabla}\" ({columnas}) VALUES ({valores});");
            return sb.ToString();
        }

        private static string FormatearValor(object valor)
        {
            if (valor == DBNull.Value)
                return "NULL";

            if (valor is string || valor is DateTime)
                return $"'{valor.ToString().Replace("'", "''")}'";

            if (valor is bool)
                return ((bool)valor) ? "TRUE" : "FALSE";

            return valor.ToString();
        }
    }
}
