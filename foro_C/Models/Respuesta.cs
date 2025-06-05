using System.Collections.Generic;

namespace foro_C.Models
{
    public class Respuesta : Interaccion
    {


        //prop navegacional
        public Pregunta Pregunta { get; set; }
        //prop navegacional
        public List<Reaccion> Reacciones { get; set; } = new();


        public int PreguntaId { get; set; }

    }
}
