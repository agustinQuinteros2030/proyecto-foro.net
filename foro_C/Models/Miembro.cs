using System.Collections.Generic;

namespace foro_C.Models
{
    public class Miembro : Persona 
    {

        public List<Entrada> Entradas { get; set; } = new();
        public List<Pregunta> Preguntas { get; set; } = new();
        public List<Respuesta> Respuestas { get; set; } = new();
        public List<Reaccion> Reacciones { get; set; } = new(); 
        public List<Habilitacion> Habilitaciones { get; set; } = new(); 



    }
}
