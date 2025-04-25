using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using NaughtyAttributes;
using UnityEngine;
using System.Collections;

public class DungeonGenerator : MonoBehaviour 
{
    private int width = 100;
    private int height = 50;

    private int minRoomSize = 6;

    RectInt startRoom;
    private List<RectInt> rooms = new List<RectInt>();

    private void Start()
    {
        GenerateDungeon();
    }


    // Split and gerenate the dungeon
    [Button]
    void GenerateDungeon() 
    {
        rooms.Clear();
        startRoom = new RectInt(0, 0, width, height);

        DebugDrawingBatcher.BatchCall(() => AlgorithmsUtils.DebugRectInt(startRoom, Color.green));
        var splitResult = SplitVertically(startRoom);
        rooms.Add(startRoom);

        int setRoomCount = rooms.Count; // to avoid out of index for now

        // Goes through list of rooms to check if the rooms aren't too small to split
        for (int i = 0; i < setRoomCount; i++)
        {
            RectInt room = rooms[i];

            if ((room.width > (minRoomSize * 2) + 1 && room.height > (minRoomSize * 2) + 1))
            {
                
                Debug.Log("Can still be split");

                // Splits rooms horizontally
                var splitResultHorizontal = SplitHorizontally(splitResult.Item1);

                DebugDrawingBatcher.BatchCall(() => AlgorithmsUtils.DebugRectInt(splitResultHorizontal.Item1, Color.blue));
                DebugDrawingBatcher.BatchCall(() => AlgorithmsUtils.DebugRectInt(splitResultHorizontal.Item2, Color.blue));

                // Splits rooms vertically
                var splitResultVertical = SplitVertically(splitResultHorizontal.Item2);

                DebugDrawingBatcher.BatchCall(() => AlgorithmsUtils.DebugRectInt(splitResultVertical.Item1, Color.yellow));
                DebugDrawingBatcher.BatchCall(() => AlgorithmsUtils.DebugRectInt(splitResultVertical.Item2, Color.yellow));

                rooms.Add(splitResultVertical.Item1);
                rooms.Add(splitResultVertical.Item2);

                rooms.Add(splitResultHorizontal.Item1);
                rooms.Add(splitResultHorizontal.Item2);
            }
            else 
            {
                Debug.Log(room);
                Debug.Log("Room cannot be smaller");
                var index = rooms.IndexOf(rooms[i]);
                rooms.RemoveAt(index);
                
            }
        }
    }

    //IEnumerator GenerateRooms()
    //{
    //    Debug.Log("hello");

    //    while (true) 
    //    {
    //        if (Input.GetKeyDown(KeyCode.Space)) 
    //        {

    //        }
            
    //    }

    //    yield return null;
    //}

    // Splits the given rooms into two new rooms (vertical)
    (RectInt,RectInt) SplitVertically(RectInt pRoom)
    {
        int newWidth = Random.Range(minRoomSize, pRoom.width - minRoomSize);

        RectInt roomVSplit = new RectInt(pRoom.x, pRoom.y, newWidth + 1, pRoom.height);
        RectInt roomVSpit2 = new RectInt(pRoom.x + newWidth, pRoom.y, pRoom.width - newWidth, pRoom.height);

        return (roomVSplit, roomVSpit2);
    }

    // Splits the given rooms into two new rooms (horizontal)
    (RectInt, RectInt) SplitHorizontally(RectInt pRoom)
    {
        int newHeight = Random.Range(minRoomSize, pRoom.height - minRoomSize);

        RectInt roomHSplit = new RectInt(pRoom.x, pRoom.y, pRoom.width, newHeight + 1);
        RectInt roomHSplit2 = new RectInt(pRoom.x, pRoom.y + newHeight, pRoom.width, pRoom.height - newHeight);

        return (roomHSplit, roomHSplit2);
    }

    [Button]
    void ListContentDebug()
    {
        Debug.Log("Current rooms in the list:");
        Debug.Log("Amount of rooms in the list: " + rooms.Count);
        foreach (var room in rooms)
        {
            Debug.Log(room);
        }
    }
} 
