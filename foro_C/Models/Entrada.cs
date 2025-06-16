using foro_C.Helpers;

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
        public string Titulo { get; set; }

        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = ErrorMsgs.longitudValida)]
        public string Texto { get; set; }

        [Required]
        public bool Privada { get; set; } = true;

        public bool Activa { get; set; } = true;

        // Relaciones
        [Required]
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        [Required]
        public int MiembroId { get; set; }
        public Miembro Miembro { get; set; }

        public List<Pregunta> Preguntas { get; set; } = new();
        public List<Habilitacion> Habilitaciones { get; set; } = new();
    }



}



