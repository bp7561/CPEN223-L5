// Lab 5
// Student name: Brian Pham
// Student number: 24626509

using System;
using System.Collections.Generic;

Console.WriteLine("CPEN223 Lab 5");

//Testing: Write test cases that exercise all four methods you are to implement.
//TODO
Console.WriteLine("Test Cases:");

// 4.1. Connected Scaffold Setup
Dictionary<string, HashSet<string>> connectedGraph = new()
{
    ["A"] = new() { "B", "C" },
    ["B"] = new() { "A", "D" },
    ["C"] = new() { "A", "D" },
    ["D"] = new() { "B", "C", "E" },
    ["E"] = new() { "D" }
};
Console.WriteLine("Test Case 4.1 Connected Scaffold:");

Console.WriteLine("Test Case ReachableFrom(g, A):");
HashSet<string> reachfrom1 = GraphLibrary.ReachableFrom(connectedGraph, "A");
Console.WriteLine($"Expected: A, B, C, D, E (not in order), Actual: {string.Join(", ", reachfrom1)}");
Console.WriteLine("");

Console.WriteLine("Test Case CountComponents(g):");
int countcomp1 = GraphLibrary.CountComponents(connectedGraph);
Console.WriteLine($"Expected: 1, Actual: {countcomp1}");
Console.WriteLine("");

Console.WriteLine("Test Case ShortestDistance(g, A, E):");
int shortdist1 = GraphLibrary.ShortestDistance(connectedGraph, "A", "E");
Console.WriteLine($"Expected: 3, Actual: {shortdist1}");
Console.WriteLine("");

Console.WriteLine("Test Case ShortestDistance(g, C, C):");
int shortdist2 = GraphLibrary.ShortestDistance(connectedGraph, "C", "C");
Console.WriteLine($"Expected: 0 | Actual: {shortdist2}");
Console.WriteLine("");

// 4.2. Disconnected Scaffold Setup
Dictionary<string, HashSet<string>> disconnectedGraph = new()
{
    ["A"] = new() { "B" },
    ["B"] = new() { "A", "C" },
    ["C"] = new() { "B" },
    ["D"] = new() { "E" },
    ["E"] = new() { "D" },
    ["F"] = new() { } // Isolated vertex
};
Console.WriteLine("Test Case 4.2 Disonnected Scaffold:");

Console.WriteLine("Test Case CountComponents(g):");
int countcomp2 = GraphLibrary.CountComponents(disconnectedGraph);
Console.WriteLine($"Expected: 3, Actual: {countcomp2}");
Console.WriteLine("");

Console.WriteLine("Test Case ReachableFrom(g, D):");
HashSet<string> reachfrom2 = GraphLibrary.ReachableFrom(disconnectedGraph, "D");
Console.WriteLine($"Expected: D, E, Actual: {string.Join(", ", reachfrom2)}");
Console.WriteLine("");

Console.WriteLine("Test Case ShortestDistance(g, A, E) :");
int shortdist3 = GraphLibrary.ShortestDistance(disconnectedGraph, "A", "E");
Console.WriteLine($"Expected: -1, Actual: {shortdist3}");
Console.WriteLine("");

//end Testing code

//Do not change the program skeleton: keep the class name, method names,
//parameters, and return types exactly as given.
//Do not use LINQ, and do not use Console inside the GraphLibrary methods.

public static class GraphLibrary
{
    /// <summary>
    /// Determines whether an undirected graph is connected.
    /// The graph is connected if every vertex can be reached from <paramref name="start"/>
    /// by following zero or more edges.
    /// An empty graph is considered connected.
    /// </summary>
    /// <param name="graph">
    /// The graph, represented as an adjacency list:
    /// each key is a vertex and its associated list contains that vertex's neighbours.
    /// </param>
    /// <param name="start">
    /// The vertex from which the traversal begins.
    /// </param>
    /// <returns>
    /// true if every vertex in <paramref name="graph"/> is reachable from
    /// <paramref name="start"/>; otherwise false.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the graph is non-empty and <paramref name="start"/> is not a vertex
    /// in the graph.
    /// </exception>
    /// <remarks>
    /// Assumes that <paramref name="graph"/> is a valid representation of an
    /// undirected graph:
    /// every neighbour appearing in an adjacency list is also a key in the graph,
    /// and if v lists w as a neighbour, then w lists v as a neighbour.
    /// The method does not modify the graph.
    /// </remarks>
    static bool IsConnected(
        IReadOnlyDictionary<string, HashSet<string>> graph, string start)
    {
        // validation
    	if (graph.Count == 0) 
			
			return true;
		
    	if (!graph.ContainsKey(start)) 
			
			throw new ArgumentException("ERROR: start node is not a key in graph");

    	// call ReachableFrom
    	HashSet<string> reachableNodes = ReachableFrom(graph, start);

    	// if the number of reachable nodes is equal to the total nodes, return true
    	return reachableNodes.Count == graph.Count;
    }


