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
    public class ProvaController : Controller
    {
        // GET: Prova
  
        [Authorize]
        public ActionResult Prova()
        {
            // Ottiene l'ID_Cliente dall'identità dell'utente autenticato
            var iD_ClienteId = HttpContext.User.Identity.Name;

            if (iD_ClienteId != null)
            {
                ViewBag.ID_Cliente = iD_ClienteId;
                ViewBag.Nome = HttpContext.Session["Nome"] as string;
                ViewBag.Cognome = HttpContext.Session["Cognome"] as string;
                // Aggiungi altre proprietà qui se necessario

                return View();

            }

            // Se l'utente non è valido, reindirizza alla pagina di login
            return RedirectToAction("Prova", "Prova");
        }


        // Metodo per l'inserimento di dati nella tabella Cliente
        public void InserisciDatiCliente(Cliente cliente)
        {
            string connString = ConfigurationManager.ConnectionStrings["MYDATABASE"].ToString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                // Query per l'inserimento nella tabella Cliente
                string queryCliente = @"INSERT INTO [SETTIMANA_6 BACK-END].[dbo].[Cliente] 
                             ([Tipo_Cliente], [Codice_Fiscale], [Partita_IVA], [Nome], [Cognome], [Indirizzo]) 
                             VALUES 
                             (@Tipo_Cliente, @Codice_Fiscale, @Partita_IVA, @Nome, @Cognome, @Indirizzo)";

                using (SqlCommand cmd = new SqlCommand(queryCliente, conn))
                {
                    // Parametri per la query
                    cmd.Parameters.AddWithValue("@Tipo_Cliente", cliente.Tipo_Cliente);
                    cmd.Parameters.AddWithValue("@Codice_Fiscale", cliente.Codice_Fiscale);
                    cmd.Parameters.AddWithValue("@Partita_IVA", cliente.Partita_IVA);
                    cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
                    cmd.Parameters.AddWithValue("@Cognome", cliente.Cognome);
                    cmd.Parameters.AddWithValue("@Indirizzo", cliente.Indirizzo);

                    // Esecuzione della query di inserimento
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Metodo per l'inserimento di dati nella tabella Spedizioni
        public void InserisciDatiSpedizione(Spedizioni spedizione)
        {
            string connString = ConfigurationManager.ConnectionStrings["MYDATABASE"].ToString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                // Query per l'inserimento nella tabella Spedizioni
                string querySpedizioni = @"INSERT INTO [SETTIMANA_6 BACK-END].[dbo].[Spedizioni] 
                    ([Numero_Spedizione], [Data_Spedizione], [Peso], [Citta_Destinataria], [Indirizzo_Destinatario], [Nominativo_Destinatario], [Costo_Spedizione], [Data_Consegna_Prevista], [ID_Cliente]) 
                    VALUES 
                    (@Numero_Spedizione, CONVERT(datetime, @Data_Spedizione, 101), @Peso, @Citta_Destinataria, @Indirizzo_Destinatario, @Nominativo_Destinatario, @Costo_Spedizione, CONVERT(datetime, @Data_Consegna_Prevista, 101), @ID_Cliente)";

                using (SqlCommand cmd = new SqlCommand(querySpedizioni, conn))
                {
                    // Parametri per la query
                    cmd.Parameters.AddWithValue("@Numero_Spedizione", spedizione.Numero_Spedizione);
                    cmd.Parameters.AddWithValue("@Data_Spedizione", spedizione.Data_Spedizione.ToString("MM/dd/yyyy"));
                    cmd.Parameters.AddWithValue("@Peso", spedizione.Peso);
                    cmd.Parameters.AddWithValue("@Citta_Destinataria", spedizione.Citta_Destinataria);
                    cmd.Parameters.AddWithValue("@Indirizzo_Destinatario", spedizione.Indirizzo_Destinatario);
                    cmd.Parameters.AddWithValue("@Nominativo_Destinatario", spedizione.Nominativo_Destinatario);
                    cmd.Parameters.AddWithValue("@Costo_Spedizione", spedizione.Costo_Spedizione);
                    cmd.Parameters.AddWithValue("@Data_Consegna_Prevista", spedizione.Data_Consegna_Prevista.ToString("MM/dd/yyyy"));
                    cmd.Parameters.AddWithValue("@ID_Cliente", spedizione.ID_Cliente);

                    // Esecuzione della query di inserimento
                    cmd.ExecuteNonQuery();
                }
            }
        }


        // Metodo per l'inserimento di dati nella tabella stato_spedizioni
        public void InserisciDatiStatoSpedizione(StatoSpedizione statoSpedizione)
        {
            string connString = ConfigurationManager.ConnectionStrings["MYDATABASE"].ToString();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                // Query per l'inserimento nella tabella stato_spedizioni
                string queryStatoSpedizioni = @"INSERT INTO [SETTIMANA_6 BACK-END].[dbo].[stato_spedizioni] 
                             ([ID_Spedizione], [Stato], [Luogo_Pacco], [Descrizione_Eventuale], [Data_Aggiornamento], [Ora_Aggiornamento]) 
                             VALUES 
                             (@ID_Spedizione, @Stato, @Luogo_Pacco, @Descrizione_Eventuale, @Data_Aggiornamento, @Ora_Aggiornamento)";

                using (SqlCommand cmd = new SqlCommand(queryStatoSpedizioni, conn))
                {
                    // Parametri per la query
                    cmd.Parameters.AddWithValue("@ID_Spedizione", statoSpedizione.ID_Spedizione);
                    cmd.Parameters.AddWithValue("@Stato", statoSpedizione.Stato);
                    cmd.Parameters.AddWithValue("@Luogo_Pacco", statoSpedizione.Luogo_Pacco);
                    cmd.Parameters.AddWithValue("@Descrizione_Eventuale", statoSpedizione.Descrizione_Eventuale);
                    cmd.Parameters.AddWithValue("@Data_Aggiornamento", statoSpedizione.Data_Aggiornamento);
                    cmd.Parameters.AddWithValue("@Ora_Aggiornamento", statoSpedizione.Ora_Aggiornamento);

                    // Esecuzione della query di inserimento
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}


