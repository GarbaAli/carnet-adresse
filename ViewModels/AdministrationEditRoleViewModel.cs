using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace carnet_adresse.ViewModels
{
    public class AdministrationEditRoleViewModel
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "Enter The New Role")]
        [MinLength(3)]
        public string RoleName { get; set; }

        public List<string> Users { get; set; }
    }
}
