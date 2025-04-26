using foro_C.HelpersDataAnotattions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace foro_C.Models
{
    public  abstract class Persona
    {
        public int Id { get; private set; }

        [Required (ErrorMessage =ErrorMsgs.Requerido)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = ErrorMsgs.CampoNumeros)]
        public String UserName { get;  private set; }

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = ErrorMsgs.CampoNumeros)]
        public String Nombre { get; private set; }

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = ErrorMsgs.CampoNumeros)]
        public String Apellido { get; private set; }

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        public DateTime FechaAlta { get; private set; }

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [EmailAddress(ErrorMessage = ErrorMsgs.ErrorEmail)]
        [StringLength(50, MinimumLength = 2, ErrorMessage = ErrorMsgs.CampoNumeros)]

        public String Email { get; private set; }
        //prop navegacional
        public Direccion direccion { get; private set; }
        //prop relacional 
        public int DireccionID { get; private set; }
       
        public List<int> telefonos { get; private set; } = new List<int>();
    }
}
