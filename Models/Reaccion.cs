using Foro2._0.Models.Helper;
using System.ComponentModel.DataAnnotations;

namespace Foro2._0.Models
{
    public class Reaccion : Interacion
    {
        [Required(ErrorMessage = ErrorMsg.CampoRequerido)]
        public TipoReaccion Tipo { get; set; }

        public int RespuestaId { get; set; }
        public Respuesta Respuesta { get; set; }

    }
}
