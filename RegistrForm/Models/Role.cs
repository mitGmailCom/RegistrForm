using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RegistrForm.Models
{
    public class Role
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }


        //public Role()
        //{
        //    new Role { Id = 1, Name = "user" };
        //    new Role { Id = 2, Name = "admin" };
        //}
    }
}