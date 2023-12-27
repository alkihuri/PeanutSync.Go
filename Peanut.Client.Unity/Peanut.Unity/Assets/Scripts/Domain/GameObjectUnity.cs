using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class GameObjectUnity
{
    [field: SerializeField] public int ID;
    [field: SerializeField] public string Name;
    [field: SerializeField] public List<Position> Positions;
    [field: SerializeField] public Position PredictedPosition;

    public GameObject GetRealSceneGameObject(List<Transform> objects)
    {
        return objects.Select(t => t.gameObject).Where(o => o.GetInstanceID() == ID).First();
    }
    public GameObjectUnity(int id, string name, List<Position> p)
    {
        this.ID = id;
        this.Name = name;
        this.Positions = p;
    }
}

[System.Serializable]
public class Position
{
    public Position(Vector3 position)
    {
        this.x = position.x;
        this.y = position.y;
        this.z = position.z;
    }

    public double x;
    public double y;
    public double z;

    public Vector3 UnityVector3
    {
        get
        {
            return new Vector3((float)this.x, (float)this.y, (float)this.z);
        }
    }
}