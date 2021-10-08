using Lms.Core.Entities;
using Lms.Core.Repositories;
using Lms.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly LmsDbContext db = null;
        private readonly DbSet<T> table = null;

        public GenericRepository(LmsDbContext context)
        {
            db = context ?? throw new ArgumentNullException(nameof(db));
            table = db.Set<T>();
        }

        public void Add(T obj)
        {
            db.Add(obj);
        }

        public async Task<bool> AnyAsync(int? id)
        {
            return await table.AnyAsync(g => g.Id == id);
        }

        public async Task<T> FindAsync(int? id)
        {
            return await table.FindAsync(id);
        }

        public async Task<T> GetAsync(int id)
        {
            return await table.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await table.ToListAsync();
        }

        public void Remove(T obj)
        {
            table.Remove(obj);
        }

        public void Update(T obj)
        {
            table.Update(obj);
        }


        //public async Task<IEnumerable<T>> GetAll(params Enum[] list)
        //{
        //    for (int i = 0; i < list.Length; i++)
        //    {
        //        if (i =1)
        //        {
        //            table.Include(a => a.activity);
        //        }
        //        if (i =2)
        //        {
        //            table.Include(a => a.course);
        //        }

        //    }
        //    return await table.ToListAsync();
        //}
    }
}
//unitOfWork.CourseRepository.GetAll("Activity", "Course", "Module");

//_context.Course.Include(a => a.activity).Include(a => a.course).Include(a => a.module);