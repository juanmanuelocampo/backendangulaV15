using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Common
{
    public static class Util
    {
        public static string FecSQL(DateTime? fecha)
        {
            if (fecha == null)
            {
                return "null";
            }
            else
            {
                return "'" + fecha.Value.Year.ToString("00") + fecha.Value.Month.ToString("00") + fecha.Value.Day.ToString("00") + "'";
            }
            
        }
        public static string NVal(int? numero)
        {
            if (numero == null)
            {
                return "0";
            }
            else
            {
                return numero.ToString();
            }

        }
        public static string NVal(decimal? numero)
        {
            if (numero == null)
            {
                return "0";
            }
            else
            {
                return numero.ToString().Replace(",", ".");
            }

        }
        public static string NVal(float? numero)
        {
            if (numero == null)
            {
                return "0";
            }
            else
            {
                return numero.ToString();
            }

        }

        public static string TxtSQL(string texto)
        {
            if (texto != null)
            {
                texto = texto.ToString().Replace("'", "");
            }            
            return (texto == null || texto.Length == 0) ? "NULL" : "'"+texto+"'";
        }

       

    }
}