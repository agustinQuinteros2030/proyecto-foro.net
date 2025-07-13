using Foro2._0.Models.Helper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foro2._0.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        [Required(ErrorMessage = ErrorMsg.CampoRequerido)]
        [StringLength(30, MinimumLength = 5, ErrorMessage = ErrorMsg.RangoCaracteres)]
        public string Nombre { get; set; }

        public List<Entrada> Entradas { get; set; } = new List<Entrada>();
    }
}
