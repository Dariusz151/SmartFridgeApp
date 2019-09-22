using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartFridgeApp.Domain.Models
{
    public class UserGroup
    {
        private List<User> _users = new List<User>();

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; private set; }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public ReadOnlyCollection<User> Users { get { return _users.AsReadOnly(); } }

        private UserGroup()
        {

        }

        public void AddUserToGroup(User user)
        {
            _users.Add(user);
        }

        public UserGroup(string name)
            : this(name, "Default group")
        {

        }

        public UserGroup(string name, string desc)
        {
            Name = name;
            Description = desc;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
