using foro_C.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace foro_C.Models
{
    public class Empleado : Persona
    {
        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [StringLength(15, MinimumLength = 4, ErrorMessage = ErrorMsgs.longitudValida)]
        //  [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = ErrorMsgs.FormatoValidoLetras)]
        public String Legajo { get; set; }
    }
}
