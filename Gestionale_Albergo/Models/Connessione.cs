using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Gestionale_Albergo.Models
{
    public class Connessione
    {
        // Metodo per ottenere la connessione al database
        public static SqlConnection GetConnection()
        {
            string con = ConfigurationManager.ConnectionStrings["AlbergoGest"].ToString();
            SqlConnection sql = new SqlConnection(con);
            return sql;
        }

        // Metodo per ottenere un oggetto SqlCommand per una query
        public static SqlCommand GetCommand(string query, SqlConnection sql)
        {
            SqlCommand com = new SqlCommand();
            com.Connection = sql;
            com.CommandText = query;
            return com;
        }

        // Metodo per ottenere un oggetto SqlCommand per una stored procedure
        public static SqlCommand GetStoreProcedure(string query, SqlConnection sql)
        {
            SqlCommand com = new SqlCommand();
            com.Connection = sql;
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.CommandText = query;
            return com;
        }
    }
}