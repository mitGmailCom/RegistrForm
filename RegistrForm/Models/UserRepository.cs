using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegistrForm.Models
{
    public class UserRepository: IRepository<User>
    {
        private static List<User> ListUsers = new List<User>();

        private static UserRepository _instance;

        public static UserRepository Instance => _instance ?? (_instance = new UserRepository());

        private UserRepository() { }

        public List<User> Users
        {
            get { return ListUsers; }
            set { ListUsers = value; }
        }
            

        public void Create(User item, Role role)
        {
            if (ListUsers.Count > 0)
            {
                for (int i = 0; i < ListUsers.Count; i++)
                {
                    if (ListUsers[i].Login != null)
                    {
                        if (ListUsers[i].Login == item.Login)
                            throw new ArgumentException("Login exists");
                    }
                }
                item.Id = (ListUsers.LastOrDefault()?.Id ?? 0) + 1;
                item.Role = role;
                ListUsers.Add(item);
            }
            if (ListUsers.Count == 0)
            {
                item.Id = 1;
                ListUsers.Add(item);
            }
        }

        public bool Delete(int id)
        {
            for (int i = 0; i < ListUsers.Count; i++)
            {
                if (ListUsers[i].Id == id)
                {
                    ListUsers.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public void Edit(int id, User user)
        {
            User existingUser = Get(id);
            if (existingUser != null)
            {
                for (int i = 0; i < ListUsers.Count; i++)
                {
                    if (ListUsers[i].Id == id)
                        ListUsers[i] = user;
                }

            }
        }

        public User Get(int id)
        {
            for (int i = 0; i < ListUsers.Count; i++)
            {
                if (ListUsers[i].Id == id)
                    return ListUsers[i];
            }
            return null;
        }

        public IEnumerable<User> GetAll() => ListUsers;

    }
}