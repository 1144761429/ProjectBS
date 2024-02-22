using System.Collections.Generic;

namespace Utilities
{
    /// <summary>
    /// A class that represents a dependency relationship among objects of type <typeparamref name="T"/>.
    /// </summary>
/*
 Given A depends on B, B depends on C, D depends on C, the following dependency relation is stored:
         |   Dependee    |   Dependent   |
 --------|---------------|---------------|
     A   |    B          |     None      |
     B   |    C          |     A         |
     C   |    None       |     B,D       |
     D   |    C          |     None      |
*/
    public class DependencyGraph<T>
    {
        /// <summary>
        /// The number of ordered pairs in this <see cref="DependencyGraph{T}"/>.
        /// </summary>
        /// 
        /// <example>
        /// If a graph has the following dependency relations, a->b, b->a,
        /// then the size of this graph will be 2.
        /// </example>
        public int Size { get; private set; }

        /// <summary>
        /// A dictionary that maps a <typeparamref name="T"/>, to its dependee <typeparamref name="T"/>s.
        /// </summary>
        private readonly Dictionary<T, HashSet<T>> _dependeeNodeDict;

        /// <summary>
        /// A dictionary that maps a <typeparamref name="T"/>, to its dependent <typeparamref name="T"/>.
        /// </summary>
        private readonly Dictionary<T, HashSet<T>> _dependentNodeDict;

    
        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            _dependeeNodeDict = new Dictionary<T, HashSet<T>>();
            _dependentNodeDict = new Dictionary<T, HashSet<T>>();
        }
    
        /// <summary>
        /// Check if any other nodes in the <see cref="DependencyGraph{T}"/> depends on the node <paramref name="id"/>
        /// </summary>
        /// <param name="id">The ID of the node to check.</param>
        /// <returns>True if node <paramref name="id"/> has dependent nodes. Otherwise, return false.</returns>
        public bool HasDependents(T id)
        {
            return _dependentNodeDict.ContainsKey(id) && _dependentNodeDict[id].Count > 0;
        }
    
        /// <summary>
        /// Check if the node <paramref name="id"/> depends on any other nodes in the <see cref="DependencyGraph{T}"/>.
        /// </summary>
        /// <param name="id">The ID of the node to check.</param>
        /// <returns>True if node <paramref name="id"/> has dependee nodes. Otherwise, return false.</returns>
        public bool HasDependees(T id)
        {
            return _dependeeNodeDict.ContainsKey(id) && _dependeeNodeDict[id].Count > 0;
        }
    
        /// <summary>
        /// Get an <see cref="IEnumerable{T}"/> of dependent nodes of a node <see cref="id"/>.
        /// </summary>
        /// <param name="id">The ID of the node.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> of dependent nodes of node with ID <paramref name="id"/>.
        /// </returns>
        public IEnumerable<T> GetDependents(T id)
        {
            if (!_dependentNodeDict.ContainsKey(id))
            {
                return new List<T>();
            }

            return _dependentNodeDict[id];
        }

        /// <summary>
        /// Get an <see cref="IEnumerable{T}"/> of dependee nodes of a node <see cref="id"/>.
        /// </summary>
        /// <param name="id">The ID of the node.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> of dependee nodes of node with ID <paramref name="id"/>.
        /// </returns>
        public IEnumerable<T> GetDependees(T id)
        {
            if (!_dependeeNodeDict.ContainsKey(id))
            {
                return new List<T>();
            }

            return _dependeeNodeDict[id];
        }
    
        /// <summary>
        /// Add a dependency relationship to the dependency graph.
        /// </summary>
        /// <param name="origin">The dependee node in a dependency relation, where the relation starts at.</param>
        /// <param name="destination">The dependent node in a dependency relation, where the relation ends at.</param>
        /// <example>
        /// AddDependency("a", "b") this will add a dependency relation as the follows:
        ///     Node "a" is a dependee of node "b", and node "b" is a dependent of node "a",
        ///     so now we need to evaluate node "a" before evaluating node "b".
        /// </example>
        public void AddDependency(T origin, T destination)
        {
            // If the dependee node is the first time appearing,
            // add it to the _nodeDependentDict with an empty HashSet.
            if (!_dependentNodeDict.ContainsKey(origin))
            {
                _dependentNodeDict.Add(origin, new HashSet<T>());
            }

            // If the dependent node is first time appearing,
            // add it to the _nodeDependeeDict with an empty HashSet.
            if (!_dependeeNodeDict.ContainsKey(destination))
            {
                _dependeeNodeDict.Add(destination, new HashSet<T>());
            }

            bool successfullyAdded = _dependentNodeDict[origin].Add(destination);
            if (_dependeeNodeDict[destination].Add(origin) || successfullyAdded)
            {
                Size++;
            }
        }
    
