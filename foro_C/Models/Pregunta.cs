using foro_C.Models;
using System;
using System.Collections.Generic;

namespace foro_C.Models
{
    public class Pregunta : Interaccion
    {
<<<<<<< HEAD
        public Boolean Activa { get; private set; }
=======

        public Boolean Activa { get;  set; }
>>>>>>> agustinQuinteros

        // Propiedad relacional
        public int EntradaId { get; set; }

        // Propiedad navegacional
        public Entrada Entrada { get;  set; }
        public List<Respuesta> Respuestas { get;  set; }
        
        //prop relacional
        public int RespuestaId {  get; set; }

    }
}

