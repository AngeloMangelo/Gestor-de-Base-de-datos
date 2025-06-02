using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace BaseDeDatosSQL
{
    public static class MigradorRelacional
    {
        public class ForeignKey
        {
            public string TablaOrigen { get; set; }
            public string ColumnaOrigen { get; set; }
            public string TablaReferencia { get; set; }
            public string ColumnaReferencia { get; set; }
            public string NombreConstraint { get; set; }
        }

        public class DependenciaTabla
        {
            public string Tabla { get; set; }
            public List<string> DependeDe { get; set; } = new List<string>();
        }

        public static List<ForeignKey> ObtenerLlavesForaneas(string tabla, string servidor, string usuario, string contraseña, string baseDatos)
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

        public static List<DependenciaTabla> ObtenerDependencias(List<string> tablasSeleccionadas, string servidor, string usuario, string contraseña, string baseDatos)
        {
            var dependencias = new Dictionary<string, DependenciaTabla>();

            foreach (var tabla in tablasSeleccionadas)
            {
                var fks = ObtenerLlavesForaneas(tabla, servidor, usuario, contraseña, baseDatos);

                if (!dependencias.ContainsKey(tabla))
                    dependencias[tabla] = new DependenciaTabla { Tabla = tabla };

                foreach (var fk in fks)
                {
                    if (tablasSeleccionadas.Contains(fk.TablaReferencia) && fk.TablaReferencia != tabla)
                    {
                        dependencias[tabla].DependeDe.Add(fk.TablaReferencia);
                    }
                }
            }

            foreach (var tabla in tablasSeleccionadas)
            {
                if (!dependencias.ContainsKey(tabla))
                    dependencias[tabla] = new DependenciaTabla { Tabla = tabla };
            }

            return dependencias.Values.ToList();
        }

        public static List<string> OrdenarTablasPorDependencias(List<DependenciaTabla> deps)
        {
            var resultado = new List<string>();
            var sinDependencias = new Queue<DependenciaTabla>(deps.Where(d => d.DependeDe.Count == 0));
            var mapa = deps.ToDictionary(d => d.Tabla, d => d);

            while (sinDependencias.Count > 0)
            {
                var actual = sinDependencias.Dequeue();
                resultado.Add(actual.Tabla);

                foreach (var otro in mapa.Values)
                {
                    if (otro.DependeDe.Contains(actual.Tabla))
                    {
                        otro.DependeDe.Remove(actual.Tabla);
                        if (otro.DependeDe.Count == 0)
                            sinDependencias.Enqueue(otro);
                    }
                }
            }

            if (resultado.Count != deps.Count)
                throw new Exception("Error: Se detectó una dependencia circular entre tablas.");

            return resultado;
        }
    }
}
