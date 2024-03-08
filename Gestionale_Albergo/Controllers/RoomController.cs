using Gestionale_Albergo.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Gestionale_Albergo.Controllers
{
    [Authorize]
    public class RoomController : Controller
    {
        // GET: Room
        public ActionResult RoomList()
        {
            SqlConnection sql = Connessione.GetConnection();
            sql.Open();
            List<Camere> Rooms = new List<Camere>();

            try
            {
                SqlCommand com = Connessione.GetCommand("SELECT * FROM CAMERA", sql);
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Camere c = new Camere();

                    c.NrCamera = Convert.ToInt32(reader["NrCamera"]);                    
                    c.Descrizione = reader["Descrizione"].ToString();
                    c.Tipo = reader["TipoCamera"].ToString();

                    Rooms.Add(c);

                }
            }
            catch (Exception ex)
            {
                ViewBag.msgerror = ex.Message;
            }
            finally { sql.Close(); }

            return View(Rooms);
        }


        // GET: Room/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Room/Create
        [HttpPost]
        public ActionResult Create(Camere c)
        {
            SqlConnection sql = Connessione.GetConnection();
            sql.Open();

            try
            {
                SqlCommand com = Connessione.GetCommand("Insert into Camera values (@NrCamera, @Descrizione, @TipoCamera) ", sql);


                com.Parameters.AddWithValue("NrCamera", c.NrCamera);
                com.Parameters.AddWithValue("Descrizione", c.Descrizione);
                com.Parameters.AddWithValue("TipoCamera", c.Tipo);


                com.ExecuteNonQuery();
            }
            catch
            {

            }
            finally { sql.Close(); }
            return RedirectToAction("RoomList");
        }

        // GET: Room/Edit/5
        public ActionResult Edit(int id)
        {
            SqlConnection sql = Connessione.GetConnection();
            sql.Open();
            Camere c = new Camere();

            try
            {
                SqlCommand com = Connessione.GetCommand("SELECT * FROM CAMERA where NrCamera=@NrCamera", sql);
                com.Parameters.AddWithValue("NrCamera", id);

                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {

                    c.NrCamera = Convert.ToInt32(reader["NrCamera"]);
                    c.Descrizione = reader["Descrizione"].ToString();
                    c.Tipo = reader["TipoCamera"].ToString();

                }
            }
            catch (Exception ex)
            {
                ViewBag.msgerror = ex.Message;
            }
            finally { sql.Close(); }
            return View(c);
        }

        // POST: Room/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Camere c)
        {
            SqlConnection sql = Connessione.GetConnection();
            sql.Open();

            try
            {
                SqlCommand com = Connessione.GetCommand("UPDATE CAMERA SET NrCamera=@NrCamera, Descrizione=@Descrizione, TipoCamera=@Tipo where NrCamera=@id", sql);

                com.Parameters.AddWithValue("NrCamera", c.NrCamera);
                com.Parameters.AddWithValue("Descrizione", c.Descrizione);
                com.Parameters.AddWithValue("Tipo", c.Tipo);
                com.Parameters.AddWithValue("id", id);

                com.ExecuteNonQuery();

            }
            catch
            {

            }
            finally { sql.Close(); }
            return RedirectToAction("RoomList");
        }

        // GET: Room/Delete/5
        public ActionResult Delete(int id)
        {
            //Controllo se ci sono prenotazioni collegate
            SqlConnection sqlc = Connessione.GetConnection();
            sqlc.Open();

            try
            {
                SqlCommand comm = Connessione.GetCommand("SELECT * FROM Prenotazione where NrCamera=@NrCamera", sqlc);
                comm.Parameters.AddWithValue("NrCamera", id);

                SqlDataReader reader = comm.ExecuteReader();

                if (reader.HasRows)
                {
                    ViewBag.msgBooking = "Impossibile eliminare la camera, ci sono ancora delle prenotazioni collegate. Aggiorna le tue prenotazioni";
                }
            }
            catch { }
            finally { sqlc.Close(); }

            SqlConnection sql = Connessione.GetConnection();
            sql.Open();
            Camere c = new Camere();

            try
            {
                SqlCommand com = Connessione.GetCommand("SELECT * FROM CAMERA where NrCamera=@NrCamera", sql);
                com.Parameters.AddWithValue("NrCamera", id);

                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {

                    c.NrCamera = Convert.ToInt32(reader["NrCamera"]);
                    c.Descrizione = reader["Descrizione"].ToString();
                    c.Tipo = reader["TipoCamera"].ToString();

                }
            }
            catch (Exception ex)
            {
                ViewBag.msgerror = ex.Message;
            }
            finally { sql.Close(); }

            return View(c);
        }

        // POST: Room/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Camere c)
        {

            //Controllo se ci sono prenotazioni collegate
            SqlConnection sqlc = Connessione.GetConnection();
            sqlc.Open();

            try
            {
                SqlCommand comm = Connessione.GetCommand("SELECT * FROM Prenotazione where NrCamera=@NrCamera", sqlc);
                comm.Parameters.AddWithValue("NrCamera", id);

                SqlDataReader reader = comm.ExecuteReader();

                if (reader.HasRows)
                {
                    ViewBag.msgBooking = "Impossibile eliminare la camera, ci sono ancora delle prenotazioni collegate.";
                    return RedirectToAction("Delete");
                }
            }
            catch { }
            finally { sqlc.Close(); }


            //ELIMINA CAMERA

            SqlConnection sql = Connessione.GetConnection();
            sql.Open();

            SqlCommand com = Connessione.GetCommand("DELETE FROM CAMERA WHERE NrCamera=@Id", sql);

            com.Parameters.AddWithValue("Id", id);

            com.ExecuteNonQuery();

            sql.Close();

          
            return RedirectToAction("RoomList");
        }
    }
    }

