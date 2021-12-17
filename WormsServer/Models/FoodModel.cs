using System.Text.Json.Serialization;
using WorldOfWorms;

namespace WormsServer.Models
{
    public class FoodModel : IFood
    {
        [JsonPropertyName("expiresIn")]
        public int ExpiresIn { get; set; }
        [JsonPropertyName("position")]
        public Position Position { get; set; }
        [JsonIgnore]
        public int Lifeforce => ExpiresIn;
        [JsonIgnore]
        public Position FoodPosition => Position;
        public override bool Equals(object obj)
        {
            if (obj is IFood food)
            {
                return food.FoodPosition.Equals(FoodPosition);
            }

            return false;
        }
        public override int GetHashCode()
        {
            return FoodPosition.GetHashCode();
        }
    }
}
