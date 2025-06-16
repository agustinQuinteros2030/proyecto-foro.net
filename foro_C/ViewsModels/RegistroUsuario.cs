using foro_C.Helpers;
using System.ComponentModel.DataAnnotations;

namespace foro_C.ViewsModels
{
    public class RegistroUsuario
    {
        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [EmailAddress(ErrorMessage = ErrorMsgs.ErrorEmail)]
        public string Email { get; set; }
        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }
        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("Password", ErrorMessage = ErrorMsgs.ErrorConfirmarContraseña)]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = ErrorMsgs.Requerido)]


        public string Nombre { get; set; }
        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        public string Apellido { get; set; }
        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        public string UserName { get; set; }
        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        public int Telefono { get; set; }

        // podemos agregar mas propiedades si es necesario a parte del modelo anemico que tenemos

    }
}