using MyShopper.core.Contracts;
using MyShopper.core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Caching;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyShopper.DataAccess.InMemory
{
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        ObjectCache cache = MemoryCache.Default;
        List<T> items;
        string ClassName;

        public InMemoryRepository()
        {
            ClassName = typeof(T).Name;
            items = cache[ClassName] as List<T>;
            if (items == null)
            {
                items = new List<T>();
            }
        }
        public void Commit()
        {
            cache[ClassName] = items;
        }

        public void Insert(T t)
        {
            items.Add(t);
        }

        public void Update(T temp)
        {
            T ttoUpdate = items.Find(t => t.Id == temp.Id);
            if (ttoUpdate != null)
            {
                ttoUpdate = temp;
            }
            else
            {
                throw new Exception(ClassName + "Not Found!");
            }
        }

        public T Find(string Id)
        {
            T t = items.Find(i => i.Id == Id);
            if (t != null)
            {
                return t;
            }
            else
            {
                throw new Exception(ClassName + "Not Found!");
            }
        }

        public IQueryable<T> Collection()
        {
            return items.AsQueryable();
        }

        public void Delete(string Id)
        {
            T ttoDelete = items.Find(t => t.Id == Id);
            if (ttoDelete != null)
            {
                items.Remove(ttoDelete);
            }
            else
            {
                throw new Exception(ClassName + "Not Found!");
            }
        }
    }
}
