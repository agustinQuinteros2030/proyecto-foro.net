using foro_C.HelpersDataAnotattions;
using System;
using System.ComponentModel.DataAnnotations;

namespace foro_C.Models
{
    public class Empleado :Persona 
    {
        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [StringLength(15, MinimumLength = 4, ErrorMessage =ErrorMsgs.longitudValida)]
        public String Legajo { get; set; }
    }
}
