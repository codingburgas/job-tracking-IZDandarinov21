using JobTracking.DataAccess.Data;
using JobTracking.DataAccess.Interfaces;
using JobTracking.Domain.Entities; 
using System; 
using System.Threading.Tasks; 

namespace JobTracking.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IRepository<User> _users;
        private IRepository<JobAdvertisement> _jobAdvertisements;
        private IRepository<Application> _applications;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepository<User> Users => _users ??= new Repository<User>(_context);
        public IRepository<JobAdvertisement> JobAdvertisements => _jobAdvertisements ??= new Repository<JobAdvertisement>(_context);
        public IRepository<Application> Applications => _applications ??= new Repository<Application>(_context);

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}