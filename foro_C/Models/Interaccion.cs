using foro_C.Models;
using System;

public class Interaccion
{
      public int Id { get; set; }
      public DateTime Fecha { get; set; }
      public String Texto { get; set; }

    // Propiedad relacional
    public int MiembroId { get; set; }

    //Propiedad navegacional
    public Miembro Miembro { get; set; }
}