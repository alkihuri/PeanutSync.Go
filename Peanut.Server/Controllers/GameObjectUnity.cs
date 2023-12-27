using System.Text.Json;
using System.Text.Json.Serialization;

namespace Peanut.Server.Domain; 

    public class GameObjectUnity
    { 
        [JsonPropertyName("ID")]
        public int ID { get; set; }
        [JsonPropertyName("Name")]
        public string Name { get; set; }
 
        [JsonPropertyName("Positions")]
        public List<Position> Positions { get; set; }

        [JsonPropertyName("PredictedPosition")]
        public Position PredictedPosition { get; set; }
    }

    public class Position
    {
    

   

        [JsonPropertyName("x")]
        public double? x { get; set; }
 
        [JsonPropertyName("y")]
        public double? y { get; set; }
 
        [JsonPropertyName("z")]
        public double? z { get; set; }


       public override string ToString()   
        {
            return  JsonSerializer.Serialize<Position>(this);
        }
    }