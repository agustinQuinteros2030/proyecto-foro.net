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
        [StringLength(50, MinimumLength = 2, ErrorMessage = ErrorMsgs.longitudValida)]
        public String UserName { get; set; }

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = ErrorMsgs.longitudValida)]
        public String Nombre { get;  set; }

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = ErrorMsgs.longitudValida)]
        public String Apellido { get;  set; }

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        public DateTime FechaAlta { get; set; }

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [EmailAddress(ErrorMessage = ErrorMsgs.ErrorEmail)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = ErrorMsgs.longitudValida)]
        public String Email { get;  set; }
        //prop navegacional
        public Direccion Direccion { get;  set; }
        //prop relacional 
        public int DireccionID { get;  set; }
       
        public int Telefono { get; set; }
    }
}
