namespace foro_C.Models
{
    public class Entrada
    {
        public DateTime Fecha { get; private set; }
        public String Titulo { get; private set; }
        public String Texto { get; private set; }
        public Bool Privada { get; private set; }
        public Categoria Categoria { get; private set; }
        public Miembro Miembro { get; private set; }  
        public Pregunta Preguntas { get; private set; }
        public List<Habiliaciones> MiembrosHabilitados { get; private set; }

       

    }
}


