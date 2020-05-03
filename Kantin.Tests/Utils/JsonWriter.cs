using Newtonsoft.Json;

namespace Kantin.Tests.Utils
{
    public class JsonWriter
    {
        public void Write(string output, object data)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            System.IO.File.WriteAllText(output, json);
        }
    }
}
