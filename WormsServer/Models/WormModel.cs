using System.Text.Json.Serialization;
using WorldOfWorms;

namespace WormsServer.Models
{
    public class WormModel : IWorm
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("lifeStrength")]
        public int LifeStrength { get; set; }
        [JsonPropertyName("position")]
        public Position Position { get; set; }
        [JsonIgnore]
        public int Lifeforce => LifeStrength;
        [JsonIgnore]
        public Position WormPosition => Position;
        public override bool Equals(object o)
        {
            if (o is IWorm worm)
            {
                return worm.Name.Equals(Name);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
