using Foro2._0.Models.Helper;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Foro2._0.Models
{
    public abstract class Persona : IdentityUser<int>
    {
        //public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsg.CampoRequerido)]
        [StringLength(20, MinimumLength = 2, ErrorMessage = ErrorMsg.RangoCaracteres)]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = ErrorMsg.SoloLetrasNumeros)]
        public override string UserName
        {
            get => base.UserName;
            set => base.UserName = value;
        }

        [Required(ErrorMessage = ErrorMsg.CampoRequerido)]
        [StringLength(20, MinimumLength = 2, ErrorMessage = ErrorMsg.RangoCaracteres)]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = ErrorMsg.SoloLetras)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = ErrorMsg.CampoRequerido)]
        [StringLength(20, MinimumLength = 2, ErrorMessage = ErrorMsg.RangoCaracteres)]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = ErrorMsg.SoloLetras)]
        public string Apellido { get; set; }

        [Required(ErrorMessage = ErrorMsg.CampoRequerido)]
        [Phone(ErrorMessage = ErrorMsg.FormatoTelefono)]
        [StringLength(20, MinimumLength = 8, ErrorMessage = ErrorMsg.RangoCaracteres)]
        public string Telefono { get; set; }

        public DateTime FechaAlta { get; set; } = DateTime.Now;

        [Required(ErrorMessage = ErrorMsg.CampoRequerido)]
        [EmailAddress(ErrorMessage = ErrorMsg.FormatoEmail)]
        public override string Email
        {
            get => base.Email;
            set => base.Email = value;
        }
    }

}
    

