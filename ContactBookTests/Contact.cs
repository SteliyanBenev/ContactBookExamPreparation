using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace ContactBookTests
{
    public class Contact
    {
        [JsonPropertyName("id")]
        public int id { get; set; }

        [JsonPropertyName("firstName")]
        public string firstName { get; set; }

        [JsonPropertyName("lastName")]
        public string lastName { get; set; }

        [JsonPropertyName("email")]
        public string email { get; set; }

        [JsonPropertyName("phone")]
        public string phone { get; set; }

        [JsonPropertyName("comments")]
        public string comments { get; set; }
    }
}