using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour 
{

    public bool splitHorizontally;
    int booleanConverter;
    int width = 100;
    int height = 50;

    int newWidth;
    int newHeight;

    // Rooms
    RectInt room;

    private void Start()
    {
        room = new RectInt(0, 0, width, height);
    }

    void Update() 
    {
        AlgorithmsUtils.DebugRectInt(room, Color.magenta);
        SplitRandomizer();
    }

    void SplitRandomizer()
    {
        for (int i = 0; i < 4; i++)
        {
            booleanConverter = Random.Range(0, 2);
            Debug.Log(booleanConverter);
        }

        if (booleanConverter == 1)
        {
            splitHorizontally = true;
        }
        else
        {
            splitHorizontally = false;
        }
        SplitDirection();
    }

    void SplitDirection() 
    {
        if (splitHorizontally)
        {
            RectInt room = new RectInt(0,0,width,height/2);
            AlgorithmsUtils.DebugRectInt(room, Color.magenta);
        }
        else if(!splitHorizontally) 
        {
            RectInt room = new RectInt(0, 0, width/2, height);
            AlgorithmsUtils.DebugRectInt(room, Color.magenta);

        }

        SplitSmallerRoom();
    }

    void SplitSmallerRoom()
    {
        if (splitHorizontally)
        {
            newHeight = room.height / 4;
            RectInt room2 = new RectInt(0, 0, width, newHeight);
            AlgorithmsUtils.DebugRectInt(room2, Color.magenta);
        }
        if (!splitHorizontally)
        {
            newWidth = room.width / 4;
            RectInt room2 = new RectInt(0, 0, newWidth, height);
            AlgorithmsUtils.DebugRectInt(room2, Color.magenta);
        }
    }
}
