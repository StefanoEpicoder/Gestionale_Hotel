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
    public class QueriesController : Controller
    {
        // GET: Queries
        public ActionResult QueriesPage()
        {
            return View();
        }

        // JSONRESULT GET BOOKINGS BY CF
        public JsonResult GetBookingsByCF(string CF)
        {
            SqlConnection sql = Connessione.GetConnection(); // Stabilisce una connessione al database
            sql.Open(); // Apre la connessione
            List<Prenotazioni> PrenotazioniCliente = new List<Prenotazioni>(); // Crea una lista per memorizzare le prenotazioni

            try
            {
                SqlCommand com = Connessione.GetCommand("SELECT * FROM PRENOTAZIONE AS P INNER JOIN CLIENTI AS C " +
                    "ON C.IDCLIENTE = P.IDCLIENTE inner join Pensione AS T ON T.IdPensione=P.IdPensione WHERE Cod_Fiscale = @CF", sql); // Crea un comando SQL per recuperare le prenotazioni per CF
                com.Parameters.AddWithValue("CF", CF); // Imposta il valore del parametro

                SqlDataReader reader = com.ExecuteReader(); // Esegue il comando e recupera i dati

                while (reader.Read()) // Loop attraverso i dati
                {
                    Prenotazioni b = new Prenotazioni(); // Crea un nuovo oggetto prenotazione
                    b.IdPrenotazione = Convert.ToInt32(reader["IdPrenotazione"]); // Imposta le proprietà dell'oggetto prenotazione
                    b.Cliente = reader["Cognome"].ToString() + " " + reader["Nome"].ToString();
                    b.NrCamera = Convert.ToInt32(reader["NrCamera"]);
                    b.DataPren = Convert.ToDateTime(reader["DataPrenotazione"]);
                    b.CheckIn = Convert.ToDateTime(reader["DataArrivo"]);
                    b.CheckOut = Convert.ToDateTime(reader["DataUscita"]);
                    b.Acconto = Convert.ToDecimal(reader["Acconto"]);
                    b.Prezzo = Convert.ToDecimal(reader["PrezzoSoggiorno"]);
                    b.Pensione = reader["TipoPensione"].ToString();
                    b.Tot = Prenotazioni.TotServizi(Convert.ToInt32(reader["IdPrenotazione"]));
                    b.Saldo = Prenotazioni.DaSaldare(Convert.ToDecimal(reader["PrezzoSoggiorno"]), Convert.ToDecimal(reader["Acconto"]), Prenotazioni.TotServizi(Convert.ToInt32(reader["IdPrenotazione"])));
                    PrenotazioniCliente.Add(b); // Aggiunge l'oggetto prenotazione alla lista
                }
            }
            catch
            {
                // Gestisce eventuali eccezioni
            }
            finally
            {
                sql.Close(); // Chiude la connessione
            }

            return Json(PrenotazioniCliente, JsonRequestBehavior.AllowGet); // Restituisce la lista delle prenotazioni come JSON
        }

        // JSONRESULT GET BOOKINGS BY Type
        public JsonResult GetBookingsByType(string Type)
        {
            SqlConnection sql = Connessione.GetConnection(); // Stabilisce una connessione al database
            sql.Open(); // Apre la connessione
            List<Prenotazioni> PrenotazioniCliente = new List<Prenotazioni>(); // Crea una lista per memorizzare le prenotazioni

            try
            {
                SqlCommand com = Connessione.GetCommand("SELECT COUNT(*) as TotPren, TipoPensione FROM PENSIONE" +
                    " inner join Prenotazione ON Pensione.IdPensione=Prenotazione.IdPensione group by TipoPensione having TipoPensione = @Type", sql); // Crea un comando SQL per recuperare le prenotazioni per tipo
                com.Parameters.AddWithValue("Type", Type); // Imposta il valore del parametro

                SqlDataReader reader = com.ExecuteReader(); // Esegue il comando e recupera i dati

                while (reader.Read()) // Loop attraverso i dati
                {
                    Prenotazioni b = new Prenotazioni(); // Crea un nuovo oggetto prenotazione
                    b.TotPren = Convert.ToInt32(reader["TotPren"]); // Imposta le proprietà dell'oggetto prenotazione
                    b.Pensione = reader["TipoPensione"].ToString();

                    PrenotazioniCliente.Add(b); // Aggiunge l'oggetto prenotazione alla lista
                }
            }
            catch
            {
                // Gestisce eventuali eccezioni
            }
            finally
            {
                sql.Close(); // Chiude la connessione
            }

            return Json(PrenotazioniCliente, JsonRequestBehavior.AllowGet); // Restituisce la lista delle prenotazioni come JSON
        }

    }
}
