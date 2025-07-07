using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private int width = 100;
    [SerializeField] private int height = 50;
    [SerializeField] private int minRoomSize = 6;
    [SerializeField] private bool showDebugGizmos = true;

    private Vector3 doorSize = new Vector3(1, 1, 1);

    RectInt startRoom;
    private List<RectInt> rooms = new();
    private List<Vector3> doorPositions = new();
    private int[,] tileMap;

    public GameObject wall;
    public GameObject wallsParent;

    Graph<Vector3> roomGraph = new Graph<Vector3>();

    private void Start()
    {

        GenerateDungeon();
    }

    /// <summary>
    /// The GenerateDungeon function adds the start room and starts the room splitting and coroutines.
    /// </summary>
    void GenerateDungeon()
    {
        startRoom = new RectInt(0, 0, width, height);
        SplitRoom(startRoom);

        // Start debug/check coroutines
        StartCoroutine(DebugGenerator());
    }

    /// <summary>
    /// The DebugGenerator function generates and visualizes the dungeon rooms when Spacebar is pressed
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// The SplitRoom function checks recursively through the rooms whether the room can be split or not
    /// </summary>
    /// <param name="room"></param>
    void SplitRoom(RectInt room)
    {
        if (room.width <= minRoomSize * 2 && room.height <= minRoomSize * 2)
        {
            rooms.Add(room);
            return;
        }

        // Vertical
        if (room.width > minRoomSize * 2)
        {
            var (left, right) = SplitVertically(room);
            SplitRoom(left);
            SplitRoom(right);
        }
        // Horizontal
        else if (room.height > minRoomSize * 2)
        {
            var (top, bottom) = SplitHorizontally(room);
            SplitRoom(top);
            SplitRoom(bottom);
        }
    }

    // Splits the given rooms into two new rooms (vertical)
    (RectInt, RectInt) SplitVertically(RectInt pRoom)
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

    /// <summary>
    /// The CheckIntersection coroutine checks through the rooms which are intersected with each other, visualizes them and adds doors to valid intersections.
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckIntersection()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            for (int j = i + 1; j < rooms.Count; j++)
            {
                RectInt roomA = rooms[i];
                RectInt roomB = rooms[j];

                if (AlgorithmsUtils.Intersects(roomA, roomB))
                {
                    // Visualize the intersections
                    RectInt intersection = AlgorithmsUtils.Intersect(roomA, roomB);
                    DebugDrawingBatcher.BatchCall(() => AlgorithmsUtils.DebugRectInt(intersection, Color.yellow));

                    // Prevent the doors being placed on invalid places (corners)
                    if (intersection.width == 1 && intersection.height >= 3)
                    {
                        int y = Random.Range(intersection.yMin + 1, intersection.yMax - 1);
                        Vector3 doorPos = new Vector3(intersection.xMin + 0.5f, 0, y + 0.5f);
                        doorPositions.Add(doorPos);
                    }

                    else if (intersection.height == 1 && intersection.width >= 3)
                    {
                        int x = Random.Range(intersection.xMin + 1, intersection.xMax - 1);
                        Vector3 doorPos = new Vector3(x + 0.5f, 0, intersection.yMin + 0.5f);
                        doorPositions.Add(doorPos);
                    }
                }
            }
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }

    /// <summary>
    /// Adds all the nodes and edges to the dictionary, accessed from the Graph class to create a graph
    /// </summary>
    [Button]
    public void CreateGraph()
    {
        // Adds the locations of rooms and doors to the Graph
        foreach (var room in rooms)
        {
            Vector3 roomCenter = new Vector3(room.center.x, 0, room.center.y);

            roomGraph.AddNode(roomCenter);
            foreach (var door in doorPositions)
            {
                roomGraph.AddNode(door);

                // Adds the edges from room centers and doors to each other to the graph
                if (room.Contains(new Vector2Int(Mathf.FloorToInt(door.x), Mathf.FloorToInt(door.z))))
                {
                    roomGraph.AddEdge(roomCenter, door);
                }
            }
        }
        // See graph in console
        roomGraph.PrintGraph();
        roomGraph.ConnectedChecker();
    }

    /// <summary>
    /// Visualizes the doors and graph with the help of Gizmos
    /// </summary>
    private void OnDrawGizmos()
    {
        // boolean that removes gizmos when set to true
        if (!showDebugGizmos) return;

        // Draw doors
        foreach (var door in doorPositions)
        {
            Gizmos.DrawCube(door, doorSize);
        }

        // If the graph exists, the graph will be visualized
        if (roomGraph == null)
            return;

        var graphData = roomGraph.GetAdjacencyList();
        Gizmos.color = Color.magenta;

        // Draw nodes
        foreach (var node in graphData.Keys)
        {
            Gizmos.DrawSphere(node, 0.5f);
        }

        // Draw edges
        foreach (var fromNode in graphData.Keys)
        {
            foreach (var toNode in graphData[fromNode])
            {
                Gizmos.DrawLine(fromNode, toNode);
            }

        }
    }

    [Button]
    private void SpawnAssets()
    {
        DebugDrawingBatcher.ClearCalls();
        showDebugGizmos = false;
        tileMap = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tileMap[x, y] = 1;
            }
        }

        foreach (var room in rooms)
        {
            for (int x = room.xMin + 1; x < room.xMax - 1; x++)
            {
                for (int y = room.yMin + 1; y < room.yMax - 1; y++)
                {
                    tileMap[x, y] = 0;
                }
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (tileMap[x, y] == 1)
                {
                    // Instantiate wall prefab at position (x, y)
                    Vector3 wallPos = new Vector3(x + 0.5f, 0, y + 0.5f); // center the tile
                    Instantiate(wall, wallPos, Quaternion.identity, wallsParent.transform);
                }
            }
        }

        // print tile map
        string mapString = "";
        for (int y = height - 1; y >= 0; y--) // Print top to bottom
        {
            for (int x = 0; x < width; x++)
            {
                mapString += tileMap[x, y] + " ";
            }
            mapString += "\n";
        }

        Debug.Log(mapString);
    }

    // Starts the coroutine CheckIntersection by pressing the button
    [Button]
    void VisualizeIntersect()
    {
        StartCoroutine(CheckIntersection());
    }

    [Button]
    void DebugAmount()
    {
        Debug.Log("Amount of rooms in the list: " + rooms.Count);
        Debug.Log("Amount of doors in the list: " + doorPositions.Count);

        if (rooms.Count - 1 <= doorPositions.Count)
        {
            Debug.Log("Right amount of doors and rooms");
        }
        else
        {
            Debug.Log("The doors and rooms do not have the right amount.");
        }

    }

    [Button]
    void DebugRoomSizes()
    {
        foreach (var room in rooms)
        {
            Debug.Log(room);
        }
    }
}
