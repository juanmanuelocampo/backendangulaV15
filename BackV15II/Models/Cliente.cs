using System.ComponentModel.DataAnnotations;

namespace BackV15II.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Domicilio { get; set; }
        public string Email { get; set; }
        public string Observaciones { get; set; }
        public string Telefono { get; set; }

        [StringLength(2)]
        public string Cuit1 { get; set; }
        
        [StringLength(10)]
        public string Cuit2 { get; set; }
        
        [StringLength(1)]
        public string Cuit3 { get; set; }

        public int SituacionIva { get; set; }
        public int Localidad { get; set; }
    }
}