        /// <summary>
        /// Remove a dependency relationship from the dependency graph.
        /// If the ordered pair does not exist in the dependency graph, then do nothing.
        /// 
        /// <para>
        /// After removal, if a node has no more dependee or dependent node, remove the node from the
        /// corresponding dictionary to save space.
        /// </para>
        /// 
        /// </summary>
        /// <param name="origin">The dependee node in a dependency relation, where the relation starts at.</param>
        /// <param name="destination">The dependent node in a dependency relation, where the relation ends at.</param>
        /// <example>
        /// RemoveDependency("a", "b") this will remove a dependency relation as the follows:
        ///     Node "a" is no longer a dependee of node "b", and node "b" is no longer a dependent of node "a",
        /// </example>
        public void RemoveDependency(T origin, T destination)
        {
            bool successfullyRemoved = false;

            if (_dependentNodeDict.ContainsKey(origin))
            {
                _dependentNodeDict[origin].Remove(destination);
                successfullyRemoved = true;
            
                // After the removal, if the origin node has no more dependent node, remove it from the dependentNodeDict.
                if (_dependentNodeDict[origin].Count == 0)
                {
                    _dependentNodeDict.Remove(origin);
                }
            }

            if (_dependeeNodeDict.ContainsKey(destination))
            {
                _dependeeNodeDict[destination].Remove(origin);
                successfullyRemoved = true;
            
                // After the removal, if the destination node has no more dependee node, remove it from the dependeeNodeDict.
                if (_dependeeNodeDict[destination].Count == 0)
                {
                    _dependeeNodeDict.Remove(destination);
                }
            }

            if (successfullyRemoved)
            {
                Size--;
            }
        }
    
        /// <summary>
        /// Remove all existing ordered pair of form (node with ID <paramref name="origin"/>, [any node]).
        /// Then, add the nodes in <paramref name="newDependents"/> as the dependent nodes of node 
        /// with ID <paramref name="origin"/>.
        /// 
        /// If the destination node with ID <paramref name="origin"/> does not exist, then we add the node
        /// to the graph and add dependent nodes of it to the graph.
        /// </summary>
        /// <param name="origin">The origin of a form of ordered pair.</param>
        /// <param name="newDependents">A enumerable collection of new dependent nodes of node with ID <paramref name="origin"/>.</param>
        public void ReplaceDependents(T origin, IEnumerable<T> newDependents)
        {
            // Return if node s does not exist in the dependent dictionary.
            if (_dependentNodeDict.TryGetValue(origin, out HashSet<T> dependentNodes))
            {
                // Remove all the dependent node of origin node.
                foreach (T dependentNode in dependentNodes)
                {
                    //_dependeeNodeDict[dependentNode].Remove(origin);
                    RemoveDependency(origin, dependentNode);
                }
            }

            // Add dependency relation for node s and all the node in the passed-in IEnumerable
            foreach (T node in newDependents)
            {
                AddDependency(origin, node);
            }
        }
    
        /// <summary>
        /// Remove all existing ordered pair of form ([any node], node with ID <paramref name="destination"/>).
        /// Then, add the nodes in <paramref name="newDependees"/> as the dependee nodes of node 
        /// with ID <paramref name="destination"/>. 
        /// 
        /// If the destination node with ID <paramref name="destination"/> does not exist, then we add the node
        /// to the graph and add dependee nodes of it to the graph.
        /// </summary>
        /// <param name="destination">The destination of a form of ordered pair.</param>
        /// <param name="newDependees">A enumerable collection of new dependee nodes of node with ID <paramref name="destination"/>.</param>
        public void ReplaceDependees(T destination, IEnumerable<T> newDependees)
        {
            // Do nothing if node s does not exist in the dependee dictionary.
            if (_dependeeNodeDict.TryGetValue(destination, out HashSet<T> dependeeNodes))
            {
                // Remove all the dependee node of destination node
                foreach (T dependeeNode in dependeeNodes)
                {
                    RemoveDependency(dependeeNode, destination);
                }
            }

            // Add dependency relation for node s and all the node in the passed-in IEnumerable
            foreach (T node in newDependees)
            {
                AddDependency(node, destination);
            }
        }
    }
}
