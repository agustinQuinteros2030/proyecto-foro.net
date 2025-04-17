using System.Collections.Generic;

namespace foro_C.Models
{
    public class Categoria
    {
        public string _Nombre { get; private set; }
        
        public List<Entrada> Entradas { get; private set; }
    }
}


// -Nombre
// -Entradas