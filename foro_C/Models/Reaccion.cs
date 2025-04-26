using System;

namespace foro_C.Models
{
    public class Reaccion : Interaccion
    {
        public TipoReaccion Tipo { get; set; }

        // Propiedad relacional
        public int RespuestaId { get; set; }

        //Propiedad navegacional
        public Respuesta Respuesta { get; set; }

    }
}
