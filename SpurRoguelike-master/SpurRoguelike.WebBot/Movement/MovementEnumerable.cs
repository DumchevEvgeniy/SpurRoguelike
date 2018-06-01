using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SpurRoguelike.WebPlayerBot.Extensions;
using SpurRoguelike.WebPlayerBot.Infractructure;

internal class MovementEnumerable<T> : IEnumerable<PonderableNode<T>> where T : IComparable<T> {
    private MovementIterator<T> iterator;

    public MovementEnumerable() {
        iterator = new MovementIterator<T>();
    }

    public void Add(PonderableNode<T> item) => iterator.AvailableNodes.Add(item);

    public Boolean WasVisited(Node node) => WasVisited(node.Location);
    public Boolean WasVisited(Location location) => iterator.VisitedNodes.Exists(el => el.Location == location);

    public PonderableNode<T> Find(Node node) => Find(node.Location);
    public PonderableNode<T> Find(Location location) => iterator.AvailableNodes.FirstOrDefault(el => el.Location == location);

    public IEnumerator<PonderableNode<T>> GetEnumerator() {
        while(iterator.MoveNext())
            yield return iterator.Current;
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    sealed class MovementIterator<TWeight> : IEnumerator<PonderableNode<TWeight>> where TWeight : IComparable<TWeight> {
        private List<PonderableNode<TWeight>> availableNodes;
        private List<PonderableNode<TWeight>> visitedNodes;
        private PonderableNode<TWeight> current;

        public MovementIterator() => Reset();

        public void Dispose() { }
        public void Reset() {
            availableNodes = new List<PonderableNode<TWeight>>();
            visitedNodes = new List<PonderableNode<TWeight>>();
            current = null;
        }

        public Boolean MoveNext() {
            if(availableNodes.IsEmpty())
                return false;
            current = availableNodes.Aggregate((c1, c2) => c1.CompareTo(c2) <= 0 ? c1 : c2);
            availableNodes.Remove(current);
            visitedNodes.Add(current);
            return true;
        }

        public PonderableNode<TWeight> Current => current;
        Object IEnumerator.Current => Current;
        public List<PonderableNode<TWeight>> AvailableNodes => availableNodes;
        public List<PonderableNode<TWeight>> VisitedNodes => visitedNodes;
    }
}