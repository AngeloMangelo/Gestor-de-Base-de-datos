using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            // Cuerpo: columnas
            for (int i = 0; i < columnas.Count; i++)
            {
                var col = columnas[i];
                string linea = FormatearColumna(col, gestorDestino);
                sb.Append("    " + linea);

                // Agregar coma solo si NO es la última columna
                if (i < columnas.Count - 1)
                    sb.Append(",");

                sb.AppendLine();
            }

            // Llave primaria: si hay columnas, agregamos coma antes del PRIMARY KEY
            var claves = columnas.FindAll(c => c.EsPrimaryKey);
            if (claves.Count > 0)
            {
                if (columnas.Count > 0)
                    sb.AppendLine(",");

                string camposPK = string.Join(", ", claves.ConvertAll(c => FormatearIdentificador(c.Nombre, gestorDestino)));
                sb.AppendLine($"    PRIMARY KEY ({camposPK})");
            }

            sb.AppendLine(");");

            // === LLAVES FORÁNEAS (solo si destino es compatible)
            var fks = ObtenerLlavesForaneas(nombreTabla);
            if (fks.Count > 0)
            {
                sb.AppendLine(); // espacio

                foreach (var fk in fks)
                {
                    string alter = GenerarScriptFK(fk, gestorDestino);
                    sb.AppendLine(alter);
                }
            }

            return sb.ToString();
        }

        private string GenerarScriptFK(ForeignKey fk, string gestor)
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

                string query = $@"
SELECT 
    c.name AS Columna,
    t.name AS TipoDato,
    c.max_length AS Tamaño,
    c.precision AS Precision,
    c.scale AS Escala,
    c.is_nullable AS EsNula,
    ISNULL(i.is_primary_key, 0) AS EsPK,
    c.is_identity AS EsAutoIncrement
FROM 
    sys.columns c
JOIN 
    sys.types t ON c.user_type_id = t.user_type_id
LEFT JOIN 
    sys.index_columns ic ON c.object_id = ic.object_id AND c.column_id = ic.column_id
