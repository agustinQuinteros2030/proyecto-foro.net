using System;
using System.Collections.Generic;

namespace foro_C.Models
{
    public class Entrada
    {
        public int Id { get; set; }
        public DateTime Fecha { get; private set; }
        public String Titulo { get; private set; }
        public String Texto { get; private set; }
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


