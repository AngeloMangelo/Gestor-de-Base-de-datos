using System.Text;
using BaseDeDatosSQL;
using Reglas_de_Negocio;

namespace Reglas_de_Negocio_PostgreSQL
{
    public static class PostgresFKGenerator
    {
        public static string GenerarScriptFK(MigradorRelacional.ForeignKey fk)
        {
            string tabla = FormatearIdentificador(fk.TablaOrigen);
            string columna = FormatearIdentificador(fk.ColumnaOrigen);
            string refTabla = FormatearIdentificador(fk.TablaReferencia);
            string refColumna = FormatearIdentificador(fk.ColumnaReferencia);
            string constraint = fk.NombreConstraint;

            return $"ALTER TABLE {tabla} ADD CONSTRAINT \"{constraint}\" FOREIGN KEY ({columna}) REFERENCES {refTabla}({refColumna});";
        }

        private static string FormatearIdentificador(string nombre)
        {
            return $"\"{nombre}\"";
        }
    }
}
