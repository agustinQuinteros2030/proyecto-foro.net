using Foro2._0.Models.Helper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foro2._0.Models
{
    public class Pregunta : Interacion
    {
        [Required(ErrorMessage = ErrorMsg.CampoRequerido)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = ErrorMsg.RangoCaracteres)]
        public string Texto { get; set; }
        public bool Activa { get; set; } = true;

        public Entrada Entrada { get; set; }
        public int EntradaId { get; set; }

        public List<Respuesta> Respuestas { get; set; } = new List<Respuesta>();
    }
}
