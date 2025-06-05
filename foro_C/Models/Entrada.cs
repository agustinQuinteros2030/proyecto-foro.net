using foro_C.HelpersDataAnotattions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace foro_C.Models
{
    public class Entrada
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [StringLength(100, MinimumLength = 10, ErrorMessage = ErrorMsgs.longitudValida)]
        public String Titulo { get; set; }
        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = ErrorMsgs.longitudValida)]
        public String Texto { get; set; }

        [Required]
        public Boolean Privada { get; set; } = true;

        // Propiedad relacional
        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        public int CategoriaId { get; set; }
        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        public int MiembroId { get; set; }

        // Propiedad navegacional
        [Required]
        public Miembro Miembro { get; set; }
        public Categoria Categoria { get; set; }

        public List<Pregunta> Preguntas { get; set; } = new();
        public List<Habilitacion> Habilitaciones { get; set; } = new();



    }
}


