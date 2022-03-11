using System.Collections.Generic;
using SS.Template.Application.Commands;

namespace SS.Template.Application.Examples.Commands.Add
{
    public class AddExamplesCommand : ICommand
    {
        public List<AddExampleModel> Examples { get; }

        public AddExamplesCommand(List<AddExampleModel> examples)
        {
            Examples = examples;
        }
    }
}
