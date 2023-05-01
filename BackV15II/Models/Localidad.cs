using System.ComponentModel.DataAnnotations;

namespace BackV15II.Models
{
    public class Localidad
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int CodPos1 { get; set; }
        public int CodPos2 { get; set; }        
    }
}
