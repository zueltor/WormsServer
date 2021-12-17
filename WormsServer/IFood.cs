using System.Text.Json.Serialization;

namespace WormsServer
{
    public interface IFood
    {
        [JsonPropertyName("expiresIn")]
        public int Lifeforce { get; }
        [JsonPropertyName("position")]
        public Position FoodPosition { get; }

    }
}