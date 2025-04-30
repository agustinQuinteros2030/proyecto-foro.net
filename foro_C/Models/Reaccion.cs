using System;

namespace foro_C.Models
{
    public class Reaccion : Interaccion
    {
<<<<<<< HEAD
        public TipoReaccion Tipo { get; set; }
=======
       public Boolean MeGusta { get;  set; }
>>>>>>> agustinQuinteros

        // Propiedad relacional
        public int RespuestaId { get; set; }

        //Propiedad navegacional
<<<<<<< HEAD
        public Respuesta Respuesta { get; set; }

=======
        public Respuesta Respuesta { get;  set; }
    
>>>>>>> agustinQuinteros
    }
}
