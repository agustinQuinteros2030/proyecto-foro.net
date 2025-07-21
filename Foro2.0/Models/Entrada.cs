using Foro2._0.Models.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foro2._0.Models
{
    public class Entrada
    {
        public int Id { get; set; }
        [Required(ErrorMessage = ErrorMsg.CampoRequerido)]
        [StringLength(100, MinimumLength = 5, ErrorMessage = ErrorMsg.RangoCaracteres)]
        public string Titulo { get; set; }

        [Required(ErrorMessage = ErrorMsg.CampoRequerido)]
        [StringLength(5000, MinimumLength = 10, ErrorMessage = ErrorMsg.RangoCaracteres)]
        public string Texto { get; set; }
        public bool Privada { get; set; } = false;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public Miembro Miembro { get; set; }
        public int MiembroId { get; set; }
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
        public List<Pregunta> Preguntas { get; set; } = new List<Pregunta>();

        public List<Habilitacion> MiembrosHabilitados { get; set; } = new List<Habilitacion>();

    }
}
