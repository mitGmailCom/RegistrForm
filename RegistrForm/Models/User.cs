using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;



namespace RegistrForm.Models
{
    public class User
    {
        public int Id { get; set; }
        //[Required(ErrorMessage = "Пожалуйста, введите свое имя")]
        [Required]
        public string FirstName { get; set; }
        [Required]
        //[Required(ErrorMessage = "Введите свое имя")]
        public string LastName { get; set; }
        [Required]
        //[StringLength(10, MinimumLength = 3)]
        public string Login { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 3)]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }

        public Role Role { get; set; }
    }
}