using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;
using System.Xml.Linq;

namespace Gestionale_Albergo.Models
{
    public class Camere
    {
        // Numero della camera
        [Display(Name = "Nr Camera")]
        public int NrCamera { get; set; }

        // Tipo della camera
        public string Tipo { get; set; }

        // Descrizione della camera
        public string Descrizione { get; set; }

        // Metodo per ottenere la lista delle camere
        public static List<SelectListItem> ListaCamere
        {
            get
            {
                List<SelectListItem> selectListItems = new List<SelectListItem>();
                SqlConnection sql = Connessione.GetConnection();
                sql.Open();
                SqlCommand com = Connessione.GetCommand("SELECT * FROM CAMERA", sql);
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    SelectListItem s = new SelectListItem
                    {
                        // Testo dell'elemento della lista
                        Text = reader["NrCamera"].ToString() + " - " + reader["TipoCamera"].ToString() + " " + reader["Descrizione"].ToString(),
                        // Valore dell'elemento della lista
                        Value = reader["NrCamera"].ToString()
                    };

                    selectListItems.Add(s);
                }

                return selectListItems;
            }

        }

    }
}