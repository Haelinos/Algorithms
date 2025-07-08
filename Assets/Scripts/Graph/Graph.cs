using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Graph<T>
{
    private Dictionary<T, List<T>> adjacencyList;
    public Graph() { adjacencyList = new Dictionary<T, List<T>>(); }


    /// <summary>
    /// Add nodes to the graph
    /// </summary>
    /// <param name="node"></param>
    public void AddNode(T node) 
    {
        if (!adjacencyList.ContainsKey(node))
        {
            adjacencyList[node] = new List<T>();
            //Debug.Log("Nodes added!");
        }

    }

    /// <summary>
    /// Add edges to the graph
    /// </summary>
    /// <param name="fromNode"></param>
    /// <param name="toNode"></param>
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

    /// <summary>
    /// BFS algorithm
    /// </summary>
    /// <param name="startNode"></param>
    /// <returns></returns>
    public List<T> SearchGraph(T startNode)
    {
        // Create a queue and list for visited nodes
        Queue<T> queue = new Queue<T>();
        List<T> visitedNodes = new();

        // Put a node in the queue and add it to the visited list
        queue.Enqueue(startNode);
        visitedNodes.Add(startNode);

        while (queue.Count > 0)
        {
            // Take the current node out of the queue and see if current node doesn't have neighbors
            T current = queue.Dequeue();
            Debug.Log("visited, " + current);

            // If the neighbor is not visited yet, put them in the queue and add them to the visited list
            foreach (var neighbor in adjacencyList[current])
            { 
                if (!visitedNodes.Contains(neighbor))
                {
                    visitedNodes.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
            
        }
        // Returns the visited nodes list
        return visitedNodes;
    }

    // Returns the adjacencylist 
    public Dictionary<T, List<T>> GetAdjacencyList()
    {
        return adjacencyList;
    }

    /// <summary>
    /// Checks whether the visited graph nodes and adjacencylist correspond for a connected dungeon
    /// </summary>
    public void ConnectedChecker()
    {
        if (adjacencyList.Count == 0)
        {
            Debug.Log("Graph is empty");
            return;
        }

        // Get the first node in the dictionary and store the reachable nodes in the list
        T startNode = adjacencyList.Keys.First();
        List<T> visitedNodes = SearchGraph(startNode);

        // Visual debug
        if (visitedNodes.Count == adjacencyList.Count)
        {
            Debug.Log("The Graph is fully connected!");
        }
        else
        {
            Debug.LogError("The Graph is NOT fully connected!");
        }

    }
}
