using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reglas_de_Negocio.Helpers
{
    public static class SqlServerHelper
    {
        public static (string Schema, string Table) DescomponerNombreTabla(string nombreCompleto)
        {
            if (string.IsNullOrWhiteSpace(nombreCompleto))
                throw new ArgumentException("El nombre de tabla no puede estar vacío");

            var partes = nombreCompleto.Split('.');

            if (partes.Length == 1)
                return ("dbo", partes[0]);
            else if (partes.Length == 2)
                return (partes[0], partes[1]);
            else
                throw new ArgumentException("El nombre de tabla tiene un formato inválido");
        }

        public static string ConstruirObjectId(string nombreCompleto)
        {
            var (schema, tabla) = DescomponerNombreTabla(nombreCompleto);
            return $"[{schema}].[{tabla}]";
        }
    }

}
