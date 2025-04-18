using System.Collections.Generic;

namespace foro_C.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; private set; }
        public List<Entrada> Entradas { get; private set; }
    }
}

