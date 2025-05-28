using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using NaughtyAttributes;
using UnityEngine;
using System.Collections;
using UnityEditor;

public class DungeonGenerator : MonoBehaviour
{
    private int width = 100;
    private int height = 50;

    private int minRoomSize = 6;

    RectInt startRoom;
    private List<RectInt> roomsToSplit = new List<RectInt>();
    private List<RectInt> roomsDone = new();

    private void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        startRoom = new RectInt(0, 0, width, height);
        SplitRoom(startRoom);

        // Start debug/check coroutines
        StartCoroutine(DebugGenerator());
        //StartCoroutine(CheckRooms());
    }

    IEnumerator DebugGenerator()
    {
        int roomIndex = 0;
        while (true)
        {
            yield return new WaitUntil(() => (Input.GetKeyDown(KeyCode.Space)));

            if (roomIndex < roomsDone.Count) 
            {
                roomIndex++;

                // Visualize the rooms
                foreach (var room in roomsDone)
                {
                    DebugDrawingBatcher.BatchCall(() => AlgorithmsUtils.DebugRectInt(room, Color.green));
                }
                
            }
        }
    }
    void SplitRoom(RectInt room)
{
    if (room.width <= minRoomSize * 2 && room.height <= minRoomSize * 2)
    {
        //roomsToSplit.Add(room);
        roomsDone.Add(room);
        return;
    }

    if (room.width > minRoomSize * 2)
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

    // Checks which rooms are intersecting with each other.
    IEnumerator CheckIntersection()
    {
        for (int i = 0; i < roomsToSplit.Count; i++)
        {
            for (int j = i + 1; j < roomsToSplit.Count; j++) 
            { 
                RectInt roomA = roomsToSplit[i];
                RectInt roomB = roomsToSplit[j];

                if (AlgorithmsUtils.Intersects(roomA, roomB)) 
                {
                    Debug.Log(roomA + " Intersects with " + roomB);

                    // Visualize the intersections
                    RectInt visualIntersect = AlgorithmsUtils.Intersect(roomA, roomB);
                    DebugDrawingBatcher.BatchCall(() => AlgorithmsUtils.DebugRectInt(visualIntersect, Color.yellow));
                }

                yield return null;
            }
        }
    }
 
    [Button]
    void ListContentDebug()
    {
        Debug.Log("Current rooms in the list:");
        Debug.Log("Amount of rooms in the list: " + roomsToSplit.Count);
        foreach (var room in roomsToSplit)
        {
            Debug.Log(room);
        }
    }

    [Button]
    void DebugIntersection() 
    {
        StartCoroutine(CheckIntersection());
    }
} 
