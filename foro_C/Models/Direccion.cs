using System;

namespace foro_C.Models
{
    public class Direccion
    {
        public int Id{ get; set; }
        public String Calle { get;  private set; }
        public String Numero { get; private set; }
        //prop navegacional
      public Persona persona { get;  private set; }
        //prop relacional
     
      
    }
}
