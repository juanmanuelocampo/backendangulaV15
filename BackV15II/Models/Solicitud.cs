using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BackV15II.Models
{
    public class Solicitud
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Planid { get; set; }
        public string Prueba { get; set; }
        public decimal Numerodecimal { get; set; }        
        public DateTime Fecha { get; set; }
    }

   


}
