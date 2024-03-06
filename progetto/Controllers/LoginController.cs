using progetto.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace progetto.Controllers
{
    public class LoginController : Controller
    {
        // Azione per la visualizzazione della pagina di login
        public ActionResult Index()
        {
            // Verifica se l'utente è già autenticato e, in caso affermativo, reindirizza alla pagina "Prova"
            if (HttpContext.User.Identity.IsAuthenticated) return RedirectToAction("Prova");
            return View();
        }

        // Azione chiamata quando viene inviato il modulo di login
        [HttpPost]
        public ActionResult Index(Cliente Cliente)
        {
            // Connessione al database utilizzando la stringa di connessione nel file di configurazione
            string connString = ConfigurationManager.ConnectionStrings["MYDATABASE"].ToString();
            var conn = new SqlConnection(connString);
            conn.Open();

            // Query SQL per cercare l'utente nel database
            var command = new SqlCommand("SELECT * FROM Clienti WHERE Tipo_Cliente = @Tipo_Cliente AND Nome = @Nome AND Cognome = @Cognome AND  Partita_IVA = @Partita_IVA AND  Codice_Fiscale = @Codice_Fiscale AND Indirizzo = @Indirizzo", conn);
            command.Parameters.AddWithValue("@Tipo_Cliente", Cliente.Tipo_Cliente);
            command.Parameters.AddWithValue("@Nome", Cliente.Nome);
            command.Parameters.AddWithValue("@Cognome", Cliente.Cognome);
            command.Parameters.AddWithValue("@Codice_Fiscale", Cliente.Codice_Fiscale);
            command.Parameters.AddWithValue("@Partita_IVA", Cliente.Partita_IVA);
            command.Parameters.AddWithValue("@Indirizzo", Cliente.Indirizzo);

            // Esecuzione della query e lettura del risultato
            var reader = command.ExecuteReader();

            // Se ci sono righe nel risultato, l'utente è valido
            if (reader.HasRows)
            {
                reader.Read();

                // Imposta il cookie di autenticazione con l'ID_Cliente e reindirizza alla pagina "Index" del controller "Home"
                FormsAuthentication.SetAuthCookie(reader["ID_Cliente"].ToString(), true);
                return RedirectToAction("Index", "Home"); // TODO: reindirizza alla pagina del pannello
            }

            // Se l'utente non è valido, reindirizza alla pagina di login
            return RedirectToAction("Index");
        }

        // Azione per la visualizzazione della pagina "Prova", accessibile solo agli utenti autenticati
        [Authorize]
        public ActionResult Prova()
        {
            // Ottiene l'ID_Cliente dall'identità dell'utente autenticato
            var iD_ClienteId = HttpContext.User.Identity.Name;

            // Passa l'ID_Cliente alla vista utilizzando ViewBag
            ViewBag.ID_Cliente = iD_ClienteId;
            return View();
        }

        // Azione per il logout
        [Authorize, HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            // Sloggare l'utente distruggendo il cookie di autenticazione
            FormsAuthentication.SignOut();

            // Ridireziona l'utente alla pagina "Index" del controller "Home"
            return RedirectToAction("Index", "Home");
        }

            public ActionResult Register()
            {
                // Controlla se l'utente è già autenticato, in tal caso reindirizza a "Prova"
                if (HttpContext.User.Identity.IsAuthenticated) return RedirectToAction("Prova");

                // Se l'utente non è autenticato, restituisce la vista di registrazione
                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Register([Bind(Exclude = "ID_Cliente")] Cliente cliente)
            {
                // Verifica se tutti i campi del modello sono validi
                if (ModelState.IsValid)
                {
                    // Ottieni la stringa di connessione al database dal file di configurazione
                    string connString = ConfigurationManager.ConnectionStrings["MYDATABASE"].ToString();

                    // Apre una nuova connessione al database
                    using (var conn = new SqlConnection(connString))
                    {
                        conn.Open();

                        // Crea un nuovo comando SQL per l'inserimento di un nuovo cliente nel database
                        using (var command = new SqlCommand(@"
                    INSERT INTO Clienti
                    (Tipo_Cliente, Nome, Cognome, Codice_Fiscale, Partita_IVA, Indirizzo)
                    VALUES (@Tipo_Cliente, @Nome, @Cognome, @Codice_Fiscale, @Partita_IVA, @Indirizzo)
                ", conn))
                        {
                            // Imposta i parametri del comando con i valori del cliente
                            command.Parameters.AddWithValue("@Tipo_Cliente", cliente.Tipo_Cliente);
                            command.Parameters.AddWithValue("@Nome", cliente.Nome);
                            command.Parameters.AddWithValue("@Cognome", cliente.Cognome);
                            command.Parameters.AddWithValue("@Codice_Fiscale", cliente.Codice_Fiscale);
                            command.Parameters.AddWithValue("@Partita_IVA", cliente.Partita_IVA);
                            command.Parameters.AddWithValue("@Indirizzo", cliente.Indirizzo);

                            // Esegue il comando SQL e ottiene il numero di righe interessate
                            var countRows = command.ExecuteNonQuery();
                        }
                    }

                    // Reindirizza all'azione "Index" dopo la registrazione avvenuta con successo
                    return RedirectToAction("Index");
                }

                // Se almeno un campo non è valido, restituisce la vista di registrazione con gli errori
                // Non effettua il reindirizzamento in questo caso
                return View();
            }
        }

    }

