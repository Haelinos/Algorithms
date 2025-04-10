using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using NaughtyAttributes;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour 
{
    private int width = 100;
    private int height = 50;


    RectInt startRoom;
    private List<RectInt> rooms = new List<RectInt>();

    //private int newWidth;
    //private int newWidthOffset;
    //private int newHeight;
    //private int newHeightOffset;
    //private int newX;
    //private int newY;
    //public int offset;

    void Update() 
    {
        // Colors the rooms
        foreach (var room in rooms)
        {
            AlgorithmsUtils.DebugRectInt(room, Color.magenta);
        }

        AlgorithmsUtils.DebugRectInt(startRoom, Color.green);
    }

    [Button]
    void GenerateDungeon() 
    {
        rooms.Clear();
        startRoom = new RectInt(0, 0, width, height);
        SplitRooms(startRoom);
    }

    void SplitRooms(RectInt pRoom)
    {
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            SplitVertically(pRoom);
        }
        else 
        {
            SplitHorizontally(pRoom);
        }
    }

    void SplitVertically(RectInt pRoom) 
    {
        int randomSplitX = Random.Range(pRoom.x + 1, pRoom.x + pRoom.width - 1);

        RectInt roomA = new RectInt(pRoom.x, pRoom.y, randomSplitX - pRoom.x, pRoom.height);
        RectInt roomB = new RectInt(randomSplitX, pRoom.y, pRoom.x + pRoom.width - randomSplitX, pRoom.height);

        rooms.Add(roomA);
        rooms.Add(roomB);

        // Further split rooms recursively if desired
        if (Random.Range(0, 2) == 0) SplitRooms(roomA);
        if (Random.Range(0, 2) == 0) SplitRooms(roomB);

        //newHeight = startRoom.height / 2;
        //newHeightOffset = startRoom.height - newHeight + offset;
        //newY = startRoom.y + newHeight;

        //newWidth = startRoom.width / 2;
        //newWidthOffset = startRoom.width - newWidth + offset;
        //newX = startRoom.x + newWidth;

        //RectInt roomVerticalSplit = new RectInt(startRoom.x, startRoom.y, newWidth, startRoom.height);
        //RectInt roomVerticalSplitOffset = new RectInt(newX - offset, startRoom.y, newWidthOffset, startRoom.height);

        //rooms.Add(roomVerticalSplit);
        //rooms.Add(roomVerticalSplitOffset);

        //RectInt roomHorizontalSplit2 = new RectInt(newX, newY, newWidth, newHeight);
        //RectInt roomHorizontalSplitOffset2 = new RectInt(newX, startRoom.y, newWidth, newHeightOffset);

        //rooms.Add(roomHorizontalSplit2);
        //rooms.Add(roomHorizontalSplitOffset2);

        //RectInt roomHorizontalSplit3 = new RectInt(startRoom.x, newY, newWidth - offset, newHeight);
        //RectInt roomHorizontalSplitOffset3 = new RectInt(startRoom.x, startRoom.y, newWidth - offset, newHeightOffset);

        //rooms.Add(roomHorizontalSplit3);
        //rooms.Add(roomHorizontalSplitOffset3);
    }

    void SplitHorizontally(RectInt pRoom)
    {
        //newWidth = startRoom.width / 2;
        //newWidthOffset = startRoom.width - newWidth + offset;
        //newX = startRoom.x + newWidth;

        //newHeight = startRoom.height / 2;
        //newHeightOffset = startRoom.height - newHeight + offset;
        //newY = startRoom.y + newHeight;

        //RectInt roomHorizontalSplit = new RectInt(startRoom.x, startRoom.y, startRoom.width, newHeight);
        //RectInt roomHorizontalSplitOffset = new RectInt(startRoom.x, newY - offset, startRoom.width, newHeightOffset);

        //rooms.Add(roomHorizontalSplit);
        //rooms.Add(roomHorizontalSplitOffset);

        //RectInt roomVerticalSplit2 = new RectInt(newX, newY, newWidth, newHeight);
        //RectInt roomVerticalSplitOffset2 = new RectInt(startRoom.x, newY, newWidthOffset, newHeight);

        //rooms.Add(roomVerticalSplit2);
        //rooms.Add(roomVerticalSplitOffset2);

        //RectInt roomVerticalSplit3 = new RectInt(newX, startRoom.y, newWidth, newHeight -offset);
        //RectInt roomVerticalSplitOffset3 = new RectInt(startRoom.x, startRoom.y, newWidthOffset, newHeight -offset);

        //rooms.Add(roomVerticalSplit3);
        //rooms.Add(roomVerticalSplitOffset3);

    }
}
