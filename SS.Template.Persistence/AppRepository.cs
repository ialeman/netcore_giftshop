using System;
using System.Collections.Generic;
using System.Text;
using SS.Data;

namespace SS.Template.Persistence
{
    public interface IAppRepository : IRepository
    {
    }

    public sealed class AppRepository : EfRepository<AppDbContext>, IAppRepository
    {
        public AppRepository(AppDbContext context)
            : base(context)
        {
        }
    }
}
