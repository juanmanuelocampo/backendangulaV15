using System.ComponentModel.DataAnnotations;

namespace BackV15II.Models
{
    public class Solicitud
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Planid { get; set; }
        public string Prueba { get; set; }        
    }
}
