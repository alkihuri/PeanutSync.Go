using System.Text.Json.Serialization;

namespace Peanut.Server.Domain;

[System.Serializable]
public class SceneData
{
    
    [JsonPropertyName("GameObjectsUnity")]
    public List<GameObjectUnity> GameObjectsUnity { get; set; }
}