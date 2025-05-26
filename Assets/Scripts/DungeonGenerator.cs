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
    private List<RectInt> rooms = new List<RectInt>();
    private List<RectInt> RoomsDone = new();
    private List<RectInt> CheckedRooms = new();

    private void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        rooms.Clear();
        startRoom = new RectInt(0, 0, width, height);
        SplitRoom(startRoom);

        RoomsDone = new List<RectInt>(rooms);

        // Start debug/check coroutines
        StartCoroutine(DebugGenerator());
        StartCoroutine(CheckRooms());
    }

    IEnumerator DebugGenerator()
    {
        int roomIndex = 0;
        while (true)
        {
            yield return new WaitUntil(() => (Input.GetKeyDown(KeyCode.Space)));

            if (roomIndex < rooms.Count) 
            {
                roomIndex++;

                // Visualize the rooms
                foreach (var room in rooms)
                {
                    DebugDrawingBatcher.BatchCall(() => AlgorithmsUtils.DebugRectInt(room, Color.green));
                }
                
            }
        }
    }

    // Debugging & Checking every room
    IEnumerator CheckRooms()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.C));
        Debug.Log("C was pressed, starting room check!");

        CheckedRooms.Clear();
        Queue<RectInt> myQueue = new Queue<RectInt>(RoomsDone);
        Debug.Log("RoomsDone count: " + RoomsDone.Count);

        while (myQueue.Count > 0)
        {
            // Wait for V key down
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.V));

            // Process one room
            RectInt current = myQueue.Dequeue();
            CheckedRooms.Add(current);

            DebugDrawingBatcher.BatchCall(() => AlgorithmsUtils.DebugRectInt(current, Color.cyan));
            Debug.Log(current + " is checked!");

            // Prevent holding key from triggering again immediately
            yield return new WaitUntil(() => !Input.GetKey(KeyCode.V));
        }

        Debug.Log("Done checking rooms");
    }
    void SplitRoom(RectInt room)
{
    if (room.width <= minRoomSize * 2 && room.height <= minRoomSize * 2)
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
    else if (!splitVertically && room.height > minRoomSize * 2)
    {
        var (top, bottom) = SplitHorizontally(room);
        SplitRoom(top);
        SplitRoom(bottom);
    }

    // Checks if room can still be split without randomization
    else if (room.width > minRoomSize * 2)
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
