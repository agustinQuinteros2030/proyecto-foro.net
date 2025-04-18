using System;

namespace foro_C.Models
{
    public class Respuesta:Interaccion
    {
     
        public Persona Miembro { get;  private set; }
          public Pregunta PreguntaQueResponde {  get; private set; }
        public Reaccion Megusta { get; set; }
        
        public int MiembroId { get; set; }
    }
}
