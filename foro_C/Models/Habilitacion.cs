using System;

namespace foro_C.Models
{
    public class Habilitacion
    {
        //prop navegacional
        public Entrada Entrada { get; set; }
        //prop navegacional
        public Miembro Miembro { get; set; }
        public Boolean IsHabilitado { get; set; } = true;
        //prop relacional
        public int MiembroId { get; set; }
        //prop relacional
        public int EntradaId { get; set; }


    }
}
