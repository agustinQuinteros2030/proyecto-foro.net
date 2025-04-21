using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace foro_C.Models
{
    public  abstract class Persona
    {
        public int Id { get; private set; }

        [Required (ErrorMessage ="campo obligatorio")]
        [StringLength(15,MinimumLength =4)]
        public String UserName { get;  private set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres.")]
        public String Nombre { get; private set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 50 caracteres.")]
        public String Apellido { get; private set; }

        [Required(ErrorMessage = "La fecha de alta es obligatoria.")]
        public DateTime FechaAlta { get; private set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        [StringLength(50, ErrorMessage = "El correo electrónico no debe exceder los 50 caracteres.")]
        public String Email { get; private set; }
        //prop navegacional
        [Required(ErrorMessage = "La dirección es obligatoria.")]
        public Direccion direccion { get; private set; }
        //prop relacional 
        public int DireccionID { get; private set; }
       
        public List<int> telefonos { get; private set; } = new List<int>();
    }
}
