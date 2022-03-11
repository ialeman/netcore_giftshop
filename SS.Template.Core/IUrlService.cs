using System;

namespace SS.Template.Core
{
    public interface IUrlService
    {
        string GetUri(params string[] paths);

        string GetSurveyUri(Guid surveyId, Guid employeeId);
    }
}
