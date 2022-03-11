using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SS.Template.Application.Commands;
using SS.Template.Core.Persistence;
using SS.Template.Domain.Entities;

namespace SS.Template.Application.Examples.Commands.Add
{
    public sealed class AddExamplesCommandHandler : CommandHandler<AddExamplesCommand>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public AddExamplesCommandHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        protected override async Task Handle(AddExamplesCommand command, CancellationToken cancellationToken)
        {
            foreach (var exampleModel in command.Examples)
            {
                var entity = _mapper.Map<Example>(exampleModel);

                _repository.Add(entity);
            }

            await _repository.SaveChangesAsync(cancellationToken);
        }
    }
}
