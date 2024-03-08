using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Gestionale_Albergo.Models
{
    public class Pensione
    {
        // Proprietà per l'ID della pensione
        public int IdPensione { get; set; }

        // Proprietà per il tipo di pensione
        public string Tipo { get; set; }

        // Metodo statico per ottenere la lista delle pensioni come oggetti SelectListItem
        public static List<SelectListItem> ListaPensione
        {
            get
            {
                List<SelectListItem> selectListItems = new List<SelectListItem>();
                SqlConnection sql = Connessione.GetConnection();
                sql.Open();
                SqlCommand com = Connessione.GetCommand("SELECT * FROM PENSIONE", sql);
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    SelectListItem s = new SelectListItem
                    {
                        // Imposta il testo dell'oggetto SelectListItem come il valore della colonna "TipoPensione" nel reader
                        Text = reader["TipoPensione"].ToString(),
                        // Imposta il valore dell'oggetto SelectListItem come il valore della colonna "IdPensione" nel reader
                        Value = reader["IdPensione"].ToString()
                    };

                    selectListItems.Add(s);
                }
                return selectListItems;
            }
        }
    }
}