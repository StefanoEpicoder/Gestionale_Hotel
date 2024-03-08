using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Deployment.Internal;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Gestionale_Albergo.Models
{
    // Classe che rappresenta un servizio
    public class Servizi
    {
        // Proprietà per l'identificatore del servizio
        public int IdServizio { get; set; }

        // Proprietà per il nome del servizio
        public string Servizio { get; set; }

        // Proprietà per la data di richiesta del servizio
        [Display(Name = "Data Richiesta")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Data { get; set; }

        // Proprietà per la quantità del servizio
        [Display(Name = "Quantità")]
        public int Quantita { get; set; }

        // Proprietà per il prezzo del servizio
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal Prezzo { get; set; }

        // Proprietà per l'identificatore della prenotazione associata al servizio
        [Display(Name = "Nr Pren.")]
        public int IdPrenotazione { get; set; }

        // Proprietà per il numero della camera associata al servizio
        [Display(Name = "Nr Camera")]
        public int NrCamera { get; set; }

        // Proprietà per il nome del cliente associato al servizio
        public string Cliente { get; set; }

        // Proprietà per il totale dei servizi associati a una prenotazione
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal Tot_Service { get; set; }

        // Metodo per calcolare il totale dei servizi associati a una prenotazione
        public decimal TotServices(int id)
        {
            // Apertura della connessione al database
            SqlConnection sql = Connessione.GetConnection();
            sql.Open();

            // Creazione di un oggetto Servizi
            Servizi c = new Servizi();

            try
            {
                // Esecuzione della query per calcolare il totale dei servizi
                SqlCommand com = Connessione.GetCommand("SELECT SUM(PrezzoTot) AS TotServices, IdPrenotazione FROM SERVIZIO group by " +
                    "IdPrenotazione having IdPrenotazione = @IdPrenotazione", sql);
                com.Parameters.AddWithValue("IdPrenotazione", id);

                // Lettura dei risultati della query
                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    // Assegnazione del valore del totale dei servizi
                    c.Tot_Service = Convert.ToDecimal(reader["TotServices"]);
                }
            }
            catch
            {

            }
            finally
            {
                // Chiusura della connessione al database
                sql.Close();
            }

            // Restituzione del totale dei servizi
            return c.Tot_Service;
        }
    }
}