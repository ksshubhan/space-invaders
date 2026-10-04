using System;
using System.Collections.Generic;

public class PriorityQueue<T>
{
    private List<System.Tuple<T, int>> elements = new List<System.Tuple<T, int>>();

    public int Count
    { 
        get { return elements.Count; }
    }

    public void Enqueue(T item, int priority)
    {
        elements.Add(Tuple.Create(item, priority));
    }

    public T Dequeue()
    {
        int bestIndex = 0;

        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i].Item2 < elements[bestIndex].Item2)
            {
                bestIndex = i;
            }
        }

        T bestItem = elements[bestIndex].Item1;
        elements.RemoveAt(bestIndex);
        return bestItem;    
    }
}
