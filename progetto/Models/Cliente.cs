using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace progetto.Models
{
    public class Cliente
    {
        [Key]
        public int ID_Cliente { get; set; }

        [Required(ErrorMessage = "Il campo Tipo Cliente è obbligatorio.")]
        [Display(Name = "Tipo Cliente")]
        public string Tipo_Cliente { get; set; }

        [StringLength(16, ErrorMessage = "Il Codice Fiscale deve essere lungo al massimo 16 caratteri.")]
        [Display(Name = "Codice Fiscale")]
        public string Codice_Fiscale { get; set; }

        [StringLength(20, ErrorMessage = "La Partita IVA deve essere lunga al massimo 20 caratteri.")]
        [Display(Name = "Partita IVA")]
        public string Partita_IVA { get; set; }

        [Required(ErrorMessage = "Il campo Nome è obbligatorio.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Il campo Cognome è obbligatorio.")]
        public string Cognome { get; set; }

        [Required(ErrorMessage = "Il campo Indirizzo è obbligatorio.")]
        public string Indirizzo { get; set; }
        // Altri campi necessari
    }

}
