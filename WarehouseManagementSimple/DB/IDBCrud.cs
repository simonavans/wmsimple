using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public interface IDBCrud<T>
    {
        public IList<T> GetAll();
        public bool Get(ref T obj);
        public bool Create(ref T obj);
        public bool Update(T obj);
        public bool Delete(T obj);
    }
}
