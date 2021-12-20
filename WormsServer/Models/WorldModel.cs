using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace WormsServer.Models
{
    public class WorldModel : IWorld
    {
        [JsonPropertyName("worms")]
        public List<WormModel> Worm { get; set; }
        [JsonPropertyName("food")]
        public List<FoodModel> Food { get; set; }
        [JsonIgnore]
        public IList<IFood> Foods => Food.Cast<IFood>().ToList();
        [JsonIgnore]
        public IList<IWorm> Worms => Worm.Cast<IWorm>().ToList();
    }
}
