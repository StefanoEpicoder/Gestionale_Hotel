using Gestionale_Albergo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Gestionale_Albergo.Controllers
{
    public class HomeController : Controller
    {
        // Metodo per visualizzare la pagina di login
        public ActionResult Login()
        {
            return View();
        }

        // Metodo per gestire la richiesta di login
        [HttpPost]
        public ActionResult Login(Dipendenti d)
        {
            // Verifica se le credenziali dell'utente sono valide
            if (d.Autenticato(d.Username, d.Password))
            {
                // Imposta il cookie di autenticazione
                FormsAuthentication.SetAuthCookie(d.Username, false);
                return Redirect(FormsAuthentication.DefaultUrl);
            }
            return View();
        }

        // Metodo per effettuare il logout
        public ActionResult Logout()
        {
            // Effettua il logout rimuovendo il cookie di autenticazione
            FormsAuthentication.SignOut();
            return Redirect(FormsAuthentication.LoginUrl);
        }

    }
}