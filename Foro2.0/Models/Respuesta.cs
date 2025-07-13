using Foro2._0.Models.Helper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foro2._0.Models
{
    public class Respuesta : Interacion
    {
        [Required(ErrorMessage = ErrorMsg.CampoRequerido)]
        [StringLength(500, MinimumLength = 2, ErrorMessage = ErrorMsg.RangoCaracteres)]
        public string Texto { get; set; }
        public Pregunta Pregunta { get; set; }
        public int PreguntaId { get; set; }

        public List<Reaccion> Reacciones { get; set; } = new List<Reaccion>();
    }
}
