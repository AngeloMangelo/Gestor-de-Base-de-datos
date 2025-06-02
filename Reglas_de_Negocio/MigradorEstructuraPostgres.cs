using System;
using System.Collections.Generic;
using System.Text;
using BaseDeDatosSQL;
using Reglas_de_Negocio;
using Reglas_de_Negocio.Helpers;
using Reglas_de_Negocio.PostgreSQL;
using Reglas_de_Negocio_PostgreSQL;

namespace Migradores.PostgreSQL
{
    public class MigradorEstructuraPostgres
    {
        private readonly string servidor;
        private readonly string usuario;
        private readonly string contraseña;
        private readonly string baseDatos;

        public MigradorEstructuraPostgres(string servidor, string usuario, string contraseña, string baseDatos)
        {
            this.servidor = servidor;
            this.usuario = usuario;
            this.contraseña = contraseña;
            this.baseDatos = baseDatos;
        }

        //public string GenerarCreateTable(string nombreTabla)
        //{
        //    var columnas = new MigradorEstructura(servidor, usuario, contraseña, baseDatos)
        //                        .GetColumnasDesdeSqlServer(nombreTabla);

        //    if (columnas.Count == 0)
        //        return $"-- La tabla {nombreTabla} no tiene columnas o no se encontró.";

        //    var sb = new StringBuilder();
        //    sb.AppendLine($"CREATE TABLE \"{nombreTabla}\" (");

        //    for (int i = 0; i < columnas.Count; i++)
        //    {
        //        var col = columnas[i];
        //        string linea = PostgreSQLColumnFormatter.FormatearColumna(col);
        //        sb.Append("    " + linea);
        //        if (i < columnas.Count - 1)
        //            sb.Append(",");
        //        sb.AppendLine();
        //    }

        //    var claves = columnas.FindAll(c => c.EsPrimaryKey);
        //    if (claves.Count > 0)
        //    {
        //        if (columnas.Count > 0)
        //            sb.AppendLine(",");

        //        string camposPK = string.Join(", ", claves.ConvertAll(c => $"\"{c.Nombre}\""));
        //        sb.AppendLine($"    PRIMARY KEY ({camposPK})");
        //    }

        //    sb.AppendLine(");");

        //    // Claves foráneas
        //    var fks = MigradorRelacional.ObtenerLlavesForaneas(nombreTabla, servidor, usuario, contraseña, baseDatos);
        //    if (fks.Count > 0)
        //    {
        //        sb.AppendLine();
        //        foreach (var fk in fks)
        //        {
        //            sb.AppendLine(PostgresFKGenerator.GenerarScriptFK(fk));
        //        }
        //    }

        //    // Índices
        //    var indices = MigradorRelacional.ObtenerIndicesNoClustered(nombreTabla, servidor, usuario, contraseña, baseDatos);
        //    if (indices.Count > 0)
        //    {
        //        sb.AppendLine();
        //        foreach (var idx in indices)
        //        {
        //            string idxPostgres = idx
        //                .Replace("`", "\"")
        //                .Replace("ON \"", $"ON \"{nombreTabla}\" ");
        //            sb.AppendLine(idxPostgres);
        //        }
        //    }

        //    return sb.ToString();
        //}
    }
}
