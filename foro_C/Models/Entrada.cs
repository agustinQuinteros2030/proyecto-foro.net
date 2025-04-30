using foro_C.HelpersDataAnotattions;
using System;
using System.Collections.Generic;

namespace foro_C.Models
{
    public class Entrada
    {
        //titulo
        public int Id { get; set; }
        public DateTime Fecha { get;  set; }

        public String Titulo { get;  set; }
        public String Texto { get;  set; }
        public Boolean Privada { get;  set; }

        // Propiedad relacional
        public int CategoriaId { get; set; }
        public int MiembroId { get; set; }

        // Propiedad navegacional
        public Miembro Miembro { get;  set; }
        public Categoria Categoria { get;  set; }

        public List<Pregunta> Preguntas { get;  set; }
        public List<Habilitacion> MiembrosHabilitados { get; set; }

       

    }
}


