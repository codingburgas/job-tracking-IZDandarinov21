using JobTracking.Domain.Entities;
using System; 
using System.Threading.Tasks; 

namespace JobTracking.DataAccess.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<JobAdvertisement> JobAdvertisements { get; }
        IRepository<Application> Applications { get; }
        Task<int> CompleteAsync();
    }
}