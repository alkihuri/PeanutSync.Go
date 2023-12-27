using UnityEngine;

[System.Serializable]
public class GameObjectUnity
{

    public GameObjectUnity()
    {

    }

    public GameObjectUnity(string name, Position position)
    {
        Name = name;
        Position = position;
    }

    [field: SerializeField] public string Name;
    [field: SerializeField] public Position Position;
}

[System.Serializable]
public class Position
{ 
    public Position()
    {

    }
    public Position(Vector3 position)
    {
        this.x = position.x;
        this.y = position.y;
        this.z = position.z;
    }

    public double x;
    public double y;
    public double z;
}