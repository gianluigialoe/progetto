using progetto.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace progetto.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Pacco()
        {
            // Connessione al database usando la stringa di connessione nel file di configurazione.
            string connString = ConfigurationManager.ConnectionStrings["MYDATABASE"].ToString();
            var conn = new SqlConnection(connString);
            conn.Open();

            // Query SQL per selezionare tutti gli stati di spedizione dal database.
            var command = new SqlCommand("SELECT * FROM stato_spedizioni", conn);
            var reader = command.ExecuteReader();

            // Lista per memorizzare gli stati di spedizione.
            List<StatoSpedizione> statiSpedizione = new List<StatoSpedizione>();

            // Verifica se ci sono righe nel risultato della query.
            if (reader.HasRows)
            {
                // Iterazione attraverso le righe del risultato e creazione degli oggetti StatoSpedizione.
                while (reader.Read())
                {
                    var stato = new StatoSpedizione();
                    stato.ID_Stato = (int)reader["ID_Stato"];
                    stato.Stato = (string)reader["Stato"];
                    stato.Luogo_Pacco = (string)reader["Luogo_Pacco"];
                    stato.Descrizione_Eventuale = reader["Descrizione_Eventuale"] as string;
                    stato.Data_Aggiornamento = (DateTime)reader["Data_Aggiornamento"];
                    stato.Ora_Aggiornamento = (TimeSpan)reader["Ora_Aggiornamento"];
                    statiSpedizione.Add(stato);
                }
            }

            return View(statiSpedizione);
        }



        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}