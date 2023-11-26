using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace TestResultsDashboard.Services.Tests.AutoFixture
{
    public sealed class AutoDomainInlineDataAttribute : InlineAutoDataAttribute
    {
        public AutoDomainInlineDataAttribute(params object[] values) : base(new AutoDomainDataAttribute(), values)
        {
        }

        private sealed class AutoDomainDataAttribute : AutoDataAttribute
        {
            private static IFixture FixtureFactory()
            {
                var fixture = new Fixture();
                fixture.Behaviors.Add(new OmitOnRecursionBehavior());
                fixture.Customize(new AutoMoqCustomization
                {
                    ConfigureMembers = true
                });


                return fixture;
            }

            public AutoDomainDataAttribute() : base(FixtureFactory)
            {
            }
        }
    }
}