LEFT JOIN 
    sys.indexes i ON ic.object_id = i.object_id AND ic.index_id = i.index_id
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
                                EsAutoIncrement = Convert.ToBoolean(reader["EsAutoIncrement"])

                            });
                        }
                    }
                }
            }
            return columnas;
        }

        private List<ForeignKey> ObtenerLlavesForaneas(string tabla)
        {
            List<ForeignKey> fks = new List<ForeignKey>();
            string connStr = $"Data Source={servidor};Initial Catalog={baseDatos};User ID={usuario};Password={contraseña};";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string query = @"
SELECT 
    fk.name AS ConstraintName,
    OBJECT_NAME(fkc.parent_object_id) AS TablaOrigen,
    COL_NAME(fkc.parent_object_id, fkc.parent_column_id) AS ColumnaOrigen,
    OBJECT_NAME(fkc.referenced_object_id) AS TablaReferencia,
    COL_NAME(fkc.referenced_object_id, fkc.referenced_column_id) AS ColumnaReferencia
FROM 
    sys.foreign_keys fk
JOIN 
    sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
WHERE 
    OBJECT_NAME(fk.parent_object_id) = @tabla;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@tabla", tabla);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            fks.Add(new ForeignKey
                            {
                                NombreConstraint = reader["ConstraintName"].ToString(),
                                TablaOrigen = reader["TablaOrigen"].ToString(),
                                ColumnaOrigen = reader["ColumnaOrigen"].ToString(),
                                TablaReferencia = reader["TablaReferencia"].ToString(),
                                ColumnaReferencia = reader["ColumnaReferencia"].ToString()
                            });
                        }
                    }
                }
            }

            return fks;
        }


        private string FormatearColumna(ColumnaTabla col, string gestor)
        {
            string tipoConvertido = ConvertirTipo(col.Tipo, col.Tamaño, gestor, col.Precision, col.Escala);
            string nulo = col.EsNula ? "" : " NOT NULL";
            string extra = "";

            // Soporte para AUTO_INCREMENT solo para MySQL
            if (col.EsAutoIncrement && gestor.ToLower() == "mysql")
            {
                extra = " AUTO_INCREMENT";
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
            string tipoDestino = "TEXT";

            switch (gestor.ToLower())
            {
                case "mysql":
                    if (tipoSqlServer.Contains("int")) tipoDestino = "INT";
                    else if (tipoSqlServer == "bit") tipoDestino = "TINYINT(1)";
                    else if (tipoSqlServer == "nvarchar" || tipoSqlServer == "varchar")
                        tipoDestino = $"VARCHAR({(tamaño == -1 ? 255 : tamaño / 2)})";
                    else if (tipoSqlServer == "text" || tipoSqlServer == "ntext") tipoDestino = "TEXT";
                    else if (tipoSqlServer == "datetime") tipoDestino = "DATETIME";
                    else if (tipoSqlServer == "decimal" || tipoSqlServer == "numeric")
                        tipoDestino = $"DECIMAL({precision},{escala})";
                    else if (tipoSqlServer == "float" || tipoSqlServer == "real") tipoDestino = "FLOAT";
                    else if (tipoSqlServer == "varbinary") tipoDestino = "BLOB";
                    break;

                case "postgresql":
                    if (tipoSqlServer.Contains("int")) tipoDestino = "INTEGER";
                    else if (tipoSqlServer == "bit") tipoDestino = "BOOLEAN";
                    else if (tipoSqlServer == "nvarchar" || tipoSqlServer == "varchar")
                        tipoDestino = $"VARCHAR({(tamaño == -1 ? 255 : tamaño / 2)})";
                    else if (tipoSqlServer == "text" || tipoSqlServer == "ntext") tipoDestino = "TEXT";
                    else if (tipoSqlServer == "datetime") tipoDestino = "TIMESTAMP";
                    else if (tipoSqlServer == "decimal" || tipoSqlServer == "numeric")
                        tipoDestino = $"NUMERIC({precision},{escala})";
                    else if (tipoSqlServer == "float" || tipoSqlServer == "real") tipoDestino = "FLOAT8";
                    else if (tipoSqlServer == "varbinary") tipoDestino = "BYTEA";
                    break;

                case "oracle":
                    if (tipoSqlServer.Contains("int")) tipoDestino = "NUMBER";
                    else if (tipoSqlServer == "bit") tipoDestino = "NUMBER(1)";
                    else if (tipoSqlServer == "nvarchar" || tipoSqlServer == "varchar")
                        tipoDestino = $"VARCHAR2({(tamaño == -1 ? 255 : tamaño / 2)})";
                    else if (tipoSqlServer == "text" || tipoSqlServer == "ntext") tipoDestino = "CLOB";
                    else if (tipoSqlServer == "datetime") tipoDestino = "DATE";
                    else if (tipoSqlServer == "decimal" || tipoSqlServer == "numeric")
                        tipoDestino = $"NUMBER({precision},{escala})";
                    else if (tipoSqlServer == "float" || tipoSqlServer == "real") tipoDestino = "FLOAT";
                    else if (tipoSqlServer == "varbinary") tipoDestino = "BLOB";
                    break;

                case "firebird":
                    if (tipoSqlServer.Contains("int")) tipoDestino = "INTEGER";
                    else if (tipoSqlServer == "bit") tipoDestino = "SMALLINT";
                    else if (tipoSqlServer == "nvarchar" || tipoSqlServer == "varchar")
                        tipoDestino = $"VARCHAR({(tamaño == -1 ? 255 : tamaño / 2)})";
                    else if (tipoSqlServer == "text" || tipoSqlServer == "ntext") tipoDestino = "BLOB SUB_TYPE TEXT";
                    else if (tipoSqlServer == "datetime") tipoDestino = "TIMESTAMP";
                    else if (tipoSqlServer == "decimal" || tipoSqlServer == "numeric")
                        tipoDestino = $"NUMERIC({precision},{escala})";
                    else if (tipoSqlServer == "float" || tipoSqlServer == "real") tipoDestino = "FLOAT";
                    else if (tipoSqlServer == "varbinary") tipoDestino = "BLOB";
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

        }

        private class ForeignKey
        {
            public string TablaOrigen { get; set; }
            public string ColumnaOrigen { get; set; }
            public string TablaReferencia { get; set; }
            public string ColumnaReferencia { get; set; }
            public string NombreConstraint { get; set; }
        }

    }
}
