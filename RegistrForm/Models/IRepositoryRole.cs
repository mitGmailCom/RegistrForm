using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace RegistrForm.Models
{
    interface IRepositoryRole<T>
    {
        void Create(Role role, List<T> roles);

        bool Delete(int id, List<T> roles, List<User> users);

        T Get(int id, List<T> roles);

        IEnumerable<T> GetAll(List<T> roles);

        void Edit(int id, T role, List<T> roles);
    }
}
