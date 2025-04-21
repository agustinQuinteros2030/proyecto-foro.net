using System;
using System.ComponentModel.DataAnnotations;

namespace foro_C.Models
{
    public class Empleado :Persona 
    {
        [Required(ErrorMessage = "Campo obligatorio")]
        [StringLength(15, MinimumLength = 4, ErrorMessage = "El legajo debe tener entre 4 y 15 caracteres.")]
        public String Legajo { get; set; }
    }
}
