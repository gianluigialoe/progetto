using progetto.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace progetto.Controllers
{
    public class PostController : Controller
    {
        // Azione per visualizzare l'elenco dei post, accessibile anche agli utenti non autenticati.
        [AllowAnonymous]
        public ActionResult Index()
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


        // Azione per visualizzare i dettagli di un singolo post, accessibile anche agli utenti non autenticati.
        [AllowAnonymous]
        // Azione per visualizzare i dettagli di una spedizione specifica.
        public ActionResult Show(int? id)
        {
            // Se l'id è nullo, reindirizza alla pagina di elenco delle spedizioni.
            if (id == null)
            {
                return RedirectToAction("Index", "Spedizioni");
            }

            // Connessione al database usando la stringa di connessione nel file di configurazione.
            string connString = ConfigurationManager.ConnectionStrings["MYDATABASE"].ToString();

            // Utilizzo di un blocco using per garantire la chiusura automatica della connessione.
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();

                // Query SQL per ottenere i dettagli della spedizione, inclusi i dati relativi allo stato e al cliente.
                var command = new SqlCommand(@"
            SELECT 
                ss.ID_Stato,
                ss.ID_Spedizione,
                ss.Stato,
                ss.Luogo_Pacco,
                ss.Descrizione_Eventuale,
                ss.Data_Aggiornamento,
                ss.Ora_Aggiornamento,
                s.Numero_Spedizione,
                s.Data_Spedizione,
                s.Peso,
                s.Citta_Destinataria,
                s.Indirizzo_Destinatario,
                s.Nominativo_Destinatario,
                s.Costo_Spedizione,
                s.Data_Consegna_Prevista,
                c.ID_Cliente,
                c.Tipo_Cliente,
                c.Codice_Fiscale,
                c.Partita_IVA,
                c.Nome,
                c.Cognome,
                c.Indirizzo
            FROM [SETTIMANA_6 BACK-END].[dbo].[stato_spedizioni] ss
            JOIN [SETTIMANA_6 BACK-END].[dbo].[Spedizioni] s ON ss.ID_Spedizione = s.ID_Spedizione
            JOIN [SETTIMANA_6 BACK-END].[dbo].[Clienti] c ON s.ID_Cliente = c.ID_Cliente
            WHERE ss.ID_Spedizione = @idSpedizione;
        ", conn);

                // Passa il parametro dell'ID della spedizione.
                command.Parameters.AddWithValue("@idSpedizione", id);

                // Esecuzione della query.
                var reader = command.ExecuteReader();

                // Creazione di un oggetto SpedizioneViewModel per contenere i dati risultanti.
                var viewModel = new SpedizioneViewModel();

                // Verifica se ci sono righe nel risultato della query.
                if (reader.HasRows)
                {
                    reader.Read();

                    // Popolamento del view model con i dati risultanti dalla query.
                    viewModel.Stato = new StatoSpedizione
                    {
                        ID_Stato = (int)reader["ID_Stato"],
                        ID_Spedizione = (int)reader["ID_Spedizione"],
                        Stato = (string)reader["Stato"],
                        Luogo_Pacco = (string)reader["Luogo_Pacco"],
                        Descrizione_Eventuale = (string)reader["Descrizione_Eventuale"],
                        Data_Aggiornamento = (DateTime)reader["Data_Aggiornamento"],
                        Ora_Aggiornamento = (TimeSpan)reader["Ora_Aggiornamento"]
                    };

                    viewModel.Spedizione = new Spedizione
                    {
                        ID_Spedizione = (int)reader["ID_Spedizione"],
                        Numero_Spedizione = (string)reader["Numero_Spedizione"],
                        Data_Spedizione = (DateTime)reader["Data_Spedizione"],
                        Peso = (decimal)reader["Peso"],
                        Citta_Destinataria = (string)reader["Citta_Destinataria"],
                        Indirizzo_Destinatario = (string)reader["Indirizzo_Destinatario"],
                        Nominativo_Destinatario = (string)reader["Nominativo_Destinatario"],
                        Costo_Spedizione = (decimal)reader["Costo_Spedizione"],
                        Data_Consegna_Prevista = (DateTime)reader["Data_Consegna_Prevista"]
                    };

                    viewModel.Cliente = new Cliente
                    {
                        ID_Cliente = (int)reader["ID_Cliente"],
                        Tipo_Cliente = (string)reader["Tipo_Cliente"],
                        Codice_Fiscale = (string)reader["Codice_Fiscale"],
                        Partita_IVA = (string)reader["Partita_IVA"],
                        Nome = (string)reader["Nome"],
                        Cognome = (string)reader["Cognome"],
                        Indirizzo = (string)reader["Indirizzo"]
                    };
                }

                // Chiude il reader e la connessione al database.
                reader.Close();

                // Restituisce la vista con il view model popolato.
                return View(viewModel);
            }
        }

        // Chiude il reader e la connessione al database.
        reader.Close();

                // Restituisce la vista con il view model popolato.
                return View(viewModel);
            }
        }


        // Azione per visualizzare la pagina di creazione di un nuovo post.
        public ActionResult Add()
        {
            return View();
        }

        // Azione per elaborare la creazione di un nuovo post, con validazione CSRF.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "Title,Contents")] Post post)
        {
            // Se il modello è valido, procedi con l'inserimento del nuovo post.
            if (ModelState.IsValid)
            {
                // Connessione al database usando la stringa di connessione nel file di configurazione.
                string connString = ConfigurationManager.ConnectionStrings["DbBlogConnection"].ToString();
                var conn = new SqlConnection(connString);
                conn.Open();

                // Query SQL per inserire un nuovo post nel database.
                var command = new SqlCommand(@"
                    INSERT INTO Posts
                    (Title, Contents, CategoryId, AuthorId)
                    VALUES (@title, @contents, @categoryId, @authorId)", conn);
                command.Parameters.AddWithValue("@title", post.Title);
                command.Parameters.AddWithValue("@contents", post.Contents);
                command.Parameters.AddWithValue("@categoryId", 1); // TODO: Aggiorna questo valore
                command.Parameters.AddWithValue("@authorId", HttpContext.User.Identity.Name);
                var numRows = command.ExecuteNonQuery();

                // Reindirizza alla pagina di elenco dei post.
                return RedirectToAction("Index");
            }

            ViewBag.isValid = false;
            return View(post);
        }

        // Azione per visualizzare la pagina di modifica di un post esistente.
        public ActionResult Edit(int? id)
        {
            // Se l'id è nullo, reindirizza alla pagina di elenco dei post.
            if (id == null) return RedirectToAction("Index");

            // Connessione al database usando la stringa di connessione nel file di configurazione.
            string connString = ConfigurationManager.ConnectionStrings["DbBlogConnection"].ToString();
            var conn = new SqlConnection(connString);
            conn.Open();

            // Query SQL per ottenere i dettagli di un post specifico, incluso il nome della categoria e dell'autore.
            var command = new SqlCommand(@"
                SELECT * FROM Posts
                INNER JOIN Categories ON (Posts.CategoryId = Categories.CategoryId)
                INNER JOIN Authors ON (Posts.AuthorId = Authors.AuthorId)
                WHERE Posts.PostId = @postId
            ", conn);
            command.Parameters.AddWithValue("@postId", id);
            var reader = command.ExecuteReader();

            // Creazione di un oggetto Post e popolamento con i dati risultanti dalla query.
            var post = new Post();
            if (reader.HasRows)
            {
                reader.Read();

                // Verifica se l'utente autenticato è l'autore del post. In caso contrario, reindirizza alla pagina di elenco dei post.
                if (HttpContext.User.Identity.Name.ToString() != reader["AuthorId"].ToString()) return RedirectToAction("Index");

                post.Category = new Category()
                {
                    CategoryId = (int)reader["CategoryId"],
                    Name = (string)reader["Name"],
                    Description = (string)reader["Description"],
                };

                post.Author = new Author()
                {
                    AuthorId = (int)reader["AuthorId"],
                    Username = (string)reader["Username"],
                };

                post.PostId = (int)reader["PostId"];
                post.Title = (string)reader["Title"];
                post.Contents = (string)reader["Contents"];
                post.CategoryId = (int)reader["CategoryId"];
                post.AuthorId = (int)reader["AuthorId"];
            }

            reader.Close();

            // Query SQL per ottenere la lista delle categorie dal database.
            var commandListCategories = new SqlCommand("SELECT * FROM Categories", conn);
            var readerCategories = command.ExecuteReader();

            // Lista per memorizzare le categorie.
            var categories = new List<Category>();
            if (readerCategories.HasRows)
            {
                // Iterazione attraverso le righe del risultato e creazione degli oggetti Category.
                while (readerCategories.Read())
                {
                    var category = new Category()
                    {
                        CategoryId = (int)readerCategories["CategoryId"],
                        Name = (string)readerCategories["Name"],
                        Description = (string)readerCategories["Description"]
                    };
                    categories.Add(category);
                }
            }

            // Passaggio delle categorie alla vista.
            ViewBag.Categories = categories;
            conn.Close();

            return View(post);
        }

        // Azione per elaborare la modifica di un post esistente.
        [HttpPost]
        public ActionResult Edit(Post post)
        {
            // TODO: Implementare i controlli sull'autore del post prima di consentire la modifica.
            // if (HttpContext.User.Identity.Name.ToString() != reader["AuthorId"].ToString()) return RedirectToAction("Index");
            return View();
        }
    }
}