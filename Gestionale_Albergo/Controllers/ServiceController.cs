using Gestionale_Albergo.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gestionale_Albergo.Controllers
{
    [Authorize]
    public class ServiceController : Controller
    {
        // GET: Service
        public ActionResult ServiceList()
        {
            SqlConnection sql = Connessione.GetConnection(); // Ottieni una connessione al database
            sql.Open(); // Apri la connessione al database
            List<Servizi> Services = new List<Servizi>(); // Crea una lista per memorizzare i servizi

            try
            {
                SqlCommand com = Connessione.GetStoreProcedure("GetAllData", sql); // Ottieni un comando di stored procedure
                SqlDataReader reader = com.ExecuteReader(); // Esegui il comando e ottieni il lettore dei dati

                while (reader.Read()) // Leggi ogni riga di dati
                {
                    Servizi c = new Servizi(); // Crea un nuovo oggetto servizio

                    c.IdPrenotazione = Convert.ToInt32(reader["IdPrenotazione"]); // Ottieni il valore di IdPrenotazione dal lettore e convertilo in intero
                    c.Cliente = reader["Cognome"].ToString() + " " + reader["Nome"].ToString(); // Ottieni i valori di Cognome e Nome dal lettore e concatenali come valore di Cliente
                    c.NrCamera = Convert.ToInt32(reader["NrCamera"]); // Ottieni il valore di NrCamera dal lettore e convertilo in intero
                    c.Prezzo = Convert.ToDecimal(reader["TotServices"]); // Ottieni il valore di TotServices dal lettore e convertilo in decimale

                    Services.Add(c); // Aggiungi l'oggetto servizio alla lista
                }
            }
            catch (Exception ex)
            {
                ViewBag.msgerror = ex.Message; // Memorizza il messaggio di errore nella ViewBag
            }
            finally
            {
                sql.Close(); // Chiudi la connessione al database
            }

            return View(Services); // Restituisci la lista di servizi alla vista
        }

        // Altri metodi omessi per brevità
    }
}
