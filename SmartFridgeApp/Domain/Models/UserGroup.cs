using SmartFridgeApp.Domain.Exceptions;
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

        public void AddUserToGroup(User user)
        {
            if (_users.Contains(user))
                throw new DuplicateUserInGroupException("User group already contains this user.");
            _users.Add(user);
        }

        public void DeleteUserFromGroup(User user)
        {
            if (_users.Count == 0)
                throw new UserGroupException("This group is empty.");
            if (!_users.Contains(user))
                throw new UserGroupException("This user doesn't belong to this group.");

            _users.Remove(user);
        }
    }
}
