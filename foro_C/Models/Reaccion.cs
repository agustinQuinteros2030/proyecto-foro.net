namespace foro_C.Models
{
    public class Reaccion
    {
        public DateTime Fecha { get; private set; }
        public String Texto { get; private set; }
        public Miembro Miembro { get; private set; }
        public Bool MeGusta { get; private set; } 
        public Respuesta Respuesta { get; private set; }
    }
}
