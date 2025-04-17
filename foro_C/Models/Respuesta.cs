using System;

namespace foro_C.Models
{
    public class Respuesta
    {
        public int RespuestaID { get; set; }
        public DateTime Fecha {  get; private set; }
        public String Texto { get;  private set; }
        public Persona Miembro { get;  private set; }
          public Pregunta PreguntaQueResponde {  get; private set; }
        public Reaccion Megusta { get; set; }
        
        public int MiembroId { get; set; }
    }
}
