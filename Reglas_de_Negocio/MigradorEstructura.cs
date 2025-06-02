using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseDeDatosSQL;

namespace Reglas_de_Negocio
{
    public class MigradorEstructura
    {
        private string servidor;
        private string usuario;
        private string contraseña;
        private string baseDatos;

        public MigradorEstructura(string servidor, string usuario, string contraseña, string baseDatos)
        {
            this.servidor = servidor;
            this.usuario = usuario;
            this.contraseña = contraseña;
            this.baseDatos = baseDatos;
        }

        public string GenerarCreateTable(string nombreTabla, string gestorDestino)
        {
            var columnas = ObtenerColumnasDesdeSQLServer(nombreTabla);
            if (columnas.Count == 0)
                return $"-- La tabla {nombreTabla} no tiene columnas o no se encontró.";

            StringBuilder sb = new StringBuilder();

            // Encabezado según motor
            switch (gestorDestino.ToLower())
            {
                case "mysql":
                    sb.AppendLine($"CREATE TABLE `{nombreTabla}` (");
                    break;
                case "postgresql":
                    sb.AppendLine($"CREATE TABLE \"{nombreTabla}\" (");
                    break;
                case "oracle":
                case "firebird":
                    sb.AppendLine($"CREATE TABLE {nombreTabla} (");
                    break;
                default:
                    return $"-- Gestor no soportado: {gestorDestino}";
            }

            for (int i = 0; i < columnas.Count; i++)
            {
                var col = columnas[i];
                string linea = FormatearColumna(col, gestorDestino);
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

                string camposPK = string.Join(", ", claves.ConvertAll(c => FormatearIdentificador(c.Nombre, gestorDestino)));
                sb.AppendLine($"    PRIMARY KEY ({camposPK})");
            }

            sb.AppendLine(");");

            // === CLAVES FORÁNEAS
            var fks = MigradorRelacional.ObtenerLlavesForaneas(nombreTabla, servidor, usuario, contraseña, baseDatos);
            if (fks.Count > 0)
            {
                sb.AppendLine(); // espacio

                foreach (var fk in fks)
                {
                    string alter = GenerarScriptFK(fk, gestorDestino);
                    sb.AppendLine(alter);
                }
            }

            // === ÍNDICES SECUNDARIOS (non-clustered)
            var indices = MigradorRelacional.ObtenerIndicesNoClustered(nombreTabla, servidor, usuario, contraseña, baseDatos);
            if (indices.Count > 0)
            {
                sb.AppendLine();
                foreach (var idx in indices)
                {
                    sb.AppendLine(idx);
                }
            }

            return sb.ToString();
        }


        private string GenerarScriptFK(MigradorRelacional.ForeignKey fk, string gestor)
        {
            string tabla = FormatearIdentificador(fk.TablaOrigen, gestor);
            string columna = FormatearIdentificador(fk.ColumnaOrigen, gestor);
            string refTabla = FormatearIdentificador(fk.TablaReferencia, gestor);
            string refColumna = FormatearIdentificador(fk.ColumnaReferencia, gestor);
            string constraint = fk.NombreConstraint;

            switch (gestor.ToLower())
            {
                case "mysql":
                    return $"ALTER TABLE {tabla} ADD CONSTRAINT `{constraint}` FOREIGN KEY ({columna}) REFERENCES {refTabla}({refColumna});";
                case "postgresql":
                    return $"ALTER TABLE {tabla} ADD CONSTRAINT \"{constraint}\" FOREIGN KEY ({columna}) REFERENCES {refTabla}({refColumna});";
                case "oracle":
                case "firebird":
                    return $"ALTER TABLE {tabla} ADD CONSTRAINT {constraint} FOREIGN KEY ({columna}) REFERENCES {refTabla}({refColumna});";
                default:
                    return $"-- FOREIGN KEY no soportado para {gestor}";
            }
        }

