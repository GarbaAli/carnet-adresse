using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace carnet_adresse.Models
{
    public class Adresses
    {
        [Key]
        public int adresseId { get; set; }
        [Required]
        public string libelleAdresse { get; set; }
        //public int ContactIdAdresse { get; set; }
    }
}
