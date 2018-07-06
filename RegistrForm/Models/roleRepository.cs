using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RegistrForm.Models
{
    public class RoleRepository : IRepositoryRole<Role>
    {
        private static RoleRepository _instance;

        public static RoleRepository Instance => _instance ?? (_instance = new RoleRepository());


        public void Create(Role role, List<Role> roles)
        {
            if (roles.Count > 0)
            {
                for (int i = 0; i < roles.Count; i++)
                {
                    if (roles[i].Name != null)
                    {
                        if (roles[i].Name == role.Name)
                            throw new ArgumentException("Role exists");
                    }
                }
            }
            role.Id = (roles.LastOrDefault()?.Id ?? 0) + 1;
            roles.Add(role);

            if (roles.Count == 0)
            {
                role.Id = 1;
                roles.Add(role);
            }
        }

        public bool Delete(int id, List<Role> roles, List<User> users)
        {
            for (int j = 0; j < users.Count; j++)
            {
                if (users[j].Role.Id == id)
                {
                    return false;
                    
                }
            }

            for (int i = 0; i < roles.Count; i++)
            {
                if (roles[i].Id == id)
                {
                    roles.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public void Edit(int id, Role role, List<Role> roles)
        {
            for (int i = 0; i < roles.Count; i++)
            {
                if (roles[i].Name != null)
                {
                    if (roles[i].Name == role.Name)
                        throw new ArgumentException("Role exists");
                }
            }

            for (int i = 0; i < roles.Count; i++)
            {
                if (roles[i].Id == id)
                    roles[i] = role;
            }
        }

        public Role Get(int id, List<Role> roles)
        {
            for (int i = 0; i < roles.Count; i++)
            {
                if (roles[i].Id == id)
                    return roles[i];
            }
            return null;
        }

        public IEnumerable<Role> GetAll(List<Role> roles)
        {
            return roles;
        }
    }
}