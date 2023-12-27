using System.Text.Json.Serialization;

namespace Peanut.Server.Domain; 

    public class GameObjectUnity
    { 
        [JsonPropertyName("Name")]
        public string Name { get; set; }
 
        [JsonPropertyName("Position")]
        public Position Position { get; set; }
    }

    public class Position
    { 
        [JsonPropertyName("x")]
        public double? x { get; set; }
 
        [JsonPropertyName("y")]
        public double? y { get; set; }
 
        [JsonPropertyName("z")]
        public double? z { get; set; }
    }