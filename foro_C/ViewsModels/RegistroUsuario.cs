using foro_C.HelpersDataAnotattions;
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

        // podemos agregar mas propiedades si es necesario a parte del modelo anemico que tenemos

    }
}