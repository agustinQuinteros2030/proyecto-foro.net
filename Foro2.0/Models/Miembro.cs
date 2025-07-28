using System.Collections.Generic;

namespace Foro2._0.Models
{
    public class Miembro:Persona
    {
        public List<Entrada> EntradasCreadas { get; set; } = new List<Entrada>();
        public List<Pregunta> PreguntasRealizadas { get; set; } = new List<Pregunta>();
        public List<Respuesta> RespuestasRealizadas { get; set; } = new List<Respuesta>();
        public List<Reaccion> ReaccionesRealizadas { get; set; } = new List<Reaccion>();
        public List<Habilitacion> AccesoEntradasPrivadas { get; set; } = new List<Habilitacion>();

        public string ImagenPerfilRuta { get; set; }


    }
}
