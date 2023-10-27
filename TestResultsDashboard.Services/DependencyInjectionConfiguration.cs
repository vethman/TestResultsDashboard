using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using TestResultsDashboard.Services.Validators;

namespace TestResultsDashboard.Services;

public static class DependencyInjectionConfiguration
{
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddTransient<ITestProjectDetailsService, TestProjectDetailsService>()
            .AddTransient<ITestResultService, TestResultService>()
            .AddTransient<ITestProjectService, TestProjectService>()
            ;
    }
    
    public static IServiceCollection AddFluentValidators(this IServiceCollection serviceCollection)
    {
        return serviceCollection
                .AddValidatorsFromAssemblyContaining<TestProjectDetailsCreateRequestValidator>()
                .AddFluentValidationAutoValidation()
            ;
    }
}