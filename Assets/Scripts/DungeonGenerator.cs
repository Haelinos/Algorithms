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
        //var splitResult = SplitHorizontally(startRoom);

        // Splits rooms vertically
        var splitResultVertical = SplitVertically(splitResult.Item1);

        DebugDrawingBatcher.BatchCall(() => AlgorithmsUtils.DebugRectInt(splitResultVertical.Item1, Color.yellow));
        DebugDrawingBatcher.BatchCall(() => AlgorithmsUtils.DebugRectInt(splitResultVertical.Item2, Color.yellow));

        // Splits rooms horizontally
        var splitResultHorizontal = SplitHorizontally(splitResult.Item2);

        DebugDrawingBatcher.BatchCall(() => AlgorithmsUtils.DebugRectInt(splitResultHorizontal.Item1, Color.blue));
        DebugDrawingBatcher.BatchCall(() => AlgorithmsUtils.DebugRectInt(splitResultHorizontal.Item2, Color.blue));
    }

    (RectInt,RectInt) SplitVertically(RectInt pRoom)
    {
        int newWidth = Random.Range(minRoomSize, pRoom.width - minRoomSize);

        RectInt roomVerticalSplit = new RectInt(pRoom.x, pRoom.y, newWidth+1, pRoom.height);
        RectInt roomVerticalSplit2 = new RectInt(pRoom.x + newWidth, pRoom.y, pRoom.width - newWidth, pRoom.height);

        return (roomVerticalSplit, roomVerticalSplit2);
    }

    (RectInt, RectInt) SplitHorizontally(RectInt pRoom)
    {
        int newHeight = Random.Range(minRoomSize, pRoom.height - minRoomSize);

        RectInt roomHorizontalSplit = new RectInt(pRoom.x, pRoom.y, pRoom.width, newHeight + 1);
        RectInt roomHorizontalSplit2 = new RectInt(pRoom.x, pRoom.y + newHeight, pRoom.width, pRoom.height - newHeight);

        return (roomHorizontalSplit, roomHorizontalSplit2);
    }
}
