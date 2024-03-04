using progetto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace progetto.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Cliente Cliente)
        {
            var hasValidCredentials = false;

            // cercare l'utente con author.Username e verificare che abbia author.Password nel DB
            hasValidCredentials = true;

            // se credenziali valide
            if (hasValidCredentials)
            {
                FormsAuthentication.SetAuthCookie(Cliente.ClienteId.ToString(), true);
                return RedirectToAction("", ""); // TODO: alla pagina di pannello
            }
            return RedirectToAction("Index");
        }