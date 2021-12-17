using System.Text.Json.Serialization;

namespace WormsServer
{
    public interface IWorm
    {
        public const int LIFEFORCE_TO_REPRODUCE = 10;
        [JsonPropertyName("name")]
        public string Name { get; }
        [JsonPropertyName("lifeStrength")]
        public int Lifeforce { get; }

        [JsonPropertyName("position")]
        public Position WormPosition { get; }
    }
}