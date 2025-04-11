namespace foro_C.Models
{
    public class Respuesta
    {
        public DateTime Fecha {  get; private set; }
        public String Texto { get;  private set; }
        public Persona Miembro { get;  private set; }
          public Pregunta PreguntaQueResponde {  get; private set; }
        public Reaccion Megusta { get; set; }
    }
}
