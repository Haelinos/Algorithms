using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using NaughtyAttributes;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour 
{
    private int width = 100;
    private int height = 50;

    private int minRoomSize = 6;


    RectInt startRoom;
    private List<RectInt> rooms = new List<RectInt>();

    private int newWidth;
    private int newWidthOffset;
    private int newHeight;
    private int newHeightOffset;
    private int newX;
    private int newY;
    public int offset;

    private void Start()
    {
        GenerateDungeon();
    }

    /*
    void Update() 
    {
        // Colors the rooms
        foreach (var room in rooms)
        {
            AlgorithmsUtils.DebugRectInt(room, Color.magenta);
        }

        AlgorithmsUtils.DebugRectInt(startRoom, Color.green);
    }
    */

    [Button]
    void GenerateDungeon() 
    {
        rooms.Clear();
        startRoom = new RectInt(0, 0, width, height);

        DebugDrawingBatcher.BatchCall(() => AlgorithmsUtils.DebugRectInt(startRoom, Color.green));

        var splitResult = SplitVertically(startRoom);

        //DebugDrawingBatcher.BatchCall(() => AlgorithmsUtils.DebugRectInt(splitResult.Item1, Color.red));
        //DebugDrawingBatcher.BatchCall(() => AlgorithmsUtils.DebugRectInt(splitResult.Item2, Color.cyan));

        //var splitResultRight = SplitVertically2(splitResult.Item2);

        //DebugDrawingBatcher.BatchCall(() => AlgorithmsUtils.DebugRectInt(splitResultRight.Item1, Color.yellow));
        //DebugDrawingBatcher.BatchCall(() => AlgorithmsUtils.DebugRectInt(splitResultRight.Item2, Color.blue));

        var splitResultLeft = SplitHorizontally(splitResult.Item1);

        DebugDrawingBatcher.BatchCall(() => AlgorithmsUtils.DebugRectInt(splitResultLeft.Item1, Color.yellow));
        DebugDrawingBatcher.BatchCall(() => AlgorithmsUtils.DebugRectInt(splitResultLeft.Item2, Color.blue));

        //SplitRooms(startRoom);
    }

    //void SplitRooms(RectInt pRoom)
    //{
    //    if (Random.Range(0, 2) == 0)
    //    {
    //        SplitVertically(pRoom);
    //    }
    //    else 
    //    {
    //        SplitHorizontally(pRoom);
    //    }
    //}

    (RectInt,RectInt) SplitVertically(RectInt pRoom)
    {
        int newWidth = Random.Range(minRoomSize, pRoom.width - minRoomSize);

        RectInt roomVerticalSplit = new RectInt(pRoom.x, pRoom.y, newWidth+1, pRoom.height);
        RectInt roomVerticalSplitOffset = new RectInt(pRoom.x + newWidth, pRoom.y, pRoom.width - newWidth, pRoom.height);

        return (roomVerticalSplit, roomVerticalSplitOffset);
    }

    (RectInt, RectInt) SplitHorizontally(RectInt pRoom)
    {
        int newHeight = Random.Range(minRoomSize, pRoom.height - minRoomSize);

        RectInt roomHorizontalSplit = new RectInt(pRoom.x, pRoom.y, pRoom.width, newHeight + 1);
        RectInt roomHorizontalSplitOffset = new RectInt(pRoom.x, pRoom.y + newHeight, pRoom.width, pRoom.height - newHeight);

        return (roomHorizontalSplit, roomHorizontalSplitOffset);
    }


    //void SplitVertically(RectInt pRoom) 
    //{

    //    newHeight = pRoom.height / 2;
    //    newHeightOffset = pRoom.height - newHeight + offset;
    //    newY = pRoom.y + newHeight;

    //    newWidth = pRoom.width / 2;
    //    newWidthOffset = pRoom.width - newWidth + offset;
    //    newX = pRoom.x + newWidth;

    //    RectInt roomVerticalSplit = new RectInt(pRoom.x, pRoom.y, newWidth, pRoom.height);
    //    RectInt roomVerticalSplitOffset = new RectInt(newX - offset, pRoom.y, newWidthOffset, pRoom.height);

    //    rooms.Add(roomVerticalSplit);
    //    rooms.Add(roomVerticalSplitOffset);

    //    RectInt roomHorizontalSplit2 = new RectInt(newX, newY, newWidth, newHeight);
    //    RectInt roomHorizontalSplitOffset2 = new RectInt(newX, pRoom.y, newWidth, newHeightOffset);

    //    rooms.Add(roomHorizontalSplit2);
    //    rooms.Add(roomHorizontalSplitOffset2);
    //}

    //void SplitHorizontally(RectInt pRoom)
    //{

    //    newWidth = pRoom.width / 2;
    //    newWidthOffset = pRoom.width - newWidth + offset;
    //    newX = pRoom.x + newWidth;

    //    newHeight = pRoom.height / 2;
    //    newHeightOffset = pRoom.height - newHeight + offset;
    //    newY = pRoom.y + newHeight;

    //    RectInt roomHorizontalSplit = new RectInt(pRoom.x, pRoom.y, pRoom.width, newHeight);
    //    RectInt roomHorizontalSplitOffset = new RectInt(pRoom.x, newY - offset, pRoom.width, newHeightOffset);

    //    rooms.Add(roomHorizontalSplit);
    //    rooms.Add(roomHorizontalSplitOffset);

    //    RectInt roomVerticalSplit2 = new RectInt(newX, newY, newWidth, newHeight);
    //    RectInt roomVerticalSplitOffset2 = new RectInt(pRoom.x, newY, newWidthOffset, newHeight);

    //    rooms.Add(roomVerticalSplit2);
    //    rooms.Add(roomVerticalSplitOffset2);
    //}
}
