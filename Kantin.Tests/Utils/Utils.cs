using System.Collections.Generic;
using System.Linq;

namespace Kantin.Tests.Utils
{
    public class Utils
    {
        public static List<string> StringToList(char delimeter, string value)
        {
            return value.Split(delimeter).ToList();
        }
    }
}
