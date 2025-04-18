using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace foro_C.Models
{
    public class Entrada
    {
        public int Id { get; set; }
        public DateTime Fecha { get;set; }

        [Required(ErrorMessage = "El {0} es obligatorio.")]
        [StringLength(100, ErrorMessage = "El {0} no puede exceder los {1} caracteres.")]
        public String Titulo { get; private set; }


        [Required(ErrorMessage = "El {0} es obligatorio.")]
        [StringLength(1000, ErrorMessage = "El {0} no puede exceder los {1} caracteres.")]
        public String Texto { get; private set; }

        [Required]
        public Boolean Privada { get; private set; }

        // Propiedad relacional
        public int CategoriaId { get; set; }
        public int MiembroId { get; set; }

        // Propiedad navegacional
        public Miembro Miembro { get; private set; }
        public Categoria Categoria { get; private set; }

        public List<Pregunta> Preguntas { get; private set; }
        public List<Habilitacion> MiembrosHabilitados { get; private set; }

       

    }
}