    public static HashSet<string> ReachableFrom(
        IReadOnlyDictionary<string, HashSet<string>> graph,
        string start)
    {
		// validation
        if (graph == null)
			
            throw new ArgumentNullException("ERROR: null graph");

        if (!graph.ContainsKey(start))
			
            throw new ArgumentException("ERROR: start node is not a key in graph");
		
		// new Hashset with new nodes reached
        HashSet<string> reached = new HashSet<string>();
		// new Stack to track node traversal
        Stack<string> track = new Stack<string>();
		
		// Push start node to the track Stack for traversal
        track.Push(start);

		// while loop that runs until there are no nodes left to traverse
        while (track.Count > 0)
        {
			// Pop current node for processing
            string current = track.Pop();
			// if node is new, add to reached Hashset, else go to next node with continue statement
            if (!reached.Add(current)) continue;

			// checks current node and every neighbouring nodes in it's HashSet
            foreach (string neighbours in graph[current])
            {
				// Push neighbouring nodes into track Stack for next loop
                track.Push(neighbours);
            }
        }

		// return reached HashSet 
        return reached;
    }
	

    public static int CountComponents(
        IReadOnlyDictionary<string, HashSet<string>> graph)
    {
		// validation
        if (graph == null)
			
            throw new ArgumentNullException("ERROR: null graph");

		// if graph is empty, return 0
        if (graph.Count == 0)
            return 0;

		// new HashSet with nodes not connected yet
        HashSet<string> unreached = new HashSet<string>();
		
		// iterates through each KeyValuePair in graph dictionary
        foreach (KeyValuePair<string, HashSet<string>> KVP in graph)
        {
			// add node Key to unreached HashSet
            unreached.Add(KVP.Key);
        }

		// initialize component count to 0
        int componentsCount = 0;

		// iterates through each KeyValuePair in graph dictionary again
        foreach (KeyValuePair<string, HashSet<string>> KVP in graph)
        {
			// current node Key is saved as a string
            string currNode = KVP.Key;
			
			//if node is in unreached HashSet, process else skip
            if (unreached.Contains(currNode))
            {
				// increment count by 1
                componentsCount++;
				
				// call ReachableFrom for a HashSet with all nodes connected to current node
                HashSet<string> component = ReachableFrom(graph, currNode);
                
				// iterates through all nodes in component
                foreach (string reachedNode in component)
                {
					// removes reached nodes from unreached HashSet
                    unreached.Remove(reachedNode);
                }
            }
        }
		
		// return component count
        return componentsCount;
    }


    public static int ShortestDistance(
        IReadOnlyDictionary<string, HashSet<string>> graph,
        string start,
        string target)
    {
		// validation
        if (graph == null)
			
            throw new ArgumentNullException("ERROR: null graph");

        if (!graph.ContainsKey(start))
			
            throw new ArgumentException("ERROR: start node is not a key in graph");

        if (!graph.ContainsKey(target))
            throw new ArgumentException("ERROR: target node is not a key in graph.");

		// if start node is target node, return 0
        if (start == target)
            return 0;

        // new Queue to process nodes outwards
        Queue<string> track = new Queue<string>();
		// new Dictionary to store nodes and distances from start node
        Dictionary<string, int> distances = new Dictionary<string, int>();

		// Enqueue start node
        track.Enqueue(start);
		// distance to start node is 0
        distances[start] = 0;

		// while loop that runs until Queue is empty
        while (track.Count > 0)
        {
			// Dequeue current node for processing
            string curr = track.Dequeue();
			// calculate distance from current node to start node
            int currDistance = distances[curr];

			// if current node is the target node, return its distance
            if (curr == target)
				
                return currDistance;

			// iterate through all neighbouring strings in current node
            foreach (string neighbours in graph[curr])
            {
				// if Dictionary has neighbour already, skip
                if (!distances.ContainsKey(neighbours))
                {
					// since neighbour is new, its distance from current node is + 1
                    distances[neighbours] = currDistance + 1;
					// Enqueue neighbour to process its neighbours later
                    track.Enqueue(neighbours);
                }
            }
        }
		
		// return if there is no path between start and target
        return -1;
    }
}

