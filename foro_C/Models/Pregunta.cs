using System;
using System.Collections.Generic;

namespace foro_C.Models
{
    public class Pregunta : Interaccion
    {

        public Boolean Activa { get; set; }

        // Propiedad relacional
      
        public List<Respuesta> Respuestas { get; set; } = new();

        //prop relacional


    }
}

