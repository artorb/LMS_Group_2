using LmsApi.Core.Entities;
using LmsApi.Core.Interfaces;
using LmsApi.Data.Data;
using LmsApi.Data.Repositories;
using System;
using System.Threading.Tasks;

namespace LmsApi.Data.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LmsApiDbContext context;
        private readonly IGenericRepository<Author> authorsRepo;
        private readonly IGenericRepository<Literature> literaturesRepo;
        private readonly IGenericRepository<Category> categoriesRepo;
        private readonly IGenericRepository<Subject> subjectsRepo;
        private readonly IGenericRepository<Level> levelsRepo;

        public UnitOfWork(LmsApiDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // using expression body for properties (w/ get) and method instead of regular block body.
        public IGenericRepository<Author> AuthorsRepo => 
            authorsRepo ?? new GenericRepository<Author>(context);

        public IGenericRepository<Literature> LiteraturesRepo => 
            literaturesRepo ?? new GenericRepository<Literature>(context);

        public IGenericRepository<Category> CategoriesRepo =>
            categoriesRepo ?? new GenericRepository<Category>(context);

        public IGenericRepository<Subject> SubjectsRepo => 
            subjectsRepo ?? new GenericRepository<Subject>(context);

        public IGenericRepository<Level> LevelsRepo =>
            levelsRepo ?? new GenericRepository<Level>(context);

        public async Task<bool> CompleteAsync() => 
            await context.SaveChangesAsync() > 0;
    }
}
