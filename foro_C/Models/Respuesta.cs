using System;
using System.Collections.Generic;

namespace foro_C.Models
{
    public class Respuesta:Interaccion
    {
     
        public Persona Persona { get;   set; }
        //prop navegacional
          public Pregunta Pregunta {  get;  set; }
        //prop navegacional
        public List<Reaccion> Reacciones { get; set; }
        
        public int PersonaId { get; set; }
        public int PreguntaId { get;set; }

    }
}
