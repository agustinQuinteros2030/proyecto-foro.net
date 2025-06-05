using foro_C.HelpersDataAnotattions;
using System.ComponentModel.DataAnnotations;

namespace foro_C.ViewsModels

{
    public class IniciarSesion
    {
        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        public bool Recordarme { get; set; }
    }
}