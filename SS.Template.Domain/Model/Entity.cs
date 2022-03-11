using System;

namespace SS.Template.Domain.Model
{
    public abstract class Entity : IEntity
    {
        public Guid Id { get; set; }

        public void EnsureId()
        {
            if (Id == default)
            {
                Id = Guid.NewGuid();
            }
        }
    }
}
