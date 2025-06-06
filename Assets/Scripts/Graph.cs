using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Graph<T>
{
    private Dictionary<T, List<T>> adjacencyList;
    public DungeonGenerator dungeonGenerator;
    public Graph() { adjacencyList = new Dictionary<T, List<T>>(); }


    public void AddNode(T node) 
    {
        foreach (var room in dungeonGenerator.rooms)
        {
            if (!adjacencyList.ContainsKey(node)) 
            {
                adjacencyList[node] = new List<T>();
            }
            //Vector3 roomCenter = new Vector3(room.center.x, 0, room.center.y);
            
        }
    }

    public void AddEdge(T fromNode, T toNode) 
    {
        if (!adjacencyList.ContainsKey(fromNode) || !adjacencyList.ContainsKey(toNode)) 
        {
            Debug.Log("One or both nodes do not exist in the graph");
            return;
        }
        adjacencyList[fromNode].Add(toNode);
        adjacencyList[toNode].Add(fromNode);
    }

    public void GetNeighbors() 
    {
     
    }
}
