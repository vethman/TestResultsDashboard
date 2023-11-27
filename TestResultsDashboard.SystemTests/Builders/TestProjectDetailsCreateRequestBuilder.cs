using Bogus;
using TestResultsDashboard.Common.Enums;
using TestResultsDashboard.Services.Models;

namespace TestResultsDashboard.AppTests.Builders;

public class TestProjectDetailsCreateRequestBuilder
{
    private Faker<TestProjectDetailsCreateRequest> _fakerTestProjectDetailsCreateRequest = new();

    public static TestProjectDetailsCreateRequestBuilder New()
    {
        return new TestProjectDetailsCreateRequestBuilder();
    }

    public TestProjectDetailsCreateRequest Build()
    {
        var item = _fakerTestProjectDetailsCreateRequest.Generate();
        _fakerTestProjectDetailsCreateRequest = new Faker<TestProjectDetailsCreateRequest>();
        return item;
    }

    public TestProjectDetailsCreateRequestBuilder WithDefault()
    {
        _fakerTestProjectDetailsCreateRequest
            .RuleFor(x => x.Name, y => y.Commerce.Product())
            .RuleFor(x => x.Version, y => y.Random.Int(1, 999))
            .RuleFor(x => x.Summary, y => y.Lorem.Paragraphs())
            .RuleFor(x => x.TestCategory, y => y.PickRandom<TestCategory>())
            .RuleFor(x => x.CreatedBy, y => y.Name.FullName())
            ;

        return this;
    }

    public TestProjectDetailsCreateRequestBuilder WithTestCategory(TestCategory testCategory)
    {
        _fakerTestProjectDetailsCreateRequest
            .RuleFor(x => x.TestCategory, testCategory);

        return this;
    }
}
