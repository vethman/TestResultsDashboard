using FluentValidation;
using TestResultsDashboard.Common.Enums;
using TestResultsDashboard.Repositories;
using TestResultsDashboard.Services.Models;

namespace TestResultsDashboard.Services.Validators;

public class TestProjectDetailsUpdateRequestValidator : AbstractValidator<TestProjectDetailsUpdateRequest>
{
    public TestProjectDetailsUpdateRequestValidator(ITestProjectDetailsService testProjectDetailsService)
    {
        RuleFor(x => x.Name)
            .NotEmpty();
        
        RuleFor(x => x.Version)
            .GreaterThan(0);
        
        RuleFor(x => x.TestCategory)
            .IsInEnum()
            .NotEqual(TestCategory.Unknown);

        RuleFor(x => x)
            .Custom((item, context) =>
            {
                if (!testProjectDetailsService.TestProjectDetailsExistsAsync(item.Name, item.Version).Result)
                    context.AddFailure($"No TestProjectDetails found for name: '{item.Name}' and version: '{item.Version}'");
            });
    }
}