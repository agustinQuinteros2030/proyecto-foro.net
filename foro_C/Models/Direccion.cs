using foro_C.HelpersDataAnotattions;
using System;
using System.ComponentModel.DataAnnotations;

namespace foro_C.Models
{
    public class Direccion
    {
        public int Id{ get; set; }
        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [StringLength(30, MinimumLength = 2, ErrorMessage = ErrorMsgs.longitudValida)]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = ErrorMsgs.FormatoValidoLetras)]
        public String Calle { get;   set; }
        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [RegularExpression(@"^[a-zA-Z0-9\s/°º.-]{1,10}$", ErrorMessage = ErrorMsgs.FormatoValidoNumero)]
        public String Numero { get;  set; }
        //prop navegacional
      public Persona Persona { get;  set; }
        //prop relacional
     
      public int PersonaId { get; set; }
    }
}
