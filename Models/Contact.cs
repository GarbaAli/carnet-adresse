using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace carnet_adresse.Models
{
    public class Contact
    {
        [Key]
        public int contactId { get; set; }
        [Display(Name = "First Name")]
        [Required(ErrorMessage ="The first Name is required")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "The Last Name is required")]
        public string LastName { get; set; }
        [EmailAddress]
        //[RegularExpression("/^([w-.]+)@((?:[w]+.)+)([a-zA-Z]{2,4})/i")]
        public string email { get; set; }
        [Phone]
        [Required]
        public string phone { get; set; }
        public string Bio { get; set; }
        public string avatar { get; set; }
        public ICollection<Adresses> Adresses { get; set; }
    }
}
