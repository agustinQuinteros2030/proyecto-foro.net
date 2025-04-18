using foro_C.Models;
using System;
using System.Collections.Generic;

namespace foro_C.Models
{
    public class Pregunta : Interaccion
    {

        public Boolean Activa { get; private set; }

        // Propiedad relacional
        public int EntradaId { get; set; }

        // Propiedad navegacional
        public Entrada Entrada { get; private set; }
        public List<Respuesta> Respuestas { get; private set; }
        
        //prop relacional
        public int RespuestaId {  get; private set; }

    }
}

