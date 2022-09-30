using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SS.Template.Application.Commands;
using SS.Template.Core.Exceptions;
using SS.Data;
using SS.Template.Domain.Entities;

namespace SS.Template.Application.Examples.Commands.Add
{
    public sealed class EditExamplesCommandHandler //: CommandHandler<Edit<AddExampleModel>>
    {
        //private readonly IRepository _repository;
        //private readonly IMapper _mapper;

        //public EditExamplesCommandHandler(IRepository repository, IMapper mapper)
        //{
        //    _repository = repository;
        //    _mapper = mapper;
        //}

        //protected override async Task Handle(Edit<AddExampleModel> command, CancellationToken cancellationToken)
        //{
        //    var entity = await _repository.FirstAsync<Example>(x => x.Id == command.Id);

        //    if (entity == null)
        //    {
        //        throw EntityNotFoundException.For<Example>(command.Id);
        //    }

        //    _mapper.Map(command.Model, entity);
        //    await _repository.SaveChangesAsync(cancellationToken);
        //}
    }
}
