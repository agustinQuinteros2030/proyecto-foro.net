using foro_C.Models;

namespace foro_C.Models
{
    public class Pregunta : Interaccion
    {

        public Boolean Activa { get; private set; }

        // Propiedad relacional
        public int EntradaId { get; set; }

        // Propiedad navegacional
        public Entrada Entrada { get; private set; }
        public List<Respuesta> Respuestas { get; private set; }

    }
}

