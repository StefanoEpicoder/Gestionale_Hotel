using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gestionale_Albergo.Models
{
    public class Dipendenti
    {
        public int IdUtente { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        // Metodo per verificare l'autenticazione di un dipendente
        public bool Autenticato(string username, string password)
        {
            SqlConnection con = Connessione.GetConnection();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Dipendente WHERE [Username] = @username and [Password]=@Password", con);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                con.Close();
            }
            return false;
        }
    }
}