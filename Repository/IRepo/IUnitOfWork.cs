namespace DropSpace.Repository.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        //IGenericRepository<Rank> Ranks { get; }

        Task Save();
    }
}