        private List<ColumnaTabla> ObtenerColumnasDesdeSQLServer(string nombreTabla)
        {
            List<ColumnaTabla> columnas = new List<ColumnaTabla>();
            string connStr = $"Data Source={servidor};Initial Catalog={baseDatos};User ID={usuario};Password={contraseña};";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string query = @"
SELECT 
    c.name AS Columna,
    t.name AS TipoDato,
    c.max_length AS Tamaño,
    c.precision AS Precision,
    c.scale AS Escala,
    c.is_nullable AS EsNula,
    (SELECT COUNT(*) 
     FROM sys.indexes i
     JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
     WHERE i.is_primary_key = 1 AND ic.column_id = c.column_id AND ic.object_id = c.object_id) AS EsPK,
    c.is_identity AS EsAutoIncrement,
    dc.definition AS ValorPorDefecto
FROM 
    sys.columns c
JOIN 
    sys.types t ON c.user_type_id = t.user_type_id
LEFT JOIN 
    sys.default_constraints dc ON c.default_object_id = dc.object_id
WHERE 
    c.object_id = OBJECT_ID(@tabla)
ORDER BY 
    c.column_id;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@tabla", nombreTabla);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            columnas.Add(new ColumnaTabla
                            {
                                Nombre = reader["Columna"].ToString(),
                                Tipo = reader["TipoDato"].ToString(),
                                Tamaño = Convert.ToInt32(reader["Tamaño"]),
                                Precision = Convert.ToByte(reader["Precision"]),
                                Escala = Convert.ToByte(reader["Escala"]),
                                EsNula = Convert.ToBoolean(reader["EsNula"]),
                                EsPrimaryKey = Convert.ToBoolean(reader["EsPK"]),
                                EsAutoIncrement = Convert.ToBoolean(reader["EsAutoIncrement"]),
                                ValorPorDefecto = reader["ValorPorDefecto"]?.ToString()
                            });
                        }
                    }
                }
            }
            return columnas;
        }

        private string FormatearColumna(ColumnaTabla col, string gestor)
        {
            string tipoConvertido = ConvertirTipo(col.Tipo, col.Tamaño, gestor, col.Precision, col.Escala);
            string nulo = col.EsNula ? "" : " NOT NULL";
            string extra = "";

            if (col.EsAutoIncrement && gestor.ToLower() == "mysql")
                extra += " AUTO_INCREMENT";

            bool tipoPermiteDefault =
                !(gestor.ToLower() == "mysql" &&
                 (tipoConvertido.StartsWith("TEXT", StringComparison.OrdinalIgnoreCase) ||
                  tipoConvertido.StartsWith("BLOB", StringComparison.OrdinalIgnoreCase)));

            if (!string.IsNullOrEmpty(col.ValorPorDefecto) && tipoPermiteDefault)
            {
                string def = col.ValorPorDefecto.Trim('(', ')').Replace("N'", "'").Trim();

                if (!decimal.TryParse(def, out _) && !def.StartsWith("'") && !def.EndsWith("'"))
                {
                    def = $"'{def}'";
                }

                extra += $" DEFAULT {def}";
            }

            return $"{FormatearIdentificador(col.Nombre, gestor)} {tipoConvertido}{nulo}{extra}";
        }




        private string FormatearIdentificador(string nombre, string gestor)
        {
            switch (gestor.ToLower())
            {
                case "mysql": return $"`{nombre}`";
                case "postgresql": return $"\"{nombre}\"";
                case "oracle":
                case "firebird": return nombre;
                default: return nombre;
            }
        }

        private string ConvertirTipo(string tipoSqlServer, int tamaño, string gestor, byte precision = 0, byte escala = 0)
        {
            tipoSqlServer = tipoSqlServer.ToLower();
            string tipoDestino = "TEXT"; // valor por defecto de respaldo

            switch (gestor.ToLower())
            {
                case "mysql":
                    if (tipoSqlServer.Contains("int")) tipoDestino = "INT";
                    else if (tipoSqlServer == "bit") tipoDestino = "TINYINT(1)";
                    else if (tipoSqlServer == "nvarchar" || tipoSqlServer == "varchar" ||
                             tipoSqlServer == "nchar" || tipoSqlServer == "char")
                    {
                        int longitud = (tamaño == -1 || tamaño > 5000) ? 255 : tamaño / 2;
                        tipoDestino = $"VARCHAR({longitud})";
                    }
                    else if (tipoSqlServer == "text" || tipoSqlServer == "ntext") tipoDestino = "TEXT";
                    else if (tipoSqlServer == "datetime" || tipoSqlServer == "smalldatetime") tipoDestino = "DATETIME";
                    else if (tipoSqlServer == "decimal" || tipoSqlServer == "numeric" || tipoSqlServer == "money" || tipoSqlServer == "smallmoney")
                        tipoDestino = $"DECIMAL({(precision == 0 ? 19 : precision)},{escala})";
                    else if (tipoSqlServer == "float" || tipoSqlServer == "real") tipoDestino = "FLOAT";
                    else if (tipoSqlServer == "varbinary" || tipoSqlServer == "image" || tipoSqlServer == "binary")
                        tipoDestino = "BLOB";
                    break;

                case "postgresql":
                    if (tipoSqlServer.Contains("int")) tipoDestino = "INTEGER";
                    else if (tipoSqlServer == "bit") tipoDestino = "BOOLEAN";
                    else if (tipoSqlServer == "nvarchar" || tipoSqlServer == "varchar" ||
                             tipoSqlServer == "nchar" || tipoSqlServer == "char")
                    {
                        int longitud = (tamaño == -1 || tamaño > 5000) ? 255 : tamaño / 2;
                        tipoDestino = $"VARCHAR({longitud})";
                    }
                    else if (tipoSqlServer == "text" || tipoSqlServer == "ntext") tipoDestino = "TEXT";
                    else if (tipoSqlServer == "datetime" || tipoSqlServer == "smalldatetime") tipoDestino = "TIMESTAMP";
                    else if (tipoSqlServer == "decimal" || tipoSqlServer == "numeric" || tipoSqlServer == "money" || tipoSqlServer == "smallmoney")
                        tipoDestino = $"NUMERIC({(precision == 0 ? 19 : precision)},{escala})";
                    else if (tipoSqlServer == "float" || tipoSqlServer == "real") tipoDestino = "FLOAT8";
                    else if (tipoSqlServer == "varbinary" || tipoSqlServer == "image" || tipoSqlServer == "binary")
                        tipoDestino = "BYTEA";
                    break;

                case "oracle":
                    if (tipoSqlServer.Contains("int")) tipoDestino = "NUMBER";
                    else if (tipoSqlServer == "bit") tipoDestino = "NUMBER(1)";
                    else if (tipoSqlServer == "nvarchar" || tipoSqlServer == "varchar" ||
                             tipoSqlServer == "nchar" || tipoSqlServer == "char")
                    {
                        int longitud = (tamaño == -1 || tamaño > 5000) ? 255 : tamaño / 2;
                        tipoDestino = $"VARCHAR2({longitud})";
                    }
                    else if (tipoSqlServer == "text" || tipoSqlServer == "ntext") tipoDestino = "CLOB";
                    else if (tipoSqlServer == "datetime" || tipoSqlServer == "smalldatetime") tipoDestino = "DATE";
                    else if (tipoSqlServer == "decimal" || tipoSqlServer == "numeric" || tipoSqlServer == "money" || tipoSqlServer == "smallmoney")
                        tipoDestino = $"NUMBER({(precision == 0 ? 19 : precision)},{escala})";
                    else if (tipoSqlServer == "float" || tipoSqlServer == "real") tipoDestino = "FLOAT";
                    else if (tipoSqlServer == "varbinary" || tipoSqlServer == "image" || tipoSqlServer == "binary")
                        tipoDestino = "BLOB";
                    break;

                case "firebird":
                    if (tipoSqlServer.Contains("int")) tipoDestino = "INTEGER";
                    else if (tipoSqlServer == "bit") tipoDestino = "SMALLINT";
                    else if (tipoSqlServer == "nvarchar" || tipoSqlServer == "varchar" ||
                             tipoSqlServer == "nchar" || tipoSqlServer == "char")
                    {
                        int longitud = (tamaño == -1 || tamaño > 5000) ? 255 : tamaño / 2;
                        tipoDestino = $"VARCHAR({longitud})";
                    }
                    else if (tipoSqlServer == "text" || tipoSqlServer == "ntext") tipoDestino = "BLOB SUB_TYPE TEXT";
                    else if (tipoSqlServer == "datetime" || tipoSqlServer == "smalldatetime") tipoDestino = "TIMESTAMP";
                    else if (tipoSqlServer == "decimal" || tipoSqlServer == "numeric" || tipoSqlServer == "money" || tipoSqlServer == "smallmoney")
                        tipoDestino = $"NUMERIC({(precision == 0 ? 19 : precision)},{escala})";
                    else if (tipoSqlServer == "float" || tipoSqlServer == "real") tipoDestino = "FLOAT";
                    else if (tipoSqlServer == "varbinary" || tipoSqlServer == "image" || tipoSqlServer == "binary")
                        tipoDestino = "BLOB";
                    break;
            }

            return tipoDestino;
        }

        private class ColumnaTabla
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

        }
    }
}
