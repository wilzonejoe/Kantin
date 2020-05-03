using Kantin.Tests.Utils;
using TechTalk.SpecFlow;

namespace Kantin.Tests.Helpers
{
    public class StepsHelper
    {
        public static string GetTimestampedName(ScenarioContext context, string name)
        {
            var currentTimestamp = ContextScout.GetCurrentTimestamp(context);

            if (string.IsNullOrEmpty(name))
                return string.Empty;

            return $"{name}{currentTimestamp}";
        }
    }
}
