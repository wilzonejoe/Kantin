using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Kantin.Tests.Utils
{
    public class JsonReader
    {
        public T ReadEntry<T>(string file)
        {
            T data = default;

            using (var r = new StreamReader(file))
            {
                var json = r.ReadToEnd();
                data = JsonConvert.DeserializeObject<T>(json);
            }

            return data;
        }

    }
}
