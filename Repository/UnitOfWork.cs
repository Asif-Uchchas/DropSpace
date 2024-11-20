using DropSpace.Context;
using DropSpace.Contracts;

namespace DropSpace.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DropSpaceDbContext _context;

        public UnitOfWork(DropSpaceDbContext context)
        {
            _context = context;
        }

        //public IGenericRepository<Rank> Ranks
        //    => _ranks ??= new GenericRepository<Rank>(_context);
            
        

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool dispose)
        {
            if(dispose)
            {
                _context.Dispose();
            }
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
