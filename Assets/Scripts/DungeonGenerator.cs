using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour 
{
    // Rooms
    RectInt room = new RectInt(0,0,100,50);

    public bool splitHorizontally;
    int booleanConverter;


    void Update() 
    {
        AlgorithmsUtils.DebugRectInt(room, Color.magenta);
        SplitDirection();
        //SplitRandomizer();
    }

    //void SplitRandomizer() 
    //{
    //    booleanConverter = Random.Range(0, 2);
    //    Debug.Log(booleanConverter);

    //    if (booleanConverter == 1)
    //    {
    //        splitHorizontally = true;
    //    }
    //    else 
    //    {
    //        splitHorizontally = false;
    //    }
    //}
    void SplitDirection() 
    {
        if (splitHorizontally)
        {
            RectInt room = new RectInt(0,0,100,25);
            AlgorithmsUtils.DebugRectInt(room, Color.magenta);
        }
        else if(!splitHorizontally) 
        {
            RectInt room = new RectInt(0, 0, 50, 50);
            AlgorithmsUtils.DebugRectInt(room, Color.magenta);
        }

    }
}
