using Foro2._0.Models.Helper;
using System.ComponentModel.DataAnnotations;

namespace Foro2._0.Models.ViewModel
{
    public class InicioSesion
    {
        [Required]
        [Display(Name = "Email o Usuario")]
        public string EmailOUsername { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Recordarme")]
        public bool Recordarme { get; set; }

    }
}
