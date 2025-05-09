using foro_C.HelpersDataAnotattions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace foro_C.Models
{
    public abstract class Persona
    {
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
       // [StringLength(50, MinimumLength = 2, ErrorMessage = ErrorMsgs.longitudValida)]
        public String UserName { get; set; }

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [StringLength(30, MinimumLength = 2, ErrorMessage = ErrorMsgs.longitudValida)]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = ErrorMsgs.FormatoValidoLetras)]
        public String Nombre { get;  set; }

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [StringLength(30, MinimumLength = 2, ErrorMessage = ErrorMsgs.longitudValida)]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = ErrorMsgs.FormatoValidoLetras)]
        public String Apellido { get;  set; }

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        public DateTime FechaAlta { get; set; }= DateTime.Now;

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [EmailAddress(ErrorMessage = ErrorMsgs.ErrorEmail)]
        public String Email { get;  set; }
        //prop relacional 
        public int DireccionID { get;  set; }
        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [RegularExpression(@"^\d{8,15}$", ErrorMessage = ErrorMsgs.FormatoValidoNumero)]
        public int Telefono { get; set; }
    }
}
