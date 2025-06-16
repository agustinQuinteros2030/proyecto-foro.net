using foro_C.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace foro_C.Models
{
    public class Categoria
    {
        private static readonly List<Entrada> entradas = new();

        public int Id { get; set; }
        [Required(ErrorMessage = ErrorMsgs.Requerido)]
        [StringLength(15, MinimumLength = 2, ErrorMessage = ErrorMsgs.longitudValida)]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = ErrorMsgs.FormatoValidoLetras)]
        public string Nombre { get; set; }
        public List<Entrada> Entradas { get; set; } = entradas;
    }
}

