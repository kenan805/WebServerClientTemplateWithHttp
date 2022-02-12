using CarsData.DAL.Repositories.Abstract;
using CarsData.Entities;
using System;

namespace CarsData.Unit_Of_Work.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        public IRepository<Car> CarRepository { get; }
        int Complete();
    }
}
