using System.Collections.Generic;

[System.Serializable]
public class SceneData
{
    
    public List<GameObjectUnity> GameObjectsUnity = new List<GameObjectUnity>();

    public SceneData()
    {
        GameObjectsUnity = new List<GameObjectUnity>();  
    }

    public SceneData(List<GameObjectUnity> objects)
    {
        this.GameObjectsUnity = objects;
    }
}
