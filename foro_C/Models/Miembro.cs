using System.Collections.Generic;

namespace foro_C.Models
{
    public class Miembro : Persona 
    {

        public List<Entrada> Entradas { get;   set; }
        public List<Pregunta> Preguntas { get;  set; } 
        public List<Respuesta> Respuestas { get;  set; }
        public List<Reaccion> Reacciones {  get;  set; }
        public List<Habilitacion> Habilitaciones { get; set; }
        

   
    }
}
