using Kantin.Tests.Utils;
using System;
using TechTalk.SpecFlow;

namespace Kantin.Tests.Case.Hooks
{
    [Binding]
    public sealed class BeforeAfterHooks
    {
        private readonly ScenarioContext Context;

        public BeforeAfterHooks(ScenarioContext injectedContext)
        {
            Context = injectedContext;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            var dbContextCreator = new DbContextCreator();
            var dbContextContainer = dbContextCreator.CreateDbContext();
            Context[ContextContants.DatabaseContainerContext] = dbContextContainer;
            Context[ContextContants.CurrentTimestampContext] = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds().ToString();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            Context.Clear();
        }
    }
}
