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
    public class CheckoutController : Controller
    {
        // GET: Checkout
        public ActionResult ListaCk()
        {
            SqlConnection sql = Connessione.GetConnection(); // Stabilisce una connessione al database
            sql.Open(); // Apre la connessione al database
            List<Prenotazioni> Bookings = new List<Prenotazioni>(); // Crea una lista per memorizzare le prenotazioni

            try
            {
                SqlCommand com = Connessione.GetStoreProcedure("GetBookings", sql); // Crea un comando SQL per eseguire la stored procedure "GetBookings"
                SqlDataReader reader = com.ExecuteReader(); // Esegue il comando SQL e ottiene il data reader

                while (reader.Read()) // Cicla attraverso il data reader
                {
                    Prenotazioni p = new Prenotazioni // Crea una nuova istanza della classe "Prenotazioni"
                    {
                        IdPrenotazione = Convert.ToInt32(reader["IdPrenotazione"]), // Ottiene il valore della colonna "IdPrenotazione" dal data reader e lo converte in intero
                        IdCliente = Convert.ToInt32(reader["IdCliente"]), // Ottiene il valore della colonna "IdCliente" dal data reader e lo converte in intero
                        Cliente = reader["Cognome"].ToString() + " " + reader["Nome"].ToString(), // Ottiene i valori delle colonne "Cognome" e "Nome" dal data reader e li concatena come stringa
                        NrCamera = Convert.ToInt32(reader["NrCamera"]), // Ottiene il valore della colonna "NrCamera" dal data reader e lo converte in intero
                        DataPren = Convert.ToDateTime(reader["DataPrenotazione"]), // Ottiene il valore della colonna "DataPrenotazione" dal data reader e lo converte in DateTime
                        CheckIn = Convert.ToDateTime(reader["DataArrivo"]), // Ottiene il valore della colonna "DataArrivo" dal data reader e lo converte in DateTime
                        CheckOut = Convert.ToDateTime(reader["DataUscita"]), // Ottiene il valore della colonna "DataUscita" dal data reader e lo converte in DateTime
                        Acconto = Convert.ToDecimal(reader["Acconto"]), // Ottiene il valore della colonna "Acconto" dal data reader e lo converte in decimal
                        Prezzo = Convert.ToDecimal(reader["PrezzoSoggiorno"]), // Ottiene il valore della colonna "PrezzoSoggiorno" dal data reader e lo converte in decimal
                        Pensione = reader["TipoPensione"].ToString(), // Ottiene il valore della colonna "TipoPensione" dal data reader come stringa
                        Tot = Prenotazioni.TotServizi(Convert.ToInt32(reader["IdPrenotazione"])), // Chiama il metodo statico "TotServizi" della classe "Prenotazioni" e passa il valore della colonna "IdPrenotazione" come argomento
                        Saldo = Prenotazioni.DaSaldare(Convert.ToDecimal(reader["PrezzoSoggiorno"]), Convert.ToDecimal(reader["Acconto"]), Prenotazioni.TotServizi(Convert.ToInt32(reader["IdPrenotazione"]))) // Chiama il metodo statico "DaSaldare" della classe "Prenotazioni" e passa i valori delle colonne "PrezzoSoggiorno", "Acconto" e "IdPrenotazione" come argomenti
                    };
                    Bookings.Add(p); // Aggiunge l'istanza creata di "Prenotazioni" alla lista delle prenotazioni
                }
            }
            catch (Exception ex)
            {
                ViewBag.msgerror = ex.Message; // Imposta il messaggio di errore nella ViewBag
            }
            finally
            {
                sql.Close(); // Chiude la connessione al database
            }

            return View(Bookings); // Restituisce la lista delle prenotazioni alla vista
        }

        // GET: Checkout/Details/5
        public ActionResult Details(int id)
        {
            SqlConnection sql = Connessione.GetConnection(); // Stabilisce una connessione al database
            sql.Open(); // Apre la connessione al database
            Prenotazioni b = new Prenotazioni(); // Crea una nuova istanza della classe "Prenotazioni"

            try
            {
                SqlCommand com = Connessione.GetStoreProcedure("GetBookingById", sql); // Crea un comando SQL per eseguire la stored procedure "GetBookingById"
                com.Parameters.AddWithValue("IdPrenotazione", id); // Aggiunge il parametro "IdPrenotazione" al comando SQL

                SqlDataReader reader = com.ExecuteReader(); // Esegue il comando SQL e ottiene il data reader

                while (reader.Read()) // Cicla attraverso il data reader
                {
                    b.IdPrenotazione = Convert.ToInt32(reader["IdPrenotazione"]); // Ottiene il valore della colonna "IdPrenotazione" dal data reader e lo converte in intero
                    b.Cliente = reader["Cognome"].ToString() + " " + reader["Nome"].ToString(); // Ottiene i valori delle colonne "Cognome" e "Nome" dal data reader e li concatena come stringa
                    b.NrCamera = Convert.ToInt32(reader["NrCamera"]); // Ottiene il valore della colonna "NrCamera" dal data reader e lo converte in intero
                    b.DataPren = Convert.ToDateTime(reader["DataPrenotazione"]); // Ottiene il valore della colonna "DataPrenotazione" dal data reader e lo converte in DateTime
                    b.CheckIn = Convert.ToDateTime(reader["DataArrivo"]); // Ottiene il valore della colonna "DataArrivo" dal data reader e lo converte in DateTime
                    b.CheckOut = Convert.ToDateTime(reader["DataUscita"]); // Ottiene il valore della colonna "DataUscita" dal data reader e lo converte in DateTime
                    b.Acconto = Convert.ToDecimal(reader["Acconto"]); // Ottiene il valore della colonna "Acconto" dal data reader e lo converte in decimal
                    b.Prezzo = Convert.ToDecimal(reader["PrezzoSoggiorno"]); // Ottiene il valore della colonna "PrezzoSoggiorno" dal data reader e lo converte in decimal
                    b.Pensione = reader["TipoPensione"].ToString(); // Ottiene il valore della colonna "TipoPensione" dal data reader come stringa
                    b.Tot = Prenotazioni.TotServizi(Convert.ToInt32(reader["IdPrenotazione"])); // Chiama il metodo statico "TotServizi" della classe "Prenotazioni" e passa il valore della colonna "IdPrenotazione" come argomento
                    b.Saldo = Prenotazioni.DaSaldare(Convert.ToDecimal(reader["PrezzoSoggiorno"]), Convert.ToDecimal(reader["Acconto"]), Prenotazioni.TotServizi(Convert.ToInt32(reader["IdPrenotazione"]))); // Chiama il metodo statico "DaSaldare" della classe "Prenotazioni" e passa i valori delle colonne "PrezzoSoggiorno", "Acconto" e "IdPrenotazione" come argomenti
                }
            }
            catch (Exception ex)
            {
                ViewBag.msgerror = ex.Message; // Imposta il messaggio di errore nella ViewBag
            }
            finally
            {
                sql.Close(); // Chiude la connessione al database
            }

            return View(b); // Restituisce i dettagli della prenotazione alla vista
        }
    }
}
        
