using CarsData.DAL.Context;
using CarsData.DAL.Repositories.Abstract;
using CarsData.DAL.Repositories.Concrete;
using CarsData.Entities;
using CarsData.Unit_Of_Work.Abstract;

namespace CarsData.Unit_Of_Work.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private CarDbContext _carDbContext;
        public IRepository<Car> CarRepository { get; private set; }

        public UnitOfWork(CarDbContext carDbContext)
        {
            _carDbContext = carDbContext;
            CarRepository = new Repository<Car>(carDbContext);
        }

        public int Complete()
        {
            return _carDbContext.SaveChanges();
        }

        public void Dispose()
        {
            _carDbContext.Dispose();
        }
    }
}
