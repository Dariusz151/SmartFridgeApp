using SmartFridgeApp.Domain.Exceptions;
using SmartFridgeApp.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartFridgeApp.Domain.Models
{
    [Table("User")]
    public class User
    {
        [Key, Required]
        public int Id { get; set; }

        [Required, MaxLength(128)]
        public string UserName { get; set; }

        [Required, MaxLength(1024)]
        public string PasswordHash { get; set; }

        [Required, MaxLength(128)]
        public string Email { get; set; }

        [MaxLength(32)]
        public string FirstName { get; set; }

        [MaxLength(1)]
        public string MiddleInitial { get; set; }

        [MaxLength(32)]
        public string LastName { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }



        //private List<FoodItem> _foodItems = new List<FoodItem>();

        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public Guid Id { get; private set; }

        //public ReadOnlyCollection<FoodItem> FoodItems { get { return _foodItems.AsReadOnly(); } }
        //public string Login { get; private set; }
        //public UserRole Role { get; private set; }
        //public DateTime CreatedAt { get; private set; }

        //private User()
        //{

        //}

        //public User (string login)
        //    :this (login, UserRole.User)
        //{

        //}

        //public User(string login, UserRole role)
        //{
        //    Login = login;
        //    Role = role;
        //    CreatedAt = DateTime.UtcNow;
        //}

        //public void AddFoodItem(FoodItem item)
        //{
        //    _foodItems.Add(item);
        //}

        //public void DeleteFoodItem(FoodItem item)
        //{
        //    if (_foodItems.Count == 0)
        //        throw new FoodItemException("List of food items is empty.");
        //    if (!_foodItems.Contains(item))
        //        throw new FoodItemException("This food item doesnt belong to this user");
        //    _foodItems.Remove(item);
        //}
    }
}
