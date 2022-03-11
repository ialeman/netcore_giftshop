using System;

namespace SS.Template.Core
{
    public interface IDateTime
    {
        DateTime Now { get; }

        DateTime Today { get; }

        DateTime FromUtc(DateTime date);
    }
}
