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

        [StringLength(200, ErrorMessage = ErrorMsgs.longitudValida)]
        public string Resumen
        {
            get
            {
                if (string.IsNullOrEmpty(Texto)) return string.Empty;
                return Texto.Length > 200 ? Texto.Substring(0, 200) + "..." : Texto;
            }
        }

        public string Imagen { get; set; }

        [Required]
        public bool Privada { get; set; } = true;

        public bool Activa { get; set; } = true;

        public bool Destacada { get; set; } = false;

        public EstadoEntrada Estado { get; set; } = EstadoEntrada.Borrador;

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



