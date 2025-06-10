using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Graph<T>
{
    private Dictionary<T, List<T>> adjacencyList;
    public Graph() { adjacencyList = new Dictionary<T, List<T>>(); }


    // Add nodes to Graph
    public void AddNode(T node) 
    {
        if (!adjacencyList.ContainsKey(node))
        {
            adjacencyList[node] = new List<T>();
            //Debug.Log("Nodes added!");
        }

    }

    // Add edges to Graph
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

    // Print graph to console with nodes and connections (edges)
    public void PrintGraph() 
    {
        foreach (var node in adjacencyList) 
        {
            string connections = string.Join(", ", node.Value);
            Debug.Log(node.Key.ToString() + "-> " + connections);
        }
    }
    public Dictionary<T, List<T>> GetAdjacencyList()
    {
        return adjacencyList;
    }
}
