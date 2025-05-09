using foro_C.HelpersDataAnotattions;
using foro_C.Models;
using System;
using System.ComponentModel.DataAnnotations;

public class Interaccion
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }= DateTime.Now;

    [Required(ErrorMessage = ErrorMsgs.Requerido)]
    [StringLength(1000, MinimumLength = 1, ErrorMessage = ErrorMsgs.longitudValida)]
    public String Texto { get; set; }

    // Propiedad relacional
    public int MiembroId { get; set; }

    //Propiedad navegacional
    public Miembro Miembro { get; set; }
}