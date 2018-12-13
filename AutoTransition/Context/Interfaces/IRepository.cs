using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTransition.Context.Interfaces
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();

        T GetItemById(Guid id);

        T GetElement(T item);

        void Create(T item);

        void Update(T item);

        void Delete(T item);
    }
}
