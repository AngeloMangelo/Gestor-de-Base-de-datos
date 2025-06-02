using System;
using System.Collections.Generic;
using System.Text;
using BaseDeDatosSQL;

namespace Reglas_de_Negocio.PostgreSQL
{
    public static class PostgresRelationalBuilder
    {
        public static List<string> GenerarClavesForaneas(string tabla, List<MigradorRelacional.ForeignKey> fks)
        {
            List<string> sentencias = new List<string>();

            foreach (var fk in fks)
            {
                string alter = $"ALTER TABLE \"{tabla}\" ADD CONSTRAINT \"{fk.NombreConstraint}\" FOREIGN KEY (\"{fk.ColumnaOrigen}\") REFERENCES \"{fk.TablaReferencia}\"(\"{fk.ColumnaReferencia}\");";
                sentencias.Add(alter);
            }

            return sentencias;
        }

        public static List<string> GenerarIndices(string tabla, List<(string NombreIndice, List<ColumnaTabla> Columnas)> indices)
        {
            List<string> sentencias = new List<string>();

            foreach (var idx in indices)
            {
                string columnas = string.Join(", ", idx.Columnas.ConvertAll(c => $"\"{c.Nombre}\""));
                string sentencia = $"CREATE INDEX \"{idx.NombreIndice}\" ON \"{tabla}\" ({columnas});";
                sentencias.Add(sentencia);
            }

            return sentencias;
        }
    }
}
