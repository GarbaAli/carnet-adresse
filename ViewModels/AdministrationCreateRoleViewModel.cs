using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace carnet_adresse.ViewModels
{
    public class AdministrationCreateRoleViewModel
    {
        [Required]
        [Display(Name ="Enter The Name Role")]
        [MinLength(3)]
        public string RoleName { get; set; }
    }
}
