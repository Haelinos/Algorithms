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

    [Button]
    void GenerateDungeon()
    {
        rooms.Clear();
        startRoom = new RectInt(0, 0, width, height); 
        SplitRoom(startRoom); 

        // Visualize the rooms
        foreach (var room in rooms)
        {
            DebugDrawingBatcher.BatchCall(() => AlgorithmsUtils.DebugRectInt(room, Color.green));
        }
    }

    void SplitRoom(RectInt room)
    {
        if (room.width <= minRoomSize * 2 || room.height <= minRoomSize * 2)
        {
            rooms.Add(room);
            return;
        }

        //Randomizes the split
        bool splitVertically = Random.value > 0.5f;

        if (splitVertically && room.width > minRoomSize * 2)
        {
            var (left, right) = SplitVertically(room);
            SplitRoom(left);  
            SplitRoom(right); 
        }
        else if (room.height > minRoomSize * 2)
        {
            var (top, bottom) = SplitHorizontally(room);
            SplitRoom(top);     
            SplitRoom(bottom);  
        }
        else
        {
            rooms.Add(room);
        }
    }

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
