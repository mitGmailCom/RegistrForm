using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrForm.Models
{
    interface IRepository<T>
    {
        void Create(T item, Role role);

        bool Delete(int id);

        T Get(int id);

        IEnumerable<T> GetAll();

        void Edit(int id, T user);
    }
}
