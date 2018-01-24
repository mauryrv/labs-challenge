using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Labs_challenge.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "E-mail required.")]
        [EmailAddress(ErrorMessage = "E-mail invalid format.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Department required.")]
        public string Department { get; set; }

    }
}