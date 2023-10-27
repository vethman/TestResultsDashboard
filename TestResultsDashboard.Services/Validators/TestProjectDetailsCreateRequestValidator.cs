using FluentValidation;
using TestResultsDashboard.Common.Enums;
using TestResultsDashboard.Services.Models;

namespace TestResultsDashboard.Services.Validators;

public class TestProjectDetailsCreateRequestValidator : AbstractValidator<TestProjectDetailsCreateRequest>
{
    public TestProjectDetailsCreateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();
        
        RuleFor(x => x.Version)
            .GreaterThan(0);
        
        RuleFor(x => x.TestCategory)
            .IsInEnum()
            .NotEqual(TestCategory.Unknown);
    }
}