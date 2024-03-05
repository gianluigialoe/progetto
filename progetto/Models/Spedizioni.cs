using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace progetto.Models
{
    public class Spedizioni
    {
        [Key]
        public int ID_Spedizione { get; set; }

        [Required(ErrorMessage = "Il campo Numero Spedizione è obbligatorio.")]
        [Display(Name = "Numero Spedizione")]
        public string Numero_Spedizione { get; set; }

        [Required(ErrorMessage = "Il campo Data Spedizione è obbligatorio.")]
        [Display(Name = "Data Spedizione")]
        public DateTime Data_Spedizione { get; set; }

        [Required(ErrorMessage = "Il campo Peso è obbligatorio.")]
        public decimal Peso { get; set; }

        [Required(ErrorMessage = "Il campo Città Destinataria è obbligatorio.")]
        [Display(Name = "Città Destinataria")]
        public string Citta_Destinataria { get; set; }

        [Required(ErrorMessage = "Il campo Indirizzo Destinatario è obbligatorio.")]
        [Display(Name = "Indirizzo Destinatario")]
        public string Indirizzo_Destinatario { get; set; }

        [Required(ErrorMessage = "Il campo Nominativo Destinatario è obbligatorio.")]
        [Display(Name = "Nominativo Destinatario")]
        public string Nominativo_Destinatario { get; set; }

        [Required(ErrorMessage = "Il campo Costo Spedizione è obbligatorio.")]
        [Display(Name = "Costo Spedizione")]
        public decimal Costo_Spedizione { get; set; }

        [Required(ErrorMessage = "Il campo Data Consegna Prevista è obbligatorio.")]
        [Display(Name = "Data Consegna Prevista")]
        public DateTime Data_Consegna_Prevista { get; set; }

        // Relazione con il modello "Cliente"
        [ForeignKey("Cliente")]
        public int ID_Cliente { get; set; }
        public virtual Cliente Cliente { get; set; }

        // Relazione con il modello "StatoSpedizione"
        public virtual StatoSpedizione StatoSpedizione { get; set; }
    }
}