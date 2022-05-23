using System.Text.Json.Serialization;

namespace JavnaNabava.ViewModels
{
    public class IdStringLabel
    {
        [JsonPropertyName("label")]
        public string Label { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
        public IdStringLabel() { }
        public IdStringLabel(string id, string label)
        {
            Id = id;
            Label = label;
        }
    }
}
