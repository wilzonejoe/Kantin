using Kantin.Data;
using Kantin.Tests.Case;
using TechTalk.SpecFlow;

namespace Kantin.Tests.Utils
{
    public static class ContextScout
    {
        public static KantinEntities GetDBContextContainer(ScenarioContext context)
        {
            return (KantinEntities)context[ContextContants.DatabaseContainerContext];
        }
        
        public static string GetCurrentTimestamp(ScenarioContext context)
        {
            return (string)context[ContextContants.CurrentTimestampContext];
        }
    }
}
