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
    public class Clienti
    {
        public int IdCliente { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }

        // Attributi di validazione per il codice fiscale
        [Display(Name = "Codice Fiscale")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Codice Fiscale incompleto")]
        [Remote("CFEsistente", "Customer", ErrorMessage = "Cliente già inserito")]
        public string CF { get; set; }

        [Display(Name = "Città")]
        public string Citta { get; set; }

        // Attributi di validazione per la provincia
        [StringLength(3, MinimumLength = 2, ErrorMessage = "Inserire Sigla Provincia")]
        public string Prov { get; set; }

        public string Contatto { get; set; }

        // Attributi di validazione per l'indirizzo email
        [EmailAddress(ErrorMessage = "Inserire indirizzo e-mail valido")]
        [Display(Name = "E-mail")]
        public string email { get; set; }

        [Display(Name = "Pren. nr")]
        public int IdPrenotazione { get; set; }

        // Attributi di formattazione per la data di arrivo
        [Display(Name = "Arrivo")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CheckIn { get; set; }

        // Metodo per ottenere la lista dei clienti
        public static List<SelectListItem> ListaClienti
        {
            get
            {
                List<SelectListItem> selectListItems = new List<SelectListItem>();
                SqlConnection sql = Connessione.GetConnection();
                sql.Open();
                SqlCommand com = Connessione.GetCommand("SELECT * FROM CLIENTI", sql);
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    SelectListItem s = new SelectListItem
                    {
                        Text = reader["Cognome"].ToString() + " " + reader["Nome"].ToString(),
                        Value = reader["IdCliente"].ToString()
                    };

                    selectListItems.Add(s);
                }
                return selectListItems;
            }

        }
    }
}