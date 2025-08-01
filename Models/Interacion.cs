using System;

namespace Foro2._0.Models
{
    public class Interacion
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public Miembro Miembro { get; set; }
        public int MiembroId { get; set; }

    }
}
