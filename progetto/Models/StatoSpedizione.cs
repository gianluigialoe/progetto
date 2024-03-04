using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace progetto.Models
{
    public class StatoSpedizione
    {
        [Key]
        public int ID_Stato { get; set; }

        [Required(ErrorMessage = "Il campo Stato è obbligatorio.")]
        public string Stato { get; set; }

        [Required(ErrorMessage = "Il campo Luogo Pacco è obbligatorio.")]
        [Display(Name = "Luogo Pacco")]
        public string Luogo_Pacco { get; set; }

        [Display(Name = "Descrizione Eventuale")]
        public string Descrizione_Eventuale { get; set; }

        [Required(ErrorMessage = "Il campo Data Aggiornamento è obbligatorio.")]
        [Display(Name = "Data Aggiornamento")]
        public DateTime Data_Aggiornamento { get; set; }

        [Required(ErrorMessage = "Il campo Ora Aggiornamento è obbligatorio.")]
        [Display(Name = "Ora Aggiornamento")]
        public TimeSpan Ora_Aggiornamento { get; set; }

   
    }
}