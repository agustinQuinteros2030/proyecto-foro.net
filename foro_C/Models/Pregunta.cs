using foro_C.Models;

namespace foro_C.Models
{
    public class Pregunta
    {

        public DateTime Fecha { get; private set; }
        public String Texto { get; private set; }
        public Miembro Miembro { get; private set; }
        public Boolean Activa { get; private set; }
        public Entrada Entrada { get; private set; }
        public List<Respuesta> Respuestas { get; private set; }


    }
}

