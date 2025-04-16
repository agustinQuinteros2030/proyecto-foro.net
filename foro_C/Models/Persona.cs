namespace foro_C.Models
{
    public  abstract class Persona
    {
        public int Id { get; private set; }
        public String UserName { get;  private set; }
        public String Nombre { get; private set; }
        public String Apellido {  get; private set; }
        public List<int> telefonos { get; private set; }
        public Direccion direccion { get; private set; }
        public DateTime FechaAlta { get; private set; }
        public String Email { get; private set; }
        ///dfs
    }
}
