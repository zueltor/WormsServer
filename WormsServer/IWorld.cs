using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WormsServer
{
    public interface IWorld
    {
        [JsonPropertyName("worms")]
        IList<IWorm> Worms { get; }
        [JsonPropertyName("food")]
        IList<IFood> Foods { get; }
    }
}