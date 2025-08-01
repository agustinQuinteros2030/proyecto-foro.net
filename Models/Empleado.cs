using Foro2._0.Models.Helper;
using System.ComponentModel.DataAnnotations;

namespace Foro2._0.Models
{
    public class Empleado : Persona
    {
        [Required(ErrorMessage = ErrorMsg.CampoRequerido)]
        [StringLength(20, MinimumLength = 2, ErrorMessage = ErrorMsg.RangoCaracteres)]
        public string Legajo { get; set; }
    }
}
