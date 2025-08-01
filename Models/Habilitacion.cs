using System;

namespace Foro2._0.Models
{
    public class Habilitacion
    {
        public int Id { get; set; }
        public int EntradaId { get; set; }
        public Entrada Entrada { get; set; }

        public int MiembroId { get; set; }
        public Miembro Miembro { get; set; }

        public bool Habilitado { get; set; } = false;  
        public DateTime FechaSolicitud { get; set; }=DateTime.Now;
    }
}
