using SS.Data;

namespace SS.Template.Persistence

{
    public interface IAppReadOnlyRepository : IReadOnlyRepository
    {
        
    }

    public sealed class AppReadOnlyRepository : ReadOnlyEfRepository<AppDbContext>, IAppReadOnlyRepository
    {
        public AppReadOnlyRepository(AppDbContext context) : base(context)
        {
        }
    }
}
