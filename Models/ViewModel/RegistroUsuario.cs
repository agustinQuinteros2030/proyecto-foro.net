using Foro2._0.Models.Helper;
using System.ComponentModel.DataAnnotations;
using System;

namespace Foro2._0.Models.ViewModel
{
    public class RegistroUsuario
    {
        [Required(ErrorMessage = ErrorMsg.CampoRequerido)]
        [StringLength(20, MinimumLength = 2, ErrorMessage = ErrorMsg.RangoCaracteres)]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = ErrorMsg.SoloLetrasNumeros)]
        public string Username { get; set; }

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
       
        public string Email { get; set; }

        [Required(ErrorMessage = ErrorMsg.CampoRequerido)]
        [DataType (DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = ErrorMsg.CampoRequerido)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarPassword { get; set; }
    }
}
