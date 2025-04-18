public class Interaccion
{
      public int Id { get; set; }
      public DateTime Fecha { get; set; }

     [Required(ErrorMessage = "El {0} es obligatorio.")]
     [Range(1,1000 ErrorMessage = "El texto debe tener entre {1} y (2) caracteres obligatorios..")]
     public String Texto { get; set; }

    // Propiedad relacional
    public int MiembroId { get; set; }

    //Propiedad navegacional
    public Miembro Miembro { get; set; }
}