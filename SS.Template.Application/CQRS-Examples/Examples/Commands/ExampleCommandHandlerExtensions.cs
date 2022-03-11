using System;
using System.Threading.Tasks;
using SS.Template.Core.Exceptions;
using SS.Template.Core.Persistence;
using SS.Template.Domain.Entities;
using SS.Template.Domain.Model;

namespace SS.Template.Application.Examples.Commands
{
    internal static class ExampleCommandHandlerExtensions
    {
        public static async Task ValidateUniqueEmail(IReadOnlyRepository readOnlyRepository, AddExampleModel request)
        {
            var query = readOnlyRepository.Query<Example>(x => x.Status == EnabledStatus.Enabled && x.Email == request.Email);
            var exists = await readOnlyRepository.AnyAsync(query);
            if (exists)
            {
                throw new ObjectValidationException(nameof(request.Email), $"Email {request.Email} is already assigned to an example.");
            }
        }

        public static async Task<Example> FindEnabledExample(this IRepositoryBase repository, Guid id)
        {
            var entity = await repository.SingleAsync<Example>(x => x.Id == id && x.Status == EnabledStatus.Enabled);
            if (entity == null)
            {
                throw EntityNotFoundException.For<Example>(id);
            }

            return entity;
        }
    }
}
