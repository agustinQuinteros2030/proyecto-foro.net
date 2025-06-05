using foro_C.HelpersDataAnotattions;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace foro_C.Models
{
    public abstract class Persona : IdentityUser<int>
    {
        //  public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        // [StringLength(50, MinimumLength = 2, ErrorMessage = ErrorMsgs.longitudValida)]

        public override String UserName
        {
            get { return base.UserName; }
            set { base.UserName = value; }
        }

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [StringLength(30, MinimumLength = 2, ErrorMessage = ErrorMsgs.longitudValida)]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = ErrorMsgs.FormatoValidoLetras)]
        public String Nombre { get; set; }

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [StringLength(30, MinimumLength = 2, ErrorMessage = ErrorMsgs.longitudValida)]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = ErrorMsgs.FormatoValidoLetras)]
        public String Apellido { get; set; }

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        public DateTime FechaAlta { get; set; } = DateTime.Now;

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [EmailAddress(ErrorMessage = ErrorMsgs.ErrorEmail)]
        public override String Email
        {
            get { return base.Email; }
            set { base.Email = value; }
        }

        //prop relacional 

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [RegularExpression(@"^\d{8,15}$", ErrorMessage = ErrorMsgs.FormatoValidoNumero)]
        public int Telefono { get; set; }
    }
}
