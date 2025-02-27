using UnityEngine;

public class DungeonGenerator : MonoBehaviour 
{
    RectInt room = new RectInt(0,0,100,50);


    void Update() 
    {
        AlgorithmsUtils.DebugRectInt(room, Color.magenta);
        GenerateRect();
    }

    void GenerateRect() 
    { 
        
    }
}
