﻿using LmsApi.Core.Entities;
using System.Threading.Tasks;

namespace LmsApi.Core.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<Author> AuthorsRepo { get; }
        IGenericRepository<Literature> LiteraturesRepo { get; }
        IGenericRepository<Subject> SubjectsRepo { get; }
        Task<bool> CompleteAsync();
    }
}