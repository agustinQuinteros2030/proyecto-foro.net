using System.Collections.Generic;

namespace foro_C.Models
{
    public class Miembro : Persona 
    {
        public List<Entrada> EntradasCreadas { get;  private set; }
        public List<Pregunta> PreguntasRealizadas { get;  private set; } 
        public List<Respuesta> RespuestasRealizadas { get; private set; }
        public List<Reaccion> ReaccionesRealizadas {  get; private set; }
        public List<Habitacion> HabitacionesPrivadas { get; private set; }

   
    }
}
