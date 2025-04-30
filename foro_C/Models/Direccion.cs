using System;

namespace foro_C.Models
{
    public class Direccion
    {
        public int Id{ get; set; }

        public String Calle { get;   set; }
        public String Numero { get;  set; }
        //prop navegacional
      public Persona Persona { get;  set; }
        //prop relacional
     
      public int PersonaId { get; set; }
    }
}
