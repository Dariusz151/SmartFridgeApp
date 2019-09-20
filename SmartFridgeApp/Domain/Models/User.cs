using SmartFridgeApp.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartFridgeApp.Domain.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; private set; }

        public List<FoodItem> FoodItems { get; set; }
        public string Login { get; private set; }
        public UserRole Role { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private User()
        {

        }

        public User (string login)
            :this (login, UserRole.User)
        {

        }

        public User(string login, UserRole role)
        {
            Login = login;
            Role = role;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
