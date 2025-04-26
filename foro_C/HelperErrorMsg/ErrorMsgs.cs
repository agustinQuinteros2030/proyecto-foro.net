using System.ComponentModel;
using System.Reflection.Metadata;

namespace foro_C.HelpersDataAnotattions
{
    public static class ErrorMsgs
    {
        public const string Requerido = "El campo {0} es obligatorio";
        public const string longitudValida = "El campo {0} debe tener entre {2} y {1} caracteres.";
        public const string ErrorEmail = "el campo {0} es invalido";
        public const string FormatoValidoLetras = "El campo {0} solo puede contener letras.";
        public const string FormatoValidoTelefono= "El campo {0} solo puede contener numeros.";
    }
}
