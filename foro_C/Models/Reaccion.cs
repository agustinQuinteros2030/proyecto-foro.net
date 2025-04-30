using System;

namespace foro_C.Models
{
    public class Reaccion : Interaccion
    {
       public Boolean MeGusta { get;  set; }

        // Propiedad relacional
        public int RespuestaId { get; set; }

        //Propiedad navegacional
        public Respuesta Respuesta { get;  set; }
    
    }
}
